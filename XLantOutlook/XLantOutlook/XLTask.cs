using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using XLant;
using System.Windows.Forms;

namespace XLantOutlook
{
    class XLTask
    {
        private static Outlook.MAPIFolder ToDoFolder = null;
        private static string folderName = "XLant Tasks";
        public static string fileIdName = "VCIndex";
        private static Outlook.Items toDos = null;
        private static List<string> ids = new List<string>();
        private static XLMain.Staff user = null;

        public static void CreateToDo(XLVirtualCabinet.FileInfo file)
        {
            try
            {
                if (ToDoFolder == null)
                {
                    SetToDoFolder();
                }
                Outlook.TaskItem toDo = Globals.ThisAddIn.Application.CreateItem(Outlook.OlItemType.olTaskItem) as Outlook.TaskItem;
                string desc = file.ClientString;
                desc = desc + " - " + file.Description;
                toDo.Subject = desc;
                toDo.StartDate = DateTime.Now;
                toDo.DueDate = DateTime.Now.AddDays(1);
                //toDo.Body = file.FileID;
                XLOutlook.UpdateParameter(fileIdName, file.FileID, toDo);
                toDo.Move(ToDoFolder);
                toDo.Save();
            }
            catch (Exception ex)
            {
                XLant.XLtools.LogException("CreateToDo", ex.ToString());
                MessageBox.Show("Could not add To Do", "Add To Do", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public static void ReassignTask(Outlook.MailItem task)
       {
           //get the recipients of the assigned task
           string[] emails = XLOutlook.EmailAddresses(task);
           //we can only set one in VC so select the first one.
           string newUser = emails[0];
           //if the recipient is not one of us we aren't interested
           if (newUser.Contains("milsted-langdon.co.uk"))
           {
               string fileId = XLOutlook.ReadParameter("VCFileID", task);
               //assign the associated file to the new user
               //get the username from the e-mail address
               newUser = newUser.Substring(0, newUser.IndexOf('@'));
               XLMain.Staff staff = XLMain.Staff.StaffFromUser(newUser);
               //build a reindex command from the data
               string commandfileloc = "";
               commandfileloc = XLVirtualCabinet.Reindex(fileId, staff.name);
               //reindex
               XLVirtualCabinet.BondResult result = XLVirtualCabinet.LaunchCabi(commandfileloc, true, true);
               if (result.ExitCode != 0)
               {
                   //if the reindex is not successful then say so otherwise silence is golden.
                   MessageBox.Show("Reindex failed, possibly because you are offline, please reindex in VC");
               }
           }
       }

        private static void SetToDoFolder()
        {
            Outlook.Folder folder = Globals.ThisAddIn.Application.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderTasks) as Outlook.Folder;
            Outlook.Folders folders = folder.Folders;
            
            //cycle through each task folder looking for VC To Dos
            foreach (Outlook.Folder f in folders)
            {
                if (f.Name == folderName)
                {
                    ToDoFolder = f;
                }
            }
            //if nothing is set, it doesn't exist so create it
            if (ToDoFolder == null)
            {
                CreateVCFolder();
            }
        }

        public static void CreateVCFolder()
        {
            Outlook.Folder folder = Globals.ThisAddIn.Application.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderTasks) as Outlook.Folder;
            Outlook.Folders folders = folder.Folders;

            try
            {
                //add our new folder
                folders.Add(folderName, Outlook.OlDefaultFolders.olFolderTasks);
                
                //then select it
                SetToDoFolder();

                //and add our user defined property
                ToDoFolder.UserDefinedProperties.Add(fileIdName, Outlook.OlUserPropertyType.olText, true);
            }
            catch (Exception ex)
            {
                XLant.XLtools.LogException("CreateVCFolder", ex.ToString());
                MessageBox.Show("Could not add task folder", "Add Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void CheckToDos()
        {
            try
            {
                List<string> newIds = new List<string>();
                //set user
                if (user == null)
                {
                    user = XLMain.Staff.StaffFromUser(Environment.UserName);
                }

                //get the current todos
                List<XLVirtualCabinet.FileInfo> newToDos = XLant.XLVirtualCabinet.GetToDos(user.name);

                //input the ids into an array for comparison with our existing list
                List<string> newtds = new List<string>();
                if (newToDos != null)
                {
                    foreach (XLVirtualCabinet.FileInfo i in newToDos)
                    {
                        //create a list of the ids for comparison
                        //with those already in the system
                        newtds.Add(i.FileID);
                    }
                }
                //get a list of the existing ids if not already filled
                if (ids == null)
                {
                    //build to dos
                    if (ToDoFolder == null)
                    {
                        SetToDoFolder();
                    }
                    foreach (Outlook.TaskItem item in ToDoFolder.Items)
                    {
                        string s = XLOutlook.ReadParameter(fileIdName, item);
                        MessageBox.Show(s);
                        ids.Add(s);
                    }
                }
                //compare the new with the old
                newIds = newtds.Except(ids).ToList();

                //where there are new ones create entries
                foreach (string id in newIds)
                {
                    //go and get the file details
                    XLVirtualCabinet.FileInfo item = XLVirtualCabinet.FileIndex(id);
                    //add to to dos
                    CreateToDo(item);
                }
                ///////////////////need to handle removal of things which have moved other than through XLant/////////////////////

            }
            catch (Exception ex)
            {
                XLant.XLtools.LogException("CheckToDos", ex.ToString());
                MessageBox.Show("Could not check To Dos", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
