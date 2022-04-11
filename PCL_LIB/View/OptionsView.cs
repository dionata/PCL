
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
using g3;

namespace PCLLib
{
    public partial class SettingView : MaterialForm
    {
        public OpenGLControl Parent;
        private int indice;
        private string selectView_;

        public SettingView(OpenGLControl myParent, int indice_, string selectView, string nome)
        {
            indice = indice_;
            string selectView_ = selectView;
            this.Parent = myParent;
            InitializeComponent();

            this.textBox9.Text = GLSettings.scaleX.ToString();
            this.textBox8.Text = GLSettings.scaleY.ToString();
            this.textBox6.Text = GLSettings.scaleZ.ToString();

            this.textBox1.Text = GLSettings.PointSize_.ToString("0.00");
            this.textBox2.Text = GLSettings.PointSizeAxis.ToString("0.00");
            this.modeloSelecionado.Text = nome;
            this.textBox3.Text = 1.ToString();//GLSettings.scaleX.ToString();
            this.textBox4.Text = 1.ToString();//GLSettings.scaleY.ToString();
            this.textBox5.Text = 1.ToString();//GLSettings.scaleZ.ToString();      

           // this.textBox10.Text = GLSettings.Bloco1XEncosto.ToString();
           // this.textBox11.Text = GLSettings.Bloco1YEncosto.ToString();
           // this.textBox12.Text = GLSettings.Bloco1ZEncosto.ToString();

            this.textBox13.Text = GLSettings.apagadorX.ToString();
            this.textBox14.Text = GLSettings.apagadorY.ToString();
            this.textBox15.Text = GLSettings.apagadorZ.ToString();

            
        }

        private void materialRaisedButton1_Click_1(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void materialRaisedButton3_Click_1(object sender, EventArgs e)
        {
            Parent.ChangeBackColor();
        }

        private void materialRaisedButton2_Click_1(object sender, EventArgs e)
        {
            Parent.ChangeModelColor();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GLSettings.PointSize_ = Convert.ToSingle(this.textBox1.Text);
            Parent.RedrawModels(indice);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            GLSettings.PointSizeAxis = Convert.ToSingle(textBox2.Text);
            Parent.RedrawModels(indice);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void modeloSelecionado_Click(object sender, EventArgs e)
        {

        }

        private void btScalaX_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btScalaY_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btScalaZ_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            GLSettings.scaleX = Convert.ToDouble(textBox3.Text);
            //Parent.RedrawModels(indice);
        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {
            GLSettings.scaleY = Convert.ToDouble(textBox4.Text);
            //Parent.RedrawModels(indice);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            GLSettings.scaleZ = Convert.ToDouble(textBox5.Text);
            //  Parent.RedrawModels(indice);
        }

        private void materialLabel11_Click(object sender, EventArgs e)
        {

        }       

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            GLSettings.apagadorX = Convert.ToDouble(this.textBox13.Text);
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            GLSettings.apagadorY = Convert.ToDouble(this.textBox14.Text);
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            GLSettings.apagadorZ = Convert.ToDouble(this.textBox14.Text);
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {
        }

        private void materialRaisedButton6_Click(object sender, EventArgs e)
        {
            Utils.GeneralTools tools = new Utils.GeneralTools();
            tools.Scale(Parent, indice, selectView_, Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text), Convert.ToDouble(textBox5.Text));
        }
    }
}
