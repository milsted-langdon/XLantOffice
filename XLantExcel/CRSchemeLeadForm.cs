using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XLant;

namespace XLantExcel
{
    public partial class CRSchemeLeadForm : Form
    {
        public int? SchemeId;
        public int? LeadId;

        public CRSchemeLeadForm()
        {
            InitializeComponent();
            XLAPI.Result result = XLAPI.GetDataFromAPI("http://localhost:49485/Scheme/GetList");
            if (result.WasSuccessful)
            {
                SchemeDdl.DataSource = (JArray)result.Data;
                SchemeDdl.ValueMember = "Id";
                SchemeDdl.DisplayMember = "Name";
                SchemeDdl.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        private void SchemeDdl_SelectedIndexChanged(object sender, EventArgs e)
        {
            dynamic selectedScheme = new JObject();
            int id = HandleSelectedValue(SchemeDdl.SelectedValue);
            if (id != 0)
            {
                XLAPI.Result result = XLAPI.GetDataFromAPI("http://localhost:49485/Firm/GetLeadForScheme?schemeId=" + id);
                if (result.WasSuccessful)
                {
                    leadDdl.DataSource = (JArray)result.Data;
                    leadDdl.ValueMember = "Id";
                    leadDdl.DisplayMember = "FirmName";
                    leadDdl.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            }
        }

        private int HandleSelectedValue(object selectedValue)
        {
            int value = 0;
            try
            {
                if (selectedValue != null)
                {
                    dynamic selectedScheme = JsonConvert.DeserializeObject(selectedValue.ToString());
                    if (!int.TryParse(selectedScheme.ToString(), out value))
                    {
                        value = selectedScheme.Id.Value;
                    }
                }
            }
            catch
            {
                value = 0;
            }
            return value;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            LeadId = null;
            SchemeId = null;
            this.Close();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            SchemeId = HandleSelectedValue(SchemeDdl.SelectedValue);
            LeadId = HandleSelectedValue(leadDdl.SelectedValue);
            this.Close();
        }
    }
}
