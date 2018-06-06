namespace XLantOutlook
{
    partial class XLantMailRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public XLantMailRibbon()
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
            this.WorkflowGroup = this.Factory.CreateRibbonGroup();
            this.QuickIndex = this.Factory.CreateRibbonSplitButton();
            this.MultiClientIndexBtn = this.Factory.CreateRibbonButton();
            this.IndexAttachmentBtn = this.Factory.CreateRibbonButton();
            this.StartBtn = this.Factory.CreateRibbonButton();
            this.ForwardBtn = this.Factory.CreateRibbonButton();
            this.ApproveBtn = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group2.SuspendLayout();
            this.WorkflowGroup.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group2);
            this.tab1.Groups.Add(this.WorkflowGroup);
            this.tab1.Label = "XLant Data";
            this.tab1.Name = "tab1";
            // 
            // group2
            // 
            this.group2.Items.Add(this.QuickIndex);
            this.group2.Label = "File";
            this.group2.Name = "group2";
            // 
            // WorkflowGroup
            // 
            this.WorkflowGroup.Items.Add(this.StartBtn);
            this.WorkflowGroup.Items.Add(this.ForwardBtn);
            this.WorkflowGroup.Items.Add(this.ApproveBtn);
            this.WorkflowGroup.Label = "Workflow";
            this.WorkflowGroup.Name = "WorkflowGroup";
            // 
            // QuickIndex
            // 
            this.QuickIndex.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.QuickIndex.Items.Add(this.MultiClientIndexBtn);
            this.QuickIndex.Items.Add(this.IndexAttachmentBtn);
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
            // IndexAttachmentBtn
            // 
            this.IndexAttachmentBtn.Label = "Index Attachments";
            this.IndexAttachmentBtn.Name = "IndexAttachmentBtn";
            this.IndexAttachmentBtn.ShowImage = true;
            this.IndexAttachmentBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.IndexAttachmentBtn_Click);
            // 
            // StartBtn
            // 
            this.StartBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.StartBtn.Label = "File Draft";
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.OfficeImageId = "EditListItems";
            this.StartBtn.ShowImage = true;
            this.StartBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.StartBtn_Click);
            // 
            // ForwardBtn
            // 
            this.ForwardBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ForwardBtn.Label = "Forward";
            this.ForwardBtn.Name = "ForwardBtn";
            this.ForwardBtn.OfficeImageId = "MacroPlay";
            this.ForwardBtn.ShowImage = true;
            this.ForwardBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ForwardBtn_Click);
            // 
            // ApproveBtn
            // 
            this.ApproveBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ApproveBtn.Label = "Approve";
            this.ApproveBtn.Name = "ApproveBtn";
            this.ApproveBtn.OfficeImageId = "DataTypeOnOff";
            this.ApproveBtn.ShowImage = true;
            this.ApproveBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ApproveBtn_Click);
            // 
            // XLantMailRibbon
            // 
            this.Name = "XLantMailRibbon";
            this.RibbonType = "Microsoft.Outlook.Mail.Compose, Microsoft.Outlook.Mail.Read";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.XLantRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.WorkflowGroup.ResumeLayout(false);
            this.WorkflowGroup.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ForwardBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ApproveBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup WorkflowGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton StartBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton QuickIndex;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MultiClientIndexBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton IndexAttachmentBtn;
    }

    partial class ThisRibbonCollection
    {
        internal XLantMailRibbon XLantMailRibbon
        {
            get { return this.GetRibbon<XLantMailRibbon>(); }
        }
    }
}
