
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
using PCL_Models.Class;
using System.ComponentModel.DataAnnotations;
//using PCLLib.View;

namespace PCLLib
{
    public partial class RegisteredUsersView : MaterialForm
    {
        private OpenGLControl OpenGLControl;
        private BindingList<CADModel> CADs_ = new BindingList<CADModel>();
        private BindingList<CAMModel> CAMs_ = new BindingList<CAMModel>();
        private BindingList<MeshModel> Meshs_ = new BindingList<MeshModel>();
        private SaveFileDialog save = new SaveFileDialog();

        public RegisteredUsersView(OpenGLControl OpenGLControl_)
        {
            InitializeComponent();
            OpenGLControl = OpenGLControl_;
            listaArquivosLocal();
            //btUploadDownload.Text = "UPLOAD";            
            //btUploadDownload.Enabled = false;
        }

        private void materialRaisedButton1_Click_1(object sender, EventArgs e)
        {
           // btPesquisar.Text = "Pesquisar";
            this.Close();
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            limparTela();
        }

        private void limparTela()
        {
           // btPesquisar.Text = "Pesquisar";
            //btUploadDownload.Text = "UPLOAD";
            //btUploadDownload.Enabled = false;

            textBairro.Text = " ";
            textCelular1.Text = " ";
            textCelular2.Text = " ";
            textCEP.Text = " ";
            textCidade.Text = " ";
            textComplemento.Text = " ";
            textCPF.Text = " ";
            textDia.Text = " ";
         //   textMes.Text = " ";
         //   textAno.Text = " ";
            textEmail.Text = " ";
            textEndereco.Text = " ";
            textNome.Text = " ";
            textRG.Text = " ";
            textTelefone.Text = " ";
            textEstado.Text = " ";
         //   texCodigo.Text = " ";

            listaArquivosLocal();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if(true)//texCodigo.Text != " " && textNome.Text != " ")
                { 
                    PacientModel novoCliente = new PacientModel();
                    novoCliente.neighborhood = (string)textBairro.Text;

              //      try { novoCliente.cellPhone1 = long.Parse(textCelular1.Text); }
              //      catch { novoCliente.cellPhone1 = long.Parse("0"); }

             //       try { novoCliente.cellPhone2 = long.Parse(textCelular2.Text); }
               //     catch { novoCliente.cellPhone2 = long.Parse("0"); }

                //    try{ novoCliente.CEP = long.Parse(textCEP.Text);}
                //    catch { novoCliente.CEP = long.Parse("0"); }

                  //  try { novoCliente.date = new DateTime(int.Parse(textAno.Text), int.Parse(textMes.Text), int.Parse(textDia.Text)); }
                  //  catch { novoCliente.date = new DateTime(1983, 10, 27); }

                //    novoCliente.cities = (string)textCidade.Text;
                //    novoCliente.complement = (string)textComplemento.Text;
                //    novoCliente.email = (string)textEmail.Text;
                //    novoCliente.address = (string)textEndereco.Text;
                //    novoCliente.name = (string)textNome.Text;

             //       try { novoCliente.RG = long.Parse(textRG.Text); }
              //      catch { novoCliente.RG = long.Parse("0"); }

              //      try { novoCliente.telephone = long.Parse(textTelefone.Text); }
              //      catch { novoCliente.telephone = long.Parse("0"); }

            //        novoCliente.states = (string)textEstado.Text;
                   // novoCliente.codigo = int.Parse(texCodigo.Text);

                    OpenGLControl.updateCliente(novoCliente, null, null);

                    enviaArquivosBancoDados();

                    limparTela();

                    WarningView sf_ = new WarningView(16);
                    sf_.ShowDialog();
                }
                else
                {
                    WarningView sf_ = new WarningView(19);
                    sf_.ShowDialog();
                }
            }
            catch 
            {
                WarningView sf_ = new WarningView(15);
                sf_.ShowDialog();
            }
   
          
        }

        private void tableLayoutPanel14_Paint(object sender, PaintEventArgs e)
        {
        }        

        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            List<PacientModel> pesquisaModel = new List<PacientModel>();

            try
            {
             //   pesquisaModel = OpenGLControl.listCliente(int.Parse(texCodigo.Text));

                if (pesquisaModel.Count > 0 )//&& //btPesquisar.Text == "Pesquisar")
                {
                    textBairro.Text     = pesquisaModel[0].neighborhood.ToString();
                    textCelular1.Text   = pesquisaModel[0].cellPhone1.ToString();
                    textCelular2.Text   = pesquisaModel[0].cellPhone2.ToString();
                    textCEP.Text        = pesquisaModel[0].CEP.ToString();
                    textCidade.Text     = pesquisaModel[0].cities.ToString();
                    textComplemento.Text = pesquisaModel[0].complement.ToString();
                    textCPF.Text        = pesquisaModel[0].CPF.ToString();
                   // textDia.Text        = pesquisaModel[0].date.Day.ToString();
                  //  textMes.Text        = pesquisaModel[0].date.Month.ToString();
                 //   textAno.Text        = pesquisaModel[0].date.Year.ToString();
                    textEmail.Text      = pesquisaModel[0].email.ToString();
                    textEndereco.Text   = pesquisaModel[0].address.ToString();
                    textNome.Text       = pesquisaModel[0].name.ToString();
                    textRG.Text         = pesquisaModel[0].RG.ToString();
                    textTelefone.Text   = pesquisaModel[0].telephone.ToString();
                    textEstado.Text     = pesquisaModel[0].states.ToString();
                  //  texCodigo.Text      = pesquisaModel[0].codigo.ToString();

                 //   btPesquisar.Text = "Modificar";
                    //btUploadDownload.Enabled = true;
                    //btUploadDownload.Text = "DONWLOAD";

                    listaArquivosBancoDados();
                }
                else
                {
                  //  DateTime dateTime = new DateTime(int.Parse(textAno.Text), int.Parse(textMes.Text), int.Parse(textDia.Text));
                    PacientModel modifyCliente = new PacientModel();
                    modifyCliente.neighborhood = (string)textBairro.Text;
                 //   modifyCliente.cellPhone1 = long.Parse(textCelular1.Text);
                  //  modifyCliente.cellPhone2 = long.Parse(textCelular2.Text);
                //    modifyCliente.CEP = long.Parse(textCEP.Text);
                    modifyCliente.cities = (string)textCidade.Text;
                    modifyCliente.complement = (string)textComplemento.Text;
                //    modifyCliente.CPF = long.Parse(textCPF.Text);
                //    modifyCliente.date = dateTime;
                    modifyCliente.email = (string)textEmail.Text;
                    modifyCliente.address = (string)textEndereco.Text;
                    modifyCliente.name = (string)textNome.Text;
                 //   modifyCliente.RG = long.Parse(textRG.Text);
                //    modifyCliente.telephone = long.Parse(textTelefone.Text);
                    modifyCliente.states = (string)textEstado.Text;
                 //   modifyCliente.codigo = int.Parse(texCodigo.Text);
                    OpenGLControl.updateCliente(null, modifyCliente, null);

                  //  btPesquisar.Text = "Pesquisar";
                    //btUploadDownload.Text = "UPLOAD";
                    //btUploadDownload.Enabled = false;

                    WarningView sf_ = new WarningView(17);
                    sf_.ShowDialog();
                }
            }
            catch 
            {
                WarningView sf_ = new WarningView(14);
                sf_.ShowDialog();
            }                           
        }

        private void materialRaisedButton5_Click(object sender, EventArgs e)
        {
           
        }

        private void materialRaisedButton5_Click_1(object sender, EventArgs e)
        {
            try
            {
                PacientModel deleteClientModel = new PacientModel();

             //   deleteClientModel.codigo = int.Parse(texCodigo.Text);

                OpenGLControl.updateCliente(null, null, deleteClientModel);

              //  btPesquisar.Text = "Pesquisar";

                WarningView sf_ = new WarningView(18);
                sf_.ShowDialog();
            }
            catch
            {
                WarningView sf_ = new WarningView(14);
                sf_.ShowDialog();
            }            
        }

        private void enviaArquivosBancoDados()
        {
            try
            {
                DirectoryInfo cad = new DirectoryInfo(GLSettings.locateCAD);
                foreach (FileInfo file in cad.GetFiles())
                {
                //    OpenGLControl.updateCAD(file.FullName, file.Name, int.Parse(texCodigo.Text), 1);
                }

                DirectoryInfo malha = new DirectoryInfo(GLSettings.locateMALHA);
                foreach (FileInfo file in malha.GetFiles())
                {
                //   OpenGLControl.updateMesh(file.FullName, file.Name, int.Parse(texCodigo.Text), 1);
                }

                /*
                DirectoryInfo STL = new DirectoryInfo(GLSettings.locateSTL);
                foreach (FileInfo file in STL.GetFiles())
                {
                }           
                */
                DirectoryInfo CAM = new DirectoryInfo(GLSettings.locateCAM);
                foreach (FileInfo file in CAM.GetFiles())
                {
                 //   OpenGLControl.updateCAM(file.FullName, file.Name, int.Parse(texCodigo.Text), 1);
                }
            }
            catch
            {
                //WarningView sf_ = new WarningView(14);
                //sf_.ShowDialog();
            }
        }

        private void materialRaisedButton6_Click(object sender, EventArgs e)
        {
          
        }                             
        private void listaArquivosLocal()
        {
           // comboBox1.Items.Clear();

            try
            {
                DirectoryInfo cad = new DirectoryInfo(GLSettings.locateCAD);
                foreach (FileInfo file in cad.GetFiles())
                {
                   // comboBox1.Items.Add(file.Name);
                }

                DirectoryInfo malha = new DirectoryInfo(GLSettings.locateMALHA);
                foreach (FileInfo file in malha.GetFiles())
                {
                   /// comboBox1.Items.Add(file.Name);
                }

                /*
                DirectoryInfo STL = new DirectoryInfo(GLSettings.locateSTL);
                foreach (FileInfo file in STL.GetFiles())
                {
                }           
                */
                DirectoryInfo CAM = new DirectoryInfo(GLSettings.locateCAM);
                foreach (FileInfo file in CAM.GetFiles())
                {
                   // comboBox1.Items.Add(file.Name);
                }
            }  
            catch
            {

            }
        }

        /// <summary>
        /// Lista os arquivos vinculados ao cliente 
        /// </summary>
        private void listaArquivosBancoDados()
        {         
           // CADs_ = OpenGLControl.listCAD(int.Parse(texCodigo.Text));
          //  CAMs_ = OpenGLControl.listCAM(int.Parse(texCodigo.Text));
           // Meshs_ = OpenGLControl.listMesh(int.Parse(texCodigo.Text));

           // comboBox1.Items.Clear();

            foreach(var CAD in CADs_)
            {
              //  comboBox1.Items.Add(CAD.name);
            }

            foreach (var CAM in CAMs_)
            {
              //  comboBox1.Items.Add(CAM.name);
            }

            foreach (var Mesh in Meshs_)
            {
               // comboBox1.Items.Add(Mesh.name);
            }
        }      

        private void btUploadDownload_Click(object sender, EventArgs e)
        {
            string name = ""; //comboBox1.SelectedItem.ToString();

            string[] typeDado = name.Split('.');
            
            if(typeDado[1] == "obj")
            {                
                save.Filter = "*salvar arquivo|*.obj";
                save.ShowDialog();

                foreach(var CAD in CADs_)
                {
                    if(CAD.name == name)
                    {
                        BinaryWriter writer = new BinaryWriter(File.Create(save.FileName));
                        using (writer)
                        {
                            //writer.Write(CAD.CAD);
                        }
                        writer.Close();
                    }
                }

                foreach (var Mesh in Meshs_)
                {
                    if (Mesh.name == name)
                    {
                        BinaryWriter writer = new BinaryWriter(File.Create(save.FileName));
                        using (writer)
                        {
                            writer.Write(Mesh.mesh);
                        }
                        writer.Close();
                    }
                }

            }
            else if(typeDado[1] == "nc")
            {                
                save.Filter = "*salvar arquivo|*.nc";
                save.ShowDialog();

                foreach (var CAM in CAMs_)
                {
                    if (CAM.name == name)
                    {
                        BinaryWriter writer = new BinaryWriter(File.Create(save.FileName));
                        using (writer)
                        {
                            //writer.Write(CAM.CAM);
                        }
                        writer.Close();
                    }
                }

            }
            else if(typeDado[1] == "stl")
            {                
                save.Filter = "*salvar arquivo|*.stl";
                save.ShowDialog();

                foreach (var CAD in CADs_)
                {
                    if (CAD.name == name)
                    {
                        BinaryWriter writer = new BinaryWriter(File.Create(save.FileName));
                        using (writer)
                        {
                            //writer.Write(CAD.CAD);
                        }
                        writer.Close();
                    }
                }
            }

            /*
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "*Carregar arquivo|*.obj; *.xyz; *.ply";
            save.Filter = "*Exportar arquivo|*.stl";
            save.ShowDialog();
            */

            //if(comboBox1.SelectedValue)
            /*
            BinaryWriter writer = new BinaryWriter(File.Create(GLSettings.locateTMP + "teste2.stl"));
            using (writer)
            {
                writer.Write(listMesh_[0].mesh);
            }
            writer.Close();
            */
        }

        private void texCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void textNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void textCidade_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
