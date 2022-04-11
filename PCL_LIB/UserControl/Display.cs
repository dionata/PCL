using PCLLib.Properties;
using OpenCLTemplate;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace PCLLib
{
    public partial class OpenGLControl
    {
        public bool DrawAtZero = false;
        CLEnum.CLRenderStyle modelRenderStyle; // pont, wireframe etc.

        public void initGLControl()
        {
            
            this.glControl1.BackColor = Color.Black;
            
            this.glControl1.Name = "glControl1";
            this.glControl1.VSync = false;
            this.glControl1.Top = 0;
            this.glControl1.Left = 0;
            this.glControl1.Width = this.Width;
            this.glControl1.Height = this.Height;
            this.glControl1.Cursor = Cursors.Hand;// Cross;                    

            initEventHandlers();
            
            this.GLrender = new OpenGLRenderer(this.glControl1);                  
            this.GLrender.Draw("*");                    

            int num = 0;
            while (num <= 100)
            {
                num += 10;
            }
            InitialSettingsOnLoad();

        }
        private void SetComboSelection(ToolStripComboBox combo , string selection)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (combo.Items[i].ToString() == selection)
                {
                    combo.SelectedIndex = i;
                    break;
                }
            }
        }
        private void InitialSettingsOnLoad()
        { 
            selectedCamera("Rotate");
        }
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            this.glControl1.MakeCurrent();
            if (this.DrawAtZero)
            {
                this.DrawAtZero = false;
                this.GLrender.Draw("*");
            }
            else
                this.GLrender.Draw("*");
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (this.glControl1 == null)
                return;
            this.glControl1.Width = this.Width;
            this.glControl1.Height = this.Height;
            if (this.glControl1.Width < 0)
                this.glControl1.Width = 1;
            if (this.glControl1.Height < 0)
                this.glControl1.Height = 1;
            this.glControl1.MakeCurrent();
            GL.Viewport(0, 0, this.glControl1.Width, this.glControl1.Height);
            this.glControl1.Invalidate();
        }
      
    }
}
