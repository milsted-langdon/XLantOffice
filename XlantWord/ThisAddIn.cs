using System;
using XLant;

namespace XlantWord
{
    public partial class ThisAddIn
    {

        
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {

        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return Globals.Factory.GetRibbonFactory().CreateRibbonManager(
                new Microsoft.Office.Tools.Ribbon.IRibbonExtension[] { new XLantWordRibbon() });
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            try
            {

                if (XLantWordRibbon.CustomxlTaskPane != null)
                {
                    XLantWordRibbon.CustomxlTaskPane.Dispose();
                }
                if (XLantWordRibbon.sigPane != null)
                {
                    XLantWordRibbon.sigPane.Dispose();
                }
                if (XLantWordRibbon.xlTaskPane1 != null)
                {
                    XLantWordRibbon.xlTaskPane1.Dispose();
                }
                Globals.Ribbons.XLantWordRibbon.Dispose();
                //Microsoft.Office.Interop.Word.Application word = Globals.ThisAddIn.Application;
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(word);
                //word = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                XLtools.LogException("Shutdown", ex.ToString());
            }
        }



        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
