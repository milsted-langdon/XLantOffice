namespace XLantExcel
{
    partial class XLExcelRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public XLExcelRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.CRRawDataBtn = this.Factory.CreateRibbonButton();
            this.CRClaimsExperienceBtn = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.PERawDataBtn = this.Factory.CreateRibbonButton();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.MLFSDirRepBtn = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Groups.Add(this.group3);
            this.tab1.Label = "XLant Data";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.CRRawDataBtn);
            this.group1.Items.Add(this.CRClaimsExperienceBtn);
            this.group1.Label = "Haslocks";
            this.group1.Name = "group1";
            // 
            // CRRawDataBtn
            // 
            this.CRRawDataBtn.Label = "RawData";
            this.CRRawDataBtn.Name = "CRRawDataBtn";
            this.CRRawDataBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CRRawDataBtn_Click);
            // 
            // CRClaimsExperienceBtn
            // 
            this.CRClaimsExperienceBtn.Label = "Claims Experience";
            this.CRClaimsExperienceBtn.Name = "CRClaimsExperienceBtn";
            this.CRClaimsExperienceBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CRClaimsExperienceBtn_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.PERawDataBtn);
            this.group2.Label = "ML";
            this.group2.Name = "group2";
            // 
            // PERawDataBtn
            // 
            this.PERawDataBtn.Label = "PE Raw Data";
            this.PERawDataBtn.Name = "PERawDataBtn";
            this.PERawDataBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.PERawDataBtn_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.MLFSDirRepBtn);
            this.group3.Label = "MLFS";
            this.group3.Name = "group3";
            // 
            // MLFSDirRepBtn
            // 
            this.MLFSDirRepBtn.Label = "Directors\' Report";
            this.MLFSDirRepBtn.Name = "MLFSDirRepBtn";
            this.MLFSDirRepBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MLFSDirRepBtn_Click);
            // 
            // XLExcelRibbon
            // 
            this.Name = "XLExcelRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.XLExcelRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton CRRawDataBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton CRClaimsExperienceBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton PERawDataBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MLFSDirRepBtn;
    }

    partial class ThisRibbonCollection
    {
        internal XLExcelRibbon XLExcelRibbon
        {
            get { return this.GetRibbon<XLExcelRibbon>(); }
        }
    }
}
