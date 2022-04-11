
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
    public partial class TutorialView : MaterialForm
    {      
        private int numberSlider = 0;

        public TutorialView()
        {
            InitializeComponent();

           this.Opacity = 0.75;

            openSlider();            
        }

        private void openSlider()
        {
            switch(numberSlider)
            {
                case 0:
                    textBox1.Text = "Conhecendo algumas funções do software";
                    pictureBox1.Image = Properties.Resources._01_1;                   
                    break;
                case 1:
                    pictureBox1.Image = Properties.Resources._01_2;                   
                    break;
                case 2:
                    pictureBox1.Image = Properties.Resources._01_3;                   
                    break;
                case 3:
                    pictureBox1.Image = Properties.Resources._01_4;                   
                    break;                                 
                case 4:
                    pictureBox1.Image = Properties.Resources._01_5_1;
                    break;
                case 5:
                    pictureBox1.Image = Properties.Resources._01_6_1;                    
                    break;
                case 6:
                    pictureBox1.Image = Properties.Resources._01_7_1;                   
                    break;
                case 7:
                    pictureBox1.Image = Properties.Resources._01_8_1;                   
                    break;
                case 8:
                    pictureBox1.Image = Properties.Resources._01_9_1;                    
                    break;
                case 9:
                    pictureBox1.Image = Properties.Resources._01_10_1;                  
                    break;
                case 10:
                    pictureBox1.Image = Properties.Resources._01_11_1;                   
                    break;
                case 11:
                    pictureBox1.Image = Properties.Resources._01_12_1;                   
                    break;
                case 12:
                    pictureBox1.Image = Properties.Resources._01_13_1;                  
                    break;
                case 13:
                    pictureBox1.Image = Properties.Resources._01_14_1;                  
                    break;
                case 14:
                    pictureBox1.Image = Properties.Resources._01_15_1;                    
                    break;
                case 15:                   
                    pictureBox1.Image = Properties.Resources._01_16_1;
                    textBox1.Text = "Conhecendo algumas funções do software";
                    break;
                case 16:
                    textBox1.Text = "Conhecendo a aba arquivos";
                    pictureBox1.Image = Properties.Resources.Arquivo_01;
                    break;
                case 17:
                    pictureBox1.Image = Properties.Resources.Arquivo_02;
                    break;
                case 18:
                    pictureBox1.Image = Properties.Resources.Arquivo_03;
                    break;
                case 19:
                    pictureBox1.Image = Properties.Resources.Arquivo_04;
                    textBox1.Text = "Conhecendo a aba arquivos";
                    break;
                case 20:
                    textBox1.Text = "Conhecendo a aba configurações";
                    pictureBox1.Image = Properties.Resources.config01;
                    break;
                case 21:
                    pictureBox1.Image = Properties.Resources.config02;
                    break;
                case 22:
                    pictureBox1.Image = Properties.Resources.config03;
                    break;            
            }
        }

        private void materialRaisedButton1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (numberSlider < 22)
            { 
                numberSlider++;
                openSlider();
            }
            else
            {
                this.Close();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (numberSlider >= 0)
            {
                numberSlider--;
                openSlider();
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
