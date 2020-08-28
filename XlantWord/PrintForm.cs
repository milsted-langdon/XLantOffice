using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Management;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using System.Drawing.Printing;
using XLant;
using System.Reflection;

namespace XlantWord
{
    public partial class PrintForm: Form
    {

        public PrintForm()
        {
            InitializeComponent();
            this.CenterToParent();
            //Collect installed printers
            String installedPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                installedPrinters = PrinterSettings.InstalledPrinters[i];
                PrinterDDL.Items.Add(installedPrinters);
            }
            //Set the selected value to the current active printer
            PrinterDDL.Text = Globals.ThisAddIn.Application.ActivePrinter.ToString();
            //Set the paper type to Plain by default
            PaperDDL.Text = "Plain";
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            try
            {

                object oMissing = System.Reflection.Missing.Value;
                Document currentDoc = Globals.ThisAddIn.Application.ActiveDocument;
                
                //Get options from user
                //Set the printer to the selection in the ddl
                string printer = (string)PrinterDDL.SelectedItem;
                string paper = ((string)PaperDDL.SelectedItem).ToLower();
                
                
                //set printer trays based on input and obtain id from XML
                if (paper == "headed & continuation")
                {
                   
                    if (currentDoc.Sections.Count > 1)
                    {
                        //currentDoc.PageSetup.SectionStart = WdSectionStart.wdSectionNewPage;
                        foreach (Section s in currentDoc.Sections)
                        {
                            int i = s.Index;
                            currentDoc.Sections[i].PageSetup.FirstPageTray = GetTray(printer, "headed");
                            currentDoc.Sections[i].PageSetup.OtherPagesTray = GetTray(printer, "continuation");
                        }
                    }
                    else
                    {
                        currentDoc.PageSetup.FirstPageTray = GetTray(printer, "headed");
                        currentDoc.PageSetup.OtherPagesTray = GetTray(printer, "continuation");
                    }
                }
                else
                {
                    currentDoc.PageSetup.FirstPageTray = GetTray(printer, paper);
                    currentDoc.PageSetup.OtherPagesTray = GetTray(printer, paper);
                }
                //Globals.ThisAddIn.Application.ActivePrinter = printer;
                object basic = Globals.ThisAddIn.Application.WordBasic;
                object[] oWordDialogParams = { printer, true };
                string[] argNames = { "Printer", "DoNotSetAsSysDefault" };
                basic.GetType().InvokeMember("FilePrintSetup", BindingFlags.InvokeMethod, null, basic, oWordDialogParams, null, null, argNames);
                PrintDocument printDoc = new PrintDocument();
    
                //Print the document
                currentDoc.PrintOut(true, false, WdPrintOutRange.wdPrintAllDocument, oMissing, oMissing, oMissing, oMissing, "1", oMissing, WdPrintOutPages.wdPrintAllPages, oMissing, true, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to allocate printer trays, please use the advanced button");
                XLtools.LogException("QuickPrint", ex.ToString());
            }

        }

        private void AdvPrintBtn_Click(object sender, EventArgs e)
        {
            //Show the full print dialog
            Globals.ThisAddIn.Application.Dialogs[WdWordDialog.wdDialogFilePrint].Show();
            this.Close();
        }

        private WdPaperTray GetTray(string printerDescription, string trayDescription)
        {
            // Initialise the trayId which will be returned
            var trayId = string.Empty;

            // Get the <Printers> element - should only be one
            var printerElement = XLtools.settingsDoc.Descendants("Printers").FirstOrDefault();

            //get the part of the printer name that we actually want
            if (printerDescription.IndexOf(" (redirect") != -1)
            {
                printerDescription = printerDescription.Substring(0, printerDescription.IndexOf(" (redirect"));
            }
            

            // If the <Printer> element was found we can continue
            if (printerElement != null)
            {
                // Get the <printer> element that has the child element <printername> that
                // matches the supplied printerDescription
                var printer = (from p in printerElement.Elements()
                               where p.Element("printername").Value.ToUpper() == printerDescription.ToUpper()
                               select p).FirstOrDefault();

                // If we found a <printer> element we can continue
                if (printer != null)
                {
                    // Get the child element that is named the same as the trayDescription
                    var trayElement = printer.Element(trayDescription);

                    // If we found an element we can continue
                    if (trayElement != null)
                    {
        
                        // Get the value from the element
                        trayId = trayElement.Value;
                    }
                }
            }
            Int32 n;
            WdPaperTray tray = WdPaperTray.wdPrinterDefaultBin;
            if (Int32.TryParse(trayId, out n))
            {
                Int32 i = Convert.ToInt32(trayId);
                tray = (WdPaperTray)i;
            }
            else
            {
                
                //This is a fudge
                if (trayId == "wdPrinterManualFeed")
                {
                    tray = WdPaperTray.wdPrinterManualFeed;
                }
            }

            return tray;
        }

        private void PaperDDL_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

         private void PageSetup_Click(object sender, EventArgs e)
        {
            Globals.ThisAddIn.Application.Dialogs[WdWordDialog.wdDialogFilePageSetup].Show();
            this.Close();
        }

        
    }
}
