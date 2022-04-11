
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using PCLLib;
using System.Windows.Media.Media3D;
using OpenTK;
using MaterialSkin.Controls;
using System.IO;
using PCLLib.Utils;
//using PCLLib.View;

namespace PCLLib
{
    public partial class OptionSmoothView : MaterialForm
    {
        private OpenGLControl OpenGLControl;
        private int indice;
        private string selectView;
        private float smoothSpeedT = 0;

        public OptionSmoothView(OpenGLControl OpenGLControl_, int indice_, string selectView_)
        {
            InitializeComponent();
            OpenGLControl = OpenGLControl_;
            trackBar1.Value = 0;
            indice = indice_;
            selectView = selectView_;
            //smoothSpeedT = (trackBar1.Value) / 100;
            //label4.Text = smoothSpeedT.ToString();

            switch(GLSettings.filterSmoothing_type)
            {
                case "Cotan":
                    materialRadioButton1.Checked = true;
                    materialRadioButton2.Checked = false;
                    materialRadioButton3.Checked = false;
                    break;
                case "Uniform":
                    materialRadioButton1.Checked = false;
                    materialRadioButton2.Checked = true;
                    materialRadioButton3.Checked = false;
                    break;
                case "MeanValue":
                    materialRadioButton1.Checked = false;
                    materialRadioButton2.Checked = false;
                    materialRadioButton3.Checked = true;
                    break;

            }
            double var = GLSettings.filterSmoothing_smoothSpeedT_ * 100;
            trackBar1.Value = (int)var;
            label4.Text = GLSettings.filterSmoothing_smoothSpeedT_.ToString();
        }

        private void suavizar()
        {
            Filters filter = new Filters();

            GLSettings.filterSmoothing_smoothSpeedT_ = smoothSpeedT;

            this.Cursor = Cursors.WaitCursor;
            if (materialRadioButton1.Checked)
            {               
                GLSettings.filterSmoothing_type = "Cotan";
                filter.FilterSmoothing();
            }
            else if(materialRadioButton2.Checked)
            {                
                GLSettings.filterSmoothing_type = "Uniform";
                filter.FilterSmoothing();
            }
            else if (materialRadioButton3.Checked)
            {                
                GLSettings.filterSmoothing_type = "MeanValue";
                filter.FilterSmoothing();
            }

            OpenGLControl.RefreshShowModels(indice, selectView, "*", GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
            GLSettings.atualizarProjecao(OpenGLControl);
            this.Cursor = Cursors.Default; 
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            suavizar();
            this.Close();
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
              
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
          }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
          
        }

        private void updateInitFinishBlocos()
        {
          
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton2_Click_1(object sender, EventArgs e)
        {

        }

        private void OK_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void materialRaisedButton2_Click_2(object sender, EventArgs e)
        {
            this.Close();
        }

        private void materialRadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {          
            double teste = Convert.ToDouble(trackBar1.Value); 
            smoothSpeedT = (float)((Convert.ToDouble(trackBar1.Value)) / 100);
            label4.Text = smoothSpeedT.ToString();            
        }

        private void tableLayoutPanel5_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void materialRaisedButton1_Click_1(object sender, EventArgs e)
        {
            suavizar();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
