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


namespace XlantWord
{
    class XLDocument
    {
        public class Header
        {
            public string description { get; set; }
            public string firmname { get; set; }
            public int headerRightIndent { get; set; }
            public int headerLeftIndent { get; set; }
            public int footerRightIndent { get; set; }
            public int footerLeftIndent { get; set; }
            public List<Line> headerLines { get; set; }
            public List<Line> footerLines { get; set; }
        }

        public class Line
        {
            public string text { get; set; }
            public string font { get; set; }
            public int size { get; set; }
        }

        public class ListItem
        {
            public string name { get; set; }
            public string dbTag { get; set; }

            public ListItem(string newName, string newTag)
            {
                name = newName;
                dbTag = newTag;
            }
        }

        private static XDocument settingsDoc = XLtools.settingsDoc;
        public static Document currentDoc;
        public static Microsoft.Office.Interop.Word.View currentView;
        private static Microsoft.Office.Interop.Word.Application app = Globals.ThisAddIn.Application;

        //########################The following declarations and methods are different in .NET 3.5######################//
        
        private static Range CurrentRange()
        {
            //reference to Global replaced with app
            Microsoft.Office.Interop.Word.Range CurrRange = Globals.ThisAddIn.Application.Selection.Range;
            return CurrRange;
        }

        private static Document OpenDoc(string name, bool readOnly, bool recent = true, bool visible = true)
        {
            Document NewDoc = new Document();
            //reference to Global replaced with app
            NewDoc = Globals.ThisAddIn.Application.Documents.Open(name, ReadOnly: readOnly, AddToRecentFiles: recent, Visible:visible); 
            return NewDoc;
        }

        public static void UpdateCurrentDoc()
        {
            currentDoc = Globals.ThisAddIn.Application.ActiveDocument;
        }

        public static Document GetCurrentDoc()
        {
            currentDoc = Globals.ThisAddIn.Application.ActiveDocument;
            return currentDoc;
        }

        public static void UpdateCurrentView()
        {
            currentView = Globals.ThisAddIn.Application.ActiveWindow.View;
        }
        
        //###################After this point all code is identical in both versions############################//

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

        public static string TempSave(string fileType = ".docx")
        {
            try
            {
                XLDocument.UpdateCurrentDoc();
                string location;
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\XLant\\temp\\";

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                Random rnd = new Random();
                string filename = DateTime.Now.ToString("yyyy-MM-dd");
                int id = rnd.Next(1000, 9999); //provides a four digit id
                filename += "-" + id.ToString();
                filename += fileType;
                //If that file already exists try again until it doesn't
                while (File.Exists(folder + filename))
                {
                    filename = DateTime.Now.ToString("yyyy-MM-dd");
                    id = rnd.Next(1000, 9999); //provides a four digit id
                    filename += "-" + id.ToString();
                    filename += fileType;
                }
                location = folder + filename;
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

        public static void UpdateBookmark(string bName, string bValue, int bold=2, string styleName="") 
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

        public static string CheckBookmark(string defaultString, string bName, int bold=2)
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
                List<Line> hLines = new List<Line>();
                List<Line> fLines = new List<Line>();

                IEnumerable<XElement> xHeadings = settingsDoc.Descendants("Headings");
                
                foreach (XElement xHeading in xHeadings.Descendants("Heading"))
                {
                    Header header = new Header();
                    header.description = xHeading.Attribute("Description").Value;
                    header.firmname = xHeading.Attribute("Firm").Value;
                    foreach (XElement xLines in xHeading.Descendants("Lines"))
                    {
                        if (xLines.Attribute("Type").Value == "Header")
                        {
                            foreach (XElement xLine in xHeading.Descendants("HeaderLine"))
                            {
                                Line line = new Line();
                                line.text = xLine.Value;
                                line.size = xLine.AttributeIntNull("Size");
                                line.font = xLine.AttributeValueNull("Font");
                                hLines.Add(line);
                            }
                        }
                        //It is only either a header or a footer
                        else
                        {
                            header.footerRightIndent = 150;
                            foreach (XElement xLine in xHeading.Descendants("FooterLine"))
                            {
                                Line line = new Line();
                                line.text = xLine.Value;
                                line.size = xLine.AttributeIntNull("Size");
                                line.font = xLine.AttributeValueNull("Font");
                                fLines.Add(line);
                            }
                        }
                        header.headerLines = hLines;
                        header.footerLines = fLines;
                    }
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
                List<Line> hLines = new List<Line>();
                List<Line> fLines = new List<Line>();
            
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
                    selectedHeader.description = foundHeader.Attribute("Description").Value;
                    selectedHeader.firmname = foundHeader.Attribute("Firm").Value;
                    foreach (XElement xLines in foundHeader.Descendants("Lines"))
                    {
                        if (xLines.Attribute("Type").Value == "Header")
                        {
                            foreach (XElement xLine in foundHeader.Descendants("HeaderLine"))
                            {
                                Line line = new Line();
                                line.text = xLine.Value;
                                line.size = xLine.AttributeIntNull("Size");
                                line.font = xLine.AttributeValueNull("Font");
                                hLines.Add(line);
                            }
                        }
                        //It is only either a header or a footer
                        else
                        {
                            selectedHeader.footerRightIndent = xLines.AttributeIntNull("RightIndent");
                            foreach (XElement xLine in foundHeader.Descendants("FooterLine"))
                            {
                                Line line = new Line();
                                line.text = xLine.Value;
                                line.size = xLine.AttributeIntNull("Size");
                                line.font = xLine.AttributeValueNull("Font");
                                fLines.Add(line);
                            }
                        }
                        selectedHeader.headerLines = hLines;
                        selectedHeader.footerLines = fLines;
                    }
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
                List<Line> hLines = new List<Line>();
                List<Line> fLines = new List<Line>();

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
                    selectedHeader.description = foundHeader.Attribute("Description").Value;
                    selectedHeader.firmname = foundHeader.Attribute("Firm").Value;
                    foreach (XElement xLines in foundHeader.Descendants("Lines"))
                    {
                        if (xLines.Attribute("Type").Value == "Header")
                        {
                            foreach (XElement xLine in foundHeader.Descendants("HeaderLine"))
                            {
                                Line line = new Line();
                                line.text = xLine.Value;
                                line.size = xLine.AttributeIntNull("Size");
                                line.font = xLine.AttributeValueNull("Font");
                                hLines.Add(line);
                            }
                        }
                        //It is only either a header or a footer
                        else
                        {
                            selectedHeader.footerRightIndent = xLines.AttributeIntNull("RightIndent");
                            foreach (XElement xLine in foundHeader.Descendants("FooterLine"))
                            {
                                Line line = new Line();
                                line.text = xLine.Value;
                                line.size = xLine.AttributeIntNull("Size");
                                line.font = xLine.AttributeValueNull("Font");
                                fLines.Add(line);
                            }
                        }
                        selectedHeader.headerLines = hLines;
                        selectedHeader.footerLines = fLines;
                    }
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
                rng.ParagraphFormat.RightIndent = header.headerRightIndent;
                rng.ParagraphFormat.LeftIndent = header.headerLeftIndent;
                //Then build the new one line by line
                foreach (Line line in header.headerLines)
                {
                    para = currentDoc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterFirstPage].Range.Paragraphs.Add();
                    para.Range.Font.Name = line.font;
                    para.Range.Font.Size = line.size;
                    para.Range.Text = line.text + Environment.NewLine;
                }
                //Then look for bolds
                //reset the range to get the whole header/footer
                rng = currentDoc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterFirstPage].Range;
                FormatRange(rng);

            
                //Setting First page footer
                //First delete and existing footer
                rng = currentDoc.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterFirstPage].Range;
                rng.Delete();
                rng.ParagraphFormat.RightIndent = header.footerRightIndent;
                rng.ParagraphFormat.LeftIndent = header.footerLeftIndent;
                //Then build the new one line by line
                foreach (Line line in header.footerLines)
                {
                    para = currentDoc.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterFirstPage].Range.Paragraphs.Add();
                    para.Range.Font.Name = line.font;
                    para.Range.Font.Size = line.size;
                    para.Range.Text = line.text + Environment.NewLine;
                }
                //Then look for bolds
                //reset the range to get the whole header/footer
                rng = currentDoc.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterFirstPage].Range;
                FormatRange(rng);
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
                CurrRange.Text=text;
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
                    textBox.Line.Visible = 0;
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
                Document sTemplate = OpenDoc(path, true, false, false );
                //object template = (object)path;
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
                int len = fileID.IndexOf("-"); //get the index of the version identifier
                fileID = fileID.Substring(0, len);//ignore everything after and including the -
                UpdateParameter("FileID", fileID); //add the parameter for next time.

                return fileID;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to discover file ID");
                XLtools.LogException("GetFileID", e.ToString());
                return null;
            }
        }

        public static void FormatRange(Range rng)
        {
            try
            {
                Boolean nextWordBold = false;
                foreach (Range word in rng.Words)
                {
                    //First decide whether this word should be bold
                    if (word.Text.Trim().Equals("|", StringComparison.CurrentCultureIgnoreCase))
                    {
                        word.Delete(WdUnits.wdWord, 1);
                    }
                    else if (nextWordBold)
                    {
                        word.Font.Bold = 1;
                        nextWordBold = false;
                    }
                    else
                    {

                        if (word.Text.Trim().Equals("NextWordBold", StringComparison.CurrentCultureIgnoreCase))
                        {
                            word.Delete(WdUnits.wdWord, 1);
                            nextWordBold = true;
                        }
                        else if (word.Text.Trim().Equals("MLBlob", StringComparison.CurrentCultureIgnoreCase))
                        {
                            word.Delete(WdUnits.wdWord, 1);
                            word.InsertSymbol(183, Unicode: true);
                            word.MoveEnd(WdUnits.wdCharacter, 1);
                            word.InsertAfter(" ");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to format range");
                XLtools.LogException("FormatRange", rng.Text + "-" + e.ToString());
            }
        }

        public static string CreatePdf()
        {
            try
            {
                string filestring = "";
                //use tempsave to create a pdf file and return location.
                filestring = TempSave(".pdf");

                return filestring;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to create a pdf document");
                XLtools.LogException("CreatePDf", e.ToString());
                return null;
            }
        }

        public static string AddHeadertoPDF(string filestring)
        {
            try
            {
                //find the watermark pdf
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\XLant\\";
                string watermarkLocation = folder + "Watermark.pdf";
                //create a filelocation for the new file
                string filelocation = folder;
                Random rnd = new Random();
                string filename = DateTime.Now.ToString("yyyy-MM-dd");
                int id = rnd.Next(1000, 9999); //provides a four digit id
                filename += "-" + id.ToString();
                filename += ".pdf";
                //If that file already exists try again until it doesn't
                while (File.Exists(folder + filename))
                {
                    filename = DateTime.Now.ToString("yyyy-MM-dd");
                    id = rnd.Next(1000, 9999); //provides a four digit id
                    filename += "-" + id.ToString();
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

        public static void IndexPDFCopy(string filestring, string origFileID)
        {
            try
            {
                //get the info of the original file
                XLVirtualCabinet.FileInfo fileInfo = XLVirtualCabinet.FileIndex(origFileID);
                //update the description (index03) to include the PDF suffix
                foreach (XLVirtualCabinet.IndexPair pair in fileInfo.Indexes)
                {
                    //alter description
                    if (pair.index == "INDEX03")
                    {
                        pair.value = pair.value + " - PDF";
                    }
                }
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
        public static void MergeFPIData(List<XLMain.FPIClient> clients, string templateXML = null)
        {
            UpdateCurrentDoc();
            string office = "";
            string tempXML = "";
            Header header = new Header();
            long startPosition = currentDoc.Content.Start;
            long endPosition = currentDoc.Content.End;
            if(templateXML == null)
            {
                tempXML = CopyRangeToWordXML(currentDoc.Range());
            }
            else
            {
                tempXML = templateXML;
            }
                       
            long letterLength = endPosition - startPosition;
            
            List<PropertyInfo> properties = clients.FirstOrDefault().GetType().GetProperties().ToList();
            
            foreach(XLMain.FPIClient client in clients)
            {
                if (client.office.ToUpper() != office.ToUpper())
                {
                    //create a new document and set up the headers
                    app.Documents.Add();
                    UpdateCurrentDoc();
                    startPosition = 0;
                    Range endRange = currentDoc.Range(currentDoc.Content.End - 1, currentDoc.Content.End - 1);
                    endRange.InsertXML(tempXML);
                    header = MapHeader(client.office, "GPB");
                    DeployHeader(header);
                    tempXML = CopyRangeToWordXML(currentDoc.Range());
                    endPosition = currentDoc.Content.End - 1;
                    office = client.office;
                }
                else
                {
                    //make the start position the previous end position (less a few to catch all)
                    if (endPosition > letterLength)
                    {
                        startPosition = endPosition - letterLength;
                    }
                    else
                    {
                        startPosition = 0;
                    }
                    currentDoc.Words.Last.InsertBreak(WdBreakType.wdSectionBreakNextPage);
                    Range endRange = currentDoc.Range(currentDoc.Content.End - 1, currentDoc.Content.End - 1);
                    endRange.InsertXML(tempXML);
                    endPosition = currentDoc.Content.End-1;
                }
                Range currentRange = currentDoc.Range(startPosition, endPosition);

                UpdateFieldsFromRange(currentRange, client, properties);
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
        public static void UpdateFieldsFromRange(Range currentRange, object client, List<PropertyInfo> properties= null)
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
            string fieldName = GetMergeFieldName(field);
            if (fieldName != "")
            {
                foreach (var prop in properties)
                {
                    if (prop.Name.ToUpper() == fieldName.ToUpper())
                    {
                        try
                        {
                            field.Select();
                            if (prop.PropertyType.Name == "Decimal")
                            {
                                decimal value = Decimal.Parse(prop.GetValue(client, null).ToString());
                                app.Selection.TypeText(String.Format("{0:0.00}", value));
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
        private static string GetMergeFieldName(Field field)
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

            return fieldName;
        }
    }
}
