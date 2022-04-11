namespace CNCViewer
{
    partial class FrmViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmViewer));
            this.CodeTip = new System.Windows.Forms.ToolTip(this.components);
            this.rmbView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuFit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFence = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPan = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRotate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tscboMachines = new System.Windows.Forms.ToolStripComboBox();
            this.tsbDisplay = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuRapidLines = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRapidPoints = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAxisLines = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAxisindicator = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbToolsFilter = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPan = new System.Windows.Forms.ToolStripButton();
            this.tsbZoom = new System.Windows.Forms.ToolStripButton();
            this.tsbRotate = new System.Windows.Forms.ToolStripButton();
            this.tsbFence = new System.Windows.Forms.ToolStripButton();
            this.tsbFit = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbView = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuTop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFront = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRight = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIsometric = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbSelect = new System.Windows.Forms.ToolStripButton();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.materialRaisedButton1 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.BreakPointSlider = new System.Windows.Forms.TrackBar();
            this.MG_Viewer1 = new MacGen.MG_CS_BasicViewer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.rmbView.SuspendLayout();
            this.ToolStrip1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BreakPointSlider)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // CodeTip
            // 
            this.CodeTip.AutoPopDelay = 3000;
            this.CodeTip.InitialDelay = 0;
            this.CodeTip.IsBalloon = true;
            this.CodeTip.OwnerDraw = true;
            this.CodeTip.ReshowDelay = 100;
            this.CodeTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.CodeTip.ToolTipTitle = "G-code source";
            // 
            // rmbView
            // 
            this.rmbView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFit,
            this.mnuFence,
            this.mnuPan,
            this.mnuRotate,
            this.mnuZoom,
            this.mnuSelect});
            this.rmbView.Name = "rmbView";
            this.rmbView.Size = new System.Drawing.Size(132, 136);
            // 
            // mnuFit
            // 
            this.mnuFit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.mnuFit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(153)))), ((int)(((byte)(214)))));
            this.mnuFit.Image = global::CNCViewer.Properties.Resources.ViewFit;
            this.mnuFit.ImageTransparentColor = System.Drawing.Color.AliceBlue;
            this.mnuFit.Name = "mnuFit";
            this.mnuFit.Size = new System.Drawing.Size(131, 22);
            this.mnuFit.Tag = "Fit";
            this.mnuFit.Text = "Origem";
            this.mnuFit.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // mnuFence
            // 
            this.mnuFence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.mnuFence.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(153)))), ((int)(((byte)(214)))));
            this.mnuFence.Image = global::CNCViewer.Properties.Resources.ViewFence;
            this.mnuFence.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mnuFence.Name = "mnuFence";
            this.mnuFence.Size = new System.Drawing.Size(131, 22);
            this.mnuFence.Tag = "Fence";
            this.mnuFence.Text = "Marcar";
            this.mnuFence.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // mnuPan
            // 
            this.mnuPan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.mnuPan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(153)))), ((int)(((byte)(214)))));
            this.mnuPan.Image = global::CNCViewer.Properties.Resources.ViewPan;
            this.mnuPan.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mnuPan.Name = "mnuPan";
            this.mnuPan.Size = new System.Drawing.Size(131, 22);
            this.mnuPan.Tag = "Pan";
            this.mnuPan.Text = "Translação";
            this.mnuPan.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // mnuRotate
            // 
            this.mnuRotate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.mnuRotate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(153)))), ((int)(((byte)(214)))));
            this.mnuRotate.Image = global::CNCViewer.Properties.Resources.ViewRotate;
            this.mnuRotate.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mnuRotate.Name = "mnuRotate";
            this.mnuRotate.Size = new System.Drawing.Size(131, 22);
            this.mnuRotate.Tag = "Rotate";
            this.mnuRotate.Text = "Rotação";
            this.mnuRotate.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // mnuZoom
            // 
            this.mnuZoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.mnuZoom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(153)))), ((int)(((byte)(214)))));
            this.mnuZoom.Image = global::CNCViewer.Properties.Resources.ViewZoom;
            this.mnuZoom.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mnuZoom.Name = "mnuZoom";
            this.mnuZoom.Size = new System.Drawing.Size(131, 22);
            this.mnuZoom.Tag = "Zoom";
            this.mnuZoom.Text = "Zoom";
            this.mnuZoom.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // mnuSelect
            // 
            this.mnuSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.mnuSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(153)))), ((int)(((byte)(214)))));
            this.mnuSelect.Image = global::CNCViewer.Properties.Resources._Select;
            this.mnuSelect.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mnuSelect.Name = "mnuSelect";
            this.mnuSelect.Size = new System.Drawing.Size(131, 22);
            this.mnuSelect.Tag = "Select";
            this.mnuSelect.Text = "Selecionar";
            this.mnuSelect.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // OpenFileDialog1
            // 
            this.OpenFileDialog1.Title = "Open File";
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbOpen.Image = global::CNCViewer.Properties.Resources.OpenFolder;
            this.tsbOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(40, 22);
            this.tsbOpen.Text = "Open";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // tscboMachines
            // 
            this.tscboMachines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscboMachines.Name = "tscboMachines";
            this.tscboMachines.Padding = new System.Windows.Forms.Padding(1);
            this.tscboMachines.Size = new System.Drawing.Size(268, 25);
            this.tscboMachines.SelectedIndexChanged += new System.EventHandler(this.TscboMachines_SelectedIndexChanged);
            this.tscboMachines.Click += new System.EventHandler(this.tscboMachines_Click);
            // 
            // tsbDisplay
            // 
            this.tsbDisplay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDisplay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRapidLines,
            this.mnuRapidPoints,
            this.mnuAxisLines,
            this.mnuAxisindicator});
            this.tsbDisplay.Image = global::CNCViewer.Properties.Resources.EditInformation;
            this.tsbDisplay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbDisplay.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbDisplay.Name = "tsbDisplay";
            this.tsbDisplay.Size = new System.Drawing.Size(58, 22);
            this.tsbDisplay.Text = "&Display";
            this.tsbDisplay.Click += new System.EventHandler(this.tsbDisplay_Click);
            // 
            // mnuRapidLines
            // 
            this.mnuRapidLines.Checked = true;
            this.mnuRapidLines.CheckOnClick = true;
            this.mnuRapidLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuRapidLines.Name = "mnuRapidLines";
            this.mnuRapidLines.Size = new System.Drawing.Size(145, 22);
            this.mnuRapidLines.Text = "Rapid &Lines";
            this.mnuRapidLines.CheckedChanged += new System.EventHandler(this.DisplayCheckChanged);
            // 
            // mnuRapidPoints
            // 
            this.mnuRapidPoints.Checked = true;
            this.mnuRapidPoints.CheckOnClick = true;
            this.mnuRapidPoints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuRapidPoints.Name = "mnuRapidPoints";
            this.mnuRapidPoints.Size = new System.Drawing.Size(145, 22);
            this.mnuRapidPoints.Text = "Rapid &Points";
            this.mnuRapidPoints.CheckedChanged += new System.EventHandler(this.DisplayCheckChanged);
            // 
            // mnuAxisLines
            // 
            this.mnuAxisLines.Checked = true;
            this.mnuAxisLines.CheckOnClick = true;
            this.mnuAxisLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuAxisLines.Name = "mnuAxisLines";
            this.mnuAxisLines.Size = new System.Drawing.Size(145, 22);
            this.mnuAxisLines.Text = "&Axis Lines";
            this.mnuAxisLines.CheckedChanged += new System.EventHandler(this.DisplayCheckChanged);
            // 
            // mnuAxisindicator
            // 
            this.mnuAxisindicator.Checked = true;
            this.mnuAxisindicator.CheckOnClick = true;
            this.mnuAxisindicator.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuAxisindicator.Name = "mnuAxisindicator";
            this.mnuAxisindicator.Size = new System.Drawing.Size(145, 22);
            this.mnuAxisindicator.Text = "Axis &Indicator";
            this.mnuAxisindicator.CheckedChanged += new System.EventHandler(this.DisplayCheckChanged);
            // 
            // tsbToolsFilter
            // 
            this.tsbToolsFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbToolsFilter.Image = global::CNCViewer.Properties.Resources.ToolLayers;
            this.tsbToolsFilter.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbToolsFilter.Name = "tsbToolsFilter";
            this.tsbToolsFilter.Size = new System.Drawing.Size(71, 22);
            this.tsbToolsFilter.Text = "Tool Layers";
            this.tsbToolsFilter.Click += new System.EventHandler(this.tsbToolsFilter_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbPan
            // 
            this.tsbPan.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbPan.CheckOnClick = true;
            this.tsbPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPan.Image = global::CNCViewer.Properties.Resources.ViewPan;
            this.tsbPan.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbPan.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbPan.Name = "tsbPan";
            this.tsbPan.Size = new System.Drawing.Size(31, 22);
            this.tsbPan.Tag = "Pan";
            this.tsbPan.Text = "Pan";
            this.tsbPan.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // tsbZoom
            // 
            this.tsbZoom.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbZoom.CheckOnClick = true;
            this.tsbZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbZoom.Image = global::CNCViewer.Properties.Resources.ViewZoom;
            this.tsbZoom.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbZoom.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbZoom.Name = "tsbZoom";
            this.tsbZoom.Size = new System.Drawing.Size(43, 22);
            this.tsbZoom.Tag = "Zoom";
            this.tsbZoom.Text = "Zoom";
            this.tsbZoom.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // tsbRotate
            // 
            this.tsbRotate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbRotate.CheckOnClick = true;
            this.tsbRotate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRotate.Image = global::CNCViewer.Properties.Resources.ViewRotate;
            this.tsbRotate.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbRotate.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbRotate.Name = "tsbRotate";
            this.tsbRotate.Size = new System.Drawing.Size(45, 22);
            this.tsbRotate.Tag = "Rotate";
            this.tsbRotate.Text = "Rotate";
            this.tsbRotate.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // tsbFence
            // 
            this.tsbFence.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbFence.CheckOnClick = true;
            this.tsbFence.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbFence.Image = global::CNCViewer.Properties.Resources.ViewFence;
            this.tsbFence.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbFence.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbFence.Name = "tsbFence";
            this.tsbFence.Size = new System.Drawing.Size(42, 22);
            this.tsbFence.Tag = "Fence";
            this.tsbFence.Text = "Fence";
            this.tsbFence.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // tsbFit
            // 
            this.tsbFit.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbFit.Image = global::CNCViewer.Properties.Resources.ViewFit;
            this.tsbFit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbFit.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.tsbFit.Name = "tsbFit";
            this.tsbFit.Size = new System.Drawing.Size(68, 22);
            this.tsbFit.Tag = "Translação";
            this.tsbFit.Text = "Translação";
            this.tsbFit.ToolTipText = "View Fit [Shift + Click All Views]";
            this.tsbFit.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolStripSeparator2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbView
            // 
            this.tsbView.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTop,
            this.mnuFront,
            this.mnuRight,
            this.mnuIsometric});
            this.tsbView.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.tsbView.Name = "tsbView";
            this.tsbView.Size = new System.Drawing.Size(13, 22);
            this.tsbView.Text = "&View";
            // 
            // mnuTop
            // 
            this.mnuTop.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuTop.Name = "mnuTop";
            this.mnuTop.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.mnuTop.Size = new System.Drawing.Size(160, 22);
            this.mnuTop.Tag = "Top";
            this.mnuTop.Text = "&Top";
            this.mnuTop.Click += new System.EventHandler(this.MnuViewOrient_Click);
            // 
            // mnuFront
            // 
            this.mnuFront.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuFront.Name = "mnuFront";
            this.mnuFront.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.mnuFront.Size = new System.Drawing.Size(160, 22);
            this.mnuFront.Tag = "Front";
            this.mnuFront.Text = "&Front";
            this.mnuFront.Click += new System.EventHandler(this.MnuViewOrient_Click);
            // 
            // mnuRight
            // 
            this.mnuRight.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuRight.Name = "mnuRight";
            this.mnuRight.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mnuRight.Size = new System.Drawing.Size(160, 22);
            this.mnuRight.Tag = "Right";
            this.mnuRight.Text = "&Right";
            this.mnuRight.Click += new System.EventHandler(this.MnuViewOrient_Click);
            // 
            // mnuIsometric
            // 
            this.mnuIsometric.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mnuIsometric.Name = "mnuIsometric";
            this.mnuIsometric.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.mnuIsometric.Size = new System.Drawing.Size(160, 22);
            this.mnuIsometric.Tag = "ISO";
            this.mnuIsometric.Text = "&Isometric";
            this.mnuIsometric.Click += new System.EventHandler(this.MnuViewOrient_Click);
            // 
            // tsbSelect
            // 
            this.tsbSelect.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbSelect.Checked = true;
            this.tsbSelect.CheckOnClick = true;
            this.tsbSelect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSelect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSelect.Name = "tsbSelect";
            this.tsbSelect.Size = new System.Drawing.Size(23, 22);
            this.tsbSelect.Tag = "Select";
            this.tsbSelect.Text = "Select";
            this.tsbSelect.Click += new System.EventHandler(this.ViewButtonClicked);
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpen,
            this.tscboMachines,
            this.tsbDisplay,
            this.tsbToolsFilter,
            this.ToolStripSeparator1,
            this.tsbPan,
            this.tsbZoom,
            this.tsbRotate,
            this.tsbFence,
            this.tsbFit,
            this.ToolStripSeparator2,
            this.tsbView,
            this.tsbSelect});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 737);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ToolStrip1.Size = new System.Drawing.Size(1084, 25);
            this.ToolStrip1.TabIndex = 7;
            this.ToolStrip1.Text = "ToolStrip1";
            this.ToolStrip1.Visible = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.233117F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.7308F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.12859F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.pictureBox2, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.9863F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1078, 51);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(81, 45);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::CNCViewer.Properties.Resources.M4;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(75, 39);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(153)))), ((int)(((byte)(214)))));
            this.label1.Location = new System.Drawing.Point(91, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 51);
            this.label1.TabIndex = 1;
            this.label1.Text = "TECPOST";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::CNCViewer.Properties.Resources.fundo42;
            this.pictureBox2.Location = new System.Drawing.Point(206, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(869, 45);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton6,
            this.toolStripSeparator3,
            this.toolStripButton5});
            this.toolStrip2.Location = new System.Drawing.Point(909, 36);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(178, 25);
            this.toolStrip2.TabIndex = 16;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::CNCViewer.Properties.Resources.ViewIso1;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "ISO";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::CNCViewer.Properties.Resources.ViewFront1;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Frontal";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::CNCViewer.Properties.Resources.ViewRight1;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Direta";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::CNCViewer.Properties.Resources.ViewTop1;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Topo";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::CNCViewer.Properties.Resources.expandir;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Text = "toolStripButton6";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::CNCViewer.Properties.Resources.OpenFolder1;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // materialRaisedButton1
            // 
            this.materialRaisedButton1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.materialRaisedButton1.Depth = 0;
            this.materialRaisedButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.materialRaisedButton1.Location = new System.Drawing.Point(438, 27);
            this.materialRaisedButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton1.Name = "materialRaisedButton1";
            this.materialRaisedButton1.Primary = true;
            this.materialRaisedButton1.Size = new System.Drawing.Size(208, 31);
            this.materialRaisedButton1.TabIndex = 17;
            this.materialRaisedButton1.Text = "Fechar exibição";
            this.materialRaisedButton1.UseVisualStyleBackColor = true;
            this.materialRaisedButton1.Click += new System.EventHandler(this.materialRaisedButton1_Click);
            // 
            // BreakPointSlider
            // 
            this.BreakPointSlider.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BreakPointSlider.BackColor = System.Drawing.Color.White;
            this.BreakPointSlider.Location = new System.Drawing.Point(0, 668);
            this.BreakPointSlider.Margin = new System.Windows.Forms.Padding(0);
            this.BreakPointSlider.Name = "BreakPointSlider";
            this.BreakPointSlider.Size = new System.Drawing.Size(1084, 32);
            this.BreakPointSlider.TabIndex = 8;
            this.BreakPointSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.BreakPointSlider.Scroll += new System.EventHandler(this.BreakPointSlider_Scroll_1);
            this.BreakPointSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.layerSelect);
            // 
            // MG_Viewer1
            // 
            this.MG_Viewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MG_Viewer1.AxisIndicatorScale = 0.75F;
            this.MG_Viewer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(25)))));
            this.MG_Viewer1.BreakPoint = -1;
            this.MG_Viewer1.ContextMenuStrip = this.rmbView;
            this.MG_Viewer1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MG_Viewer1.DrawRapidPoints = false;
            this.MG_Viewer1.DynamicViewManipulation = true;
            this.MG_Viewer1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.MG_Viewer1.FourthAxis = 0F;
            this.MG_Viewer1.Location = new System.Drawing.Point(0, 71);
            this.MG_Viewer1.Margin = new System.Windows.Forms.Padding(0);
            this.MG_Viewer1.Name = "MG_Viewer1";
            this.MG_Viewer1.Pitch = 0F;
            this.MG_Viewer1.Roll = 0F;
            this.MG_Viewer1.RotaryType = MacGen.RotaryMotionType.BMC;
            this.MG_Viewer1.Size = new System.Drawing.Size(1084, 597);
            this.MG_Viewer1.TabIndex = 0;
            this.MG_Viewer1.ViewManipMode = MacGen.MG_CS_BasicViewer.ManipMode.SELECTION;
            this.MG_Viewer1.Yaw = 0F;
            this.MG_Viewer1.Enter += new System.EventHandler(this.ViewportActivated);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.BreakPointSlider, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.MG_Viewer1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox3, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 64);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.150945F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.02265F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.33748F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.488927F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1084, 700);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.Image = global::CNCViewer.Properties.Resources.fundo44;
            this.pictureBox3.Location = new System.Drawing.Point(3, 60);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(1078, 8);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
            // 
            // FrmViewer
            // 
            this.ClientSize = new System.Drawing.Size(1084, 762);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.materialRaisedButton1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.ToolStrip1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(68, 66);
            this.MinimumSize = new System.Drawing.Size(425, 225);
            this.Name = "FrmViewer";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmViewer_FormClosing);
            this.Load += new System.EventHandler(this.FrmViewer_Load);
            this.ResizeEnd += new System.EventHandler(this.FrmViewer_ResizeEnd);
            this.rmbView.ResumeLayout(false);
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BreakPointSlider)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ToolTip CodeTip;
        internal System.Windows.Forms.ContextMenuStrip rmbView;
        internal System.Windows.Forms.ToolStripMenuItem mnuFit;
        internal System.Windows.Forms.ToolStripMenuItem mnuFence;
        internal System.Windows.Forms.ToolStripMenuItem mnuPan;
        internal System.Windows.Forms.ToolStripMenuItem mnuRotate;
        internal System.Windows.Forms.ToolStripMenuItem mnuZoom;
        internal System.Windows.Forms.ToolStripMenuItem mnuSelect;
        internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
        internal System.Windows.Forms.ToolStripButton tsbOpen;
        internal System.Windows.Forms.ToolStripComboBox tscboMachines;
        internal System.Windows.Forms.ToolStripDropDownButton tsbDisplay;
        internal System.Windows.Forms.ToolStripMenuItem mnuRapidLines;
        internal System.Windows.Forms.ToolStripMenuItem mnuRapidPoints;
        internal System.Windows.Forms.ToolStripMenuItem mnuAxisLines;
        internal System.Windows.Forms.ToolStripMenuItem mnuAxisindicator;
        internal System.Windows.Forms.ToolStripButton tsbToolsFilter;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripButton tsbPan;
        internal System.Windows.Forms.ToolStripButton tsbZoom;
        internal System.Windows.Forms.ToolStripButton tsbRotate;
        internal System.Windows.Forms.ToolStripButton tsbFence;
        internal System.Windows.Forms.ToolStripButton tsbFit;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripDropDownButton tsbView;
        internal System.Windows.Forms.ToolStripMenuItem mnuTop;
        internal System.Windows.Forms.ToolStripMenuItem mnuFront;
        internal System.Windows.Forms.ToolStripMenuItem mnuRight;
        internal System.Windows.Forms.ToolStripMenuItem mnuIsometric;
        internal System.Windows.Forms.ToolStripButton tsbSelect;
        internal System.Windows.Forms.ToolStrip ToolStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        internal System.Windows.Forms.TrackBar BreakPointSlider;
        private System.Windows.Forms.PictureBox pictureBox2;
        internal MacGen.MG_CS_BasicViewer MG_Viewer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}

