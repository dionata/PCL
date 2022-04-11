
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
using PCL_Models;
using PCL_Models.Class;
using PCLLib.IO;

namespace PCLLib
{
    public partial class OpenGLControl
    {
        public Comunication comunication = new Comunication();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayMode"></param>        
        public void ChangeDisplayMode(CLEnum.CLRenderStyle displayMode)
        {
            for (int i = 0; i < this.GLrender.Models3D.Count; i++)
            {
                this.GLrender.Models3D[i].ModelRenderStyle = displayMode;
            }
            RefreshView(true);            
        }

        public static void draw_skybox(float size, int front, int back, int right, int left, int up, int bottom)
        {
            float scale = 1; // important 

            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);
            GL.Scale(size, size, size);

            //Front Face
            //Gl.glEnable(Gl.GL_CULL_FACE);
            GL.CullFace(CullFaceMode.Back);
            GL.BindTexture(TextureTarget.Texture2D, front); //GL_TEXTURE_2D, front);

            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 0, 1);
            GL.TexCoord2(0, 0); GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.TexCoord2(1, 0); GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.TexCoord2(1, 1); GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.TexCoord2(0, 1); GL.Vertex3(-0.5f, 0.5f, -0.5f);
            GL.End();
            //Gl.glCullFace(Gl.GL_BACK);
            // Gl.glDisable(Gl.GL_CULL_FACE);
            //Back Face
            GL.BindTexture(TextureTarget.Texture2D, back);
            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 0, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.TexCoord2(0, 0); GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.TexCoord2(0, 1); GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.TexCoord2(1, 1); GL.Vertex3(-0.5f, 0.5f, 0.5f);
            GL.End();

            //right face
            GL.BindTexture(TextureTarget.Texture2D, right);
            GL.Begin(BeginMode.Quads);
            GL.Normal3(1, 0, 0);
            GL.TexCoord2(0, 0); GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.TexCoord2(0, 1); GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.TexCoord2(1, 1); GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.TexCoord2(1, 0); GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.End();

            //Left Face
            GL.BindTexture(TextureTarget.Texture2D, left);
            GL.Begin(BeginMode.Quads);
            GL.Normal3(-1, 0, 0);
            GL.TexCoord2(1, 0); GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.TexCoord2(1, 1); GL.Vertex3(-0.5f, 0.5f, -0.5f);
            GL.TexCoord2(0, 1); GL.Vertex3(-0.5f, 0.5f, 0.5f);
            GL.TexCoord2(0, 0); GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.End();

            //Top Face
            GL.BindTexture(TextureTarget.Texture2D, up);
            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, 1, 0);
            GL.TexCoord2(0, 0); GL.Vertex3(-0.5f, 0.5f, -0.5f);
            GL.TexCoord2(1, 0); GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.TexCoord2(1, 1); GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.TexCoord2(0, 1); GL.Vertex3(-0.5f, 0.5f, 0.5f);
            GL.End();

            //Bottom Face
            GL.BindTexture(TextureTarget.Texture2D, bottom);
            GL.Begin(BeginMode.Quads);
            GL.Normal3(0, -1, 0);
            GL.TexCoord2(0, 0); GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.TexCoord2(1, 0); GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.TexCoord2(1, 1); GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.TexCoord2(0, 1); GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.End();

            GL.PopMatrix();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayMode"></param>
        public void ChangeTextureMode()
        {
            for (int i = 0; i < this.GLrender.Models3D.Count; i++)
            {
                
            }
            RefreshView(true);

        }

        /// <summary>
        /// 
        /// </summary>
        public void ChangeBackColor()
        {
            ColorDialog backColor = new ColorDialog();

            if (backColor.ShowDialog() == DialogResult.OK)
            {
                this.GLrender.ClearColor[0] = backColor.Color.R;
                this.GLrender.ClearColor[1] = backColor.Color.G;
                this.GLrender.ClearColor[2] = backColor.Color.B;
                for (int index = 0; index < 3; ++index)
                    this.GLrender.ClearColor[index] /= (float)byte.MaxValue;
                this.glControl1.Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ChangeModelColor()
        {
            
            if (modelsListSelect >= 0)
            {
                ColorDialog colDiag = new ColorDialog();

                if (colDiag.ShowDialog() == DialogResult.OK)
                {
                    SetColor("SetColor|" + modelsListSelect.ToString() + ";" + colDiag.Color.R.ToString()
                        + ";" + colDiag.Color.G.ToString() + ";" + colDiag.Color.B.ToString());
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please load a 3D object first");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ChangeModelColor(int R, int G, int B)
        {

            if (modelsListSelect >= 0)
            {               
                SetColor("SetColor|" + modelsListSelect.ToString() + ";" + R.ToString() + ";" + G.ToString() + ";" + B.ToString());  
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please load a 3D object first");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indice"></param>
        public void RedrawModels(int indice)
        {
           // for (int i = 0; i < this.GLrender.Models3D.Count; i++)
           // {
            this.GLrender.Models3D[indice].ForceRedraw = true;
           // }
            this.GLrender.Draw(this.GLrender.Models3D[indice].Name);
            this.glControl1.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RedrawModels()
        {
            //for (int i = 0; i < this.GLrender.Models3D.Count; i++)
           // {
                this.GLrender.Models3D[this.modelsListSelect].ForceRedraw = true;
            //}
            this.GLrender.Draw(this.GLrender.Models3D[this.modelsListSelect].Name);
            this.glControl1.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void ChangeColorOfModels(int r, int g, int b)
        {
            for (int i = 0; i < this.GLrender.Models3D.Count; i++)
            {
                for (int index = 0; index < this.GLrender.Models3D[i].Parts.Count; ++index)
                {
                    this.GLrender.Models3D[modelsListSelect-1].Parts[index].ColorOverall.X = (float)r / (float)byte.MaxValue;
                    this.GLrender.Models3D[modelsListSelect-1].Parts[index].ColorOverall.Y = (float)g / (float)byte.MaxValue;
                    this.GLrender.Models3D[modelsListSelect-1].Parts[index].ColorOverall.Z = (float)b / (float)byte.MaxValue;
                }
                
            }
            RedrawModels();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        private void ExecCommand(string command)
        {
            string[] strArray1 = command.Split('|');
            if (strArray1[0].ToLower() == "loadmodel")
            {
                string file = strArray1[1];
                string errorMessage = string.Empty;
                if (this.GLrender.LoadModel(file, errorMessage) == null)
                    throw new Exception("Error loading file");
                string[] strArray2 = file.Split('\\');
                this.modelsList.Add((object)strArray2[strArray2.Length - 1]);
            }
            else if (strArray1[0].ToLower() == "setcolor")
            {
                string[] strArray2 = strArray1[1].Split(';');
                int result1;
                int.TryParse(strArray2[0], out result1);
                int result2;
                int.TryParse(strArray2[1], out result2);
                int result3;
                int.TryParse(strArray2[2], out result3);
                int result4;
                int.TryParse(strArray2[3], out result4);
                ChangeColorOfModels(result2, result3, result4);

            }
            else if (strArray1[0].ToLower() == "setdisplacement")
            {
                string[] strArray2 = strArray1[1].Split(';');
                int result1;
                int.TryParse(strArray2[0], out result1);
                float result2;
                float.TryParse(strArray2[1], out result2);
                float result3;
                float.TryParse(strArray2[2], out result3);
                float result4;
                float.TryParse(strArray2[3], out result4);
                float result5;
                float.TryParse(strArray2[4], out result5);
                float result6;
                float.TryParse(strArray2[5], out result6);
                float result7;
                float.TryParse(strArray2[6], out result7);
                this.GLrender.Models3D[result1].vetTransl.X = (float)result2;
                this.GLrender.Models3D[result1].vetTransl.Y = (float)result3;
                this.GLrender.Models3D[result1].vetTransl.Z = (float)result4;
                this.GLrender.Models3D[result1].vetRot.X = (float)result5;
                this.GLrender.Models3D[result1].vetRot.Y = (float)result6;
                this.GLrender.Models3D[result1].vetRot.Z = (float)result7;
                this.GLrender.Draw("*");
                this.glControl1.Refresh();
            }
            else if (strArray1[0].ToLower() == "backcolor")
            {
                string[] strArray2 = strArray1[1].Split(';');
                float.TryParse(strArray2[0], out this.GLrender.ClearColor[0]);
                float.TryParse(strArray2[1], out this.GLrender.ClearColor[1]);
                float.TryParse(strArray2[2], out this.GLrender.ClearColor[2]);
                for (int index = 0; index < 3; ++index)
                    this.GLrender.ClearColor[index] /= (float)byte.MaxValue;
                this.glControl1.Invalidate();
            }
           
        }        

        /// <summary>
        /// Método que insere, modifica ou remove um cliente do Banco de Dados
        /// </summary>
        /// <param name="tempInsertClientModel"></param>
        /// <param name="tempModifyClientModel"></param>
        /// <param name="tempDeleteClientModel"></param>
        public void updateCliente(PacientModel tempInsertClientModel, PacientModel tempModifyClientModel, PacientModel tempDeleteClientModel)
        {
            //DataBaseModule db = new DataBaseModule();

            //db.RestGET(tempInsertClientModel, tempModifyClientModel, tempDeleteClientModel);
        }


        /// <summary>
        /// Método que insere, modifica ou remove uma malha do Banco de Dados
        /// </summary>
        /// <param name="path"> Caminho do arquivo temporário para enviar para servidor </param>
        /// <param name="clientCodigo"> Código do cliente associado</param>
        /// <param name="type"> 1: inserir - 2: modificar - 3: deletar </param>
        public static void updateMesh(string path, string nome, int clientCodigo, int type)
        {
            //DataBaseModule db = new DataBaseModule();

            MeshModel mesh_ = new MeshModel();
            List<MeshModel> meshs = new List<MeshModel>();
            BinaryReader reader = new BinaryReader(File.OpenRead(path));        

            using (reader)
            {
                mesh_.mesh = reader.ReadBytes((int)reader.BaseStream.Length);
            }
           // mesh_.ClientModel = new PacientModel();
           // mesh_.ClientModel.codigo = clientCodigo;
            mesh_.name = nome;
            meshs.Add(mesh_);

            switch(type)
            {
                case 1: //db.updateMeshModel(meshs, null, null);
                    break;
                case 2:// db.updateMeshModel(null, meshs, null);
                    break;
                case 3:// db.updateMeshModel(null, null, meshs);
                    break;
            }            
        }
     

        /// <summary>
        /// Método que insere, modifica ou remove um CAD do Banco de Dados
        /// </summary>
        /// <param name="path"> Caminho do arquivo temporário para enviar para servidor </param>
        /// <param name="clientCodigo"> Código do cliente associado</param>
        /// <param name="type"> 1: inserir - 2: modificar - 3: deletar </param>
        public static void updateCAD(string path, string nome, int clientCodigo, int type)
        {
           // DataBaseModule db = new DataBaseModule();

            CADModel CAD_ = new CADModel();
            List<CADModel> CADs = new List<CADModel>();
            BinaryReader reader = new BinaryReader(File.OpenRead(path));
            using (reader)
            {
               // CAD_.CAD = reader.ReadBytes((int)reader.BaseStream.Length);
            }
            ///CAD_.ClientModel = new PacientModel();
           // CAD_.ClientModel.codigo = clientCodigo;
            CAD_.name = nome;
            CADs.Add(CAD_);

            switch (type)
            {
                case 1:
                  //  db.updateCADModel(CADs, null, null);
                    break;
                case 2:
                   // db.updateCADModel(null, CADs, null);
                    break;
                case 3:
                   // db.updateCADModel(null, null, CADs);
                    break;
            }
        }
  
        /// <summary>
        /// Método que insere, modifica ou remove um CAD do Banco de Dados
        /// </summary>
        /// <param name="path"> Caminho do arquivo temporário para enviar para servidor </param>
        /// <param name="clientCodigo"> Código do cliente associado</param>
        /// <param name="type"> 1: inserir - 2: modificar - 3: deletar </param>
        public static void updateCAM(string path, string nome, int clientCodigo, int type)
        {
          //  DataBaseModule db = new DataBaseModule();

            CAMModel CAM_ = new CAMModel();
            List<CAMModel> CAMs = new List<CAMModel>();
            BinaryReader reader = new BinaryReader(File.OpenRead(path));
            using (reader)
            {
               // CAM_.CAM = reader.ReadBytes((int)reader.BaseStream.Length);
            }
           // CAM_.ClientModel = new PacientModel();
           // CAM_.ClientModel.codigo = clientCodigo;
            CAM_.name = nome;
            CAMs.Add(CAM_);

            switch (type)
            {
                case 1:
                  //  db.updateCAMModel(CAMs, null, null);
                    break;
                case 2:
                 //   db.updateCAMModel(null, CAMs, null);
                    break;
                case 3:
                  //  db.updateCAMModel(null, null, CAMs);
                    break;
            }
        }

        /// <summary>
        /// Método que insere, modifica ou remove um CAM do Banco de Dados
        /// </summary>
        /// <returns></returns>
 
    }
}
