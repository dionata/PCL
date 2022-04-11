using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using MacGen;
using System.IO;
using MaterialSkin.Controls;
using OpenCL;
using OpenCL.Net;

namespace CNCViewer
{

    public partial class FrmViewer : MaterialForm
    {
        private string mCncFile;
        private clsProcessor mProcessor = clsProcessor.Instance();
        private clsSettings mSetup = clsSettings.Instance();
        private MG_CS_BasicViewer mViewer;
        private static bool WindowState_ = false;

        public FrmViewer(string path)
        {
            InitializeComponent();
            mViewer = this.MG_Viewer1;
            mProcessor.OnAddBlock += new clsProcessor.OnAddBlockEventHandler(MProcessor_OnAddBlock);

            MG_CS_BasicViewer.OnSelection += new MG_CS_BasicViewer.OnSelectionEventHandler(MViewer_OnSelection);
            MG_CS_BasicViewer.MouseLocation += new MG_CS_BasicViewer.MouseLocationEventHandler(MViewer_MouseLocation);
            MG_CS_BasicViewer.OnStatus += new MG_CS_BasicViewer.OnStatusEventHandler(MViewer_OnStatus);

            mSetup.MachineActivated += new clsSettings.MachineActivatedEventHandler(MSetup_MachineActivated);
            mSetup.MachineAdded += new clsSettings.MachineAddedEventHandler(MSetup_MachineAdded);
            mSetup.MachineDeleted += new clsSettings.MachineDeletedEventHandler(MSetup_MachineDeleted);
            mSetup.MachineLoaded += new clsSettings.MachineLoadedEventHandler(MSetup_MachineLoaded);
            mSetup.MachineMatched += new clsSettings.MachineMatchedEventHandler(MSetup_MachineMatched);

            mSetup.LoadAllMachines(Directory.GetCurrentDirectory() + "\\Data");
            mProcessor.Init(mSetup.Machine);

            this.Cursor = Cursors.WaitCursor;
            string path_ = Directory.GetCurrentDirectory() + "\\" + path;

            try
            {
                OpenFile(@path_);
            }
            catch
            {
            }

            this.Cursor = Cursors.Default;
        }

        private void FrmViewer_Load(object sender, System.EventArgs e)
        {

        }

        private void FrmViewer_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            
        }

        private void OpenFile(string fileName)
        {
            long[] ticks = new long[2];
            mCncFile = fileName;
            mSetup.MatchMachineToFile(mCncFile);

            ProcessFile(mCncFile);
            mViewer.BreakPoint = MG_CS_BasicViewer.MotionBlocks.Count - 1;

            mViewer.Pitch = mSetup.Machine.ViewAngles[0];
            mViewer.Roll = mSetup.Machine.ViewAngles[1];
            mViewer.Yaw = mSetup.Machine.ViewAngles[2];
            mViewer.Init();

            ticks[0] = DateTime.Now.Ticks;
            MG_Viewer1.FindExtents();
            ticks[1] = DateTime.Now.Ticks;
            MG_Viewer1.DynamicViewManipulation = (ticks[1] - ticks[0]) < 20000; //dio
            mViewer.Redraw(true);
        }

        private void ProcessFile(string fileName)
        {
            if (fileName == null)
            {
                return;
            }
            if (!System.IO.File.Exists(fileName))
            {
                return;
            }
            MG_CS_BasicViewer.MotionBlocks.Clear();
            mProcessor.Init(mSetup.Machine);
            mProcessor.ProcessFile(fileName, MG_CS_BasicViewer.MotionBlocks);

            BreakPointSlider.Maximum = MG_CS_BasicViewer.MotionBlocks.Count - 1;
            if (mViewer.BreakPoint > MG_CS_BasicViewer.MotionBlocks.Count - 1)
            {
                mViewer.BreakPoint = MG_CS_BasicViewer.MotionBlocks.Count - 1;
            }
            mViewer.GatherTools();
        }

        private void MnuViewOrient_Click(object sender, System.EventArgs e)
        {
            switch (((System.Windows.Forms.ToolStripMenuItem)sender).Tag.ToString())
            {
                case "Top":
                    mViewer.Pitch = 0;
                    mViewer.Roll = 0;
                    mViewer.Yaw = 0;
                    break;
                case "Front":
                    mViewer.Pitch = 270;
                    mViewer.Roll = 0;
                    mViewer.Yaw = 360;
                    break;
                case "Right":
                    mViewer.Pitch = 270;
                    mViewer.Roll = 0;
                    mViewer.Yaw = 270;
                    break;
                case "ISO":
                    mViewer.Pitch = 315;
                    mViewer.Roll = 0;
                    mViewer.Yaw = 315;
                    break;
            }
            mViewer.FindExtents();
            mViewer.Redraw(true);
        }

        private void orientacao(string tipo)
        {
            switch (tipo)
            {
                case "Top":
                    mViewer.Pitch = 0;
                    mViewer.Roll = 0;
                    mViewer.Yaw = 0;
                    break;
                case "Front":
                    mViewer.Pitch = 270;
                    mViewer.Roll = 0;
                    mViewer.Yaw = 360;
                    break;
                case "Right":
                    mViewer.Pitch = 270;
                    mViewer.Roll = 0;
                    mViewer.Yaw = 270;
                    break;
                case "ISO":
                    mViewer.Pitch = 315;
                    mViewer.Roll = 0;
                    mViewer.Yaw = 315;
                    break;
            }
            mViewer.FindExtents();
            mViewer.Redraw(true);

        }

        private void MViewer_MouseLocation(float x, float y)
        {
        }

        private void MProcessor_OnAddBlock(int idx, int ct)
        {
            try
            {
                if (ct > 10000)
                {
                    if (1000 % idx == 0)
                    {
                        mViewer.FindExtents();
                        mViewer.Redraw(true);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ViewportActivated(object sender, System.EventArgs e)
        {
            mViewer = (MG_CS_BasicViewer)sender;
        }

        private void SetDefaultViews()
        {
            MG_Viewer1.Pitch = 0f;
            MG_Viewer1.Roll = 0f;
            MG_Viewer1.Yaw = 0f;
            MG_Viewer1.FindExtents();
        }

        private void MViewer_OnSelection(System.Collections.Generic.List<clsMotionRecord> hits)
        {
            string[] tipString = new string[hits.Count];
            for (int r = 0; r <= hits.Count - 1; r++)
            {
                tipString[r] = hits[r].Codestring;
            }
            this.CodeTip.SetToolTip(mViewer, string.Join(System.Environment.NewLine, tipString));
        }

        private void MViewer_OnStatus(string msg, int index, int max)
        {
        }

        private void MSetup_MachineActivated(clsMachine m)
        {
            {
                MG_Viewer1.RotaryDirection = (RotaryDirection)m.RotaryDir;
                MG_Viewer1.RotaryPlane = (Axis)m.RotaryAxis;
                MG_Viewer1.RotaryType = (RotaryMotionType)m.RotaryType;
                MG_Viewer1.ViewManipMode = MG_CS_BasicViewer.ManipMode.SELECTION;
                MG_Viewer1.FindExtents();
                mViewer.Redraw(true);
            }
        }

        private void MSetup_MachineAdded(clsMachine m)
        {
            tscboMachines.Items.Add(m.Name);
        }

        private void MSetup_MachineDeleted(string name)
        {
            tscboMachines.Items.RemoveAt(tscboMachines.FindStringExact(name));
        }

        private void MSetup_MachineLoaded(clsMachine m)
        {
            tscboMachines.Items.Add(m.Name);
        }

        private void MSetup_MachineMatched(clsMachine m)
        {
            tscboMachines.SelectedIndex = tscboMachines.FindStringExact(m.Name);
        }

        private void MSetup_MachinesCleared()
        {
            tscboMachines.Items.Clear();
        }


        private void BreakPointSlider_ValueChanged(object sender, EventArgs e)
        {
            //mViewer.BreakPoint = BreakPointSlider.Value;
            //mViewer.Redraw(true);//dio
        }

        private void ViewButtonClicked(object sender, EventArgs e)
        {
            string tag = sender.GetType().GetProperty("Tag").GetValue(sender, null).ToString();
            switch (tag)
            {
                case "Fit":
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        MG_Viewer1.FindExtents();
                    }
                    else
                    {
                        mViewer.FindExtents();
                    }

                    break;
                case "Pan":
                    mViewer.ViewManipMode = MG_CS_BasicViewer.ManipMode.PAN;
                    tsbPan.Checked = true;
                    tsbRotate.Checked = false;
                    tsbZoom.Checked = false;
                    tsbFence.Checked = false;
                    tsbSelect.Checked = false;
                    break;
                case "Fence":
                    mViewer.ViewManipMode = MG_CS_BasicViewer.ManipMode.FENCE;
                    tsbFence.Checked = true;
                    tsbRotate.Checked = false;
                    tsbZoom.Checked = false;
                    tsbPan.Checked = false;
                    tsbSelect.Checked = false;
                    break;
                case "Zoom":
                    mViewer.ViewManipMode = MG_CS_BasicViewer.ManipMode.ZOOM;
                    tsbZoom.Checked = true;
                    tsbRotate.Checked = false;
                    tsbFence.Checked = false;
                    tsbPan.Checked = false;
                    tsbSelect.Checked = false;
                    break;
                case "Rotate":
                    mViewer.ViewManipMode = MG_CS_BasicViewer.ManipMode.ROTATE;
                    tsbRotate.Checked = true;
                    tsbZoom.Checked = false;
                    tsbFence.Checked = false;
                    tsbPan.Checked = false;
                    tsbSelect.Checked = false;
                    break;
                case "Select":
                    mViewer.ViewManipMode = MG_CS_BasicViewer.ManipMode.SELECTION;
                    tsbRotate.Checked = false;
                    tsbZoom.Checked = false;
                    tsbFence.Checked = false;
                    tsbPan.Checked = false;
                    tsbSelect.Checked = true;
                    break;
            }
        }

        private void ScreensClicked(object sender, EventArgs e)
        {
        }

        private void TscboMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSetup.MachineName = tscboMachines.SelectedItem.ToString();
        }

        private void FrmViewer_ResizeEnd(object sender, EventArgs e)
        {
            MG_Viewer1.FindExtents();
            mViewer.Redraw(true);
        }

        /*abrir*/
        private void tsbOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog1.ShowDialog();
            this.Refresh();
            if (OpenFileDialog1.FileName.Length > 0)
            {
                OpenFile(OpenFileDialog1.FileName);
            }
        }

        private void DisplayCheckChanged(object sender, EventArgs e)
        {
            if (mViewer == null) return;

            mViewer.DrawRapidLines = mnuRapidLines.Checked;
            mViewer.DrawRapidPoints = mnuRapidPoints.Checked;
            mViewer.DrawAxisLines = mnuAxisLines.Checked;
            mViewer.DrawAxisIndicator = mnuAxisindicator.Checked;
            mViewer.Redraw(true);
        }

        private void tsbToolsFilter_Click(object sender, EventArgs e)
        {
            TreeNode nd = default(TreeNode);
            using (FrmToolLayers frm = new FrmToolLayers())
            {
                frm.tvTools.Nodes.Clear();
                foreach (clsToolLayer tl in MG_CS_BasicViewer.ToolLayers.Values)
                {
                    nd = frm.tvTools.Nodes.Add("Tool " + tl.Number.ToString());
                    nd.ForeColor = tl.Color;
                    nd.Checked = !tl.Hidden;
                    nd.Tag = tl;
                }
                frm.tvTools.BackColor = this.MG_Viewer1.BackColor;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Control.MousePosition;
                frm.ShowDialog();
            }
            mViewer.Redraw(true);
        }

        private void tsbWebCheck_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Properties.Resources.datBasicViewerUrl + "1.0");
        }

        private void tscboMachines_Click(object sender, EventArgs e)
        {
        }

        private void tsbDisplay_Click(object sender, EventArgs e)
        {
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tblScreens_Paint(object sender, PaintEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void BreakPointSlider_Scroll(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void layerSelect(object sender, MouseEventArgs e)
        {
            //mViewer.BreakPoint = BreakPointSlider.Value;
            //mViewer.Redraw(true);//dio
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openModel = new OpenFileDialog();
            openModel.Filter = "*Carregar arquivo|*.nc; *.gcode;";
            openModel.ShowDialog();
            OpenFile(@openModel.FileName);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //isometrico
            mViewer.Pitch = 315;
            mViewer.Roll = 0;
            mViewer.Yaw = 315;
            mViewer.Redraw(true);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //frontal
            mViewer.Pitch = 270;
            mViewer.Roll = 0;
            mViewer.Yaw = 360;
            mViewer.Redraw(true);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //"Right":
            mViewer.Pitch = 270;
            mViewer.Roll = 0;
            mViewer.Yaw = 270;
            mViewer.Redraw(true);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //"Top":
            mViewer.Pitch = 0;
            mViewer.Roll = 0;
            mViewer.Yaw = 0;
            mViewer.Redraw(true);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
                     
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (!WindowState_)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
            WindowState_ = !WindowState_;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BreakPointSlider_Scroll_1(object sender, EventArgs e)
        {
            mViewer.BreakPoint = BreakPointSlider.Value;
            mViewer.Redraw(true);//dio
        }
    }
}  