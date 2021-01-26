namespace XlantWord
{
    partial class XLantWordRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public XLantWordRibbon()
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
            this.XLantDataTab = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.CreateBtns = this.Factory.CreateRibbonSplitButton();
            this.CreateLetterBtn = this.Factory.CreateRibbonButton();
            this.CreateFaxBtn = this.Factory.CreateRibbonButton();
            this.CreateFNBtn = this.Factory.CreateRibbonButton();
            this.GetAddressMain = this.Factory.CreateRibbonSplitButton();
            this.ClientAddressBtn = this.Factory.CreateRibbonButton();
            this.ContactAddressBtn = this.Factory.CreateRibbonButton();
            this.StaffAddressBtn = this.Factory.CreateRibbonButton();
            this.InsolAddressBtn = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.IndexBtn = this.Factory.CreateRibbonButton();
            this.ForwardBtn = this.Factory.CreateRibbonButton();
            this.ApproveBtn = this.Factory.CreateRibbonButton();
            this.group4 = this.Factory.CreateRibbonGroup();
            this.UpdateDatebtn = this.Factory.CreateRibbonButton();
            this.UpdateRefBtn = this.Factory.CreateRibbonButton();
            this.docRefButton = this.Factory.CreateRibbonButton();
            this.button1 = this.Factory.CreateRibbonButton();
            this.Signature = this.Factory.CreateRibbonGroup();
            this.button2 = this.Factory.CreateRibbonButton();
            this.ScannedImage = this.Factory.CreateRibbonSplitButton();
            this.MySigBtn = this.Factory.CreateRibbonButton();
            this.MLsignature = this.Factory.CreateRibbonButton();
            this.StaffSig = this.Factory.CreateRibbonButton();
            this.SigViewBtn = this.Factory.CreateRibbonButton();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.BioBtn = this.Factory.CreateRibbonButton();
            this.TempSaveBtn = this.Factory.CreateRibbonButton();
            this.PdfBtn = this.Factory.CreateRibbonSplitButton();
            this.pdfAttachmentsBtn = this.Factory.CreateRibbonButton();
            this.MLFSPdfBtn = this.Factory.CreateRibbonButton();
            this.MLFSPdfAttachBtn = this.Factory.CreateRibbonButton();
            this.MLFPPdfBtn = this.Factory.CreateRibbonButton();
            this.MLFPPdfAttachBtn = this.Factory.CreateRibbonButton();
            this.PrintBtn = this.Factory.CreateRibbonButton();
            this.HeaderFooterBtn = this.Factory.CreateRibbonButton();
            this.StyleBtn = this.Factory.CreateRibbonButton();
            this.ShowHiddenBtn = this.Factory.CreateRibbonToggleButton();
            this.FPI = this.Factory.CreateRibbonGroup();
            this.MergeBtn = this.Factory.CreateRibbonButton();
            this.Merge2Btn = this.Factory.CreateRibbonButton();
            this.Merge3Btn = this.Factory.CreateRibbonButton();
            this.Merge4Btn = this.Factory.CreateRibbonButton();
            this.InvoiceBtn = this.Factory.CreateRibbonButton();
            this.BulkInvoiceButton = this.Factory.CreateRibbonButton();
            this.XLantDataTab.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group4.SuspendLayout();
            this.Signature.SuspendLayout();
            this.group3.SuspendLayout();
            this.FPI.SuspendLayout();
            this.SuspendLayout();
            // 
            // XLantDataTab
            // 
            this.XLantDataTab.Groups.Add(this.group1);
            this.XLantDataTab.Groups.Add(this.group2);
            this.XLantDataTab.Groups.Add(this.group4);
            this.XLantDataTab.Groups.Add(this.Signature);
            this.XLantDataTab.Groups.Add(this.group3);
            this.XLantDataTab.Groups.Add(this.FPI);
            this.XLantDataTab.Label = "XLant Data";
            this.XLantDataTab.Name = "XLantDataTab";
            // 
            // group1
            // 
            this.group1.Items.Add(this.CreateBtns);
            this.group1.Items.Add(this.GetAddressMain);
            this.group1.Label = "Data";
            this.group1.Name = "group1";
            // 
            // CreateBtns
            // 
            this.CreateBtns.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.CreateBtns.Items.Add(this.CreateLetterBtn);
            this.CreateBtns.Items.Add(this.CreateFaxBtn);
            this.CreateBtns.Items.Add(this.CreateFNBtn);
            this.CreateBtns.ItemSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.CreateBtns.Label = "Create";
            this.CreateBtns.Name = "CreateBtns";
            this.CreateBtns.OfficeImageId = "EditLabel";
            this.CreateBtns.ScreenTip = "Click to create letter or select document type";
            this.CreateBtns.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CreateBtns_Click);
            // 
            // CreateLetterBtn
            // 
            this.CreateLetterBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.CreateLetterBtn.Label = "Letter";
            this.CreateLetterBtn.Name = "CreateLetterBtn";
            this.CreateLetterBtn.OfficeImageId = "EditLabel";
            this.CreateLetterBtn.ShowImage = true;
            this.CreateLetterBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CreateLetterBtn_Click);
            // 
            // CreateFaxBtn
            // 
            this.CreateFaxBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.CreateFaxBtn.Label = "Fax";
            this.CreateFaxBtn.Name = "CreateFaxBtn";
            this.CreateFaxBtn.OfficeImageId = "FileInternetFax";
            this.CreateFaxBtn.ShowImage = true;
            this.CreateFaxBtn.Visible = false;
            this.CreateFaxBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CreateFaxBtn_Click);
            // 
            // CreateFNBtn
            // 
            this.CreateFNBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.CreateFNBtn.Label = "File Note";
            this.CreateFNBtn.Name = "CreateFNBtn";
            this.CreateFNBtn.OfficeImageId = "EditForm";
            this.CreateFNBtn.ShowImage = true;
            this.CreateFNBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CreateFNBtn_Click);
            // 
            // GetAddressMain
            // 
            this.GetAddressMain.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.GetAddressMain.Items.Add(this.ClientAddressBtn);
            this.GetAddressMain.Items.Add(this.ContactAddressBtn);
            this.GetAddressMain.Items.Add(this.StaffAddressBtn);
            this.GetAddressMain.Items.Add(this.InsolAddressBtn);
            this.GetAddressMain.Label = "Get Address";
            this.GetAddressMain.Name = "GetAddressMain";
            this.GetAddressMain.OfficeImageId = "EnvelopesAndLabelsDialog";
            // 
            // ClientAddressBtn
            // 
            this.ClientAddressBtn.Label = "Client Address";
            this.ClientAddressBtn.Name = "ClientAddressBtn";
            this.ClientAddressBtn.OfficeImageId = "EnvelopesAndLabelsDialog";
            this.ClientAddressBtn.ShowImage = true;
            this.ClientAddressBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ClientAddressBtn_Click);
            // 
            // ContactAddressBtn
            // 
            this.ContactAddressBtn.Label = "Contact Address";
            this.ContactAddressBtn.Name = "ContactAddressBtn";
            this.ContactAddressBtn.OfficeImageId = "FileSendMenu";
            this.ContactAddressBtn.ShowImage = true;
            this.ContactAddressBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ContactAddressBtn_Click);
            // 
            // StaffAddressBtn
            // 
            this.StaffAddressBtn.Label = "Staff Address";
            this.StaffAddressBtn.Name = "StaffAddressBtn";
            this.StaffAddressBtn.OfficeImageId = "SlideMasterClipArtPlaceholderInsert";
            this.StaffAddressBtn.ShowImage = true;
            this.StaffAddressBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.StaffAddressBtn_Click);
            // 
            // InsolAddressBtn
            // 
            this.InsolAddressBtn.Label = "Insol Address";
            this.InsolAddressBtn.Name = "InsolAddressBtn";
            this.InsolAddressBtn.ShowImage = true;
            this.InsolAddressBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.InsolAddressBtn_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.IndexBtn);
            this.group2.Items.Add(this.ForwardBtn);
            this.group2.Items.Add(this.ApproveBtn);
            this.group2.Label = "File";
            this.group2.Name = "group2";
            // 
            // IndexBtn
            // 
            this.IndexBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.IndexBtn.Label = "First Index";
            this.IndexBtn.Name = "IndexBtn";
            this.IndexBtn.OfficeImageId = "EditListItems";
            this.IndexBtn.ShowImage = true;
            this.IndexBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.IndexBtn_Click);
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
            // group4
            // 
            this.group4.Items.Add(this.UpdateDatebtn);
            this.group4.Items.Add(this.UpdateRefBtn);
            this.group4.Items.Add(this.docRefButton);
            this.group4.Items.Add(this.button1);
            this.group4.Label = "Update";
            this.group4.Name = "group4";
            // 
            // UpdateDatebtn
            // 
            this.UpdateDatebtn.ImageName = "ViewAllProposals";
            this.UpdateDatebtn.Label = "Update Date";
            this.UpdateDatebtn.Name = "UpdateDatebtn";
            this.UpdateDatebtn.ShowImage = true;
            this.UpdateDatebtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.UpdateDatebtn_Click);
            // 
            // UpdateRefBtn
            // 
            this.UpdateRefBtn.ImageName = "FileWorkflowTasks";
            this.UpdateRefBtn.Label = "Update Ref";
            this.UpdateRefBtn.Name = "UpdateRefBtn";
            this.UpdateRefBtn.ScreenTip = "Add FileID to the current reference";
            this.UpdateRefBtn.ShowImage = true;
            this.UpdateRefBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.UpdateRefBtn_Click);
            // 
            // docRefButton
            // 
            this.docRefButton.ImageName = "FileWorkflowTasks";
            this.docRefButton.Label = "Doc Ref";
            this.docRefButton.Name = "docRefButton";
            this.docRefButton.ScreenTip = "Add DocRef to the current reference";
            this.docRefButton.ShowImage = true;
            this.docRefButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.docRefButton_Click);
            // 
            // button1
            // 
            this.button1.Label = "";
            this.button1.Name = "button1";
            // 
            // Signature
            // 
            this.Signature.Items.Add(this.button2);
            this.Signature.Items.Add(this.ScannedImage);
            this.Signature.Items.Add(this.SigViewBtn);
            this.Signature.Label = "Signatures";
            this.Signature.Name = "Signature";
            this.Signature.Tag = "Manage Signatures";
            // 
            // button2
            // 
            this.button2.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button2.Label = "Internal";
            this.button2.Name = "button2";
            this.button2.OfficeImageId = "GroupInk";
            this.button2.ScreenTip = "Add sign off for internal documents";
            this.button2.ShowImage = true;
            this.button2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button2_Click);
            // 
            // ScannedImage
            // 
            this.ScannedImage.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ScannedImage.Items.Add(this.MySigBtn);
            this.ScannedImage.Items.Add(this.MLsignature);
            this.ScannedImage.Items.Add(this.StaffSig);
            this.ScannedImage.Label = "Scanned Image";
            this.ScannedImage.Name = "ScannedImage";
            this.ScannedImage.OfficeImageId = "GroupInk";
            this.ScannedImage.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ScannedImage_Click);
            // 
            // MySigBtn
            // 
            this.MySigBtn.Label = "Mine";
            this.MySigBtn.Name = "MySigBtn";
            this.MySigBtn.ScreenTip = "Add your own scanned signature";
            this.MySigBtn.ShowImage = true;
            this.MySigBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MySigBtn_Click);
            // 
            // MLsignature
            // 
            this.MLsignature.Label = "Milsted Langdon LLP";
            this.MLsignature.Name = "MLsignature";
            this.MLsignature.ShowImage = true;
            this.MLsignature.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MLsignature_Click);
            // 
            // StaffSig
            // 
            this.StaffSig.Label = "Staff Member";
            this.StaffSig.Name = "StaffSig";
            this.StaffSig.ShowImage = true;
            this.StaffSig.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.StaffSig_Click);
            // 
            // SigViewBtn
            // 
            this.SigViewBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.SigViewBtn.Label = "View";
            this.SigViewBtn.Name = "SigViewBtn";
            this.SigViewBtn.OfficeImageId = "SignatureLineInsert";
            this.SigViewBtn.ScreenTip = "See history of signatures";
            this.SigViewBtn.ShowImage = true;
            this.SigViewBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SigViewBtn_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.BioBtn);
            this.group3.Items.Add(this.TempSaveBtn);
            this.group3.Items.Add(this.PdfBtn);
            this.group3.Items.Add(this.PrintBtn);
            this.group3.Items.Add(this.HeaderFooterBtn);
            this.group3.Items.Add(this.StyleBtn);
            this.group3.Items.Add(this.ShowHiddenBtn);
            this.group3.Label = "Other";
            this.group3.Name = "group3";
            // 
            // BioBtn
            // 
            this.BioBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.BioBtn.Label = "Add Bio";
            this.BioBtn.Name = "BioBtn";
            this.BioBtn.OfficeImageId = "NewUnifiedGroup";
            this.BioBtn.ShowImage = true;
            this.BioBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BioBtn_Click);
            // 
            // TempSaveBtn
            // 
            this.TempSaveBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.TempSaveBtn.Label = "Temp Save";
            this.TempSaveBtn.Name = "TempSaveBtn";
            this.TempSaveBtn.OfficeImageId = "FileSaveAs";
            this.TempSaveBtn.ScreenTip = "Creates a temporary save when using read-only documents like standards from Share" +
    "point";
            this.TempSaveBtn.ShowImage = true;
            this.TempSaveBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.TempSaveBtn_Click);
            // 
            // PdfBtn
            // 
            this.PdfBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.PdfBtn.Items.Add(this.pdfAttachmentsBtn);
            this.PdfBtn.Items.Add(this.MLFSPdfBtn);
            this.PdfBtn.Items.Add(this.MLFSPdfAttachBtn);
            this.PdfBtn.Items.Add(this.MLFPPdfBtn);
            this.PdfBtn.Items.Add(this.MLFPPdfAttachBtn);
            this.PdfBtn.Label = "ML Pdf";
            this.PdfBtn.Name = "PdfBtn";
            this.PdfBtn.OfficeImageId = "GroupInk";
            this.PdfBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MLPdfBtn_Click);
            // 
            // pdfAttachmentsBtn
            // 
            this.pdfAttachmentsBtn.Label = "ML With Attachments";
            this.pdfAttachmentsBtn.Name = "pdfAttachmentsBtn";
            this.pdfAttachmentsBtn.ScreenTip = "Create an ML PDF and add attachments to it";
            this.pdfAttachmentsBtn.ShowImage = true;
            this.pdfAttachmentsBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.pdfAttachmentsBtn_Click);
            // 
            // MLFSPdfBtn
            // 
            this.MLFSPdfBtn.Label = "MLFS";
            this.MLFSPdfBtn.Name = "MLFSPdfBtn";
            this.MLFSPdfBtn.ScreenTip = "Create an MLFS PDF";
            this.MLFSPdfBtn.ShowImage = true;
            this.MLFSPdfBtn.Visible = false;
            this.MLFSPdfBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MLFSPdfBtn_Click);
            // 
            // MLFSPdfAttachBtn
            // 
            this.MLFSPdfAttachBtn.Label = "MLFS with Attachments";
            this.MLFSPdfAttachBtn.Name = "MLFSPdfAttachBtn";
            this.MLFSPdfAttachBtn.ScreenTip = "Create an MLFS PDF and add attachments to it";
            this.MLFSPdfAttachBtn.ShowImage = true;
            this.MLFSPdfAttachBtn.Visible = false;
            this.MLFSPdfAttachBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MLFSAttachBtn_Click);
            // 
            // MLFPPdfBtn
            // 
            this.MLFPPdfBtn.Label = "MLFP";
            this.MLFPPdfBtn.Name = "MLFPPdfBtn";
            this.MLFPPdfBtn.ScreenTip = "Create an MLFP PDF";
            this.MLFPPdfBtn.ShowImage = true;
            this.MLFPPdfBtn.Visible = false;
            this.MLFPPdfBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MLFPPdfBtn_Click);
            // 
            // MLFPPdfAttachBtn
            // 
            this.MLFPPdfAttachBtn.Label = "MLFP with Attachments";
            this.MLFPPdfAttachBtn.Name = "MLFPPdfAttachBtn";
            this.MLFPPdfAttachBtn.ScreenTip = "Create an MLFP PDF and add attachments to it";
            this.MLFPPdfAttachBtn.ShowImage = true;
            this.MLFPPdfAttachBtn.Visible = false;
            this.MLFPPdfAttachBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MLFPAttachBtn_Click);
            // 
            // PrintBtn
            // 
            this.PrintBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.PrintBtn.Description = "Print Current Document";
            this.PrintBtn.Label = "Print";
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.OfficeImageId = "FilePrint";
            this.PrintBtn.ShowImage = true;
            this.PrintBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.PrintBtn_Click);
            // 
            // HeaderFooterBtn
            // 
            this.HeaderFooterBtn.Label = "Header/Footer";
            this.HeaderFooterBtn.Name = "HeaderFooterBtn";
            this.HeaderFooterBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.HeaderFooterBtn_Click);
            // 
            // StyleBtn
            // 
            this.StyleBtn.Label = "Styles";
            this.StyleBtn.Name = "StyleBtn";
            this.StyleBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.StyleBtn_Click);
            // 
            // ShowHiddenBtn
            // 
            this.ShowHiddenBtn.Label = "Show/Hide Status";
            this.ShowHiddenBtn.Name = "ShowHiddenBtn";
            this.ShowHiddenBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ShowHiddenBtn_Click);
            // 
            // FPI
            // 
            this.FPI.Items.Add(this.MergeBtn);
            this.FPI.Items.Add(this.Merge2Btn);
            this.FPI.Items.Add(this.Merge3Btn);
            this.FPI.Items.Add(this.Merge4Btn);
            this.FPI.Items.Add(this.InvoiceBtn);
            this.FPI.Items.Add(this.BulkInvoiceButton);
            this.FPI.Label = "FPI";
            this.FPI.Name = "FPI";
            // 
            // MergeBtn
            // 
            this.MergeBtn.Label = "New To FPI";
            this.MergeBtn.Name = "MergeBtn";
            this.MergeBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.MergeBtn_Click);
            // 
            // Merge2Btn
            // 
            this.Merge2Btn.Label = "Existing DD Setup";
            this.Merge2Btn.Name = "Merge2Btn";
            this.Merge2Btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Merge2Btn_Click);
            // 
            // Merge3Btn
            // 
            this.Merge3Btn.Label = "Existing No DD Setup";
            this.Merge3Btn.Name = "Merge3Btn";
            this.Merge3Btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Merge3Btn_Click);
            // 
            // Merge4Btn
            // 
            this.Merge4Btn.Label = "Merge Single Client";
            this.Merge4Btn.Name = "Merge4Btn";
            this.Merge4Btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Merge4Btn_Click);
            // 
            // InvoiceBtn
            // 
            this.InvoiceBtn.Label = "Merge Invoice";
            this.InvoiceBtn.Name = "InvoiceBtn";
            this.InvoiceBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.InvoiceBtn_Click);
            // 
            // BulkInvoiceButton
            // 
            this.BulkInvoiceButton.Label = "Bulk Invoice";
            this.BulkInvoiceButton.Name = "BulkInvoiceButton";
            this.BulkInvoiceButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BulkInvoiceButton_Click);
            // 
            // XLantWordRibbon
            // 
            this.Name = "XLantWordRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.XLantDataTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.XLantWordRibbon_Load);
            this.XLantDataTab.ResumeLayout(false);
            this.XLantDataTab.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.group4.ResumeLayout(false);
            this.group4.PerformLayout();
            this.Signature.ResumeLayout(false);
            this.Signature.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.FPI.ResumeLayout(false);
            this.FPI.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab XLantDataTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton CreateBtns;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton CreateLetterBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton CreateFaxBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton CreateFNBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton IndexBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ApproveBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton ShowHiddenBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton HeaderFooterBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton PrintBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton GetAddressMain;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ClientAddressBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ContactAddressBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton StyleBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ForwardBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton UpdateDatebtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton TempSaveBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group4;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton UpdateRefBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton StaffAddressBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton InsolAddressBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup Signature;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SigViewBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton ScannedImage;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MLsignature;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton StaffSig;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MySigBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton docRefButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup FPI;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MergeBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Merge2Btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Merge3Btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Merge4Btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton InvoiceBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonSplitButton PdfBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton pdfAttachmentsBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MLFSPdfBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MLFSPdfAttachBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MLFPPdfBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton MLFPPdfAttachBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BioBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BulkInvoiceButton;
    }

    partial class ThisRibbonCollection
    {
        internal XLantWordRibbon XLantWordRibbon
        {
            get { return this.GetRibbon<XLantWordRibbon>(); }
        }
    }
}
