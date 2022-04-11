/*
 * Arquivo: MainView.cs
 * 
 * Autor: Dionata Nunes <dionata.silva@senairs.org.br>
 * 
 * Data Criação: 14/01/2019
 * 
 * Descrição: Classe de controle de visualização da tela principal do programa
 * 
 */ 



using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using OpenTK;
using MaterialSkin.Controls;
using System.IO;
using PCLLib.Utils;
using PCLLib.IO;
using PCL_Models.Class;
using OpenTK.Input;
using System.Timers;
using OpenCL.Net;

namespace PCLLib
{
    public partial class MainView : MaterialForm
    {
        public OpenGLControl OpenGLControl;
        private SaveFileDialog exportModel = new SaveFileDialog();
        private FolderBrowserDialog importModel = new FolderBrowserDialog();
        private static bool marcardo = false;
        private int indiceMarcadorCorte = 0;
        private static int inverterOrientaçãoModelo3D_select = 0;
        private static string selectView = "Wireframe";
        private static bool union = false;
        private static bool CheckedStartOpen = false;
        private static bool newProjetBlock = false;
        private static string select = "";
        private StreamWriter escritor;
        private Stream saida;
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        static int alarmCounter = 1;
        static bool exitFlag = false;

        public MainView()
        {         
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureInfo.CurrentCulture.LCID);
            
            InitializeComponent();

            initProjeto();

            GLSettings.InitFromSettings();

            AddOpenGLControl();
            
            showNormalsToolStripMenuItem.Text = "Hide Normals";

            try
            {
                OpenGLControl.showNormalsTool(showNormalsToolStripMenuItem.Text, toolStripComboBox1.SelectedIndex);
                showAxisToolStripMenuItem.Text = "Show Axis";
                OpenGLControl.showAxesToolString(showAxisToolStripMenuItem.Text);
                OpenGLControl.ParentFormWindow_set();
                OpenGLControl.selectedCamera("Rotação");
                OpenGLControl.viewMode(selectView);

                projecaoFrontal();

                MarcarAreaCorte();
                MarcarAreaCorte();

                toolStripComboBox1.Text = "Selecionar Modelo";
                OpenGLControl.modelsListSelect = -1;

                info.Text = "Bem Vindo: PCL Studio V1.0.0";
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }                            

            if (GLSettings.projectionFree)
            {
                projeçãoLivreToolStripMenuItem.CheckState = CheckState.Checked;
                toolStripButton5.Enabled = false;
                marcarRegiao.Enabled = false;
            }

            perspectivaToolStripMenuItem1.CheckState = CheckState.Unchecked;
            ortogonalToolStripMenuItem.CheckState = CheckState.Checked;

            myTimer.Tick += new EventHandler(TimerEventProcessor);
            myTimer.Interval = 10;
            myTimer.Start();
        }
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            myTimer.Stop();
            myTimer.Enabled = true;
            labX.Text = "X:" + GLSettings.EX.ToString();
            labY.Text = "Y:" + GLSettings.EY.ToString();

            labTX.Text = "X:" + Math.Round(Model3D.vetTransl_port.X, 2).ToString();
            labTY.Text = "Y:" + Math.Round(Model3D.vetTransl_port.Y,2).ToString();
            labTZ.Text = "Z:" + Math.Round(Model3D.vetTransl_port.Z,2).ToString();

            labRX.Text = "X:" + Math.Round(Model3D.vetRot_port.X,2).ToString();
            labRY.Text = "Y:" + Math.Round(Model3D.vetRot_port.Y,2).ToString();
            labRZ.Text = "Z:" + Math.Round(Model3D.vetRot_port.Z,2).ToString();
            GLSettings.panelOpenKinectX = panelOpenKinect.Width;
            GLSettings.panelOpenKinectY = panelOpenKinect.Height;
        }

        /// <summary>
        /// 
        /// </summary>
        private void initProjeto()
        {
            DirectoryInfo info = new DirectoryInfo(@".\");
            string[] names = Directory.GetDirectories(@".\");
            
            foreach(var name in names)
            {
                if(name.StartsWith(".\\projeto"))
                {                 
                    Directory.Delete(name, true);
                }
            }          
        }

        /// <summary>
        /// 
        /// </summary>
        private void fecharProjeto()
        {
            bool status = false;

            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                if (treeView1.Nodes[i].Checked)
                {      
                    Directory.Delete(GLSettings.locateRAIZ + treeView1.Nodes[i].FullPath);
                    status = true;

                    atualizaProjeto(true, false, true, true);

                    WarningView sf = new WarningView(12);
                    sf.ShowDialog();
                }
            }

            if (!status)
            {
                WarningView sf = new WarningView(10);
                sf.ShowDialog();
            }

            atualizaProjeto(true, true, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameProject"></param>
        private void selectProjeto(string nameProject)
        {
            GLSettings.locateCAD = @".\" + nameProject + "\\cad\\";
            GLSettings.locateMALHA = @".\" + nameProject + "\\malha\\";
            GLSettings.locateSTL = @".\" + nameProject + "\\stl\\";
            GLSettings.locateCAM = @".\" + nameProject + "\\cam\\";
            GLSettings.locateTMP = @".\" + nameProject + "\\tmp\\";
            GLSettings.locateRAIZ = nameProject;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void clearSelectProject()
        {
            try
            {
                newProjetBlock = true;
                for (int h = 0; h < GLSettings.numberProject; h++)
                {
                    for (int j = 0; j < treeView1.Nodes[h].Nodes.Count; j++)
                    {
                        for (int i = 0; i < treeView1.Nodes[h].Nodes[j].Nodes.Count; i++)
                        {
                            treeView1.Nodes[h].Checked = false;
                            treeView1.Nodes[h].Nodes[j].Checked = false;
                            treeView1.Nodes[h].Nodes[j].Nodes[i].Checked = false;
                        }
                    }
                }            
         
                OpenGLControl.RemoveAllModels();
                toolStripComboBox1.Items.Clear();
                newProjetBlock = false;
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }      
        }

        /// <summary>
        /// 
        /// </summary>
        private void criaProjeto(string type)
        {
            if(type != "newProject") clearSelectProject();

            GLSettings.newMproject.Add("projeto" + GLSettings.numberProject);
            treeView1.Nodes.Add(GLSettings.newMproject[GLSettings.numberProject]);
            treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add("cad");
            treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add("malha");
            treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add("stl");
            treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add("cam");                    
            GLSettings.locateCAD = @".\" + GLSettings.newMproject[GLSettings.numberProject] + "\\cad\\";
            GLSettings.locateMALHA = @".\" + GLSettings.newMproject[GLSettings.numberProject] + "\\malha\\";
            GLSettings.locateSTL = @".\" + GLSettings.newMproject[GLSettings.numberProject] + "\\stl\\";
            GLSettings.locateCAM = @".\" + GLSettings.newMproject[GLSettings.numberProject] + "\\cam\\";
            GLSettings.locateTMP = @".\" + GLSettings.newMproject[GLSettings.numberProject] + "\\tmp\\";
            if (type != "newProject")
            {
                treeView1.Nodes[GLSettings.numberProject].Checked = true;
                GeneralTools ge = new GeneralTools();
                ge.createDirectoryInit();
                GLSettings.projectCURRENT = GLSettings.numberProject;
                GLSettings.numberProject++;
            }
            else
            {               
                GeneralTools ge = new GeneralTools();
                ge.createDirectoryInit();
                GLSettings.projectCURRENT = GLSettings.numberProject;               
                treeView1.Nodes[GLSettings.numberProject].Checked = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cad_"></param>
        /// <param name="malha"></param>
        /// <param name="stl"></param>
        /// <param name="cam"></param>
        private void atualizaProjeto(bool cad_, bool malha, bool stl, bool cam)
        {
            bool registrar = true;            

            for (int j = 0; j < treeView1.Nodes[GLSettings.projectCURRENT].Nodes.Count; j++)
            {
                for (int i = 0; i < treeView1.Nodes[GLSettings.projectCURRENT].Nodes[j].Nodes.Count; i++)
                {
                    switch(j)
                    {
                        case 0:
                            if(cad_) treeView1.Nodes[GLSettings.projectCURRENT].Nodes[j].Nodes[i].Remove();
                            break;
                        case 1:
                            if (malha) treeView1.Nodes[GLSettings.projectCURRENT].Nodes[j].Nodes[i].Remove();
                            break;
                        case 2:
                            if (stl) treeView1.Nodes[GLSettings.projectCURRENT].Nodes[j].Nodes[i].Remove();
                            break;
                        case 3:
                            if (cam) treeView1.Nodes[GLSettings.projectCURRENT].Nodes[j].Nodes[i].Remove();
                            break;
                    }                    
                }
            }

            DirectoryInfo cad = new DirectoryInfo(GLSettings.locateCAD);
            foreach (FileInfo file in cad.GetFiles())
            {
                foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.cad].Nodes)
                {
                    if (st.Text == file.Name)
                    {
                        registrar = false;
                    }
                }

                if (registrar) treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.cad].Nodes.Add(file.Name);
                registrar = true;
            }

            registrar = true;          

            DirectoryInfo MODELOS = new DirectoryInfo(GLSettings.locateMALHA);
            foreach (FileInfo file in MODELOS.GetFiles())
            {
                foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.malha].Nodes)
                {
                    if (st.Text == file.Name)
                    {
                        registrar = false;
                    }
                }

                if (registrar) treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.malha].Nodes.Add(file.Name);
                registrar = true;
            }

            registrar = true;

            DirectoryInfo STL = new DirectoryInfo(GLSettings.locateSTL);
            foreach (FileInfo file in STL.GetFiles())
            {
                foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.stl].Nodes)
                {
                    if (st.Text == file.Name)
                    {
                        registrar = false;
                    }
                }

                if (registrar) treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.stl].Nodes.Add(file.Name);
                registrar = true;
            }

            registrar = true;

            DirectoryInfo CAM = new DirectoryInfo(GLSettings.locateCAM);
            foreach (FileInfo file in CAM.GetFiles())
            {
                foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.cam].Nodes)
                {
                    if (st.Text == file.Name)
                    {
                        registrar = false;
                    }
                }

                if (registrar) treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.cam].Nodes.Add(file.Name);
                registrar = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        private void removeArquivo(string name)
        {
            DirectoryInfo SOLIDO = new DirectoryInfo(GLSettings.locateCAD);
            foreach (FileInfo file in SOLIDO.GetFiles())
            {
                if (name == file.Name)
                {
                    File.Delete(GLSettings.locateCAD + name);
                }                             
            }

            DirectoryInfo MODELOS = new DirectoryInfo(GLSettings.locateMALHA);
            foreach (FileInfo file in MODELOS.GetFiles())
            {
                if (name == file.Name)
                {
                    File.Delete(GLSettings.locateMALHA + name);
                }                
            }

            DirectoryInfo STL = new DirectoryInfo(GLSettings.locateSTL);
            foreach (FileInfo file in STL.GetFiles())
            {              
                if (name == file.Name)
                {
                    File.Delete(GLSettings.locateSTL + name);
                }
            }           

            DirectoryInfo CAM = new DirectoryInfo(GLSettings.locateCAM);
            foreach (FileInfo file in CAM.GetFiles())
            {
                if (name == file.Name)
                {
                    File.Delete(GLSettings.locateCAM + name);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOpenGLControl()
        {
            this.OpenGLControl = new OpenGLControl();
            this.SuspendLayout();
            // 
            // openGLControl1
            // 
            this.OpenGLControl.Dock = DockStyle.Fill;
            this.OpenGLControl.Location = new Point(0, 0);
            this.OpenGLControl.Name = "openGLControl1";
            this.OpenGLControl.Size = new Size(854, 453);
            this.OpenGLControl.TabIndex = 0;

            panelOpenKinect.Controls.Add(this.OpenGLControl);

            this.ResumeLayout(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depthInfo"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ShowPointCloud(ushort[] depthInfo, int width, int height)
        {
            List<Vector3d> myVectors = Vertices.ConvertToVector3DList_FromArray(depthInfo, width, height);
            this.OpenGLControl.ShowPointCloud("Depth Point Cloud", myVectors, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            GLSettings.SaveSettings();
            base.OnClosed(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myVerticesList"></param>
        /// <param name="color"></param>
        public void ShowListOfVertices(List<Vertex> myVerticesList, byte[] color)
        {
            if (color != null)
            {
                List<float[]> myColors = PointCloudUtils.CreateColorList(myVerticesList.Count, color[0], color[1], color[2], color[3]);
                Vertices.SetColorToList(myVerticesList, myColors);

            }

            this.OpenGLControl.ShowPointCloud("Point Cloud", myVerticesList);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myModel"></param>
        /// <param name="removeAllOthers"></param>
        public void ShowModel(Model3D myModel, bool removeAllOthers)
        {
            if (removeAllOthers)
                this.OpenGLControl.RemoveAllModels();
            this.OpenGLControl.ShowModel(myModel);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myLinesFrom"></param>
        /// <param name="myLinesTo"></param>
        public void SetLineData(List<Vertex> myLinesFrom, List<Vertex> myLinesTo)
        {

            this.OpenGLControl.GLrender.LinesFrom = myLinesFrom;
            this.OpenGLControl.GLrender.LinesTo = myLinesTo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void abrirArquivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            { 
                GLSettings.newMproject.Add("projeto" + GLSettings.numberProject);
                Directory.CreateDirectory(@".\\" + GLSettings.newMproject[GLSettings.numberProject]);
                this.Cursor = Cursors.WaitCursor;
                info.Text = "INFO: Iniciando carregamento da imangem 3D ...";
                importModel.ShowDialog();
                GeneralTools tools = new GeneralTools();
                tools.DirectoryCopy(importModel.SelectedPath, @".\\" + GLSettings.newMproject[GLSettings.numberProject], true);

                treeView1.Nodes.Add(GLSettings.newMproject[GLSettings.numberProject]);
                treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add("solido");
                treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add("modelo");
                treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add("stl");
                treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add("cam");
                GLSettings.locateCAD = @".\" + GLSettings.newMproject[GLSettings.numberProject] + "\\solido\\";
                GLSettings.locateMALHA = @".\" + GLSettings.newMproject[GLSettings.numberProject] + "\\modelo\\";
                GLSettings.locateSTL = @".\" + GLSettings.newMproject[GLSettings.numberProject] + "\\stl\\";
                GLSettings.locateCAM = @".\" + GLSettings.newMproject[GLSettings.numberProject] + "\\cam\\";
                GLSettings.locateTMP = @".\" + GLSettings.newMproject[GLSettings.numberProject] + "\\tmp\\";
                treeView1.Nodes[GLSettings.numberProject].Checked = true;
                GLSettings.projectCURRENT = GLSettings.numberProject;

                atualizaProjeto(true, true, true, true);
                GLSettings.numberProject++;

                this.Cursor = Cursors.Default;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showAxisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showAxisToolStripMenuItem.Text = OpenGLControl.showAxesToolString(showAxisToolStripMenuItem.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showNormalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                showNormalsToolStripMenuItem.Text = OpenGLControl.showNormalsTool(showNormalsToolStripMenuItem.Text, toolStripComboBox1.SelectedIndex);
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGLControl.viewMode(wireframeToolStripMenuItem.Text);
            selectView = wireframeToolStripMenuItem.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void solidToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenGLControl.viewMode(solidToolStripMenuItem.Text);
            selectView = solidToolStripMenuItem.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectedCamera(rotateToolStripMenuItem.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void translateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectedCamera(translateToolStripMenuItem.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removerModelo3DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removerModelo3D();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGLControl.viewMode(pointToolStripMenuItem.Text);
            selectView = pointToolStripMenuItem.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sobreToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void cadastroDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegisterUserView sf = new RegisterUserView(OpenGLControl);
            sf.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tutorialToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TutorialView sf = new TutorialView();
            sf.StartPosition = FormStartPosition.CenterScreen;
            sf.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               if (saveOBJ(true, false))
               {
                    OpenGLControl.settingsTool(toolStripComboBox1.SelectedIndex, toolStripComboBox1.SelectedText, selectView);
                    MarcarAreaCorte();
                    MarcarAreaCorte();
                    toolStripComboBox1.SelectedIndex = toolStripComboBox1.FindString("Marcação Corte");
                }
            }
            catch
            {
                warning(1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cuboToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Vector3d colorCubo = new Vector3d(10, 10, 10);
                Bitmap texturaCubo = new Bitmap(1, 1);
                Model3D modeloCubo = Models.StandardModels3D.Cuboid("Cubo", 2000, 2000, 100, colorCubo, texturaCubo);
                OpenGLControl.AddModel(modeloCubo);
                OpenGLControl.ShowModels();
                toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cilindroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Vector3d colorCilindro = new Vector3d(0, 0, 0);
                Model3D modeloCilindro = Models.StandardModels3D.Cylinder("Cilindro", 5, 30, 500, colorCilindro, null);
                OpenGLControl.AddModel(modeloCilindro);
                OpenGLControl.ShowModels();
                toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";               
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void discoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Vector3d colorDisco = new Vector3d(0, 0, 0);
                Model3D modeloDisco = Models.StandardModels3D.Disk("Disco", 5, 10, 100, colorDisco, null);
                OpenGLControl.AddModel(modeloDisco);
                OpenGLControl.ShowModels();
                toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";               
            }          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void esferaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Vector3d colorEsfera = new Vector3d(0, 0, 0);
                Model3D modeloEsfera = Models.StandardModels3D.Sphere("Esfera", 5, 100, colorEsfera, null);
                OpenGLControl.AddModel(modeloEsfera);
                OpenGLControl.ShowModels();
                toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cubo2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void coneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Vector3d colorCone = new Vector3d(0, 0, 0);
                Model3D modeloCone = Models.StandardModels3D.Cone("Cone", 5, 10, 100, colorCone, null);
                OpenGLControl.AddModel(modeloCone);
                OpenGLControl.ShowModels();
                toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forma1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subtracaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutView sf = new AboutView();
            sf.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainView_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 4;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            OpenGLControl.viewMode("Point");
            selectView = "Point";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            OpenGLControl.viewMode("Wireframe");
            selectView = "Wireframe";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            OpenGLControl.viewMode("Solid");
            selectView = "Solid";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {
            OpenGLControl.selectedCamera("Translação");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click_2(object sender, EventArgs e)
        {
            OpenGLControl.selectedCamera("Rotação");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click_3(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stl"></param>
        /// <param name="noAddDirectoryTreeFile"></param>
        /// <returns></returns>
        private bool saveOBJ(bool stl, bool noAddDirectoryTreeFile)
        {
            try
            {
                if (stl)
                {
                    if(!union) Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[toolStripComboBox1.Items.IndexOf(toolStripComboBox1.SelectedItem)], GLSettings.locateTMP, GLSettings.ModeloAux);
                    else
                    {
                        Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[toolStripComboBox1.Items.IndexOf(toolStripComboBox1.SelectedItem)], GLSettings.locateTMP, GLSettings.ModeloAux2);
                    }
                }
                else
                {
                    exportModel.Filter = "*Carregar arquivo|*.obj";
                    exportModel.ShowDialog();
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[toolStripComboBox1.Items.IndexOf(toolStripComboBox1.SelectedItem)], exportModel.FileName, "");
                }

                if (!noAddDirectoryTreeFile)
                {

                    for (int j = 0; j < treeView1.Nodes[GLSettings.projectCURRENT].Nodes.Count; j++)
                    {
                        for (int i = 0; i < treeView1.Nodes[GLSettings.projectCURRENT].Nodes[j].Nodes.Count; i++)
                        {
                            if (treeView1.Nodes[GLSettings.projectCURRENT].Nodes[j].Nodes[i].Checked) CheckedStartOpen = true;
                        }
                    }
              
                    atualizaProjeto(true, true, true, true);                   
                }              
                return true;
            }
            catch
            {
                warning(1);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool status = false; 

            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                if (treeView1.Nodes[i].Checked)
                {
                   // exportModel.Filter = "*Salvar projeto |"; //+ treeView1.Nodes[i].Text;
                    exportModel.ShowDialog();                  
                    GeneralTools tools = new GeneralTools();
                    tools.DirectoryCopy(treeView1.Nodes[i].FullPath, exportModel.FileName , true);                   
                    status = true;

                    WarningView sf = new WarningView(11);
                    sf.ShowDialog();
                }
            }    
            
            if(!status)
            {
                WarningView sf = new WarningView(10);
                sf.ShowDialog();
            }
        }

        /// <summary>
        /// Exporta para o formato *.STL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportModel.Filter = "*Exportar arquivo|*.stl";
            exportModel.ShowDialog();
            STLinIO stl = new STLinIO();
            stl.export(exportModel.FileName, GLSettings.locateTMP + GLSettings.ModeloAux, "stl");         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xyzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportModel.Filter = "*Exportar arquivo|*.off";
            exportModel.ShowDialog();
            STLinIO off = new STLinIO();
            off.export(exportModel.FileName, GLSettings.locateTMP + GLSettings.ModeloAux, "off");       
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void g3meshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportModel.Filter = "*Exportar arquivo|*.g3mesh";
            exportModel.ShowDialog();
            STLinIO g3mesh = new STLinIO();
            g3mesh.export(exportModel.FileName, GLSettings.locateTMP + GLSettings.ModeloAux, "g3mesh");      
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportModel.Filter = "*Exportar arquivo|*.obj";
            exportModel.ShowDialog();
            STLinIO obj = new STLinIO();
            obj.export(exportModel.FileName, GLSettings.locateTMP + GLSettings.ModeloAux, "obj");               
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //importMalha();
            //OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
        }

        /// <summary>
        /// F001
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void filtroSuavizar01ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// O002
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otimizacapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Filters filter = new Filters();
                filter.FilterOptimization();
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                this.Cursor = Cursors.Default;
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }           
        }

        /// <summary>
        /// O002: EdgeRefineFlags.FullyConstrained
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Filters filter = new Filters();
                filter.FilterReduceConstraintsFixedverts("FullyConstrained");
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                this.Cursor = Cursors.Default;
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";                
            }           
        }

        /// <summary>
        /// F002
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filtroSuavizar02ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// F003
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filtroDeSuavizaToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// O003: EdgeRefineFlags.NoCollapse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otimizaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Filters filter = new Filters();
                filter.FilterReduceConstraintsFixedverts("NoCollapse");
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                this.Cursor = Cursors.Default;
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }           
        }

        /// <summary>
        /// O004: EdgeRefineFlags.NoConstraint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otimizaçãoO004ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Filters filter = new Filters();
                filter.FilterReduceConstraintsFixedverts("NoConstraint");
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                this.Cursor = Cursors.Default;
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }
          
        }

        /// <summary>
        /// O005: EdgeRefineFlags.NoFlip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otimizaçãoO005ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Filters filter = new Filters();
                filter.FilterReduceConstraintsFixedverts("NoFlip");
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                this.Cursor = Cursors.Default;
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";                
            }           
        }

        /// <summary>
        /// O006: EdgeRefineFlags.NoSplit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otimizaçãoO006ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Filters filter = new Filters();
                filter.FilterReduceConstraintsFixedverts("NoSplit");
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                this.Cursor = Cursors.Default;
            }
            catch (Exception)
            {
                info.Text = "INFO: Confirmar seleção de modelo";                
            }           
        }

        /// <summary>
        /// EdgeRefineFlags.PreserveTopology
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otimizaçãoO007ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Filters filter = new Filters();
                filter.FilterReduceConstraintsFixedverts("PreserveTopology");
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                this.Cursor = Cursors.Default;
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";              
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iNToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void marcarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MarcarAreaCorte();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aplicarCorteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            recorteMalhaX();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btMarcar_Click(object sender, EventArgs e)
        {
            MarcarAreaCorte();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Recorte_Click(object sender, EventArgs e)
        {
            recorteMalhaX();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MarcarAreaCorte()
        {
            try
            {
                if (!marcardo)
                {
                    Vector3d colorForm1 = new Vector3d(0.9, 0.5, 0.3);
                    Model3D modeloForm1 = Models.StandardModels3D.Cuboid_AllLines("Marcação Corte", 100, colorForm1, null);
                    OpenGLControl.AddModel(modeloForm1);
                    OpenGLControl.viewMode("Wireframe");
                    OpenGLControl.ShowModels();
                    marcardo = true;
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                }
                else
                {

                    foreach (var item in toolStripComboBox1.Items)
                    {
                        if (item.Equals("Marcação Corte"))
                        {
                            OpenGLControl.removeSelectedModel(toolStripComboBox1.Items.IndexOf(item));
                        }
                    }
                    OpenGLControl.modelsList.Remove("Marcação Corte");
                    toolStripComboBox1.Items.Remove("Marcação Corte");
                    marcardo = false;
                }
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        private void recorteMalhaX()
        {
            try
            {           
                if(toolStripComboBox1.SelectedText != "Marcação Corte")
                { 
                    info.Text = "INFO: Recorte iniciado ...";
                    this.Cursor = Cursors.WaitCursor;

                    int contBloco = 0;

                    if (saveOBJ(true, true))
                    {            
                        GeneralTools tools = new GeneralTools();                   
                        tools.cutmeshX();
                        GLSettings.nomeDorso = toolStripComboBox1.Text;              

                        if(!GLSettings.separarEncostoAssento)
                        { 
                            if (GLSettings.numberDivblocoExecutado == 1)
                            {
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco1Out_);                   
                            }
                            else if (GLSettings.numberDivblocoExecutado == 2)
                            {
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco1Out_);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco2Out_);
                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);                           
                            }
                            else if (GLSettings.numberDivblocoExecutado == 3)
                            {
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco1Out_);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco2Out_);
                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco3Out_);
                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                            }
                            else if (GLSettings.numberDivblocoExecutado == 4)
                            {
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco1Out_);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco2Out_);
                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco3Out_);
                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco4Out_);
                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco4XEncostoInit);
                            }
                            else if (GLSettings.numberDivblocoExecutado == 5)
                            {
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco1Out_);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco2Out_);
                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco3Out_);
                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco4Out_);
                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco4XEncostoInit);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco5Out_);
                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco5XEncostoInit);
                            }
                            contBloco = GLSettings.numberDivblocoExecutado;

                            do
                            {
                                toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - contBloco]);                                                                                              
                                contBloco--;

                            } while (contBloco != 0);

                        }
                        try
                        {
                            foreach (var item in toolStripComboBox1.Items)
                            {
                                if (item.Equals(GLSettings.nomeDorso))
                                {
                                    OpenGLControl.removeSelectedModel(toolStripComboBox1.Items.IndexOf(item));
                                    toolStripComboBox1.Items.Remove(item);
                                }
                            }
                        }
                        catch
                        {
                            info.Text = "INFO: Confirmar seleção de modelo";
                        }

                        foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.malha].Nodes)
                        {                       
                            st.Checked = false;                        
                        }

                        toolStripComboBox1.Text = "Selecionar Modelo";
                        OpenGLControl.modelsListSelect = -1;                    
                    }

                    atualizaProjeto(true, true, true, true);
                    contBloco = GLSettings.numberDivblocoExecutado;
                    do
                    {
                        CheckedStartOpen = true;
                        foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.malha].Nodes)
                        {
                            if (st.Text == OpenGLControl.modelsList[OpenGLControl.modelsList.Count - contBloco].ToString())
                            {
                                st.Checked = true;
                            }
                        }

                        contBloco--;

                    } while (contBloco != 0);

                    info.Text = "INFO: Recorte finalizado ...";
            
                }
                else
                {
                    WarningView sf = new WarningView(2);
                    sf.ShowDialog();
                }

                this.Cursor = Cursors.Default;
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";               
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void recorteMalhaZ()
        {
            GeneralTools generalTools = new GeneralTools();
            Filters filter = new Filters();

            try
            {
                if (toolStripComboBox1.SelectedText != "Marcação Corte")
                {
                    info.Text = "INFO: Recorte iniciado ...";
                    this.Cursor = Cursors.WaitCursor;

                    int contBloco = 0;

                    if (saveOBJ(true, true))
                    {
                        GeneralTools tools = new GeneralTools();
                        tools.cutmeshZ("", false, false, 0);
                        GLSettings.nomeDorso = toolStripComboBox1.Text;

                        if (!GLSettings.separarEncostoAssento)
                        {
                            if (GLSettings.numberDivblocoExecutado == 1)
                            {
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco1Out_);
                            }
                            else if (GLSettings.numberDivblocoExecutado == 2)
                            {
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco1Out_);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco2Out_);
                                //OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                            }
                            else if (GLSettings.numberDivblocoExecutado == 3)
                            {
                                generalTools.MarchingCubes2(GLSettings.locateMALHA, GLSettings.Bloco1Out_, OpenGLControl, filter);
                                generalTools.MarchingCubes2(GLSettings.locateMALHA, GLSettings.Bloco2Out_, OpenGLControl, filter);
                                generalTools.MarchingCubes2(GLSettings.locateMALHA, GLSettings.Bloco3Out_, OpenGLControl, filter);
                                tools.cutmeshZ(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, true, true, 0);                         
                                generalTools.MarchingCubes2(GLSettings.locateMALHA, GLSettings.Bloco1InvOut_, OpenGLControl, filter);                                                               
                                generalTools.MarchingCubes2(GLSettings.locateMALHA, GLSettings.Bloco2InvTMP_, OpenGLControl, filter);
                                tools.cutmeshZ(GLSettings.locateCAD + GLSettings.Bloco2InvTMP_, true, false, 1);
                                generalTools.MarchingCubes2(GLSettings.locateMALHA, GLSettings.Bloco2InvOut_, OpenGLControl, filter);  
                                tools.RotateAxisAngle(GLSettings.locateMALHA + GLSettings.Bloco3InvOut_, GLSettings.locateMALHA + "INV" + GLSettings.Bloco3InvOut_, OpenGLControl, 90, 0, -1 , 0);
                                generalTools.MarchingCubes2(GLSettings.locateMALHA, "INV" + GLSettings.Bloco3InvOut_, OpenGLControl, filter);
                                tools.RotateAxisAngle(GLSettings.locateMALHA + GLSettings.Bloco3InvOut_, GLSettings.locateMALHA + GLSettings.Bloco3InvOut_, OpenGLControl, 90, 0, 1, 0);

                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco1Out_);
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco2Out_);               
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.Bloco3Out_);                              

                                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncosto);                             
                                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
                                OpenGLControl.Refresh();

                                /*
                                toolStripComboBox1.Items.Clear();

                                for (int i = 2; i < OpenGLControl.modelsList.Count; i++)
                                {
                                     toolStripComboBox1.Items.Add(OpenGLControl.modelsList[i]);
                                }
                                */

                                //remover daqui colocar em outro lugar 
                                tools.RotateAxisAngle(GLSettings.locateCAD + GLSettings.Bloco2InvOut_, GLSettings.locateCAD + GLSettings.Bloco2InvOut_, OpenGLControl, 90, 0, -1, 0);
                                tools.RotateAxisAngle(GLSettings.locateCAD + GLSettings.Bloco3InvOut_, GLSettings.locateCAD + GLSettings.Bloco3InvOut_, OpenGLControl, 180, 0, -1, 0);

                                //Adicionado somente para melhorar o aproveitamento da espuma
                                //tools.RotateAxisAngleAdjustment(GLSettings.locateCAD + GLSettings.Bloco2InvOut_, GLSettings.locateCAD + GLSettings.Bloco2InvOut_, OpenGLControl, 90, 0, 0, -1, 2);
                                //tools.RotateAxisAngleAdjustment(GLSettings.locateCAD + GLSettings.Bloco3InvOut_, GLSettings.locateCAD + GLSettings.Bloco3InvOut_, OpenGLControl, 90, 0, 0, -1, 3);

                                tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco1InvOut_, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl, 1);
                                tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco1InvOut_, GLSettings.locateCAD + GLSettings.Bloco1InvOut_, OpenGLControl, 1);                             
                                tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco2InvOut_, GLSettings.locateTMP + GLSettings.Desbaste_Complemento2, OpenGLControl, 2);
                                tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco2InvOut_, GLSettings.locateCAD + GLSettings.Bloco2InvOut_, OpenGLControl, 2);                      
                                tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco3InvOut_, GLSettings.locateTMP + GLSettings.Desbaste_Complemento3, OpenGLControl, 3);
                                tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco3InvOut_, GLSettings.locateCAD + GLSettings.Bloco3InvOut_, OpenGLControl, 3);
                                
                                btGcodePart.Enabled = true;

                                toolStripComboBox1.Items.Clear();
                                OpenGLControl.RemoveAllModels();                                                                

                                while (!File.Exists(GLSettings.locateSTL + GLSettings.Bloco3InvOutSTL)) { }                                
                            }
                        }
                    }          

                    atualizaProjeto(true, true, true, true);
                    
                    info.Text = "INFO: Recorte finalizado ...";
                }
                else
                {
                    WarningView sf = new WarningView(2);
                    sf.ShowDialog();
                }

                this.Cursor = Cursors.Default;
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";                
            }           
                    
        }

        /// <summary>
        /// 
        /// </summary>
        private void removerModelo3D()
        {
            info.Text = "INFO: Removendo modelo selecionado ...";

            removeArquivo(toolStripComboBox1.SelectedItem.ToString());
            atualizaProjeto(true, true, true, true);
            atualizaProjeto(true, true, true, true);
            atualizaProjeto(true, true, true, true);

            try
            {
                if (toolStripComboBox1.SelectedIndex >= 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    info.Text = "     :Removendo imagem ...";
                    OpenGLControl.removeSelectedModel(toolStripComboBox1.SelectedIndex);
                    info.Text = "     :Imagem removida!";
                    this.Cursor = Cursors.Default;
                    info.Text = "     :Inf";
                    toolStripComboBox1.Items.Remove(toolStripComboBox1.SelectedItem);
                    toolStripComboBox1.Text = "Selecinar Modelo";

                    if (marcardo)
                    {
                        indiceMarcadorCorte--;
                    }
                }
                info.Text = "INFO: Modelo ";
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";             
            }          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click_3(object sender, EventArgs e)
        {
            removerModelo3D();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotaçãoModeloEixoXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectModelo(object sender, EventArgs e)
        {
            try
            {
                OpenGLControl.modelsListSelect = toolStripComboBox1.SelectedIndex + 1;
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";               
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotaçãoModeloEixoYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotaçãoModeloEixoZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void translaçãoModeloEixoXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void translaçãoModeloEixoYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 4;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void translaçãoModeloEixoZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inverterOrientaçãoModelo3DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inverterOrientaçãoModelo3D();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click_2(object sender, EventArgs e)
        {
            inverterOrientaçãoModelo3D();
        }

        /// <summary>
        /// 
        /// </summary>
        private void inverterOrientaçãoModelo3D()
        {
            //TODO: Pegar as coordenadas de translação realizadas na tela e enviar para metodo salvar e testar 
            // se realmente vai ter a mesma movimentação 
            info.Text = "INFO: Inverter triangulos do modelo ";

            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (saveOBJ(true, true))
                {
                    GeneralTools tools = new GeneralTools();
                    tools.ConvertpToUp(inverterOrientaçãoModelo3D_select);
                    OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                    inverterOrientaçãoModelo3D_select++;
                    if (inverterOrientaçãoModelo3D_select > 1) inverterOrientaçãoModelo3D_select = 0;
                }
                //   atualizaProjeto();
                this.Cursor = Cursors.Default;

                atualizaProjeto(true, true, true, true);

                info.Text = "INFO: Triangulos invertidos com sucesso!";
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";              
            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btConfirmacao_Click(object sender, EventArgs e)
        {
            confirmarMudancas();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void confirmarMudançasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            confirmarMudancas();
        }

        /// <summary>
        /// 
        /// </summary>
        public void confirmarMudancas()
        {
            string errorText = "";
            try
            {
                info.Text = "INFO: Confirmando as anterações realizado no modelo ...";

                if (toolStripComboBox1.SelectedText != "Marcação Corte")
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (saveOBJ(true, true))
                    {
                        GLSettings.teste = true;
                        object nomeSecionado = toolStripComboBox1.SelectedItem;                   
                        GeneralTools tools = new GeneralTools();                       

                        tools.ConfirmChanges(OpenGLControl, "confirmar");
                        tools.atualizarStatusMarcacao(OpenGLControl, "salvar");
                        OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                        tools.atualizarStatusMarcacao(OpenGLControl, "atualizar");
                        toolStripComboBox1.SelectedItem = nomeSecionado;

                        File.Delete(GLSettings.locateCURRENT);
                        File.Copy(GLSettings.locateTMP + GLSettings.ModeloAuxOut_, GLSettings.locateCURRENT);

                        OpenGLControl.viewMode("Wireframe");
                        OpenGLControl.GLrender.LoadModel(GLSettings.locateCURRENT, errorText);
                        OpenGLControl.ShowModels();
                    }
                }              
                info.Text = "INFO: Alterações confirmadas com sucesso!";             
            }
            catch 
            {
                WarningView sf = new WarningView(7);
                sf.ShowDialog();
                this.Cursor = Cursors.Default;
            }
       
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click_4(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (saveOBJ(true, true))
                {
                    GeneralTools tools = new GeneralTools();
                    tools.ReverseTriOrientation();
                    OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                }
                this.Cursor = Cursors.Default;
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";             
            }          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uniãoDeModelosToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
          
        }

        private void generatePart()
        {         
            if (GLSettings.modoFuncionamentoCorte == "Corte X")
            {
                geradorSolidoX();
            }
            else
            {
                geradorSolidoZ();
            }

            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
        }

        private void gcode(bool part)
        {
            WarningViewDecision sf = new WarningViewDecision(1);
            sf.ShowDialog();

            if (GLSettings.gerarGcode)
            {
                if(!part)
                {
                    if (!File.Exists(GLSettings.locateSTL + GLSettings.Bloco1Out_Tringulos_solido_desbasteSTL))
                    {
                        WarningView sf_ = new WarningView(20);
                        sf_.ShowDialog();
                    }
                    else
                    {
                        gerarGcode(part);
                        atualizaProjeto(false, false, false, true);
                    }
                }               
                else
                {
                    if (!File.Exists(GLSettings.locateSTL + GLSettings.Bloco1InvOutSTL))
                    {
                        WarningView sf_ = new WarningView(20);
                        sf_.ShowDialog();
                    }
                    else
                    {
                        gerarGcode(part);
                        atualizaProjeto(false, false, false, true);
                    }
                }               
            }
            else
            {      
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void geradorSolidoX()
        {
           // if (saveOBJ(true)) ;
            this.Cursor = Cursors.WaitCursor;
         
            Filters filter = new Filters();
            GeneralTools tools = new GeneralTools();

            info.Text = "INFO: Gerando arquivos CAD ...";

            try
            {
                if (GLSettings.numberDivblocoExecutado == 0)
                {                   
                }
                else if (GLSettings.numberDivblocoExecutado == 1)
                {
                    this.Cursor = Cursors.WaitCursor;

                    tools.GenerationTriangle_corte_X();

                    OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);
                
                    OpenGLControl.RemoveAllModels();
                    toolStripComboBox1.Items.Clear();

                    /* BLOCO 1 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl, 1);                              

                    this.Cursor = Cursors.WaitCursor;
                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;
                }
                else if (GLSettings.numberDivblocoExecutado == 2)
                {
                    this.Cursor = Cursors.WaitCursor;
                    tools.GenerationTriangle_corte_X();
                    OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco2Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco2Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);

                    OpenGLControl.RemoveAllModels();
                    toolStripComboBox1.Items.Clear();

                    /* BLOCO 1 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl, 1);                           

                    this.Cursor = Cursors.WaitCursor;
                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 2 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos, GLSettings.Bloco2Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos_Desbaste, GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento2, OpenGLControl, 2);                

                    this.Cursor = Cursors.WaitCursor;
                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
         
                    toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;
                }
                else if (GLSettings.numberDivblocoExecutado == 3)
                {
                    this.Cursor = Cursors.WaitCursor;

                    /* Gerando os triangulos para criar os blocos */
                    tools.GenerationTriangle_corte_X();

                    OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco2Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco2Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco3Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco3Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);

                    /* "Limpando" visualização */
                    OpenGLControl.RemoveAllModels();
                    toolStripComboBox1.Items.Clear();

                    /* BLOCO 1 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.Girar0GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl);                
                    tools.RotateAxisAngle(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl, 90, 0, -1, 0);               

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 2 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos, GLSettings.Bloco2Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos_Desbaste, GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento2, OpenGLControl, 2);                

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 3 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos, GLSettings.Bloco3Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos_Desbaste, GLSettings.Bloco3Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos180GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento3, OpenGLControl);                
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, OpenGLControl, 3);                

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                }
                else if (GLSettings.numberDivblocoExecutado == 4)
                {
                    this.Cursor = Cursors.WaitCursor;

                    tools.GenerationTriangle_corte_X();

                    OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco2Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco2Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco3Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco3Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco4Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco4Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco4XEncostoInit);

                    OpenGLControl.RemoveAllModels();
                    toolStripComboBox1.Items.Clear();

                    /* BLOCO 1 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl, 1);                

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 2 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos, GLSettings.Bloco2Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos_Desbaste, GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento2, OpenGLControl, 2);                

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 3 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos, GLSettings.Bloco3Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos_Desbaste, GLSettings.Bloco3Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento3, OpenGLControl, 3);                

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 4 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco4Out_, GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos, GLSettings.Bloco4Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco4Out_, GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos_Desbaste, GLSettings.Bloco4Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento4, OpenGLControl, 4);                

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco4XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                }
                else if (GLSettings.numberDivblocoExecutado == 5)
                {
                    this.Cursor = Cursors.WaitCursor;
                    tools.GenerationTriangle_corte_X();

                    OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco2Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco2Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco3Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco3Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco4Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco4Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco4XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco5Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco5Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco5XEncostoInit);

                    OpenGLControl.RemoveAllModels();
                    toolStripComboBox1.Items.Clear();

                    /* BLOCO 1 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl, 1);                

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 2 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos, GLSettings.Bloco2Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos_Desbaste, GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento2, OpenGLControl, 2);                

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 3 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos, GLSettings.Bloco3Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos_Desbaste, GLSettings.Bloco3Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento3, OpenGLControl, 3);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 4 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco4Out_, GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos, GLSettings.Bloco4Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco4Out_, GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos_Desbaste, GLSettings.Bloco4Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento4, OpenGLControl, 4);                

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco4XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 5 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco5Out_, GLSettings.locateTMP + GLSettings.Bloco5Out_Tringulos, GLSettings.Bloco5Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco5Out_, GLSettings.locateTMP + GLSettings.Bloco5Out_Tringulos_Desbaste, GLSettings.Bloco5Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixoY(GLSettings.locateCAD + GLSettings.Bloco5Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento5, OpenGLControl, 5);                

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco5Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco5XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                }
                toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;

                foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.malha].Nodes)
                {
                    st.Checked = false;
                }
                atualizaProjeto(true, false, true, true);

                int contBloco = GLSettings.numberDivblocoExecutado;
                do
                {
                    CheckedStartOpen = true;
                    foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.cad].Nodes)
                    {
                        if (st.Text == OpenGLControl.modelsList[OpenGLControl.modelsList.Count - contBloco].ToString())
                        {
                            st.Checked = true;
                        }
                    }

                    contBloco--;

                } while (contBloco != 0);

                info.Text = "INFO: Arquivos CAD gerados";

                this.Cursor = Cursors.Default;
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }
        }

        /// <summary>
        /// Solid Generate
        /// </summary>
        private void geradorSolido()
        {
            this.Cursor = Cursors.WaitCursor;


            if (File.Exists(GLSettings.locateSTL + GLSettings.Bloco1Out_Tringulos_solido_desbasteSTL))
            {
                WarningView sf_ = new WarningView(21);
                sf_.ShowDialog();
            }
            else
            {            
                Filters filter = new Filters();
                GeneralTools tools = new GeneralTools();

                info.Text = "INFO: Gerando arquivos CAD ...";

                try
                {
                    if (GLSettings.numberDivblocoExecutado == 0)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        tools.GenerationTriangle_corte_Z();

                        OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                        toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                        Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);

                        OpenGLControl.RemoveAllModels();
                        toolStripComboBox1.Items.Clear();

                        /* BLOCO 1 */
                        tools.Union(GLSettings.locateTMP + GLSettings.ModeloAux, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                        tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.ModeloAux, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                        tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl, 0);
                        tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                        this.Cursor = Cursors.WaitCursor;

                        OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                        toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                        toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;
                    }
                    else if (GLSettings.numberDivblocoExecutado == 1)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        tools.GenerationTriangle_corte_Z();

                        OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                        toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                        Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);

                        OpenGLControl.RemoveAllModels();
                        toolStripComboBox1.Items.Clear();

                        /* BLOCO 1 */
                        tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                        tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                        tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl, 1);
                        tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                        this.Cursor = Cursors.WaitCursor;

                        OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                        toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                        toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;
                    }  
 
                    toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;
                
                    foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.malha].Nodes)
                    {
                        st.Checked = false;
                    }
                
                    atualizaProjeto(true, false, true, true);

                
                    int contBloco = GLSettings.numberDivblocoExecutado;
                    do
                    {
                        CheckedStartOpen = true;
                        foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.cad].Nodes)
                        {
                            if (st.Text == OpenGLControl.modelsList[OpenGLControl.modelsList.Count - contBloco].ToString())
                            {
                                st.Checked = true;
                            }
                        }

                        contBloco--;

                    } while (contBloco != 0);

                    OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);

                    info.Text = "INFO: Arquivos CAD gerados";
                
                    this.Cursor = Cursors.Default;
                }
                catch
                {
                    info.Text = "INFO: Confirmar seleção de modelo";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void geradorSolidoZ()
        {
            this.Cursor = Cursors.WaitCursor;

            Filters filter = new Filters();
            GeneralTools tools = new GeneralTools();

            info.Text = "INFO: Gerando arquivos CAD ...";
            try
            {
                if (GLSettings.numberDivblocoExecutado == 0)
                {
                }
                else if (GLSettings.numberDivblocoExecutado == 1)
                {
                    this.Cursor = Cursors.WaitCursor;

                    tools.GenerationTriangle_corte_Z();

                    OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);

                    OpenGLControl.RemoveAllModels();
                    toolStripComboBox1.Items.Clear();

                    /* BLOCO 1 */
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl, 1);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    this.Cursor = Cursors.WaitCursor;

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;                                                   
                }
                else if (GLSettings.numberDivblocoExecutado == 2)
                {
                    this.Cursor = Cursors.WaitCursor;
                    tools.GenerationTriangle_corte_Z();

                    OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco2Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco2Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);

                    OpenGLControl.RemoveAllModels();
                    toolStripComboBox1.Items.Clear();

                    /* BLOCO 1 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl,1);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl, 0);


                    this.Cursor = Cursors.WaitCursor;
                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 2 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos, GLSettings.Bloco2Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos_Desbaste, GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento2, OpenGLControl,2);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    this.Cursor = Cursors.WaitCursor;
                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;
                }
                else if (GLSettings.numberDivblocoExecutado == 3)
                {
                    this.Cursor = Cursors.WaitCursor;
                    tools.GenerationTriangle_corte_Z();

                    OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco2Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco2Out_Tringulos);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco3Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco3Out_Tringulos);

                    OpenGLControl.RemoveAllModels();
                    toolStripComboBox1.Items.Clear();

                    /* BLOCO 1 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl,1);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 2 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos, GLSettings.Bloco2Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos_Desbaste, GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento2, OpenGLControl,2);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 3 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos, GLSettings.Bloco3Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos_Desbaste, GLSettings.Bloco3Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento3, OpenGLControl,3);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                }
                else if (GLSettings.numberDivblocoExecutado == 4)
                {
                    this.Cursor = Cursors.WaitCursor;
                    tools.GenerationTriangle_corte_Z();

                    OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco2Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco2Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco3Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco3Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco4Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco4Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco4XEncostoInit);

                    OpenGLControl.RemoveAllModels();
                    toolStripComboBox1.Items.Clear();

                    /* BLOCO 1 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl,1);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 2 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos, GLSettings.Bloco2Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos_Desbaste, GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento2, OpenGLControl,2);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 3 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos, GLSettings.Bloco3Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos_Desbaste, GLSettings.Bloco3Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento3, OpenGLControl,3);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 4 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco4Out_, GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos, GLSettings.Bloco4Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco4Out_, GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos_Desbaste, GLSettings.Bloco4Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento4, OpenGLControl,4);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco4XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                }
                else if (GLSettings.numberDivblocoExecutado == 5)
                {
                    this.Cursor = Cursors.WaitCursor;

                    tools.GenerationTriangle_corte_Z();

                    OpenGLControl.loadTringuleGeneration(tools.Bloco1Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco1Out_Tringulos);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco2Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco2Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco3Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco3Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco4Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco4Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco4XEncostoInit);
                    OpenGLControl.loadTringuleGeneration(tools.Bloco5Out_Tringulos);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1], GLSettings.locateTMP, GLSettings.Bloco5Out_Tringulos);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco5XEncostoInit);

                    OpenGLControl.RemoveAllModels();
                    toolStripComboBox1.Items.Clear();

                    /* BLOCO 1 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos, GLSettings.Bloco1Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl);
                    filter.FilterSmoothing(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento1, OpenGLControl,1);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco1Out_Tringulos_solido);
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 2 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos, GLSettings.Bloco2Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos_Desbaste, GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento2, OpenGLControl,2);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco2Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco2XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 3 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos, GLSettings.Bloco3Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos_Desbaste, GLSettings.Bloco3Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento3, OpenGLControl,3);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco3Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco3XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);

                    /* BLOCO 4 */
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco4Out_, GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos, GLSettings.Bloco4Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco4Out_, GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos_Desbaste, GLSettings.Bloco4Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento4, OpenGLControl,4);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco4Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco4XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
               
                    this.Cursor = Cursors.WaitCursor;
                    tools.Union(GLSettings.locateTMP + GLSettings.Bloco5Out_, GLSettings.locateTMP + GLSettings.Bloco5Out_Tringulos, GLSettings.Bloco5Out_Tringulos_solido, OpenGLControl, filter);
                    tools.UnionDesbaste(GLSettings.locateTMP + GLSettings.Bloco5Out_, GLSettings.locateTMP + GLSettings.Bloco5Out_Tringulos_Desbaste, GLSettings.Bloco5Out_Tringulos_solido_desbaste, OpenGLControl);
                    tools.GirarMenos90GrausEixo_Y_X(GLSettings.locateCAD + GLSettings.Bloco5Out_Tringulos_solido_desbaste, GLSettings.locateTMP + GLSettings.Desbaste_Complemento5, OpenGLControl,5);
                    tools.AjustePosicionamento(GLSettings.locateCAD + GLSettings.Bloco5Out_Tringulos_solido_desbaste, GLSettings.locateCAD + GLSettings.Bloco5Out_Tringulos_solido_desbaste, OpenGLControl, 0);

                    OpenGLControl.loadTringuleGeneration(GLSettings.locateCAD + GLSettings.Bloco5Out_Tringulos_solido);
                    OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, GLSettings.Bloco5XEncostoInit);
                    OpenGLControl.Refresh();
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                }
                toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;

                foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.malha].Nodes)
                {
                    st.Checked = false;
                }
                atualizaProjeto(true, false, true, true);

                int contBloco = GLSettings.numberDivblocoExecutado;
                do
                {
                    CheckedStartOpen = true;
                    foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.cad].Nodes)
                    {
                        if (st.Text == OpenGLControl.modelsList[OpenGLControl.modelsList.Count - contBloco].ToString())
                        {
                            st.Checked = true;
                        }
                    }

                    contBloco--;

                } while (contBloco != 0);

                info.Text = "INFO: Arquivos CAD gerados";

                this.Cursor = Cursors.Default;
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionMarkerView sf = new OptionMarkerView(OpenGLControl);

            if (sf.ShowDialog() == DialogResult.OK)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            optionMarker();
        }

        /// <summary>
        /// 
        /// </summary>
        private void optionMarker()
        {
            info.Text = "INFO: Opções do marcador. ";

            OptionMarkerView sf = new OptionMarkerView(this.OpenGLControl);
            sf.StartPosition = FormStartPosition.CenterScreen;
            if (sf.ShowDialog() == DialogResult.OK)
            {
            }
            MarcarAreaCorte();
            MarcarAreaCorte();          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {
            if (saveOBJ(true, true))
            {
                // filter.FilterSmoothing("Uniform");
                // OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click_4(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 0;
            info.Text = "INFO: Translação X";     
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click_3(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 1;
            info.Text = "INFO: Translação Y";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 2;
            info.Text = "INFO: Translação Z";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click_5(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 3;
            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
            info.Text = "INFO: Rotação X";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton6_Click_2(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 4;
            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
            info.Text = "INFO: Rotação Y";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectMoveXYZ = 5;
            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
            info.Text = "INFO: Rotação Z";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton20_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectedCamera("Translação");
            info.Text = "INFO: Translação Modelo";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            OpenGLControl.selectedCamera("Rotação");
            info.Text = "INFO: Rotação Modelo";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            try
            {
                OpenGLControl.viewMode("Point");
                selectView = pointToolStripMenuItem.Text;
                info.Text = "INFO: Visualização PONTOS";
                OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            try
            {
                OpenGLControl.viewMode("Wireframe");
                selectView = pointToolStripMenuItem.Text;
                info.Text = "INFO: Visualização MALHA";
                OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
            }
            catch 
            {
        
            }     
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            try
            {
                OpenGLControl.viewMode("Solid");
                selectView = pointToolStripMenuItem.Text;
                info.Text = "INFO: Visualização SOLIDO";
                OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
            }
            catch
            {               
            }  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            try
            {
                MarcarAreaCorte();
            }
            catch
            {
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            try
            {
                GLSettings.numberDivblocoExecutado = GLSettings.numberDivblocoPlanejado;
                saida = File.Create(GLSettings.locateTMP + "numberDivblocoExecutado");
                escritor = new StreamWriter(saida);
                escritor.WriteLine(GLSettings.numberDivblocoExecutado);
                escritor.Close();
                saida.Close();

                if (GLSettings.modoFuncionamentoCorte == "Corte X")
                {
                    recorteMalhaX();

                    if(File.Exists(GLSettings.locateSTL + GLSettings.Bloco3InvOutSTL))
                    {
                        btGcodePart.Enabled = true;
                    }
                    else
                    {
                        WarningView sf = new WarningView(20);
                        sf.ShowDialog();
                    }
                }
                else if (GLSettings.modoFuncionamentoCorte == "Corte Z")
                {
                    recorteMalhaZ();
                }

                int tmp = 0;
                for (int h = 0; h < GLSettings.numberProject; h++)
                {
                    if (treeView1.Nodes[h].Checked)
                    {
                        tmp++;
                    }
                }

                clearSelectProject();
                treeView1.Nodes[tmp].Checked = true;
            }
            catch 
            {                
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            reverseTriOrientation_();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inverterOrientaçãoTriangulosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reverseTriOrientation_();
        }

        /// <summary>
        /// 
        /// </summary>
        private void reverseTriOrientation_()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (saveOBJ(true, true))
                {
                    GeneralTools tools = new GeneralTools();
                    tools.ReverseTriOrientation();
                    OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                }
                this.Cursor = Cursors.Default;
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";         
            }         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            try
            {
                inverterOrientaçãoModelo3D();
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "recorteMalhaX", GLSettings.ModeloAuxOut_);
                OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Translate(0, 0);
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";             
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
           
            if (Model3D.vetRot_portDesfazer.Count > 0)
            {
                Model3D.vetRot_port.X = Model3D.vetRot_portDesfazer[Model3D.vetRot_portDesfazer.Count - 1].X;
                Model3D.vetRot_port.Y = Model3D.vetRot_portDesfazer[Model3D.vetRot_portDesfazer.Count - 1].Y;
                Model3D.vetRot_port.Z = Model3D.vetRot_portDesfazer[Model3D.vetRot_portDesfazer.Count - 1].Z;                       
                Model3D.vetRot_portDesfazer.RemoveAt(Model3D.vetRot_portDesfazer.Count - 1);
            }

            info.Text = "INFO: Confirmando as anterações realizado no modelo ...";

            try
            {
                if (toolStripComboBox1.SelectedText != "Marcação Corte")
                {
                    if (saveOBJ(true, true))
                    {
                        object nomeSecionado = toolStripComboBox1.SelectedItem;
                        GeneralTools tools = new GeneralTools();
                        tools.ConfirmChanges(OpenGLControl, "desfazer");
                        tools.atualizarStatusMarcacao(OpenGLControl, "salvar");
                        OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                        tools.atualizarStatusMarcacao(OpenGLControl, "atualizar");
                        toolStripComboBox1.SelectedItem = nomeSecionado;
                    }
                }

                Model3D.vetTransl_port.X = 0;
                Model3D.vetTransl_port.Y = 0;
                Model3D.vetTransl_port.Z = 0;

                Model3D.vetRot_port.X = 0;
                Model3D.vetRot_port.Y = 0;
                Model3D.vetRot_port.Z = 0;

                OpenGLControl.Refresh();
                this.Cursor = Cursors.Default;

                info.Text = "INFO: Alterações confirmadas com sucesso!";
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";                
            }         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            try
            {
                OpenGLControl.viewMode("Wireframe");
                confirmarMudancas();                          
                confirmarMudancas();
                OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
            }
            catch (Exception)
            {        
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            removerModelo3D();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            try
            {
                info.Text = "INFO: Ferramenta para realizar a suavização da face do modelo ";
                suavizador();
                info.Text = "INFO: ";
            }
            catch 
            {
            }
          
        }

        /// <summary>
        /// 
        /// </summary>
        private void modeloSelecionado()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modeloSelecionado_(object sender, EventArgs e)
        {
            try
            {
                OpenGLControl.modelsListSelect = toolStripComboBox1.SelectedIndex + 1;
            }
            catch (Exception)
            {
                info.Text = "INFO: Confirmar seleção de modelo";                
            }
                  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void suavizadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        private void suavizador()
        {
            try
            {                
                OptionSmoothView sf = new OptionSmoothView(OpenGLControl, toolStripComboBox1.SelectedIndex, selectView);
                sf.StartPosition = FormStartPosition.CenterScreen;
                if (sf.ShowDialog() == DialogResult.OK) { }                           
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelOpenKinect_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipo"></param>
        private void warning(int tipo)
        {
            switch (tipo)
            {
                case 1:
                    WarningView sf = new WarningView(0);
                    sf.StartPosition = FormStartPosition.CenterScreen;
                    if (sf.ShowDialog() == DialogResult.OK) { }
                    break;
                case 2:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            projecaoSuperior();            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton24_Click(object sender, EventArgs e)
        {
            projecaoFrontal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton23_Click(object sender, EventArgs e)
        {
            projecaoDireita();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projecaoFrontal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void esquerdaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projecaoEsquerda();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void direitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projecaoDireita();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projecaoSuperior();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inferiorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projecaoInverior();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fundoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projecaoFundo();
        }

        /// <summary>
        /// 
        /// </summary>
        private void projecaoFrontal()
        {
            OpenGLControl.GLrender.projecaoFrontal(false);
            OpenGLControl.GLrender.projecaoFrontal(false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void projecaoEsquerda()
        {
            OpenGLControl.GLrender.projecaoEsquerda(false);
            OpenGLControl.GLrender.projecaoEsquerda(false);
        }

        private void projecaoDireita()
        {
            OpenGLControl.GLrender.projecaoDireita(false);
            OpenGLControl.GLrender.projecaoDireita(false);
        }

        private void projecaoSuperior()
        {          
            OpenGLControl.GLrender.projecaoSuperior(false);
            OpenGLControl.GLrender.projecaoSuperior(false);
        }

        private void projecaoInverior()
        {
            OpenGLControl.GLrender.projecaoInverior(false);
            OpenGLControl.GLrender.projecaoInverior(false);
        }

        private void projecaoFundo()
        {
            OpenGLControl.GLrender.projecaoFundo(false);
            OpenGLControl.GLrender.projecaoFundo(false);
        }

        private void isometricoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projecaoZX();
        }

        private void projecaoZX()
        {
            OpenGLControl.GLrender.projecaoZX(false);
            OpenGLControl.GLrender.projecaoZX(false);
        }

        private void toolStripButton25_Click(object sender, EventArgs e)
        {
            marcarRegiaoRemove();
        }

        private void marcarRegiãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            marcarRegiaoRemove();
        }

        private void removerPontosMarcadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removerPontos();
            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
        }

        private void removerTriangulosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removerTriangulos();
            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
        }

        private void toolStripButton26_Click(object sender, EventArgs e)
        {
            removerTriangulos();
            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
        }

        private void toolStripButton27_Click(object sender, EventArgs e)
        {
            removerPontos();
            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
        }

        /// <summary>
        /// 
        /// </summary>
        private void marcarRegiaoRemove()
        {            
            if (OpenGLControl.modelsListSelect - 1 >= 0)
            {
                if (!marcarRegiao.CheckOnClick)
                {
                    marcarRegiao.CheckState = CheckState.Checked;
                    marcarRegiao.CheckOnClick = !marcarRegiao.CheckOnClick;
                    toolStripButton27.Enabled = true;
                    toolStripButton26.Enabled = true;
                    toolStripButton25.Enabled = true;              
                    desmarcarToolStripMenuItem.Enabled = false;
                    toolStripButton5.Enabled = false;
                    toolStripButton31.Enabled = false;
                    removerPontosMarcadosToolStripMenuItem.Enabled = true;
                    removerTriangulosToolStripMenuItem.Enabled = true;
                    marcaçãoPolygonoToolStripMenuItem.Enabled = false;
                    GLSettings.marcarRegiaoOn = true;
                }
                else
                {
                    marcarRegiao.CheckState = CheckState.Unchecked;
                    marcarRegiao.CheckOnClick = !marcarRegiao.CheckOnClick;
                    toolStripButton27.Enabled = false;
                    toolStripButton26.Enabled = false;
                    toolStripButton25.Enabled = false;
                    toolStripButton31.Enabled = true;
                    desmarcarToolStripMenuItem.Enabled = false;
                    toolStripButton5.Enabled = true;
                    removerPontosMarcadosToolStripMenuItem.Enabled = false;
                    removerTriangulosToolStripMenuItem.Enabled = false;
                    marcaçãoPolygonoToolStripMenuItem.Enabled = true;
                    GLSettings.marcarRegiaoOn = false;

                    for (int i = 0; i < OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList.Count; i++)
                    {
                        OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                        OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                    }
                    OpenGLControl.ChangeDisplayMode(OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].ModelRenderStyle);
                }
            }
            else
            {
                WarningView sf = new WarningView(0);
                sf.ShowDialog();
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        private void removerPontos()
        {          
            this.Cursor = Cursors.WaitCursor;
            GeneralTools tools = new GeneralTools();
            tools.removerPontos(OpenGLControl, "removerPontos");
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        private void removerTriangulos()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                GeneralTools tools = new GeneralTools();
                tools.removerPontos(OpenGLControl, "removerTriangulos");
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAuxOut_);
                confirmarMudancas();
                this.Cursor = Cursors.Default;

            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";                
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void perspectivaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            perspectiva();
        }

        private void perspectiva()
        {
            GLSettings.projecaoPerpectiva = true;
            OpenGLControl.Refresh();

            perspectivaToolStripMenuItem1.CheckState = CheckState.Checked;
            ortogonalToolStripMenuItem.CheckState = CheckState.Unchecked;
            toolStripButton5.Enabled = false;
            toolStripButton31.Enabled = false;
            marcarRegiao.Enabled = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ortogonalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ortogonal();
        }

        private void ortogonal()
        {
            GLSettings.projecaoPerpectiva = false;
            OpenGLControl.Refresh();

            perspectivaToolStripMenuItem1.CheckState = CheckState.Unchecked;
            ortogonalToolStripMenuItem.CheckState = CheckState.Checked;

            if (!GLSettings.projectionFree)
            {
                toolStripButton5.Enabled = true;
                toolStripButton31.Enabled = true;
                marcarRegiao.Enabled = true;
            }
        }

        private void toolStripButton5_Click_2(object sender, EventArgs e)
        {
            marcacaoPolygon(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remove"></param>
        private void marcacaoPolygon(bool remove)
        {
            
            if (OpenGLControl.modelsListSelect - 1 >= 0)
            {
                if (!remove)
                {
                    if (!toolStripButton5.CheckOnClick)
                    {
                        toolStripButton5.CheckState = CheckState.Checked;
                        toolStripButton5.CheckOnClick = !toolStripButton5.CheckOnClick;
                        toolStripButton27.Enabled = true;
                        toolStripButton26.Enabled = true;
                        toolStripButton25.Enabled = true;
                        desmarcarToolStripMenuItem.Enabled = true;
                        marcarRegiao.Enabled = false;
                        toolStripButton31.Enabled = false;
                        removerPontosMarcadosToolStripMenuItem.Enabled = true;
                        removerTriangulosToolStripMenuItem.Enabled = true;
                        marcarRegiãoToolStripMenuItem.Enabled = false;
                        GLSettings.marcacaoPolygonOn = true;
                        GLSettings.initPolygon = 0;
                    }
                    else
                    {
                        toolStripButton5.CheckState = CheckState.Unchecked;
                        toolStripButton5.CheckOnClick = !toolStripButton5.CheckOnClick;
                        toolStripButton27.Enabled = false;
                        toolStripButton26.Enabled = false;
                        toolStripButton25.Enabled = false;
                        desmarcarToolStripMenuItem.Enabled = false;
                        marcarRegiao.Enabled = true;
                        toolStripButton31.Enabled = true;
                        toolStripButton25.Enabled = false;
                        removerPontosMarcadosToolStripMenuItem.Enabled = false;
                        removerTriangulosToolStripMenuItem.Enabled = false;
                        marcarRegiãoToolStripMenuItem.Enabled = true;
                        GLSettings.marcacaoPolygonOn = false;
                        GLSettings.initPolygon = 0;

                        OpenGLControl.removeSelectedModel(OpenGLControl.modelsList.IndexOf("polygon"));
                        OpenGLControl.modelsList.Remove("polygon");                    
                        //dio OpenGLControl.GLrender.projecaoSuperior(false);                
                    }
                }
                else
                {
                    if (!toolStripButton31.CheckOnClick)
                    {
                        toolStripButton31.CheckState = CheckState.Checked;
                        toolStripButton31.CheckOnClick = !toolStripButton31.CheckOnClick;
                        toolStripButton27.Enabled = true;
                        toolStripButton26.Enabled = true;
                        toolStripButton25.Enabled = true;
                        desmarcarToolStripMenuItem.Enabled = true;
                        marcarRegiao.Enabled = false;
                        toolStripButton5.Enabled = false;
                        removerPontosMarcadosToolStripMenuItem.Enabled = true;
                        removerTriangulosToolStripMenuItem.Enabled = true;
                        marcarRegiãoToolStripMenuItem.Enabled = false;
                        GLSettings.marcacaoPolygonOnRemove = true;
                        GLSettings.marcacaoPolygonOn = true;
                        GLSettings.initPolygon = 0;
                    }
                    else
                    {
                        toolStripButton31.CheckState = CheckState.Unchecked;
                        toolStripButton31.CheckOnClick = !toolStripButton31.CheckOnClick;
                        toolStripButton27.Enabled = false;
                        toolStripButton26.Enabled = false;
                        toolStripButton25.Enabled = false;
                        desmarcarToolStripMenuItem.Enabled = false;
                        marcarRegiao.Enabled = true;
                        toolStripButton25.Enabled = false;
                        toolStripButton5.Enabled = true;
                        removerPontosMarcadosToolStripMenuItem.Enabled = false;
                        removerTriangulosToolStripMenuItem.Enabled = false;
                        marcarRegiãoToolStripMenuItem.Enabled = true;
                        GLSettings.marcacaoPolygonOnRemove = false;
                        GLSettings.marcacaoPolygonOn = false;
                        GLSettings.initPolygon = 0;

                        OpenGLControl.removeSelectedModel(OpenGLControl.modelsList.IndexOf("polygon"));
                        OpenGLControl.modelsList.Remove("polygon");
                        //dio OpenGLControl.GLrender.projecaoSuperior(false);
                    }
                }
            }
            else
            {
                WarningView sf = new WarningView(0);
                sf.ShowDialog();
            }
        }

        private void marcaçãoPolygonoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            marcacaoPolygon(false);
        }

        private void ferramentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void cuboRefToolStripMenuItem_Click(object sender, EventArgs e)
        {
            atualizaProjeto(true, true, true, true);
        }

        private void toolStripButton25_Click_1(object sender, EventArgs e)
        {
            desmarcar();
            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
        }

        private void toolStripSeparator4_Click(object sender, EventArgs e)
        {
        }

        private void desmarcarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            desmarcar();
        }

        /// <summary>
        /// 
        /// </summary>
        private void desmarcar()
        {
            GLSettings.desmarcar = !GLSettings.desmarcar;

            if (GLSettings.desmarcar) toolStripButton25.CheckState = CheckState.Checked;
            else toolStripButton25.CheckState = CheckState.Unchecked;
        }

        private void salvarTextoSelecionado(object sender, EventArgs e)
        {
            try
            {
                GLSettings.ModeloSelect = toolStripComboBox1.Text;
            }
            catch (Exception)
            {
                info.Text = "INFO: Confirmar seleção de modelo";         
            }            
        }

        private void teste1(object sender, EventArgs e)
        {
        }

      //  private void teste2(object sender, MouseEventArgs e)
      //  {
      //  }

        private void test2(object sender, EventArgs e)
        {
        }

        private void toolStripComboBox1_Click_1(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void gerarGcode(bool part)
        {
            this.Cursor = Cursors.WaitCursor;
            CNC.Gcode gcode = new CNC.Gcode();
            gcode.GcodeGenerate(part);
            this.Cursor = Cursors.Default;

            WarningView sf = new WarningView(4);
            sf.ShowDialog();
        }

        private void geraçãoGcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gerarGcodeIndividual();
        }

        /// <summary>
        /// 
        /// </summary>
        private void gerarGcodeIndividual()
        {
            this.Cursor = Cursors.WaitCursor;
            CNC.Gcode gcode = new CNC.Gcode();
            gcode.gerarGcodeIndividual();
            this.Cursor = Cursors.Default;

            WarningView sf = new WarningView(4);
            sf.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void projetçãoLivreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openModel = new OpenFileDialog();
            openModel.Filter = "*Carregar arquivo|*.nc";
            openModel.ShowDialog();

            for (int index = 0; index < openModel.FileNames.Length; ++index)
            {

            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void projecaoLivreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projecaoLivre();
        }

        private void projecaoLivre()
        {
            if (!marcarRegiao.CheckOnClick)
            {
                GLSettings.projectionFree = !GLSettings.projectionFree;

                if (GLSettings.projectionFree)
                {
                    projeçãoLivreToolStripMenuItem.CheckState = CheckState.Checked;
                    toolStripButton5.Enabled = false;
                    toolStripButton31.Enabled = false;
                    marcarRegiao.Enabled = false;
                }
                else
                {
                    projeçãoLivreToolStripMenuItem.CheckState = CheckState.Unchecked;

                    if (!GLSettings.projecaoPerpectiva)
                    {
                        toolStripButton5.Enabled = true;
                        toolStripButton31.Enabled = true;
                        marcarRegiao.Enabled = true;
                    }
                }
            }
            else
            {
                WarningView sf = new WarningView(8);
                sf.ShowDialog();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addModeloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!union)
            {
                saveOBJ(true, true);
                union = true;
                addModeloToolStripMenuItem.Text = "Adicionar 2 modelo";

                WarningView sf_ = new WarningView(5);
                sf_.ShowDialog();
            }
            else
            {
                saveOBJ(true, true);
                addModeloToolStripMenuItem.Text = "Modelos Prontos";

                processarUniãoToolStripMenuItem.Enabled = true;
            }                            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void processarUniãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Filters();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                GeneralTools tools = new GeneralTools();
                tools.Union(GLSettings.locateTMP + GLSettings.ModeloAux, GLSettings.locateTMP + GLSettings.ModeloAux2, GLSettings.locateTMP + GLSettings.ModeloAux3, OpenGLControl, filter);
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.ModeloAux3);
                OpenGLControl.loadTringuleGeneration(GLSettings.locateTMP + GLSettings.ModeloAux3);
                toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;

                addModeloToolStripMenuItem.Text = "Add Modelo";
                processarUniãoToolStripMenuItem.Enabled = false;
                union = false;
                this.Cursor = Cursors.Default;

                WarningView sf_ = new WarningView(6);
                sf_.ShowDialog();
            }
            catch
            {
                info.Text = "INFO: Confirmar seleção de modelo";          
            }
          
        }

        private void removerModelosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addModeloToolStripMenuItem.Text = "Add Modelo";
            processarUniãoToolStripMenuItem.Enabled = false;
            union = false;
        }

        private void tratamentoGcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CNC.Gcode g = new CNC.Gcode();
            g.CAMGenerate();
        }

        private void toolStripButton28_Click(object sender, EventArgs e)
        {
            projecaoEsquerda();
        }

        private void toolStripButton29_Click(object sender, EventArgs e)
        {
            projecaoInverior();
        }

        private void toolStripButton30_Click(object sender, EventArgs e)
        {
            projecaoFundo();
        }

        private void machingCubesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            repararModelo();
        }

        /// <summary>
        /// 
        /// </summary>
        private void repararModelo()
        {
            try
            {    
                GeneralTools tools = new GeneralTools();
                tools.MarchingCubes();
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
                OpenGLControl.Refresh();              
            }
            catch
            {
                WarningView sf_ = new WarningView(7);
                sf_.ShowDialog();
            }
        }

        private void novoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importMalha("addProjeto");
            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
        }

        private void girar90ºEixoYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                info.Text = "INFO: Girando 90 graus no eixo Y ";
                this.Cursor = Cursors.WaitCursor;
                GeneralTools tools = new GeneralTools();
                tools.RotateAxisAngle();
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.locateTMP + GLSettings.ModeloAuxOut_);            
                this.Cursor = Cursors.Default;
                info.Text = "INFO: Girado 90 graus no eixo Y com sucesso!";
            }
            catch (Exception)
            {
                info.Text = "INFO: Confirmar seleção de modelo";               
            }          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void girar45EixoYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                info.Text = "INFO: Girando 45 graus no eixo Y ";
                this.Cursor = Cursors.WaitCursor;
                GeneralTools tools = new GeneralTools();
                tools.Girar45GrausEixoY();
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.locateTMP + GLSettings.ModeloAuxOut_);            
                this.Cursor = Cursors.Default;
                info.Text = "INFO: Girado 45 graus no eixo Y com sucesso!";
            }
            catch
            { 
                info.Text = "INFO: Confirmar seleção de modelo";
            }           
        }

        private void repararToolStripMenuItem_Click(object sender, EventArgs e)
        {
            repararModelo();
        }

        private void gerarCAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gerarGcodeIndividual();
            atualizaProjeto(true, true, true, true);
        }

        private void conToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            GcodeView gc = new GcodeView();
            gc.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void girarMenos45EixoYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                info.Text = "INFO: Girando 135 graus no eixo Y ";
                this.Cursor = Cursors.WaitCursor;        
                GeneralTools tools = new GeneralTools();
                tools.GirarMenos45GrausEixoY();
                OpenGLControl.RefreshShowModels(toolStripComboBox1.SelectedIndex, selectView, "*", GLSettings.locateTMP + GLSettings.ModeloAuxOut_);           
                this.Cursor = Cursors.Default;
                info.Text = "INFO: Girado 135 graus no eixo Y com sucesso!";
            }
            catch 
            {
                info.Text = "INFO: Confirmar seleção de modelo";               
            }        
        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            
        }

        private void treeCheck(object sender, TreeViewCancelEventArgs e)
        {                                 
        }

        private void teste(object sender, EventArgs e)
        {
            System.Console.WriteLine(e.ToString());
        }

        private void teste2(object sender, EventArgs e)
        {
            System.Console.WriteLine(e.ToString());
        }

        private void abrirVisualizadorCAM(string path)
        {
            string pathAjuste;
            string[] quebra = path.Split('\\');

            switch (quebra[2])
            {
                case "Bloco1_solido_acabamentoXZ.nc":
                    pathAjuste = GLSettings.locateTMP + "A" + GLSettings.Bloco1_solido_acabamento_NC;
                    break;
                case "Bloco2_solido_acabamentoXZ.nc":
                    pathAjuste = GLSettings.locateTMP + "A" + GLSettings.Bloco2_solido_acabamento_NC;
                    break;
                case "Bloco3_solido_acabamentoXZ.nc":
                    pathAjuste = GLSettings.locateTMP + "A" + GLSettings.Bloco3_solido_acabamento_NC;
                    break;
                case "Bloco4_solido_acabamentoXZ.nc":
                    pathAjuste = GLSettings.locateTMP + "A" + GLSettings.Bloco4_solido_acabamento_NC;
                    break;
                case "Bloco5_solido_acabamentoXZ.nc":
                    pathAjuste = GLSettings.locateTMP + "A" + GLSettings.Bloco5_solido_acabamento_NC;
                    break;
                case "Bloco1_solido_AD_NC.nc":
                    pathAjuste = GLSettings.locateCAM + GLSettings.Bloco1_solido_desbaste_NC;
                    break;
                case "Bloco2_solido_AD_NC.nc":
                    pathAjuste = GLSettings.locateCAM + GLSettings.Bloco2_solido_desbaste_NC;
                    break;
                case "Bloco3_solido_AD_NC.nc":
                    pathAjuste = GLSettings.locateCAM + GLSettings.Bloco3_solido_desbaste_NC;
                    break;
                case "Bloco4_solido_AD_NC.nc":
                    pathAjuste = GLSettings.locateCAM + GLSettings.Bloco4_solido_desbaste_NC;
                    break;
                case "Bloco5_solido_AD_NC.nc":
                    pathAjuste = GLSettings.locateCAM + GLSettings.Bloco5_solido_desbaste_NC;
                    break;
                default:
                    break;
            }
                  
        }

        /// <summary>
        /// Tratamento de evento da treeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeCheck_(object sender, TreeViewEventArgs e)
        {
            string tmp = e.Node.ToString();
            string[] tmp2 = tmp.Split(' ');
            string[] value = e.Node.FullPath.Split('\\');         
            int selectOnly = 0;

            GLSettings.numberDivblocoExecutado = 0;

            if (File.Exists(GLSettings.locateTMP + "numberDivblocoExecutado"))
            {
                Stream entrada = File.Open(GLSettings.locateTMP + "numberDivblocoExecutado", FileMode.Open);
                StreamReader leitor = new StreamReader(entrada);
                string linha = leitor.ReadLine();
                GLSettings.numberDivblocoExecutado = Convert.ToInt32(linha);
                entrada.Close();
                leitor.Close();
            }

            if (e.Node.Checked)
            {
                string[] tmp3 = tmp2[1].Split('.');              

                if (value.Length == 3)
                { 
                    if(tmp3[1] == "nc")
                    {
                        /*  
                         *  Visualizador desativado
                         */ 
                        //abrirVisualizadorCAM(e.Node.FullPath);  
                        e.Node.Checked = false;
                    }
                }
            }     

            if (!newProjetBlock)
            {               
                for (int h = 0; h < GLSettings.numberProject; h++)
                {
                    if (treeView1.Nodes[h].Checked)
                    {                        
                        selectOnly++;
                    }
                }

                if (selectOnly == 1)
                {                   
                    for (int h = 0; h < GLSettings.numberProject; h++)
                    {
                        if (treeView1.Nodes[h].Checked)
                        {
                            selectProjeto(treeView1.Nodes[h].Text);
                            GLSettings.projectCURRENT = h;
                        }
                    }
                }
                else if (selectOnly > 1)
                {
                    for (int h = 0; h < GLSettings.numberProject; h++)
                    {
                        if (treeView1.Nodes[h].Checked)
                        {
                            if (treeView1.Nodes[h].Text == value[0]) treeView1.Nodes[h].Checked = false;
                        }
                    }                   
                }
                else if (selectOnly <= 0)
                {
                    clearSelectProject();                   
                }
                
                if (GLSettings.locateRAIZ == value[0])
                {                
                    if (!CheckedStartOpen)
                    { 
                        if (!e.Node.Checked)
                        {
                            if (e.Node.Text != value[0])
                            {
                                try
                                {
                                    foreach (var item in toolStripComboBox1.Items)
                                    {
                                        if (item.Equals(e.Node.Text))
                                        {
                                            OpenGLControl.removeSelectedModel(toolStripComboBox1.Items.IndexOf(item));
                                        }
                                    }
                                    OpenGLControl.modelsList.Remove(e.Node.Text);
                                    toolStripComboBox1.Items.Remove(e.Node.Text);
                                }
                                catch 
                                {
                                    info.Text = "INFO: Confirmar seleção de modelo";                                    
                                }                             
                            }
                        }
                        else
                        {
                            if (e.Node.Text != value[0])
                            {
                                try             
                                {
                                    toolStripComboBox1.Items.Add(e.Node.Text);
                                    toolStripComboBox1.Text = e.Node.Text;
                                    Model3D modelo = OpenGLControl.GLrender.LoadModel(e.Node.FullPath, "");
                                    OpenGLControl.AddModel(modelo);
                                    OpenGLControl.ShowModels();
                                    GLSettings.locateCURRENT = e.Node.FullPath;                                   
                                    Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[toolStripComboBox1.Items.IndexOf(toolStripComboBox1.SelectedItem)], GLSettings.locateTMP, GLSettings.ModeloAux);
                                    OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
                                }
                                catch
                                {
                                    info.Text = "INFO: Confirmar seleção de modelo";                                   
                                }                              
                            }
                        }
                    }
                    else
                    {
                        CheckedStartOpen = false;
                    }
                }              
            }            
        }

        private void fecharProjetoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fecharProjeto();
        }

        /// <summary>
        /// Cria um novo projeto, 
        /// cria os diretórios e a treeView
        /// </summary>
        private void importMalha(string type)
        {
            /*
         importModel.Filter = "Importar|*.stl";
         importModel.ShowDialog();
         STLinIO obj = new STLinIO();
         obj.importSTL(importModel.FileName);
         */

            criaProjeto(type);
            GLSettings.numberDivblocoExecutado = 0;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                CheckedStartOpen = true;

                info.Text = "INFO: Iniciando carregamento da imangem 3D ...";

                if (OpenGLControl.LoadFileDialog())
                {
                    toolStripComboBox1.Items.Add(OpenGLControl.modelsList[OpenGLControl.modelsList.Count - 1]);
                    info.Text = "INFO: Carregamento da imangem 3D finalizada";
                    toolStripComboBox1.Text = OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;
                    OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
                    OpenGLControl.modelsListSelect = -1;
                    this.Cursor = Cursors.Default;
  
                    GLSettings.locateCURRENT = GLSettings.locateMALHA + OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name;

                    if (saveOBJ(true, false)) { }

                    File.Delete(GLSettings.locateCURRENT);
                    File.Copy(GLSettings.locateTMP + GLSettings.ModeloAux, GLSettings.locateCURRENT);

                    if(type != "newProject") atualizaProjeto(true, true, true, true);

                    foreach (TreeNode st in treeView1.Nodes[GLSettings.projectCURRENT].Nodes[(int)GLSettings.structProject.malha].Nodes)
                    {
                        if (st.Text == OpenGLControl.GLrender.Models3D[OpenGLControl.GLrender.Models3D.Count - 1].Name)
                        {
                            st.Checked = true;
                        }
                    }

                    OpenGLControl.modelsListSelect = OpenGLControl.GLrender.Models3D.Count;
                }
                else
                {
                    WarningView sf = new WarningView(0);
                    sf.ShowDialog();
                }
            }
            catch
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void importarMalhaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importMalha("addProjeto");
            OpenGLControl.ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
        }

        private void MaterialRaisedButton1_Click(object sender, EventArgs e)
        {
        }

        private void ToolStripButton31_Click(object sender, EventArgs e)
        {
            marcacaoPolygon(true);
        }

        private void toolStripComboBox1_DropDownStyleChanged(object sender, EventArgs e)
        {
        }

        private void configuracoesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Teste de comunicação com banco de dados 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inserçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {                    
        }

        private void testeRest(int metodo)
        {
            OpenGLControl.comunication.RestGET();           
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deletarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aPIRestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //testeRest(1); //teste RestSharp remover posteriormente 
            labX.Text = GLSettings.EX.ToString();
        }

        private void aPIRestMétodoPostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testeRest(2); //teste RestSharp remover posteriormente 
        }

        private void enviarProjetoToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void enviarProjetoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int status = 0;

            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                if (treeView1.Nodes[i].Checked)
                {
                    // exportModel.Filter = "*Salvar projeto |"; //+ treeView1.Nodes[i].Text;
                    exportModel.ShowDialog();
                    GeneralTools tools = new GeneralTools();
                    tools.DirectoryCopy(treeView1.Nodes[i].FullPath, exportModel.FileName, true);
                    status++;

                    WarningView sf = new WarningView(11);
                    sf.ShowDialog();
                }
            }

            switch(status)
            {
                case 0:
                    break;
                case 1:
                    ServerForwardingView view = new ServerForwardingView(OpenGLControl);
                    view.Show();
                    break;
                default:
                    break;

            }            
        }

        private void downloadProjetoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton28_Click_1(object sender, EventArgs e)
        {     
            geradorSolido();
        }

        private void toolStripButton29_Click_1(object sender, EventArgs e)
        {
            gcode(false);
        }

        private void toolStripButton32_Click(object sender, EventArgs e)
        {
            perspectiva();
        }

        private void toolStripButton33_Click(object sender, EventArgs e)
        {
            ortogonal();
        }

        private void toolStripButton30_Click_1(object sender, EventArgs e)
        {
            projecaoLivre();
        }

        private void toolStripButton28_Click_2(object sender, EventArgs e)
        {
            projecaoZX();
        }

        private void toolStripButton29_Click_2(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton29_Click_3(object sender, EventArgs e)
        {
            gcode(true);
        }

        private void tableLayoutPanel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
