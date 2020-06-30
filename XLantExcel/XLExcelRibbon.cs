using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using Newtonsoft.Json.Linq;
using XLant;

namespace XLantExcel
{
    public partial class XLExcelRibbon
    {
        private void XLExcelRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void CRRawDataBtn_Click(object sender, RibbonControlEventArgs e)
        {
            RawDataForm form = new RawDataForm("CR");
            form.ShowDialog();
            DataTable data = form.ReturnedTable;
            if (data != null)
            {
                XLSheet.CreateWorkSheet(data, "Table1");
            }
            else
            {
                MessageBox.Show("No data returned");
            }
        }

        private void CRClaimsExperienceBtn_Click(object sender, RibbonControlEventArgs e)
        {
            CRSchemeLeadForm form = new CRSchemeLeadForm();
            form.ShowDialog();
            if (form.SchemeId != null)
            {
                //get the data from the API
                string url = "http://localhost:49485/ClaimsExperience/Get?schemeId=" + form.SchemeId;
                if (form.LeadId != null)
                {
                    url += "&leadId=" + form.LeadId;
                }
                XLAPI.Result result = XLAPI.GetDataFromAPI(url);
                JObject package = (JObject)result.Data;
                XLSheet.HandleClaimsExperience(package);
            }
        }

        private void PERawDataBtn_Click(object sender, RibbonControlEventArgs e)
        {
            RawDataForm form = new RawDataForm("PE");
            form.ShowDialog();
            DataTable data = form.ReturnedTable;
            if (data != null)
            {
                XLSheet.CreateWorkSheet(data, "Table1");
            }
            else
            {
                MessageBox.Show("No data returned");
            }
        }

        private void MLFSDirRepBtn_Click(object sender, RibbonControlEventArgs e)
        {
            MLFSDirRepForm form = new MLFSDirRepForm();
            form.ShowDialog();
            if (!String.IsNullOrEmpty(form.PlansFile) && !String.IsNullOrEmpty(form.FeesFile) && form.PeriodDate != null)
            {
                XLSheet.BuildMLFSDirectorsReport(form.FeesFile, form.PlansFile, form.PeriodDate, form.InitialTarget, form.TrailTarget);
            }
        }
    }
}
