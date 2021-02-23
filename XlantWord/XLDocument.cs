using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Core;
using System.Windows.Forms;
using System.IO;
using XLant;
using System.Diagnostics;
using System.Reflection;
using System.Net;

namespace XlantWord
{
    class XLDocument
    {
        public class Header
        {
            public Header()
            {
                HeaderLines = new List<Line>();
                FooterLines = new List<Line>();
                Images = new List<Image>();
            }

            public Header(XElement element)
            {
                HeaderLines = new List<Line>();
                FooterLines = new List<Line>();
                Images = new List<Image>();
                Description = element.Attribute("Description").Value;
                Firmname = element.Attribute("Firm").Value;
                foreach (XElement xLines in element.Descendants("Lines"))
                {
                    if (xLines.Attribute("Type").Value == "Header")
                    {
                        foreach (XElement xLine in element.Descendants("HeaderLine"))
                        {
                            Line line = new Line();
                            line.Text = xLine.Value;
                            line.Size = xLine.AttributeIntNull("Size");
                            line.Font = xLine.AttributeValueNull("Font");
                            line.Bold = xLine.AttributeIntNull("Bold");
                            HeaderLines.Add(line);
                        }
                    }
                    //It is only either a header or a footer
                    else
                    {
                        FooterRightIndent = xLines.AttributeIntNull("RightIndent");
                        foreach (XElement xLine in element.Descendants("FooterLine"))
                        {
                            Line line = new Line();
                            line.Text = xLine.Value;
                            line.Size = xLine.AttributeIntNull("Size");
                            line.Font = xLine.AttributeValueNull("Font");
                            line.Bold = xLine.AttributeIntNull("Bold");
                            FooterLines.Add(line);
                        }
                    }
                }
                //Then deal with any images/logos
                foreach (XElement xImage in element.Descendants("Image"))
                {
                    Image logo = new Image()
                    {
                        SourceLocation = xImage.AttributeValueNull("ImageSource"),
                        PointsFromLeftEdge = xImage.AttributeIntNull("LeftEdge"),
                        PointsFromTop = xImage.AttributeIntNull("Top"),
                        Width = xImage.AttributeIntNull("Width"),
                        Height = xImage.AttributeIntNull("Height"),
                    };
                    Images.Add(logo);
                }
            }
            public string Description { get; set; }
            public string Firmname { get; set; }
            public int HeaderRightIndent { get; set; }
            public int HeaderLeftIndent { get; set; }
            public int FooterRightIndent { get; set; }
            public int FooterLeftIndent { get; set; }
            public List<Line> HeaderLines { get; set; }
            public List<Line> FooterLines { get; set; }
            public List<Image> Images { get; set; }
        }

        public class Line
        {
            public string Text { get; set; }
            public string Font { get; set; }
            public int Size { get; set; }
            public int Bold { get; set; }
        }

        public class Image
        {
            public string SourceLocation { get; set; }
            public int PointsFromLeftEdge { get; set; }
            public int PointsFromTop { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public class ListItem
        {
            public string Name { get; set; }
            public string DbTag { get; set; }

            public ListItem(string newName, string newTag)
            {
                Name = newName;
                DbTag = newTag;
            }
        }

        public class MergeField
        {
            public MergeField()
            {
                Type = "Text";
                Parameters = new Dictionary<string, string>();
            }
            public string Name { get; set; }
            public string Type { get; set; }
            public Dictionary<string, string> Parameters { get; set; }
        }

        private static XDocument settingsDoc = XLtools.settingsDoc;
        public static Document currentDoc;
        public static Microsoft.Office.Interop.Word.View currentView;
        private static Microsoft.Office.Interop.Word.Application app = Globals.ThisAddIn.Application;

        private static Range CurrentRange()
        {
            //reference to Global replaced with app
            Microsoft.Office.Interop.Word.Range CurrRange = app.Selection.Range;
            return CurrRange;
        }

        private static Document OpenDoc(string name, bool readOnly, bool recent = true, bool visible = true)
        {
            Document NewDoc = new Document();
            //reference to Global replaced with app
            NewDoc = app.Documents.Open(name, ReadOnly: readOnly, AddToRecentFiles: recent, Visible: visible);
            return NewDoc;
        }

        public static void UpdateCurrentDoc()
        {
            currentDoc = app.ActiveDocument;
        }

        public static Document GetCurrentDoc()
        {
            currentDoc = app.ActiveDocument;
            return currentDoc;
        }

        public static void UpdateCurrentView()
        {
            currentView = app.ActiveWindow.View;
        }

        public static void openTemplate(string type)
        {
            try
            {
                string dir = StandardLocation();
                Document NewDoc = new Document();
                NewDoc = OpenDoc(dir + type + ".docx", true);
                TempSave();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to open template");
                XLtools.LogException("OpenTemplate", e.ToString());
            }
        }

        public static string StandardLocation()
        {
            try
            {
                string dir = "";

                //query the setting files and try to find a match
                XElement setting = (from map in settingsDoc.Descendants("Standards")
                                    select map).FirstOrDefault();
                if (setting != null)
                {
                    dir = setting.Attribute("Location").Value;
                }
                return dir;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to find location of standard documents");
                XLtools.LogException("StandardLocation", e.ToString());
                return null;
            }
        }

        public static string TempSave(string fileType = ".docx", string fileLocation = "")
        {
            try
            {
                string location;
                XLDocument.UpdateCurrentDoc();
                if (String.IsNullOrEmpty(fileLocation))
                {
                    string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\XLant\\temp\\";

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    location = TempFileName(folder, fileType);
                }
                else
                {
                    location = fileLocation;
                }
                if (fileType == ".docx")
                {
                    currentDoc.SaveAs(location, WdSaveFormat.wdFormatXMLDocument);
                }
                else if (fileType == ".pdf")
                {
                    currentDoc.SaveAs(location, WdSaveFormat.wdFormatPDF);
                }

                return location;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to create a temporary save please do so manually");
                XLtools.LogException("TempSave", e.ToString());
                return null;
            }
        }

        private static string TempFileName(string folder, string ext)
        {
            string location = "";
            if (!ext.Contains("."))
            {
                ext = "." + ext;
            }
            Random rnd = new Random();
            string filename = DateTime.Now.ToString("yyyyMMdd");
            int id = rnd.Next(1000, 9999); //provides a four digit id
            filename += id.ToString();
            filename += ext;
            //If that file already exists try again until it doesn't
            while (File.Exists(folder + filename))
            {
                filename = DateTime.Now.ToString("yyyyMMdd");
                id = rnd.Next(1000, 9999); //provides a four digit id
                filename += id.ToString();
                filename += ext;
            }
            location = folder + filename;
            return location;
        }

        public static void UpdateParameter(string pName, string pValue)
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                //the cast is required in .NET 3.5 so is included here for ease although not strictly required
                DocumentProperties properties = (DocumentProperties)currentDoc.CustomDocumentProperties;
                if (properties.Cast<DocumentProperty>().Where(c => c.Name == pName).Count() == 0)
                {
                    properties.Add(pName, false, MsoDocProperties.msoPropertyTypeString, pValue);
                }
                else
                {
                    properties[pName].Value = pValue;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to update parameter");
                XLtools.LogException("UpdateParameter", e.ToString());
            }
        }

        public static string ReadParameter(string pName)
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                DocumentProperties properties = (DocumentProperties)currentDoc.CustomDocumentProperties;
                foreach (DocumentProperty prop in properties)
                {
                    if (prop.Name == pName)
                    {
                        return prop.Value.ToString();
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to read parameter");
                XLtools.LogException("ReadParameter", e.ToString());
                return null;
            }
        }

        public static string CheckParamter(string defaultString, string pName)
        {
            try
            {
                string rtnString = "";
                string pValue = XLDocument.ReadBookmark(pName);
                if (pValue != "")
                {
                    rtnString = pValue;
                }
                else
                {
                    rtnString = defaultString;
                    UpdateParameter(pName, rtnString);
                }
                return rtnString;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to check parameter");
                XLtools.LogException("CheckParameter", e.ToString());
                return null;
            }
        }

        public static void UpdateBookmark(string bName, string bValue, int bold = 2, string styleName = "")
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                if (currentDoc.Bookmarks.Exists(bName))
                {
                    Range bRange = currentDoc.Bookmarks.get_Item(bName).Range;
                    bRange.Text = bValue;
                    //apply style first so that the bold can override if necessary
                    if (styleName != "")
                    {
                        bRange.set_Style(currentDoc.Styles[styleName]);
                    }
                    //bold is not specified make no changes so it stickes with the style in the document
                    if (bold < 2)
                    {
                        bRange.Bold = bold;
                    }
                    currentDoc.Bookmarks.Add(bName, bRange);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to update bookmark");
                XLtools.LogException("UpdateBookmark", e.ToString());
            }
        }

        public static string ReadBookmark(string bName)
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                string bText = "";
                if (currentDoc.Bookmarks.Exists(bName))
                {
                    Range bRange = currentDoc.Bookmarks.get_Item(bName).Range;
                    bText = bRange.Text;
                }
                return bText;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to read bookmark");
                XLtools.LogException("ReadBookmark", e.ToString());
                return null;
            }
        }

        public static string CheckBookmark(string defaultString, string bName, int bold = 2)
        {
            try
            {
                string rtnString = "";
                string bValue = XLDocument.ReadBookmark(bName);
                if (bValue != "")
                {
                    rtnString = bValue;
                }
                else
                {
                    rtnString = defaultString;
                    UpdateBookmark(bName, rtnString, bold);
                }
                return rtnString;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to check bookmark");
                XLtools.LogException("CheckBookmark", e.ToString());
                return null;
            }
        }

        public static void ChangeStatus(string Status)
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                string currentStatus = ReadParameter("VCStatus");
                if (currentStatus != Status)
                {
                    if (currentDoc.Bookmarks.Exists("Status"))
                    {
                        Range bRange = currentDoc.Bookmarks.get_Item("Status").Range;
                        bRange.Text = Status;
                        currentDoc.Bookmarks.Add("Status", bRange);
                    }
                    UpdateParameter("VCStatus", Status);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to update status");
                XLtools.LogException("OpenTemplate", e.ToString());
            }
        }

        public static List<Header> GetHeaders()
        {
            try
            {
                List<Header> headings = new List<Header>();

                IEnumerable<XElement> xHeadings = settingsDoc.Descendants("Headings");

                foreach (XElement xHeading in xHeadings.Descendants("Heading"))
                {
                    Header header = new Header(xHeading);
                    headings.Add(header);
                }
                return headings;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to load settings file: " + e.ToString());
                XLtools.LogException("GetHeader", e.ToString());
                return null;
            }
        }

        public static Header MapHeader(string office, string department)
        {
            try
            {
                Header selectedHeader = new Header();

                //query the setting files and try to find a match
                XElement selectedMap = (from map in settingsDoc.Descendants("Map")
                                        where (string)map.Attribute("Office") == office.ToUpper() && (string)map.Attribute("Department") == department
                                        select map).FirstOrDefault();
                if (selectedMap != null)
                {
                    //find the header with that description
                    XElement foundHeader = (from heading in settingsDoc.Descendants("Heading")
                                            where (string)heading.Attribute("Description").Value == selectedMap.Attribute("Header").Value
                                            select heading).FirstOrDefault();
                    //then build the object
                    selectedHeader = new Header(foundHeader);
                }
                else
                {
                    MessageBox.Show("Unable to Map " + office + ":" + department + ". Please review mappings or make another selection.");
                }
                return selectedHeader;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Map " + office + ":" + department + ". Please review client data.");
                XLtools.LogException("MapHeader", e.ToString());
                return null;
            }
        }

        public static Header MapHeaderFooter(string office, string footer)
        {
            try
            {
                Header selectedHeader = new Header();

                //query the setting files and try to find a match
                XElement selectedMap = (from map in settingsDoc.Descendants("Map")
                                        where (string)map.Attribute("Office") == office && (string)map.Attribute("Footer") == footer
                                        select map).FirstOrDefault();
                if (selectedMap != null)
                {
                    //find the header with that description
                    XElement foundHeader = (from heading in settingsDoc.Descendants("Heading")
                                            where (string)heading.Attribute("Description").Value == selectedMap.Attribute("Header").Value
                                            select heading).FirstOrDefault();
                    //then build the object
                    selectedHeader = new Header(foundHeader);
                }
                else
                {
                    MessageBox.Show("Unable to Map " + office + ":" + footer + ". Please review mappings or make another selection.");
                }
                return selectedHeader;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to map header");
                XLtools.LogException("MapHeaderFooter", e.ToString());
                return null;
            }
        }

        public static void DeployHeader(Header header)
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                try
                {
                    //in some circumstances this is not setting the attribute
                    //still adds the header and footer correctly so just warn the user.
                    currentDoc.PageSetup.DifferentFirstPageHeaderFooter = -1;
                }
                catch
                {
                    MessageBox.Show("Unable to set the first page to a different header please double check the document");
                }
                Microsoft.Office.Interop.Word.Paragraph para = null;
                Microsoft.Office.Interop.Word.Range rng = null;
                // Setting First page Header
                //Clear any existing header
                rng = currentDoc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterFirstPage].Range;
                rng.Delete();
                rng.ParagraphFormat.RightIndent = header.HeaderRightIndent;
                rng.ParagraphFormat.LeftIndent = header.HeaderLeftIndent;
                rng.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                //Then build the new one line by line
                foreach (Line line in header.HeaderLines)
                {
                    para = currentDoc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterFirstPage].Range.Paragraphs.Add();
                    para.Range.Font.Name = line.Font;
                    para.Range.Font.Size = line.Size;
                    para.Range.Font.Bold = line.Bold;
                    para.Range.Text = line.Text + Environment.NewLine;
                }

                //Setting First page footer
                //First delete and existing footer
                rng = currentDoc.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterFirstPage].Range;
                rng.Delete();
                rng.ParagraphFormat.RightIndent = header.FooterRightIndent;
                rng.ParagraphFormat.LeftIndent = header.FooterLeftIndent;
                //Then build the new one line by line
                foreach (Line line in header.FooterLines)
                {
                    para = currentDoc.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterFirstPage].Range.Paragraphs.Add();
                    para.Range.Font.Name = line.Font;
                    para.Range.Font.Size = line.Size;
                    para.Range.Font.Bold = line.Bold;
                    para.Range.Font.Color = WdColor.wdColorGray50;
                    para.Range.Text = line.Text + Environment.NewLine;
                }
                foreach(Image logo in header.Images)
                {
                    Microsoft.Office.Interop.Word.Range anchorRng = currentDoc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterFirstPage].Range;
                    currentDoc.Shapes.AddPicture(logo.SourceLocation, false, true, logo.PointsFromLeftEdge, logo.PointsFromTop, logo.Width, logo.Height, anchorRng);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to deploy header");
                XLtools.LogException("DeployHeader", e.ToString());
            }
        }

        public static List<ListItem> GetList(string type)
        {
            try
            {
                List<ListItem> items = new List<ListItem>();

                //query the setting files and try to find a match
                IEnumerable<XElement> xItems = (from item in settingsDoc.Descendants(type)
                                                select item);
                foreach (XElement xItem in xItems)
                {
                    items.Add(new ListItem(xItem.ElementValueNull(), xItem.AttributeValueNull("DbTag")));
                }
                return items;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to obtain list of type: " + type);
                XLtools.LogException("GetList", e.ToString());
                return null;
            }
        }

        public static void InsertText(string text)
        {
            try
            {
                Microsoft.Office.Interop.Word.Range CurrRange = CurrentRange();
                CurrRange.Text = text;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to insert text");
                XLtools.LogException("InsertText", e.ToString());
            }
        }

        public static void AddStatusBox()
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                Microsoft.Office.Interop.Word.Shape textBox;
                //Check whether the bookmark already exists and only if it doesn't add it
                if (!currentDoc.Bookmarks.Exists("Status"))
                {
                    textBox = currentDoc.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 400, 50, 100, 50);
                    textBox.Line.Visible = MsoTriState.msoFalse;
                    textBox.Name = "MLStatus";
                    textBox.TextFrame.TextRange.Text = "Status";
                    textBox.TextFrame.TextRange.Font.Hidden = 1;
                    textBox.TextFrame.TextRange.Font.Italic = 1;
                    textBox.TextFrame.TextRange.Font.Name = "Calibri";
                    textBox.TextFrame.TextRange.Font.Size = 16;
                    textBox.TextFrame.TextRange.Bookmarks.Add("Status");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to add the status box");
                XLtools.LogException("AddStatusBox", e.ToString());
            }
        }

        public static void EndDocument()
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                //close panel if open
                if (XLantWordRibbon.xlTaskPane1 != null)
                {
                    XLantWordRibbon.xlTaskPane1.Visible = false;
                    XLantWordRibbon.xlTaskPane1.Dispose();
                    //remove the task pane
                    Globals.ThisAddIn.CustomTaskPanes.Remove(XLantWordRibbon.CustomxlTaskPane);
                }
                string str = currentDoc.FullName;
                //close document
                ((_Document)currentDoc).Close(SaveChanges: WdSaveOptions.wdDoNotSaveChanges);
                //no longer necessary to delete file if it is a VC doc as it does it itself
                //File.Delete(str);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to close document and exit");
                XLtools.LogException("EndDocument", e.ToString());
            }
        }

        public static void AddStyles()
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                string path = StandardLocation() + "Letter.dotx";
                Document sTemplate = OpenDoc(path, true, false, false);
                currentDoc.set_AttachedTemplate((object)sTemplate);
                sTemplate.Close(SaveChanges: false);
                currentDoc.UpdateStyles();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to add styles" + e.ToString());
                XLtools.LogException("AddStyles", e.ToString());
            }

        }

        public static string GetFileID()
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                string fileID = "";

                //Check whether the parameter has already been set
                fileID = ReadParameter("FileID");

                fileID = currentDoc.Name;
                if(fileID.IndexOf("-") > -1)
                {
                    int len = fileID.IndexOf("-"); //get the index of the version identifier
                    fileID = fileID.Substring(0, len);//ignore everything after and including the -
                    if (String.IsNullOrEmpty(fileID) || !int.TryParse(fileID, out int i))
                    {
                        throw new ArgumentException("Document doesn't appear to be in VC");
                    }
                    UpdateParameter("FileID", fileID); //add the parameter for next time.

                    return fileID;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to discover file ID");
                XLtools.LogException("GetFileID", e.ToString());
                return null;
            }
        }

        public static string CreatePdf(string fileString = "")
        {
            try
            {
                //use tempsave to create a pdf file and return location.
                fileString = TempSave(".pdf", fileString);

                return fileString;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to create a pdf document");
                XLtools.LogException("CreatePDf", e.ToString());
                return null;
            }
        }

        public static string AddHeadertoPDF(string filestring, string watermarkFile = "", string fileNamePref = "", string folderPref = "")
        {
            try
            {
                string folder = "";
                if (folderPref != "")
                {
                    folder = folderPref;
                }
                else
                {
                    folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\XLant\\"; 
                }
                if (String.IsNullOrEmpty(watermarkFile))
                {
                    watermarkFile = "Watermark.pdf";
                }
                ////if it isn't in the Roaming folder fetch from sharepoint
                //if (!System.IO.File.Exists(folder + watermarkFile))
                //{
                string webLocation = StandardLocation();
                //download remote files
                using (WebClient client = new WebClient())
                {
                    string fileName = folder + watermarkFile;
                    client.UseDefaultCredentials = true;
                    client.DownloadFile(webLocation + watermarkFile, fileName);
                }
                //}
                string watermarkLocation = folder + watermarkFile;

                //create a filelocation for the new file
                string filelocation = folder;
                Random rnd = new Random();
                string filename;

                if (fileNamePref == "")
                {
                    filename = DateTime.Now.ToString("yyyy-MM-dd");
                    int id = rnd.Next(1000, 9999); //provides a four digit id
                    filename += "-" + id.ToString();
                }
                else
                {
                    filename = fileNamePref;
                }
                filename += ".pdf";
                //If that file already exists try again until it doesn't
                while (File.Exists(folder + filename))
                {
                    if (fileNamePref == "")
                    {
                        filename = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        filename = fileNamePref;
                    }
                    int id2 = rnd.Next(1000, 9999); //provides a four digit id
                    filename += "-" + id2.ToString();
                    filename += ".pdf";
                }
                filelocation += filename;

                //run the merging process, taking the given pdf, the watermark and outputting the location of the newfile to filelocation
                if (File.Exists(filestring))
                {
                    Process p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = "pdftk";
                    p.StartInfo.Arguments = String.Format("\"{0}\" multibackground \"{1}\" output \"{2}\"", filestring, watermarkLocation, filelocation);

                    //start
                    p.Start();

                    //wait for output and return result
                    p.WaitForExit();
                    return filelocation;
                }
                else
                {
                    //if the location is incorrect throw and return null
                    Exception ex = new Exception("PDF does not exist");
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to create pdf document" + filestring);
                XLtools.LogException("AddHeadertoPDf", e.ToString());
                return null;
            }
        }

        internal static void AddBio(XLMain.Staff staff)
        {
            throw new NotImplementedException();
        }

        public static string AddAttachments(List<string> fileStrings)
        {
            //create a filelocation for the new file
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\XLant\\";

            //build the string of locations of files to merge
            string inputfiles = "";
            foreach (string s in fileStrings)
            {
                if (s.Contains("http"))
                {
                    //download remote files
                    using (WebClient client = new WebClient())
                    {
                        string tempFileName = TempFileName(folder, ".pdf");
                        client.UseDefaultCredentials = true;
                        client.DownloadFile(s, tempFileName);
                        inputfiles += tempFileName + " ";
                    }
                }
                else
                {
                    inputfiles += "\"" + s + "\" ";
                }
            }
            string outputFile = TempFileName(folder, ".pdf");
            //merge the documents
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "pdftk";
            string processStr = inputfiles + " cat output \"" + outputFile + "\"";
            p.StartInfo.Arguments = processStr;

            //start
            p.Start();

            //wait for output and return result
            p.WaitForExit();

            return outputFile;
        }

        public static void IndexPDFCopy(string filestring, string origFileID)
        {
            try
            {
                //get the info of the original file
                XLVirtualCabinet.FileInfo fileInfo = XLVirtualCabinet.FileIndex(origFileID);
                XLVirtualCabinet.BondResult result = XLVirtualCabinet.IndexDocument(filestring, fileInfo);

                if (result.ExitCode != 0)
                {
                    MessageBox.Show("Unable to index pdf, please index manually.  Error code: " + result.ExitCode.ToString() + "-" + result.StandardOutput.ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to index pdf document" + filestring);
                XLtools.LogException("IndexPDFCopy", e.ToString());
            }
        }

        public static bool AddSignature(string username)
        {
            try
            {
                //Add the actual signature
                string dir = StandardLocation();
                Microsoft.Office.Interop.Word.Range CurrRange = CurrentRange();
                CurrRange.InlineShapes.AddPicture(dir + username + ".gif");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to find signature");
                XLtools.LogException("AddSignature", e.ToString());
                return false;
            }
        }

        public static bool AddSignatureMetaData(XLMain.Staff user, XLMain.Staff sig)
        {
            try
            {
                //Add the meta data
                //Build string and add to document
                string str = user.name + ";";
                str += user.grade + ";";
                str += DateTime.Now.ToString("d MMMM yyyy");
                str += ";" + sig.name;
                //insert into document properties

                for (int i = 0; i < 10; i++)
                {
                    string param = "Sign" + i;
                    string s = XLDocument.ReadParameter(param);
                    if (String.IsNullOrEmpty(s))
                    {
                        XLDocument.UpdateParameter(param, str);
                        break;
                    }
                    if (i == 10)
                    {
                        MessageBox.Show("Signature fields full");
                        break;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to add signature metadata");
                XLtools.LogException("AddSignature", ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// Merge an FPI list of clients into the current active document
        /// </summary>
        /// <param name="clients">The list of FPIClients you want to merge</param>
        public static void MergeFPIData(List<XLMain.FPIClient> clients, string templateXML = null, bool forceNewDocument = false, bool asPdf = false, string saveLocationForPdf = "")
        {
            UpdateCurrentDoc();
            string office = "TAUNTON";
            string department = "GPT";
            string tempXML = "";
            Header header = new Header();
            long startPosition = currentDoc.Content.Start;
            long endPosition = currentDoc.Content.End;
            if (templateXML == null)
            {
                tempXML = CopyRangeToWordXML(currentDoc.Range());
            }
            else
            {
                tempXML = templateXML;
            }

            long letterLength = endPosition - startPosition;

            List<PropertyInfo> properties = clients.FirstOrDefault().GetType().GetProperties().ToList();

            app.Documents.Add();
            UpdateCurrentDoc();
            Range endRange = currentDoc.Range(currentDoc.Content.End - 1, currentDoc.Content.End - 1);
            endRange.InsertXML(tempXML);
            startPosition = 0;
            header = MapHeader(office, department);
            DeployHeader(header);
            tempXML = CopyRangeToWordXML(currentDoc.Range());
            //List<HeaderFooter> headers = CopyHeaders();
            //List<HeaderFooter> footers = CopyFooters();
            endPosition = currentDoc.Content.End - 1;
            foreach (XLMain.FPIClient client in clients)
            {
                //make the start position the previous end position (less a few to catch all) unless new document
                if (forceNewDocument)
                {
                    app.Documents.Add();
                    UpdateCurrentDoc();
                    endRange = currentDoc.Range();
                }
                else
                {
                    if (endPosition > letterLength)
                    {
                        startPosition = endPosition - letterLength;
                    }
                    else
                    {
                        startPosition = 0;
                    }
                    currentDoc.Words.Last.InsertBreak(WdBreakType.wdSectionBreakNextPage);
                    endRange = currentDoc.Range(currentDoc.Content.End - 1, currentDoc.Content.End - 1);
                }
                
                endRange.InsertXML(tempXML);
                DeployHeader(header);
                endPosition = currentDoc.Content.End - 1;
                Range currentRange = currentDoc.Range(startPosition, endPosition);

                UpdateFieldsFromRange(currentRange, client, properties);

                if (asPdf)
                {
                    string pdf = CreatePdf();
                    AddHeadertoPDF(pdf, fileNamePref: client.clientcode, folderPref: saveLocationForPdf);
                    currentDoc.Close();
                }
            }
        }

        /// <summary>
        /// Takes the range and copies the data into an xml string for use later on
        /// </summary>
        /// <param name="range">The range of the document you want a copy of</param>
        /// <returns>A string of XML format</returns>
        public static string CopyRangeToWordXML(Range range)
        {
            string xml = range.WordOpenXML;
            return xml;
        }

        /// <summary>
        /// Given a range it will merge any properties of the object which match the names of the fields in the document
        /// </summary>
        /// <param name="currentRange">The range in the document you want the method to act on</param>
        /// <param name="client">The object you want to take the data from</param>
        /// <param name="properties">The properties of the object, if null will ascertain from the object</param>
        public static void UpdateFieldsFromRange(Range currentRange, object client, List<PropertyInfo> properties = null)
        {
            if (properties == null)
            {
                properties = client.GetType().GetProperties().ToList();
            }
            Fields fields = currentRange.Fields;
            foreach (Section section in currentRange.Sections)
            {
                HeadersFooters headers = section.Headers;
                foreach (HeaderFooter sectionHeader in headers)
                {
                    foreach (Field field in sectionHeader.Range.Fields)
                    {
                        UpdateMergeField(client, field, properties);
                    }
                }
                HeadersFooters footers = section.Footers;
                foreach (HeaderFooter sectionFooter in footers)
                {
                    foreach (Field field in sectionFooter.Range.Fields)
                    {
                        UpdateMergeField(client, field, properties);
                    }
                }
            }
            foreach (Field field in fields)
            {
                UpdateMergeField(client, field, properties);
            }
        }

        /// <summary>
        /// Updates a specific field with data from the object
        /// </summary>
        /// <param name="client">The object you want the data to come from</param>
        /// <param name="field">The field to be updated</param>
        /// <param name="properties">A list of the properties of the object, if null it will ascertain</param>
        public static void UpdateMergeField(object client, Field field, List<PropertyInfo> properties = null)
        {
            if (properties == null)
            {
                properties = client.GetType().GetProperties().ToList();
            }
            MergeField mergeField = GetMergeFieldData(field);
            if (mergeField.Name != "")
            {
                foreach (var prop in properties)
                {
                    if (prop.Name.ToUpper() == mergeField.Name.ToUpper())
                    {
                        try
                        {
                            field.Select();
                            if (prop.PropertyType.Name == "Decimal")
                            {
                                decimal value = Decimal.Parse(prop.GetValue(client, null).ToString());
                                app.Selection.TypeText(String.Format("{0:0.00}", value));
                            }
                            else if (mergeField.Type.ToUpper() == "IMAGE")
                            {
                                app.Selection.TypeText(" ");
                                string imageLocation = prop.GetValue(client, null).ToString();
                                Range range = app.Selection.Range;
                                Microsoft.Office.Interop.Word.InlineShape shape = range.InlineShapes.AddPicture(imageLocation);
                                shape.LockAspectRatio = MsoTriState.msoTrue;
                                if (mergeField.Parameters.TryGetValue("height", out string sHeight))
                                {
                                    try
                                    {
                                        shape.Height = float.Parse(sHeight);
                                    }
                                    catch
                                    {
                                        throw new Exception("Height was not an float value");
                                    }
                                }
                            }
                            else
                            {
                                app.Selection.TypeText(prop.GetValue(client, null).ToString());
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ascertains the "name" part of the field stripping out extraneous data
        /// </summary>
        /// <param name="fieldText"></param>
        /// <returns></returns>
        private static MergeField GetMergeFieldData(Field field)
        {
            string fieldName = "";
            Range rngFieldCode = field.Code;
            String fieldText = rngFieldCode.Text;

            if (fieldText.Contains(" MERGEFIELD"))
            {
                Int32 endMerge = fieldText.IndexOf("\\");
                if (endMerge > 0)
                {
                    Int32 fieldNameLength = fieldText.Length - endMerge;
                    fieldName = fieldText.Substring(11, endMerge - 11);
                }
                else
                {
                    fieldName = fieldText.Substring(11);
                }
                fieldName = fieldName.Trim();
            }
            if (fieldName.Contains("@"))
            {
                MergeField mergeField = new MergeField();
                string[] parts = fieldName.Split('@');
                mergeField.Name = parts[0].Trim();
                string[] paramaters = parts[1].Split(';');
                foreach (string p in paramaters)
                {
                    string[] details = p.Split('=');
                    mergeField.Parameters.Add(details[0].Trim().ToLower(), details[1].Trim().ToLower());
                    if (details[0].Trim().ToUpper() == "TYPE")
                    {
                        mergeField.Type = details[1].Trim();
                    }
                }
                return mergeField;
            }
            else
            {
                MergeField mergeField = new MergeField()
                {
                    Name = fieldName
                };
                return mergeField;
            }
        }

        public static List<Tuple<string, string>> GetAttachmentFiles()
        {
            List<Tuple<string, string>> docList = new List<Tuple<string, string>>();
            IEnumerable<XElement> xAttachments = settingsDoc.Descendants("Attachments");
            foreach (XElement xAttachment in xAttachments.Descendants("Document"))
            {
                Tuple<string, string> document = Tuple.Create(xAttachment.Attribute("Description").Value, xAttachment.Attribute("Location").Value);
                docList.Add(document);
            }
            return docList;
        }
    }
}
