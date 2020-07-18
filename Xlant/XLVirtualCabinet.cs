using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace XLantCore
{
    public class XLVirtualCabinet
    {
       public static string exeLocation { get; set; }

       public class VCrecord
       {
           //has to be a string to cope with headers, converted to int at runtime
           public string FileId;
           public string IndexData;
       }

       public class BondResult
       {
            public int ExitCode = -1;
            public string StandardOutput = String.Empty;
            public string CommandLine = String.Empty;
            public string DocPath = string.Empty;
       }

       public class IndexList
       {
           public string name;
           public string index;
           public List<string> items;
       }

        public class IndexPair
        {
            public string index { get; set; }
            public string value { get; set; }
        }

        public class FileInfo
        {
            public string FileID {get; set;}
            public string Cabinet {get; set;}
            public DateTime EntryDate { get; set; }
            public string ClientCode { get; set; }
            public string ClientString { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public string ToBeActionedBy { get; set; }
            public string Extension { get; set; }
            public List<IndexPair> Indexes {get; set;}
        }

        private static string CabiLocation()
        {
            try
            {
                // Check the rigistry for the location of the Cabibond.Net.Exe
                string strExePath = string.Empty;

                Microsoft.Win32.RegistryKey REGKEY1;
                REGKEY1 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Lindenhouse", false);
            
                if (REGKEY1 != null)
                {
                    try
                    {
                        strExePath = REGKEY1.GetValue("IntegrationPath", "DOESNOTEXIST").ToString();
                    }
                    catch 
                    {
                        strExePath = string.Empty;
                    }
                }

                if (!File.Exists(strExePath))
                {
                    Microsoft.Win32.RegistryKey REGKEY2;
                    REGKEY2 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Lindenhouse", false);
                    if (REGKEY2 != null)
                    {
                        strExePath = REGKEY1.GetValue("IntegrationPath2", "DOESNOTEXIST").ToString();
                    }
               //If unable to obtain from registry try the usual program files directories.
                    if (!File.Exists(strExePath))
                    {
                        strExePath = Environment.GetEnvironmentVariable("ProgramFiles") + "\\Lindenhouse Software Ltd\\CabiBond.Net\\CabiBond.Net.exe";

                        if (!File.Exists(strExePath))
                        {
                            strExePath = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + "\\Lindenhouse Software Ltd\\CabiBond.Net\\CabiBond.Net.exe";
                        }
                    }
                }
                return strExePath;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-CabiLocation", ex.ToString());
                return null;
            }
        }

        public static BondResult IndexDocument(string docPath, string cabinet = "", string client = "", string status = "", string sender = "", string section = "", string desc = "", string docDate = null, IndexPair additionalIndex1 = null, IndexPair additionalIndex2 = null)
        {
            try
            {
                if (docDate == null)
                {
                    docDate = DateTime.Now.ToString("dd/MM/yyyy");
                }
                string commandFile = BuildCommandFile(docPath, cabinet, client, status, sender, section, desc, docDate, additionalIndex1, additionalIndex2);
                BondResult result = LaunchCabi(commandFile, false);
                result.DocPath = docPath;
                return result;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-IndexDocument", ex.ToString());
                return new BondResult();
            }
        }

        public static BondResult IndexDocument(string docPath, FileInfo fileInfo)
        {
            try
            {
                string commandFile = BuildCommandFile(docPath, fileInfo);
                BondResult result = LaunchCabi(commandFile, false);
                result.DocPath = docPath;
                return result;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-IndexDocument", ex.ToString());
                return new BondResult();
            }
        }
        
        public static BondResult LaunchCabi(string commandFile, Boolean reindex, Boolean silent = false)
        {
            try
            {
                BondResult result = new BondResult();
                //see if cabibond location is already populted and if not do so.
                exeLocation = CabiLocation();
            
                //Build the command file with the relevant options
                if (File.Exists(commandFile))
                {
                    //configure the execution
                    Process p = new Process();
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.FileName = exeLocation;
                    string args = "";
                    if (silent)
                    {
                        args = args + "-s ";
                    }
                    if (reindex)
                    {
                        args = args + "UPDATEDB " + commandFile;
                    }
                    else
                    {
                        args = args + commandFile;
                    }
                    
                    p.StartInfo.Arguments = args;
                    //start
                    p.Start();


                    //wait for output and return result
                    p.WaitForExit();
                    result.ExitCode = p.ExitCode;
                    result.StandardOutput = p.StandardOutput.ReadToEnd();
                    result.CommandLine = p.StartInfo.FileName.ToString() + " " + p.StartInfo.Arguments.ToString();
                }
                else
                {
                    result.ExitCode = -1;
                    result.StandardOutput = "Unable to find command file";
                
                }
                return result;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-LaunchCabi", ex.ToString());
                return new BondResult();
            }
        }

        private static string BuildCommandFile(string docPath, string cabinet = "", string client = "", string status = "", string sender = "", string section = "", string desc = "", string docDate = null, IndexPair additionalIndex1 = null, IndexPair additionalIndex2 = null)
        {
            try
            {
                if (docDate == null)
                {
                    docDate = DateTime.Now.ToString("dd/MM/yyyy");
                }

                //blanked in an attempt to handle the weird random saving
                string tempPath = "";
                tempPath = XLtools.TempPath();
                        
                string commandFileLoc = tempPath + "VC.command";

                string statusIndex = "";
                string toBeIndex = "";
                XDocument settingsDoc = XLtools.settingsDoc;
                //query the setting files and try to find a match
                XElement setting = (from index in settingsDoc.Descendants("Indexes")
                                    select index).FirstOrDefault();
                foreach (XElement xIndex in setting.Descendants("Index"))
                {
                    if (xIndex.AttributeValueNull("Type") == "Status")
                    {
                        statusIndex = xIndex.Value;
                    }
                    if (xIndex.AttributeValueNull("Type") == "ToBe")
                    {
                        toBeIndex = xIndex.Value;
                    }
                }

                //Create stream and add to file.
                StreamWriter sw = new StreamWriter(commandFileLoc, false, System.Text.Encoding.Default);
                sw.WriteLine("<<MODE=FILE>>");
                sw.WriteLine("<<FILE=" + docPath + ">>");
                sw.WriteLine("<<LEVEL01=" + cabinet + ">>");
                sw.WriteLine("<<INDEX02=" + client + ">>");
                sw.WriteLine("<<INDEX03=" + desc + ">>");
                sw.WriteLine("<<INDEX09=" + section + ">>");
                sw.WriteLine("<<INDEX20=" + docDate + ">>");
                sw.WriteLine("<<INDEX" + statusIndex + "=" + status + ">>");
                sw.WriteLine("<<INDEX" + toBeIndex + "=" + sender + ">>");
                if (additionalIndex1 != null)
                {
                    sw.WriteLine("<<" + additionalIndex1.index + "=" + additionalIndex1.value + ">>");
                }
                if (additionalIndex2 != null)
                {
                    sw.WriteLine("<<" + additionalIndex2.index + "=" + additionalIndex2.value + ">>");
                }
                sw.Flush();
                sw.Close();

                //return the location of our new command file.
                return commandFileLoc;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-BuildCommandFile", ex.ToString());
                return null;
            }
        }

        private static string BuildCommandFile(string docPath, FileInfo fileInfo)
        {
            try
            {
                //blanked in an attempt to handle the weird random saving
                string tempPath = "";
                tempPath = XLtools.TempPath();

                string commandFileLoc = tempPath + "VC.command";

                //Create stream and add to file.
                StreamWriter sw = new StreamWriter(commandFileLoc, false, System.Text.Encoding.Default);
                sw.WriteLine("<<MODE=FILE>>");
                sw.WriteLine("<<FILE=" + docPath + ">>");
                sw.WriteLine("<<LEVEL01=" + fileInfo.Cabinet + ">>");
                foreach (IndexPair i in fileInfo.Indexes)
                {
                    sw.WriteLine("<<" + i.index + "=" + i.value + ">>");
                }
                sw.Flush();
                sw.Close();

                //return the location of our new command file.
                return commandFileLoc;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-BuildCommandFile with FileInfo", ex.ToString());
                return null;
            }
        }

        public static String Reindex(string fileID, string sender="", string status = null, string docDate = null)
        {
            try
            {
                //get temp path and create a file to fill with our commands
                string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\XLant\\temp\\";
            
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);
                        
                string commandFileLoc = tempPath + "VC.command";
                string statusIndex = "";
                string toBeIndex = "";
            
                XDocument settingsDoc = XLtools.settingsDoc;
                IEnumerable<XElement> xIndexes = settingsDoc.Descendants("Indexes");

                foreach (XElement xIndex in xIndexes.Descendants("Index"))
                {
                    if (xIndex.AttributeValueNull("Type") == "Status")
                    {
                        statusIndex = xIndex.ElementValueNull();
                    }
                    else if (xIndex.AttributeValueNull("Type") == "ToBe")
                    {
                        toBeIndex = xIndex.ElementValueNull();
                    }
                }

                StreamWriter sw = new StreamWriter(commandFileLoc, false, System.Text.Encoding.Default);
                string text = "<<SET INDEX" + toBeIndex + "='" + sender + "'";
                if (docDate != null)
                {
                    text +=  ", INDEX20='" + docDate + "'";
                }
                if (status != null)
                {
                    text += ", INDEX" + statusIndex + "='" + status + "'";
                }

                text += " WHERE INDEX01='" + fileID + "'>>";
                sw.WriteLine(text);
            
            
                sw.Flush();
                sw.Close();

               return commandFileLoc;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-Reindex", ex.ToString());
                return null;
            }
        }

        public static String UpdateIndexField(string fileID, string index, string newData)
        {
            try
            {
            
                //get temp path and create a file to fill with our commands
                string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\XLant\\temp\\";

                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);

                string commandFileLoc = tempPath + "VC.command";

                StreamWriter sw = new StreamWriter(commandFileLoc, false, System.Text.Encoding.Default);
                string text = "<<SET " + index + "='" + newData + "'";
                text += " WHERE INDEX01='" + fileID + "'>>";
                sw.WriteLine(text);
            
            
                sw.Flush();
                sw.Close();

               return commandFileLoc;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-UpdateIndexField", ex.ToString());
                return null;
            }
        }

        public static string FileStore(string office, string department)
        {
            try
            {
                string fileStore = "";

                //query the setting files and try to find a match
                XElement selectedMap = (from map in XLtools.settingsDoc.Descendants("Map")
                                        where (string)map.Attribute("Office") == office && (string)map.Attribute("Department") == department
                                        select map).FirstOrDefault();
                if (selectedMap != null)
                {
                    fileStore = selectedMap.Attribute("FileStore").Value;
                }
                return fileStore;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-FileStore", ex.ToString());
                return null;
            }
        }

        public static string DefaultIndex(string department)
        {
            try
            {
                string index = null;
                XElement foundDept = (from xelement in XLtools.settingsDoc.Descendants("VCIndex")
                                      where (string)xelement.Attribute("Department").Value == department
                                      select xelement).FirstOrDefault();
                if (foundDept.Value == null)
                {
                    index = "Correspondence";
                }
                else
                {
                    index = foundDept.Value;
                }
                return index;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-DefalutIndex", ex.ToString());
                return null;
            }
        }

        public static List<string> SectionValues(string office, string department)
        {
            try
            {
                List<string> sections = new List<string>();
                string fileStore = FileStore(office, department);
                DataTable xlReader = XLSQL.ReturnTable("SELECT Distinct SectionValue FROM [XLant].[dbo].[VCSectionValuesView] where CabinetName='" + fileStore + "' order by SectionValue");
                if (xlReader.Rows.Count != 0)
                {
                    foreach (DataRow row in xlReader.Rows)
                    {
                        sections.Add(row["SectionValue"].ToString());
                    }
                }
                return sections;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-SectionValues", ex.ToString());
                return null;
            }
        }

        public static List<IndexList> SectionLists(string office, string department, string sectionValue)
        {
            try
            {
                string fileStore = FileStore(office, department);
                DataTable xlReader = XLSQL.ReturnTable("SELECT Label, ListId, indexno FROM [XLant].[dbo].[VCSectionValuesView] where CabinetName='" + fileStore + "' and SectionValue='" + sectionValue + "' and label is not null order by Listid");
                List<IndexList> lists = new List<IndexList>();
                if (xlReader.Rows.Count != 0)
                {
                    foreach (DataRow row in xlReader.Rows)
                    {
                        IndexList iList = new IndexList();
                        iList.name = row["Label"].ToString();
                        iList.index = row["IndexNo"].ToString();
                        DataTable listReader = XLSQL.ReturnTable("SELECT Value FROM [XLant].[dbo].[VCListsView] where ListId='" + row["ListId"].ToString() + "' order by Value");
                        List<string> list = new List<string>();
                        if (listReader.Rows.Count != 0)
                        {
                            foreach (DataRow r in listReader.Rows)
                            {
                                list.Add(r["Value"].ToString());
                            }
                        }
                        iList.items = list;
                        lists.Add(iList);
                    }
                }
                return lists;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-SectionLists", ex.ToString());
                return null;
            }
        } 

        public static FileInfo FileIndex(string fileID, bool current = true)
        {
            try
            {

                FileInfo file = new FileInfo();
                if (fileID != null)
                {
                    DataTable xlReader = new DataTable();
                    if (current)
                    {
                        xlReader = XLSQL.ReturnTable("Select TOP(1) * from [XLant].[dbo].[VCFileIndexView] where fileID = '" + fileID + "'");
                    }
                    else
                    {
                        //if not current, returns the last set of audit data
                        xlReader = XLSQL.ReturnTable("select TOP(1)* from VCAuditLog where fileid = '" + fileID + "' and index01 is null order by auditId desc");

                    }

                    string statusIndex = "";
                    string toBeIndex = "";

                    XDocument settingsDoc = XLtools.settingsDoc;
                    IEnumerable<XElement> xIndexes = settingsDoc.Descendants("Indexes");

                    foreach (XElement xIndex in xIndexes.Descendants("Index"))
                    {
                        if (xIndex.AttributeValueNull("Type") == "Status")
                        {
                            statusIndex = xIndex.ElementValueNull();
                        }
                        else if (xIndex.AttributeValueNull("Type") == "ToBe")
                        {
                            toBeIndex = xIndex.ElementValueNull();
                        }
                    }
                    
                    file.FileID = fileID;
                    file.Cabinet = xlReader.Rows[0]["FolderName"].ToString();
                    file.ToBeActionedBy = xlReader.Rows[0]["INDEX" + toBeIndex].ToString();
                    file.Status = xlReader.Rows[0]["INDEX" + statusIndex].ToString();
                    file.Extension = xlReader.Rows[0]["Extension"].ToString();
                    string indexNumber = "";
                    List<IndexPair> list = new List<IndexPair>();
                    for (int i = 1; i < 51; i++)
                    {
                        //add the 0 where required. Index09 is the section name and is handled differently
                        try
                        {
                            if (i < 9)
                            {
                                indexNumber = "INDEX0" + i.ToString(); 
                            }
                            else if (i==9)
                            {
                                IndexPair index = new IndexPair();
                                index.index = "INDEX09";
                                index.value = xlReader.Rows[0]["SectionName"].ToString();
                                list.Add(index);
                            }
                            else
                            {
                                indexNumber = "INDEX" + i.ToString();
                            }
                        
                            string value = xlReader.Rows[0][indexNumber].ToString();
                            if (!String.IsNullOrEmpty(value))
                            {
                                IndexPair index = new IndexPair();
                                index.index = indexNumber;
                                index.value = value;
                                list.Add(index);
                            }
                        }
                        catch 
                        {
                            continue;
                        }
                    }
                    file.Indexes = list;
                    foreach (IndexPair i in file.Indexes)
                    {
                        //grab the client string and code
                        if (i.index == "INDEX02")
                        {
                            file.ClientString = i.value;
                            file.ClientCode = i.value.Substring(0, i.value.IndexOf("-") - 1);
                        }
                        //and the description
                        if (i.index == "INDEX03")
                        {
                            file.Description = i.value;
                        }
                    }
                    
                }
                return file;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-FileIndex", ex.ToString());
                return null;
            }
        }

        public static XLMain.Client GetClientFromIndex(string fileID)
        {
            try
            {
                FileInfo info = FileIndex(fileID);
                string clientStr = "";
                foreach (IndexPair pair in info.Indexes)
                {
                    if (pair.index == "INDEX02")
                    {
                        clientStr = pair.value;
                        //once we have found what we want we need not look at other indexes
                        break;
                    }
                }
                clientStr = clientStr.Substring(0, clientStr.IndexOf("-"));
                XLMain.Client client = new XLMain.Client();
                client = XLMain.Client.FetchClientFromCode(clientStr.TrimEnd());
                return client;
            }
            catch (Exception ex)
            {
                XLtools.LogException("XLVC-ClientInfofromIndex", ex.ToString());
                return null;
            }
        }

        public static List<FileInfo> GetToDos(string userName)
        {
            try
            {
                //Run query against database
                List<FileInfo> newToDos = new List<FileInfo>();
                string description = "";
                string status = "";
                string toBeIndex = "";
                //discover the to be actioned by index
                XDocument settingsDoc = XLtools.settingsDoc;
                //query the setting files and try to find a match
                XElement setting = (from index in settingsDoc.Descendants("Indexes")
                                    select index).FirstOrDefault();
                foreach (XElement xIndex in setting.Descendants("Index"))
                {
                    if (xIndex.AttributeValueNull("Type") == "ToBe")
                    {
                        toBeIndex = xIndex.Value;
                    }
                    if (xIndex.AttributeValueNull("Type") == "Description")
                    {
                        description = xIndex.Value;
                    }
                    if (xIndex.AttributeValueNull("Type") == "Status")
                    {
                        status = xIndex.Value;
                    }
                }
                
                string str = "select * from VCFileIndexView where index" + toBeIndex + " ='" + userName + "'";
                DataTable xlReader = XLSQL.ReturnTable(str);
                if (xlReader.Rows.Count != 0)
                {
                    foreach (DataRow row in xlReader.Rows)
                    {
                        FileInfo file = new FileInfo();
                        file = FileIndex(row["FileId"].ToString());
                        //file.FileID = row["FileId"].ToString();
                        //file.Cabinet = row["FolderName"].ToString();
                        //file.EntryDate = DateTime.Parse(row["EntryDate"].ToString());
                        //string indexNumber = "";
                        //List<IndexPair> list = new List<IndexPair>();
                        //for (int i = 1; i < 51; i++)
                        //{
                        //    //add the 0 where required. Index09 is the section name and is handled differently
                        //    try
                        //    {
                        //        if (i == int.Parse(description))
                        //        {
                        //            file.Description = xlReader.Rows[0][indexNumber].ToString();
                        //        }
                        //        if (i == int.Parse(status))
                        //        {
                        //            file.Status = xlReader.Rows[0][indexNumber].ToString();
                        //        }
                        //        if (i == int.Parse(toBeIndex))
                        //        {
                        //            file.ToBeActionedBy = xlReader.Rows[0][indexNumber].ToString();
                        //        }
                        //        if (i == 2)
                        //        {
                        //            file.ClientString = xlReader.Rows[0][indexNumber].ToString();
                        //        }
                        //        if (i < 9)
                        //        {
                        //            indexNumber = "INDEX0" + i.ToString();
                        //        }
                        //        else if (i == 9)
                        //        {
                        //            IndexPair index = new IndexPair();
                        //            index.index = "INDEX09";
                        //            index.value = xlReader.Rows[0]["SectionName"].ToString();
                        //            list.Add(index);
                        //        }
                        //        else
                        //        {
                        //            indexNumber = "Index" + i.ToString();
                        //        }

                        //        string value = xlReader.Rows[0][indexNumber].ToString();
                        //        if (!String.IsNullOrEmpty(value))
                        //        {
                        //            IndexPair index = new IndexPair();
                        //            index.index = indexNumber;
                        //            index.value = value;
                        //            list.Add(index);
                        //        }
                        //    }
                        //    catch
                        //    {
                        //        continue;
                        //    }
                        //}
                        //file.Indexes = list;
                        //string clientStr = file.ClientString;
                        //clientStr = clientStr.Substring(0, clientStr.IndexOf("-"));
                        //file.ClientCode = clientStr.TrimEnd();
                        newToDos.Add(file);
                    }
                }
                return newToDos;
                
            }
            catch (Exception e)
            {
                XLtools.LogException("XLant Monitor", e.ToString());
                return null;
            }
        }
    }
}
