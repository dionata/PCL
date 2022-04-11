
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using PCLLib.Utils;

namespace PCLLib
{
    public partial class OpenGLControl
    {
        public OpenGLRenderer GLrender;
        private Model3D modelOld;
        private Model3D model3D;
        public OpenFileDialog openModel;
        public List<object> modelsList = new List<object>();
        public int modelsListSelect = 1;
        public int selectMoveXYZ;

        /*
         * Carregando arquivo *.obj para visualizacao e tratamento
         */
        public bool LoadFileDialog()
        {
            this.openModel = new OpenFileDialog();
            openModel.Filter = "*Carregar arquivo|*.obj; *.xyz; *.ply";
            openModel.ShowDialog();

            string[] name_1 = this.openModel.FileName.Split('\\');
            string[] name_2 = name_1[name_1.LongLength - 1].Split('.');

            DirectoryInfo MODELOS = new DirectoryInfo(GLSettings.locateMALHA);
            foreach (FileInfo file in MODELOS.GetFiles())
            {                
                if (name_2[name_2.LongLength - 2] == file.Name) return false;                                       
            }            

            loadFile();
            return true;
        }

        public void ShowModel(Model3D myModel)
        {
            AddModel(myModel);
            ShowModels();
        }

        public void AddModel(Model3D myModel)
        {
            lock (myModel)
            this.GLrender.Models3D.Add(myModel);
            this.GLrender.SelectModel(this.GLrender.Models3D.Count - 1);
            modelsList.Add((object)myModel.Name);
            myModel.ModelRenderStyle = this.modelRenderStyle;
        }
        public void ShowModels()
        {
            if (this.GLrender.Models3D == null)
                return;
            if (this.GLrender.Models3D.Count > 1)
                this.modelOld = this.GLrender.Models3D[this.GLrender.Models3D.Count - 1];
            this.RefreshView_MakeCurrent();
        }

        public void RefreshShowModels(int indice, string selectView, string tipo, String name)
        {
            string errorText = string.Empty;

            if (tipo == "recorteMalhaX")
            {

                model3D = GLrender.LoadModel(GLSettings.locateTMP + name, errorText);
                loadTringuleGeneration(GLSettings.locateTMP + name);
                Models.Model3DAUX.Save_OBJ(model3D, GLSettings.locateTMP, name);

                if (model3D != null)
                {
                    GLrender.Models3D[indice] = model3D;
                    modelsList[indice] = model3D;
                    if (this.GLrender.Models3D == null)
                        return;
                    this.modelOld = this.GLrender.Models3D[indice];
                    viewMode(selectView);
                    this.RefreshView_MakeCurrent();
                }
            }
            else
            {
                model3D = GLrender.LoadModel(GLSettings.locateTMP + GLSettings.ModeloAuxOut_, errorText);
                if (model3D != null)
                {
                    GLrender.Models3D[indice] = model3D;
                    modelsList[indice] = model3D;
                    if (this.GLrender.Models3D == null)
                        return;
                    this.modelOld = this.GLrender.Models3D[indice];
                    viewMode(selectView);
                    this.RefreshView_MakeCurrent();
                }
            }
        }

        public void RemoveModel(int indModel)
        {
            if (indModel >= 0)
            {
                //indModel = indModel - 1;
                try
                {
                    for (int index = 0; index < this.GLrender.Models3D[indModel].Parts.Count; ++index)
                        GL.DeleteLists(this.GLrender.Models3D[indModel].Parts[index].GLListNumber, 1);
                    lock (this.GLrender.Models3D)
                        this.GLrender.RemoveModel(indModel);

                    if (modelsListSelect > 0)
                    {
                        modelsListSelect = modelsListSelect - 1;
                    }

                    GC.Collect();

                    if (GLrender.Models3D.Count > 0)
                    {
                        Model3D model3D = GLrender.Models3D[0];
                        ShowModels();
                        GLrender.SelectModel(0);
                    }
                    this.glControl1.Refresh();
                }
                catch
                {
                    //TODO
                }
            }
        }
        private void WriteModelName(int modelInd)
        {
            modelsList.Add((object)this.GLrender.Models3D[modelInd].Name);
        }



        private int[] readTriangles(int indModelo)
        {
            int num1 = 0;
            int num2 = 0;
            for (int index1 = 0; index1 < this.GLrender.Models3D[indModelo].Parts.Count; ++index1)
            {
                for (int index2 = 0; index2 < this.GLrender.Models3D[indModelo].Parts[index1].Triangles.Count; ++index2)
                {
                    num1 += this.GLrender.Models3D[indModelo].Parts[index1].Triangles[index2].IndVertices.Count - 2;
                    ++num2;
                }
            }
            if (num1 != 0)
            {
                int[] numArray = new int[3 * num1];
                int index1 = 0;
                for (int index2 = 0; index2 < this.GLrender.Models3D[indModelo].Parts.Count; ++index2)
                {
                    for (int index3 = 0; index3 < this.GLrender.Models3D[indModelo].Parts[index2].Triangles.Count; ++index3)
                    {
                        for (int index4 = 0; index4 < this.GLrender.Models3D[indModelo].Parts[index2].Triangles[index3].IndVertices.Count - 2; ++index4)
                        {
                            numArray[index1] = this.GLrender.Models3D[indModelo].Parts[index2].Triangles[index3].IndVertices[index4];
                            numArray[index1 + 1] = this.GLrender.Models3D[indModelo].Parts[index2].Triangles[index3].IndVertices[index4 + 1];
                            numArray[index1 + 2] = this.GLrender.Models3D[indModelo].Parts[index2].Triangles[index3].IndVertices[index4 + 2];
                            index1 += 3;
                        }
                    }
                }
                return numArray;
            }
            else
            {
                int[] numArray = new int[num2 * 3];
                int index1 = 0;
                for (int index2 = 0; index2 < this.GLrender.Models3D[indModelo].Parts.Count; ++index2)
                {
                    for (int index3 = 0; index3 < this.GLrender.Models3D[indModelo].Parts[index2].Triangles.Count; ++index3)
                    {
                        numArray[index1] = this.GLrender.Models3D[indModelo].Parts[index2].Triangles[index3].IndVertices[0];
                        numArray[index1 + 1] = this.GLrender.Models3D[indModelo].Parts[index2].Triangles[index3].IndVertices[1];
                        numArray[index1 + 2] = this.GLrender.Models3D[indModelo].Parts[index2].Triangles[index3].IndVertices[1];
                        index1 += 3;
                    }
                }
                return numArray;
            }
        }

        public void loadFile()
        {
            this.Cursor = Cursors.WaitCursor;
            Model3D model3D;

            for (int index = 0; index < this.openModel.FileNames.Length; ++index)
            {
                string errorText = string.Empty;
                model3D = this.GLrender.LoadModel(this.openModel.FileNames[index], errorText);

                if (model3D != null)
                {
                    AddModel(model3D);
                    ShowModels();
                }
            }           
        }

        public void loadTringuleGeneration(string path)
        {
            this.Cursor = Cursors.WaitCursor;
            Model3D model3D;

            string errorText = string.Empty;
            model3D = this.GLrender.LoadModel(path, errorText);

            if (model3D != null)
            {
                AddModel(model3D);
                ShowModels();
            }
        }
    }
}
