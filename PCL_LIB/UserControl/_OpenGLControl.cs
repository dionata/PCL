
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using PCLLib.Models;

namespace PCLLib
{
    public partial class OpenGLControl : System.Windows.Forms.UserControl
    {
        int vertexPointer;
        int colorPointer;
        int verticesLength;

        public OpenGLControl()
        {
            InitializeComponent();
            initEventHandlers();

            this.modelRenderStyle = CLEnum.CLRenderStyle.Point;
            this.initGLControl();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void viewMode(string type)
        {
            string strDisplay;

            switch (type)
            {
                case "Point":
                    this.modelRenderStyle = CLEnum.CLRenderStyle.Point;
                    strDisplay = "Point";
                    setModelView(strDisplay);
                    break;
                case "Wireframe":
                    this.modelRenderStyle = CLEnum.CLRenderStyle.Wireframe;
                    strDisplay = "Wireframe";
                    setModelView(strDisplay);
                    break;
                case "Solid":
                    this.modelRenderStyle = CLEnum.CLRenderStyle.Solid;
                    strDisplay = "Solid";
                    setModelView(strDisplay);
                    break;
            }

            //do not change during load control
            if (this.GLrender == null)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDisplay"></param>
        private void setModelView(string strDisplay)
        {

            GLSettings.ViewMode = strDisplay;

            modelRenderStyle = CLEnum.CLRenderStyle.Point;
            for (int i = 0; i < Enum.GetValues(typeof(CLEnum.CLRenderStyle)).GetLength(0); i++)
            {

                string strVal = Enum.GetValues(typeof(CLEnum.CLRenderStyle)).GetValue(i).ToString();
                if (strVal == strDisplay)
                {
                    modelRenderStyle = (CLEnum.CLRenderStyle)Enum.GetValues(typeof(CLEnum.CLRenderStyle)).GetValue(i);
                    break;
                }

            }

            ChangeDisplayMode(modelRenderStyle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenGLControl_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Vector3d colorVector = new Vector3d(1, 0, 0);

            Model3D Model1 = StandardModels3D.Sphere("Sphere", 2, 8, colorVector, (Bitmap)null);
            AddModel(Model1);
            this.glControl1.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testPointCloudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestData.CreateTestData(out vertexPointer, out colorPointer, out verticesLength);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFileDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonColor_Click(object sender, EventArgs e)
        {
            ChangeModelColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        private void SetColor(string command)
        {
            string[] s = command.Split('|');
            s = s[1].Split(';');
            int ind;
            int R, G, B;
            int.TryParse(s[0], out ind);
            int.TryParse(s[1], out R);
            int.TryParse(s[2], out G);
            int.TryParse(s[3], out B);


           if(!GLSettings.marcarRegiaoOn && !GLSettings.marcacaoPolygonOn && !GLSettings.marcarRegiaoOn)
            { 
                int indexModel = modelsListSelect - 1;
                for (int i = 0; i < GLrender.Models3D[ind-1].Parts.Count; i++)
                {
                    float referencia_max = 0;
                    float referencia_min = 0;

                    //Vertices.ColorDelete(GLrender.Models3D[indexModel].VertexList);
                    GLrender.Models3D[indexModel].Parts[i].ColorOverall.X = (double)R * 0.004; //200
                    GLrender.Models3D[indexModel].Parts[i].ColorOverall.Y = (double)G * 0.004;
                    GLrender.Models3D[indexModel].Parts[i].ColorOverall.Z = (double)B * 0.004;

                    for (int j = 0; j < GLrender.Models3D[indexModel].VertexList.Count; j++)
                    {
                        if((GLrender.Models3D[modelsListSelect - 1].VertexList[j].Vector.Z) > referencia_max)
                        {
                            referencia_max = (float)(GLrender.Models3D[modelsListSelect - 1].VertexList[j].Vector.Z);
                        }
                    }

                    for (int j = 0; j < GLrender.Models3D[indexModel].VertexList.Count; j++)
                    {
                        if ((GLrender.Models3D[modelsListSelect - 1].VertexList[j].Vector.Z) < referencia_min)
                        {
                            referencia_min = (float)(GLrender.Models3D[modelsListSelect - 1].VertexList[j].Vector.Z);
                        }
                    }                

                    referencia_min = Math.Abs(referencia_min);                

                    for (int j = 0; j < GLrender.Models3D[indexModel].VertexList.Count; j++)
                    {
                        GLrender.Models3D[indexModel].VertexList[j].Color[0] = (float)((((GLrender.Models3D[modelsListSelect - 1].VertexList[j].Vector.Z + referencia_min) * 255 / (referencia_max + referencia_min)) + R) * 0.004) - 0.2f;
                        GLrender.Models3D[indexModel].VertexList[j].Color[1] = (float)((((GLrender.Models3D[modelsListSelect - 1].VertexList[j].Vector.Z + referencia_min) * 255 / (referencia_max + referencia_min)) + G) * 0.004) - 0.2f;
                        GLrender.Models3D[indexModel].VertexList[j].Color[2] = (float)((((GLrender.Models3D[modelsListSelect - 1].VertexList[j].Vector.Z + referencia_min) * 255 / (referencia_max + referencia_min)) + B) * 0.004) - 0.2f;
                    }
                }
                GLrender.Models3D[modelsListSelect - 1].ForceRedraw = true;
                GLrender.Draw("*");
                glControl1.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshView_MakeCurrent()
        {
            this.glControl1.MakeCurrent();
            this.GLrender.Draw("*");
            this.glControl1.Refresh();
            //GLSettings.atualizarProjecao(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forceRedraw"></param>
        private void RefreshView(bool forceRedraw)
        {
            for (int i = 0; i < GLrender.Models3D.Count; i++)
            {
                GLrender.Models3D[i].ForceRedraw = true;
            }
            this.glControl1.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboTransparency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modelsListSelect >= 1)
            {
                for (int i = 0; i < GLrender.Models3D[modelsListSelect-1 ].Parts.Count; i++)
                {
                }
            }
            RefreshView(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedCamera_"></param>
        public void selectedCamera(string selectedCamera_)
        {
            if (selectedCamera_ == "Rotação")
            {
                this.cameraMode = GLrender.MODE_ROT;                

                glControl1.Cursor = Cursors.NoMove2D;
            }
            else
            {
                cameraMode = GLrender.MODE_TRANSL;
                glControl1.Cursor = Cursors.Cross;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveSettings()
        {
            GLSettings.SaveSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axes"></param>
        /// <returns></returns>
        public string showAxesToolString(String axes)
        {
            if (axes == "Show Axis")
            {
                GLSettings.ShowAxis = true;
                axes = "Hide Axis";
                glControl1.Refresh();
            }
            else if (axes == "Hide Axis")
            {
                GLSettings.ShowAxis = false;
                axes = "Show Axis";
                glControl1.Refresh();
            }

            return axes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeSelectedModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveModel(modelsListSelect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void removeSelectedModel(int index)
        {            
            RemoveModel(index);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ParentFormWindow_set()
        {

            if (ParentFormWindow != null)
            {
                if (ParentFormWindow.FormBorderStyle == FormBorderStyle.Fixed3D)// .None)
                {
                    this.glControl1.SendToBack();
                    ParentFormWindow.FormBorderStyle = FormBorderStyle.Sizable;
                }
                else
                {
                    this.glControl1.BringToFront();
                    ParentFormWindow.FormBorderStyle = FormBorderStyle.None;
                    ParentFormWindow.WindowState = FormWindowState.Maximized;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void convexHullTool()
        {
            int indexModel = modelsListSelect;
            Model3D myModel = GLrender.Models3D[modelsListSelect-1];

            List<Vector3d> myListVectors = Vertices.ConvertToVector3dList(myModel.VertexList);
            ConvexHull3D convHull = new ConvexHull3D(myListVectors);
        }

        /// <summary>
        /// 
        /// </summary>
        public void saveNormalOfCurrentModel()
        {
            Model3D myModel = GLrender.Models3D[modelsListSelect-1];
            CheckNormals(myModel);
            string path = AppDomain.CurrentDomain.BaseDirectory + "TestData";

            Model3D.Save_OBJ(myModel, path, myModel.Name + ".obj");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myModel"></param>
        private void CheckNormals(Model3D myModel)
        {
            if (myModel.Normals.Count == 0)
            {
                myModel.CalculateNormals_Triangulation();
            }               
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indice"></param>
        /// <param name="nomeModelo"></param>
        /// <param name="selectView"></param>
        public void settingsTool(int indice, string nomeModelo, string selectView)
        {
            SettingView sf = new SettingView(this, indice, selectView, nomeModelo);
            sf.StartPosition = FormStartPosition.CenterScreen;
            if (sf.ShowDialog() == DialogResult.OK)
            {
                RefreshView(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="normals"></param>
        /// <param name="indice"></param>
        /// <returns></returns>
        public string showNormalsTool(String normals, int indice)
        {
            if (normals == "Show Normals")
            {
                GLSettings.ShowNormals = true;
                normals = "Hide Normals";

                Model3D myModel = GLrender.Models3D[indice];
                CheckNormals(myModel);
                this.GLrender.CreateLinesForNormals(myModel);
                RefreshView(true);
            }
            else if (normals == "Hide Normals")
            {
                GLSettings.ShowNormals = false;
                normals = "Show Normals";
                this.GLrender.DeleteLinesForNormals();
                RefreshView(true);
            }

            return normals;
        }
    }
}
