
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
    public partial class WarningView : MaterialForm
    {
        public WarningView(int text)
        {
            InitializeComponent();

            switch(text)
            {
                case 0: label2.Text = "Selecione um modelo válido";
                    break;
                case 1: label2.Text = "Não é possivel remover os pontos";
                    break;
                case 2: label2.Text = "Esta selecionado a marcação de corte";
                    break;
                case 3: label2.Text = "Selecine o modelo para gerar o assento";
                    break;
                case 4:label2.Text = "Gcode gerado com Sucesso!";
                    break;
                case 5:label2.Text = "Selecione o segundo modelo para realizar a união";
                    break;
                case 6:
                    label2.Text = "União realizada com sucesso!";
                    break;
                case 7: label2.Text = "Modelo não selecionado!";
                    break;
                case 8:
                    label2.Text = "O botão <Marcar Região> está selecionado!";
                    break;
                case 9:
                    label2.Text = "Conflito de name do modelo!";
                    break;
                case 10:
                    label2.Text = "Nenhum projeto selecionado!";
                    break;
                case 11:
                    label2.Text = "Projeto salvo com sucesso!";
                    break;
                case 12:
                    label2.Text = "Projeto fechado sucesso!";
                    break;
                case 13:
                    label2.Text = "Erro ao gerar encosto!";
                    break;
                case 14:
                    label2.Text = "Nenhum cliente encontrado!";
                    break;
                case 15:
                    label2.Text = "Erro ao enviar informações para o Banco de dados";
                    break;                    
                case 16:
                    label2.Text = "Dados gravados com sucesso!";
                    break;
                case 17:
                    label2.Text = "Dados mdificados com sucesso!";
                    break;
                case 18:
                    label2.Text = "Dados excluídos com sucesso!";
                    break;
                case 19:
                    label2.Text = "O campo código ou name não está preenchido!";
                    break;
                case 20:
                    label2.Text = "Arquivo STL Inexistente!";
                    break;
                case 21:
                    label2.Text = "Malha já processada!";
                    break;
                default:
                    break;
            }            
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
            this.Close();
        }
    }
}
