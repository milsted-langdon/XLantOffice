using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using Newtonsoft.Json.Linq;
using XLantCore;
using XLantCore.Models;
using XLForms;

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

        private async void MLFSDirRepBtn_Click(object sender, RibbonControlEventArgs e)
        {
            APIAccess.Result result = APIAccess.GetDataFromXLAPI<List<MLFSReportingPeriod>>("/MLFSReportingPeriod/GetCurrentPeriods");
            MLFSDirRepForm form = new MLFSDirRepForm((List<MLFSReportingPeriod>)result.Data);
            form.ShowDialog();
            if (form.NewPeriods != null)
            {
                //post the new periods
                foreach (MLFSReportingPeriod period in form.NewPeriods)
                {
                    APIAccess.PostDataToXLAPI("/MLFSReportPeriod/Put", period);
                }
            }
            if (!String.IsNullOrEmpty(form.PlansFile) && !String.IsNullOrEmpty(form.FeesFile) && form.PeriodId != null && !String.IsNullOrEmpty(form.FCIFile))
            {
                string response = await XLSheet.BuildMLFSDirectorsReport(1, form.FeesFile, form.PlansFile, form.FCIFile);
                if (response == "Success")
                {
                    MessageBox.Show("Data Uploaded");
                }
                else
                {
                    MessageBox.Show("Upload Unsuccessful, check your data");
                }
            }
            else
            {
                MessageBox.Show("Upload Unsuccessful, check your data");
            }
        }
    }
}
