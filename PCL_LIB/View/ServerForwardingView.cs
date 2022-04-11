
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
    public partial class ServerForwardingView : MaterialForm
    {
        private int select = 0;
        private OpenGLControl OpenGLControl;

        public ServerForwardingView(OpenGLControl OpenGLControl_)
        {
            InitializeComponent();
            OpenGLControl = OpenGLControl_;
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
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

        private void materialRaisedButton2_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton1_Click_1(object sender, EventArgs e)
        {
            switch(select)
            {
                case 0:
                    GLSettings.gerarAssento = true;
                    this.Close();
                    break;
                case 1:
                    GLSettings.gerarGcode = true;
                    this.Close();
                    break;
                case 2:
                    GLSettings.uniao = true;
                    this.Close();
                    break;
            }           
        }

        private void materialRaisedButton2_Click_2(object sender, EventArgs e)
        {
            RegisterUserView view = new RegisterUserView(OpenGLControl);
            view.Show();
        }
    }
}
