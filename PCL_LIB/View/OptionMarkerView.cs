
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
    public partial class OptionMarkerView : MaterialForm
    {
        private OpenGLControl OpenGLControl;
        private bool chControl = true;

        public OptionMarkerView(OpenGLControl OpenGLControl_)
        {
            InitializeComponent();

            GLSettings.MainBlock = false;
            controlElement();

            OpenGLControl = OpenGLControl_;
            comboBox1.Text = GLSettings.modoFuncionamentoCorte;

            if (GLSettings.separarEncostoAssento)
            {
                GLSettings.DiferenciarAssentoArquivo = "";
            }
            else
            {
            }

            if (GLSettings.MarcarEncosto)
            {
                GLSettings.DiferenciarAssentoArquivo = "";
            }
            else
            {
            }

            if (GLSettings.MarcarAssento)
            {
                GLSettings.DiferenciarAssentoArquivo = "Assento_";
            }
            else
            {
                GLSettings.DiferenciarAssentoArquivo = "";
            }

            this.textBox1.Text = GLSettings.Bloco1XEncosto.ToString();
            this.textBox2.Text = GLSettings.Bloco1YEncosto.ToString();
            this.textBox3.Text = GLSettings.Bloco1ZEncosto.ToString();

            this.textBox4.Text = GLSettings.Bloco2XEncosto.ToString();
            this.textBox5.Text = GLSettings.Bloco2YEncosto.ToString();
            this.textBox6.Text = GLSettings.Bloco2ZEncosto.ToString();

            this.textBox7.Text = GLSettings.Bloco3XEncosto.ToString();
            this.textBox8.Text = GLSettings.Bloco3YEncosto.ToString();
            this.textBox9.Text = GLSettings.Bloco3ZEncosto.ToString();

            /* Iniciando valores sobre as macações referentes ao encosto
             * 
             */
            try
            {
                if (GLSettings.MainBlock)
                {
                    this.textBox1.Text = GLSettings.Bloco1XEncosto.ToString();
                    this.textBox2.Text = GLSettings.Bloco1YEncosto.ToString();
                    this.textBox3.Text = GLSettings.Bloco1ZEncosto.ToString();

                    textBox4.Enabled = false;
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    textBox7.Enabled = false;
                    textBox8.Enabled = false;
                    textBox9.Enabled = false;
                }
                else
                {
                    textBox4.Enabled = true;
                    textBox5.Enabled = true;
                    textBox6.Enabled = true;
                    textBox7.Enabled = true;
                    textBox8.Enabled = true;
                    textBox9.Enabled = true;

                    this.textBox1.Text = GLSettings.Bloco1XEncosto.ToString();
                    this.textBox2.Text = GLSettings.Bloco1YEncosto.ToString();
                    this.textBox3.Text = GLSettings.Bloco1ZEncosto.ToString();

                    this.textBox4.Text = GLSettings.Bloco2XEncosto.ToString();
                    this.textBox5.Text = GLSettings.Bloco2YEncosto.ToString();
                    this.textBox6.Text = GLSettings.Bloco2ZEncosto.ToString();

                    this.textBox7.Text = GLSettings.Bloco3XEncosto.ToString();
                    this.textBox8.Text = GLSettings.Bloco3YEncosto.ToString();
                    this.textBox9.Text = GLSettings.Bloco3ZEncosto.ToString();

                    if (comboBox1.Text == "Corte Z")
                    {
                        textBox1.Enabled = false;
                        textBox2.Enabled = false;
                        textBox5.Enabled = false;
                        textBox8.Enabled = false;
                    }
                    else
                    {
                    }
                }
            }
            catch
            {
            }
            if (GLSettings.numberDivblocoExecutado > 0)
            {
                this.numericUpDown1.Value = GLSettings.numberDivblocoExecutado;
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
            try
            {
                GLSettings.Bloco1XEncosto = Convert.ToDouble(textBox1.Text);
            }
            catch 
            {
            }            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "Corte X")
                {

                    GLSettings.Bloco1YEncosto = Convert.ToDouble(textBox2.Text);
                }
                else
                {
                    GLSettings.Bloco1YEncosto = Convert.ToDouble(textBox2.Text);
                    GLSettings.Bloco2YEncosto = Convert.ToDouble(textBox2.Text);
                    GLSettings.Bloco3YEncosto = Convert.ToDouble(textBox2.Text);
                    GLSettings.Bloco4YEncosto = Convert.ToDouble(textBox2.Text);
                    GLSettings.Bloco5YEncosto = Convert.ToDouble(textBox2.Text);

                    textBox5.Text = textBox2.Text;
                    textBox8.Text = textBox2.Text;
                }
            }
            catch
            {
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GLSettings.Bloco1ZEncosto = Convert.ToDouble(textBox3.Text);
            }
            catch
            {
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (GLSettings.modoFuncionamentoCorte == "Corte Z")
            {
                try
                {
                    GLSettings.Bloco2XEncosto = Convert.ToDouble(textBox4.Text);
                    GLSettings.Bloco3XEncosto = Convert.ToDouble(textBox7.Text);                 
                }
                catch 
                {                    
                }
               
            }
            else
            {
                try
                {
                    GLSettings.Bloco2XEncosto = Convert.ToDouble(textBox4.Text);
                }
                catch
                {

                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Corte X")
            {
                GLSettings.Bloco2YEncosto = Convert.ToDouble(textBox5.Text);
            }
            else
            {
                GLSettings.Bloco1YEncosto = Convert.ToDouble(textBox5.Text);
                GLSettings.Bloco2YEncosto = Convert.ToDouble(textBox5.Text);
                GLSettings.Bloco3YEncosto = Convert.ToDouble(textBox5.Text);
                GLSettings.Bloco4YEncosto = Convert.ToDouble(textBox5.Text);
                GLSettings.Bloco5YEncosto = Convert.ToDouble(textBox5.Text);

                textBox2.Text = textBox5.Text;
                textBox8.Text = textBox5.Text;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GLSettings.Bloco2ZEncosto = Convert.ToDouble(textBox6.Text);
            }
            catch
            {
            }

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (GLSettings.modoFuncionamentoCorte == "Corte Z")
            {
                try
                {
                    GLSettings.Bloco3XEncosto = Convert.ToDouble(textBox7.Text);
                    GLSettings.Bloco2XEncosto = Convert.ToDouble(textBox4.Text);
                }
                catch 
                {                  
                }
                          
            }
            else
            {
                GLSettings.Bloco3XEncosto = Convert.ToDouble(textBox7.Text);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Corte X")
            {
                GLSettings.Bloco3YEncosto = Convert.ToDouble(textBox8.Text);
            }
            else
            {
                GLSettings.Bloco1YEncosto = Convert.ToDouble(textBox8.Text);
                GLSettings.Bloco2YEncosto = Convert.ToDouble(textBox8.Text);
                GLSettings.Bloco3YEncosto = Convert.ToDouble(textBox8.Text);
                GLSettings.Bloco4YEncosto = Convert.ToDouble(textBox8.Text);
                GLSettings.Bloco5YEncosto = Convert.ToDouble(textBox8.Text);

                textBox5.Text = textBox8.Text;
                textBox2.Text = textBox8.Text;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GLSettings.Bloco3ZEncosto = Convert.ToDouble(textBox9.Text);
            }
            catch 
            {                
            }           
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            GLSettings.numberDivblocoPlanejado = (int)numericUpDown1.Value;
            updateInitFinishBlocos();

            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!GLSettings.MainBlock)
            {
                GLSettings.numberDivblocoPlanejado = 3;
                GLSettings.numberDivblocoExecutado = 3; //rever 
                numericUpDown1.Value = 3;
                numericUpDown1.Enabled = false;
            }
        }

        private void updateInitFinishBlocos()
        {
            switch (this.numericUpDown1.Value)
            {
                case 2:
                    GLSettings.Bloco2XEncostoInit = Convert.ToDouble(textBox1.Text);
                    GLSettings.Bloco2XEncostoFinish = GLSettings.Bloco2XEncostoInit + GLSettings.Bloco2XEncosto;
                    GLSettings.Bloco2ZEncostoInit = Convert.ToDouble(textBox3.Text);
                    GLSettings.Bloco2ZEncostoFinish = GLSettings.Bloco2ZEncostoInit + GLSettings.Bloco2YEncosto;
                    break;
                case 3:
                    GLSettings.Bloco2XEncostoInit = Convert.ToDouble(textBox1.Text);
                    GLSettings.Bloco2XEncostoFinish = GLSettings.Bloco2XEncostoInit + GLSettings.Bloco2XEncosto;
                    GLSettings.Bloco3XEncostoInit = GLSettings.Bloco2XEncostoFinish;
                    GLSettings.Bloco3XEncostoFinish = GLSettings.Bloco3XEncostoInit + GLSettings.Bloco3XEncosto;

                    GLSettings.Bloco2ZEncostoInit = Convert.ToDouble(textBox3.Text);
                    GLSettings.Bloco2ZEncostoFinish = GLSettings.Bloco2ZEncostoInit + GLSettings.Bloco2ZEncosto;
                    GLSettings.Bloco3ZEncostoInit = GLSettings.Bloco2ZEncostoFinish;
                    GLSettings.Bloco3ZEncostoFinish = GLSettings.Bloco3ZEncostoInit + GLSettings.Bloco3ZEncosto;
                    break;
                case 4:
                    GLSettings.Bloco2XEncostoInit = Convert.ToDouble(textBox1.Text);
                    GLSettings.Bloco2XEncostoFinish = GLSettings.Bloco2XEncostoInit + GLSettings.Bloco2XEncosto;
                    GLSettings.Bloco3XEncostoInit = GLSettings.Bloco2XEncostoFinish;
                    GLSettings.Bloco3XEncostoFinish = GLSettings.Bloco3XEncostoInit + GLSettings.Bloco3XEncosto;
                    GLSettings.Bloco4XEncostoInit = GLSettings.Bloco3XEncostoFinish;
                    GLSettings.Bloco4XEncostoFinish = GLSettings.Bloco4XEncostoInit + GLSettings.Bloco4XEncosto;

                    GLSettings.Bloco2ZEncostoInit = Convert.ToDouble(textBox3.Text);
                    GLSettings.Bloco2ZEncostoFinish = GLSettings.Bloco2ZEncostoInit + GLSettings.Bloco2ZEncosto;
                    GLSettings.Bloco3ZEncostoInit = GLSettings.Bloco2ZEncostoFinish;
                    GLSettings.Bloco3ZEncostoFinish = GLSettings.Bloco3ZEncostoInit + GLSettings.Bloco3ZEncosto;
                    GLSettings.Bloco4ZEncostoInit = GLSettings.Bloco3ZEncostoFinish;
                    GLSettings.Bloco4ZEncostoFinish = GLSettings.Bloco4ZEncostoInit + GLSettings.Bloco4ZEncosto;
                    break;
                case 5:
                    GLSettings.Bloco2XEncostoInit = Convert.ToDouble(textBox1.Text);
                    GLSettings.Bloco2XEncostoFinish = GLSettings.Bloco2XEncostoInit + GLSettings.Bloco2XEncosto;
                    GLSettings.Bloco3XEncostoInit = GLSettings.Bloco2XEncostoFinish;
                    GLSettings.Bloco3XEncostoFinish = GLSettings.Bloco3XEncostoInit + GLSettings.Bloco3XEncosto;
                    GLSettings.Bloco4XEncostoInit = GLSettings.Bloco3XEncostoFinish;
                    GLSettings.Bloco4XEncostoFinish = GLSettings.Bloco4XEncostoInit + GLSettings.Bloco4XEncosto;
                    GLSettings.Bloco5XEncostoInit = GLSettings.Bloco4XEncostoFinish;
                    GLSettings.Bloco5XEncostoFinish = GLSettings.Bloco5XEncostoInit + GLSettings.Bloco5XEncosto;

                    GLSettings.Bloco2ZEncostoInit = Convert.ToDouble(textBox3.Text);
                    GLSettings.Bloco2ZEncostoFinish = GLSettings.Bloco2ZEncostoInit + GLSettings.Bloco2ZEncosto;
                    GLSettings.Bloco3ZEncostoInit = GLSettings.Bloco2ZEncostoFinish;
                    GLSettings.Bloco3ZEncostoFinish = GLSettings.Bloco3ZEncostoInit + GLSettings.Bloco3ZEncosto;
                    GLSettings.Bloco4ZEncostoInit = GLSettings.Bloco3ZEncostoFinish;
                    GLSettings.Bloco4ZEncostoFinish = GLSettings.Bloco4ZEncostoInit + GLSettings.Bloco4ZEncosto;
                    GLSettings.Bloco5ZEncostoInit = GLSettings.Bloco4ZEncostoFinish;
                    GLSettings.Bloco5ZEncostoFinish = GLSettings.Bloco5ZEncostoInit + GLSettings.Bloco5ZEncosto;
                    break;
                default:
                    break;
            }
        }

        private void materialLabel7_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged_1(object sender, EventArgs e)
        {
            if (GLSettings.modoFuncionamentoCorte == "Corte Z")
            {
            }
            else
            {
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Corte X")
            {
            }
            else
            {
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (GLSettings.modoFuncionamentoCorte == "Corte Z")
            {
            }
            else
            {
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Corte X")
            {
            }
            else
            {
            }
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
        }

        private void MaterialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void MaterialCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void MaterialCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void MaterialLabel3_Click(object sender, EventArgs e)
        {

        }

        private void NumericUpDown3_ValueChanged_1(object sender, EventArgs e)
        {
        }

        private void MaterialRaisedButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!GLSettings.MainBlock) GLSettings.modoFuncionamentoCorte = comboBox1.Text;

            if (comboBox1.Text == "Corte Z")
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox5.Enabled = false;
                textBox8.Enabled = false;
            }
            else
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox5.Enabled = true;
                textBox8.Enabled = true;
            }
        }

        private void materialCheckBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            controlElement();
        }

        private void controlElement()
        {
            if (GLSettings.MainBlock)
            { 
                GLSettings.MainBlock = false;
                lbBlock1.Text = "Bloco 1:";                                      
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                textBox9.Enabled = true;
                comboBox1.Enabled = true;
                //numericUpDown1.Enabled = true;

                GLSettings.numberDivblocoPlanejado = 3;
                GLSettings.numberDivblocoExecutado = 3; //rever 
                numericUpDown1.Value = 3;

                if (comboBox1.Text == "Corte Z")
                {
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox5.Enabled = false;
                    textBox8.Enabled = false;
                }
                else
                {
                }

                if(File.Exists(GLSettings.locateTMP + GLSettings.Bloco1Out_))
                {
                    File.Delete(GLSettings.locateTMP + GLSettings.Bloco1Out_);
                }

                if(File.Exists(GLSettings.locateMALHA + GLSettings.Bloco1Out_))
                {
                    File.Delete(GLSettings.locateMALHA + GLSettings.Bloco1Out_);
                }               
            }
            else
            {
                GLSettings.MainBlock = true;
                lbBlock1.Text = "Blank:";             
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;         
                comboBox1.Enabled = false;
                //numericUpDown1.Enabled = false;
                GLSettings.numberDivblocoPlanejado = 1;
                GLSettings.numberDivblocoExecutado = 1; //rever 
                numericUpDown1.Value = 1;

                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
            }
        }
    }
}
