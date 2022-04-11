
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
    public partial class AboutView : MaterialForm
    {      
        public AboutView()
        {
            InitializeComponent();
            texts();
        }

        private void materialRaisedButton1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TECPOST_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        void texts()
        {
            textBox2.Text = " Software para importação de malha tratamento e geração de arquivo CAD/CAM ";
            textBox1.Text = " Software comtempla vários recurso de para remozação parte não importantes da malha importada. " +
                "Tambem é possivel realizar suavização da malha para a superfície mais lisa. O Software tem recursos para cortar a malha. Software pode" +
                "gerar blocos sólidos a partir da malha e suas partes. E por fim pode gerar os códigos CAM para desbastes e cabamento.";
        }
    }
}
