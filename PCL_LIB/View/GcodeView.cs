
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
//using PCLLib.View;

namespace PCLLib
{
    public partial class GcodeView : MaterialForm
    {      
        public GcodeView()
        {
            InitializeComponent();
            label6.Text = GLSettings.stepLayersDesbaste.ToString();
            label18.Text = GLSettings.filament_diameter_aux;
            comboBox2.SelectedItem = GLSettings.turnDirection;
            label16.Text = GLSettings.spindle_aux;
            label15.Text = GLSettings.feedrate_aux;
            comboBox1.SelectedItem = GLSettings.source;
        }

        private void materialRaisedButton1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            label15.Text = (Convert.ToDouble(trackBar4.Value) * 1000).ToString();
            label20.Text = (Convert.ToDouble(trackBar4.Value) * 1000).ToString();
            GLSettings.feedrate_aux = label15.Text;
            trackBar3.Value = trackBar4.Value;
            GLSettings.feedrate = "F" + label20.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label20.Text = (Convert.ToDouble(trackBar3.Value) * 1000).ToString();
            label15.Text = (Convert.ToDouble(trackBar3.Value) * 1000).ToString();
            trackBar4.Value = trackBar3.Value;
            GLSettings.feedrate = "F" + label20.Text;
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            label16.Text = (Convert.ToDouble(trackBar5.Value) * 1000).ToString();
            GLSettings.spindle_aux = label16.Text;
            GLSettings.spindle = "S" + GLSettings.spindle_aux;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label6.Text = (Convert.ToDouble(trackBar1.Value)).ToString();
            GLSettings.stepLayersDesbaste = Convert.ToDouble(label6.Text);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label18.Text = (Convert.ToDouble(trackBar2.Value)).ToString();
            GLSettings.filament_diameter_aux = label18.Text;
            //GLSettings.filament_diameter = ((Convert.ToDouble(GLSettings.filament_diameter_aux) * 1.7) / 6).ToString().Replace(',', '.');//default
            //GLSettings.filament_diameter = ((Convert.ToDouble(GLSettings.filament_diameter_aux) * 1.7) / 6).ToString().Replace(',', '.');//teste
            //GLSettings.nozzle_diameter = GLSettings.filament_diameter;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GLSettings.source = comboBox1.SelectedItem.ToString();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            //GLSettings.stepLayersDesbaste = Convert.ToDouble(label6.Text);
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            GLSettings.turnDirection = comboBox2.SelectedItem.ToString();
        }

        private void label18_Click(object sender, EventArgs e)
        {
           // GLSettings.filament_diameter = ((Convert.ToDouble(label18.Text) * 1.7) / 6).ToString();
           // GLSettings.nozzle_diameter = GLSettings.filament_diameter;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
