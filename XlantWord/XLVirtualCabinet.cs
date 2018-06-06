using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using System.Xml.Linq;

namespace XlantWord
{
    class XLVirtualCabinet
    {
        public static string exeLocation { get; set; }

       public class BondResult
       {
            public int ExitCode = -1;
            public string StandardOutput = String.Empty;
            public string CommandLine = String.Empty;
       }

        private static string CabiLocation()
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

        public static BondResult IndexDocument(string cabinet = "", string client = "", string status = "", string sender = "", string section = "", string desc = "")
        {
            string commandFile = BuildCommandFile("file", cabinet, client, status, sender, section, desc);
            BondResult result = LaunchCabi(commandFile);
            return result;
        }
        
        public static BondResult LaunchCabi(string commandFile)
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
                p.StartInfo.Arguments = commandFile;
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

        private static string BuildCommandFile(string option, string cabinet="", string client="", string status="", string sender="", string section="", string desc="")
        {
            //get temp path and create a file to fill with our commands
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\XLant\\temp\\";
            
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
                        
            string commandFileLoc = tempPath + "VC.command";

            string statusIndex = "";
            string toBeIndex = "";
            XDocument settingsDoc = XLDocument.settingsDoc;
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

            if (option == "file")
            {

                Document currentDoc = Globals.ThisAddIn.Application.ActiveDocument;
                string docPath = currentDoc.FullName;

                //Create stream and add to file.
                StreamWriter sw = new StreamWriter(commandFileLoc, false, System.Text.Encoding.Default);
                sw.WriteLine("<<MODE=FILE>>");
                sw.WriteLine("<<FILE=" + docPath + ">>");
                sw.WriteLine("<<LEVEL01=" + cabinet + ">>");
                sw.WriteLine("<<INDEX02=" + client + ">>");
                sw.WriteLine("<<INDEX03=" + desc + ">>");
                sw.WriteLine("<<INDEX09=" + section + ">>");
                sw.WriteLine("<<INDEX20=" + DateTime.Now.ToString() + ">>");
                sw.WriteLine("<<INDEX" + statusIndex + "=" + status + ">>");
                sw.WriteLine("<<INDEX" + toBeIndex + "=" + sender + ">>");
                sw.Flush();
                sw.Close();
            }
            else if (option == "index")
            {
                //Not in use
                //FileInfo fi = new FileInfo(tempPath + "info.xml");
                //MessageBox.Show(tempPath + "info.xml");
                //// Create the Info command file
                //StreamWriter sw = new StreamWriter(tempPath + "info.command", false, System.Text.Encoding.Default);
                //sw.WriteLine("<<MODE=INFO>>");
                //sw.WriteLine("<<INFOPATH=" + tempPath + ">>");
                //sw.Flush();
                //sw.Close();
            }
            //return the location of our new command file.
            return commandFileLoc;
        }

        public static void ReindexStatus(string fileID, string status, string sender)
        {
            //get temp path and create a file to fill with our commands
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\XLant\\temp\\";
            
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
                        
            string commandFileLoc = tempPath + "VC.command";

            string statusIndex = "";
            XDocument settingsDoc = XLDocument.settingsDoc;
            //query the setting files and try to find a match
            XElement setting = (from index in settingsDoc.Descendants("Indexes")
                                where (string)index.Attribute("Type").Value == "Status"
                                select index).FirstOrDefault();
            if (setting != null)
            {
                statusIndex = setting.Value;
            }
            string toBeIndex = "";
            //query the setting files and try to find a match
            setting = (from index in settingsDoc.Descendants("Indexes")
                                where (string)index.Attribute("Type").Value == "Status"
                                select index).FirstOrDefault();
            if (setting != null)
            {
                toBeIndex = setting.Value;
            }
            
            StreamWriter sw = new StreamWriter(commandFileLoc, false, System.Text.Encoding.Default);
            sw.WriteLine("<<MODE=UPDATEDB>>");
            sw.WriteLine("<<SET INDEX" + statusIndex + "=" + status + ", INDEX" + toBeIndex + "=" + sender + " WHERE FILEID = " + fileID + ">>");
            sw.Flush();
            sw.Close();
        }

        public static string FileStore(string office, string department)
        {
            string fileStore = "";

            //query the setting files and try to find a match
            XElement selectedMap = (from map in XLDocument.settingsDoc.Descendants("Map")
                                    where (string)map.Attribute("Office") == office && (string)map.Attribute("Department") == department
                                    select map).FirstOrDefault();
            if (selectedMap != null)
            {
                fileStore = selectedMap.Attribute("FileStore").Value;
            }
            return fileStore;
        }

        public static string DefaultIndex(string department)
        {
            string index = null;
            XElement foundDept = (from xelement in XLDocument.settingsDoc.Descendants("VCIndex")
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
    }
}
