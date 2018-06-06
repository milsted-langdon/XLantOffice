namespace XLantOutlook
{
    partial class XLantRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public XLantRibbon()
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
            this.group2 = this.Factory.CreateRibbonGroup();
            this.QuickIndex = this.Factory.CreateRibbonSplitButton();
            this.MultiClientIndexBtn = this.Factory.CreateRibbonButton();
            this.indexAttachmentsBtn = this.Factory.CreateRibbonButton();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.PromptTglBtn = this.Factory.CreateRibbonCheckBox();
            this.ExInternalCheck = this.Factory.CreateRibbonCheckBox();
            this.ExInternal = this.Factory.CreateRibbonCheckBox();
            this.tab1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group3.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group2);
            this.tab1.Groups.Add(this.group3);
            this.tab1.Label = "XLant Data";
            this.tab1.Name = "tab1";
            // 
            // group2
            // 
            this.group2.Items.Add(this.QuickIndex);
            this.group2.Label = "Emails";
            this.group2.Name = "group2";
            // 
            // QuickIndex
            // 
            this.QuickIndex.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.QuickIndex.Items.Add(this.MultiClientIndexBtn);
            this.QuickIndex.Items.Add(this.indexAttachmentsBtn);
            this.QuickIndex.Label = "Quick Index";
            this.QuickIndex.Name = "QuickIndex";
            this.QuickIndex.OfficeImageId = "EditListItems";
            this.QuickIndex.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.QuickIndex_Click);
            // 
            // MultiClientIndexBtn
            // 
            this.MultiClientIndexBtn.Label = "Mulitple Client Index";
            this.MultiClientIndexBtn.Name = "MultiClientIndexBtn";
            this.MultiClientIndexBtn.ShowImage = true;
            this.MultiClientIndexBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MultiClientIndexBtn_Click);
            // 
            // indexAttachmentsBtn
            // 
            this.indexAttachmentsBtn.Label = "Index Attachments";
            this.indexAttachmentsBtn.Name = "indexAttachmentsBtn";
            this.indexAttachmentsBtn.ShowImage = true;
            this.indexAttachmentsBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.indexAttachmentsBtn_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.PromptTglBtn);
            this.group3.Items.Add(this.ExInternalCheck);
            this.group3.Label = "Settings";
            this.group3.Name = "group3";
            // 
            // PromptTglBtn
            // 
            this.PromptTglBtn.Label = "Prompt to Index";
            this.PromptTglBtn.Name = "PromptTglBtn";
            this.PromptTglBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.PromptTglBtn_Click);
            // 
            // ExInternalCheck
            // 
            this.ExInternalCheck.Label = "Exclude Internal Emails";
            this.ExInternalCheck.Name = "ExInternalCheck";
            this.ExInternalCheck.Visible = false;
            this.ExInternalCheck.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ExInternal_Click);
            // 
            // ExInternal
            // 
            this.ExInternal.Label = "";
            this.ExInternal.Name = "ExInternal";
            // 
            // XLantRibbon
            // 
            this.Name = "XLantRibbon";
            this.RibbonType = "Microsoft.Outlook.Explorer";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.XLantRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton QuickIndex;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MultiClientIndexBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton indexAttachmentsBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox ExInternal;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox PromptTglBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox ExInternalCheck;
    }

    partial class ThisRibbonCollection
    {
        internal XLantRibbon XLantRibbon
        {
            get { return this.GetRibbon<XLantRibbon>(); }
        }
    }
}
