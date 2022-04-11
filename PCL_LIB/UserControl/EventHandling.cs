/*
 * Arquivo: EventHandling.cs
 * 
 * Autor: Dionata Nunes <dionata.silva@senairs.org.br>
 * 
 * Data da Criação: 14/01/2019
 * 
 * Descricao: Clase de controle de eventos de mouse
 * 
 */ 

using OpenTK;
using OpenTK.Graphics.ES20;
using PCLLib.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCLTemplate.CLGLInterop;
using System.Collections.Generic;
using PCLLib.Utils;

namespace PCLLib
{
    public partial class OpenGLControl
    {
        #region var
        private Point mousePos = new Point(0, 0);
        private Point mousePosOriginal = new Point(0, 0);
        private bool mouseClicked = false;
        private static bool mouseClickedLeft = false;
        private static bool mouseClickedLeftMouve = false;
        private int cameraMode = 0;
        private bool clickedDirect = false;
        private int xOrigin = 0;
        private int yOrigin = 0;
        public Form ParentFormWindow;        
        private Point startPoint;
        private Point endingPoint;
        private List<Point> startPointPolygon = new List<Point>();
        private List<Point> endingPointPolygon = new List<Point>();
        private static bool bugMouseScroll = false;
        private static double[] coeficienteAngular = new double[3];
        #endregion

        /// <summary>
        /// Eventos de mouse
        /// </summary>
        private void initEventHandlers()
        {            
            this.glControl1.MouseMove += new MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseDown += new MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseUp += new MouseEventHandler(this.glControl1_MouseUp);
            this.glControl1.MouseWheel += new MouseEventHandler(this.glControl1_MouseWheel);
            this.glControl1.KeyDown += new KeyEventHandler(this.glControl1_KeyDown);
            this.glControl1.Paint += new PaintEventHandler(this.glControl1_Paint);
            this.glControl1.Resize += new EventHandler(this.glControl1_Resize);
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right && !this.mouseClicked)
                {
                    this.mouseClicked = true;
                    mousePosOriginal.X = e.X;
                    mousePosOriginal.Y = e.Y;
                }
                else if (e.Button == MouseButtons.Left)
                {
                    mouseClickedLeft = true;
                }

                if (e.Button != MouseButtons.Left || this.clickedDirect)
                {
                    if (GLSettings.marcarRegiaoOn)
                    {
                        startPoint = screenCoordinate.ToWorld(GLSettings.projecaoOrtogonalEscala, e.Location);
                    }
                    return;
                }

                this.clickedDirect = true;
                this.xOrigin = e.X;
                this.yOrigin = e.Y;
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
        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (GLSettings.marcacaoPolygonOn)
                    {
                        /*
                         *  Desenha linhas 
                         */
                        /*                   
                        endingPoint = Utils.screenCoordinate.ToWorld(GLSettings.projecaoOrtogonalEscala, e.Location);                    
                        Models.StandardModels3D.polygon(endingPoint, this, GLSettings.initPolygon);               
                        ChangeDisplayMode(GLrender.Models3D[modelsListSelect - 1].ModelRenderStyle);
                        //this.GLrender.projecaoSuperior();
                        GLSettings.initPolygon = 0;
                        */
                        if (GLSettings.initPolygon == 0)
                        {
                            startPointPolygon.Add(Utils.screenCoordinate.ToWorld(GLSettings.projecaoOrtogonalEscala, e.Location));
                        }
                        else if (GLSettings.initPolygon == 2)
                        {
                            endingPointPolygon.Add(Utils.screenCoordinate.ToWorld(GLSettings.projecaoOrtogonalEscala, e.Location));
                        }
                        else if (GLSettings.initPolygon == 4)
                        {
                            startPointPolygon.Add(endingPointPolygon[endingPointPolygon.Count - 1]);
                            endingPointPolygon.Add(Utils.screenCoordinate.ToWorld(GLSettings.projecaoOrtogonalEscala, e.Location));
                            startPointPolygon.Add(startPointPolygon[0]);
                            endingPointPolygon.Add(endingPointPolygon[endingPointPolygon.Count - 1]);
                            MarcacaoPolygon();
                            Point aux = new Point();
                            Point aux2 = new Point();
                            aux = startPointPolygon[0];
                            aux2 = endingPointPolygon[endingPointPolygon.Count - 1];
                            startPointPolygon.Clear();
                            endingPointPolygon.Clear();
                            startPointPolygon.Add(aux);
                            endingPointPolygon.Add(aux2);
                            GLSettings.initPolygon = 2;
                        }
                    }
                    else if (GLSettings.marcarRegiaoOn)
                    {
                        Marcacao_();
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (GLSettings.marcacaoPolygonOn)
                    {
                        /*
                         * Cria finalizador de interligação de linhas 
                         */
                        /*
                       endingPoint = Utils.screenCoordinate.ToWorld(GLSettings.projecaoOrtogonalEscala, e.Location);
                       Models.StandardModels3D.polygon(endingPoint, this, GLSettings.initPolygon, MarcacaoPolygon());
                       ChangeDisplayMode(GLrender.Models3D[modelsListSelect - 1].ModelRenderStyle);
                       this.GLrender.projecaoSuperior();
                       GLSettings.initPolygon = 2;      
                       */
                        startPointPolygon.Clear();
                        endingPointPolygon.Clear();
                        startPointPolygon.Add(screenCoordinate.ToWorld(GLSettings.projecaoOrtogonalEscala, e.Location));
                        GLSettings.initPolygon = 0;
                    }
                }

                GLSettings.initPolygon++;

                if (!GLSettings.marcacaoPolygonOn)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        this.mouseClicked = false;
                        this.GLrender.ConsolidateMove();
                    }
                    else if (e.Button == MouseButtons.Left && mouseClickedLeft && mouseClickedLeftMouve)
                    {
                        mouseClickedLeft = false;
                        mouseClickedLeftMouve = false;
                       // if (!GLSettings.marcarRegiaoOn) //SaveStatus.ConfirmChanges(this, "confirmar");
                    }
                    if (e.Button != MouseButtons.Left || this.GLrender.Models3D.Count <= 0)
                    {
                        return;
                    }
                    this.clickedDirect = false;
                    this.GLrender.ConsolidaMoveModel();
                }
                if (!GLSettings.projectionFree) GLSettings.atualizarProjecao(this);

               // ChangeModelColor(GLSettings.color[0], GLSettings.color[1], GLSettings.color[2]);
            }
            catch 
            {                
            }     
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            GLSettings.EX = e.X;
            GLSettings.EY = e.Y;

            try
            {
                if (GLSettings.marcarRegiaoOn || GLSettings.marcacaoPolygonOn)
                {
                    this.Cursor = Cursors.Cross;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    this.mousePos.X = e.X;
                    this.mousePos.Y = e.Y;

                    if (e.Button == MouseButtons.Right && this.mouseClicked)
                    {
                        GLSettings.reposicaoX = e.X - this.mousePosOriginal.X;
                        GLSettings.reposicaoY = e.Y - this.mousePosOriginal.Y;

                        this.GLrender.RepositionCamera((float)e.X - (float)this.mousePosOriginal.X, (float)e.Y - (float)this.mousePosOriginal.Y, this.cameraMode);
                        this.glControl1.Refresh();
                    }
                }
                if (!GLSettings.marcacaoPolygonOn)
                {
                    if (GLSettings.marcarRegiaoOn)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            endingPoint = Utils.screenCoordinate.ToWorld(GLSettings.projecaoOrtogonalEscala, e.Location);
                            return;
                        }
                        else
                        {
                            return;
                        }
                        this.GLrender.MoveModel(modelsListSelect - 1, selectMoveXYZ, (e.X - e.Y - this.xOrigin + this.yOrigin), this);
                    }
                    else
                    {
                        if (e.Button != MouseButtons.Left || modelsListSelect < 1)
                        {
                            return;
                        }
                        else
                        {
                            this.GLrender.MoveModel(modelsListSelect - 1, selectMoveXYZ, (e.X - e.Y - this.xOrigin + this.yOrigin), this);
                            mouseClickedLeftMouve = true;
                        }
                    }
                }
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
        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (GLSettings.projecaoPerpectiva)
                {
                    this.GLrender.distEye *= 1.0 - (double)e.Delta * 0.001;
                    this.GLrender.zFar = (float)this.GLrender.distEye * 5f;
                    this.GLrender.RepositionCamera(0.0f, 0.0f, this.cameraMode);
                    this.glControl1.Refresh();
                }
                else
                {
                    bugMouseScroll = !bugMouseScroll;
                    if (bugMouseScroll)
                    {
                        // if (GLSettings.projecaoOrtogonalEscala == 8)
                        // {
                        //     GLSettings.projecaoOrtogonalEscala = 0.5f;
                        //  }                  
                        if (e.Delta == -120)
                        {
                            if (GLSettings.projecaoOrtogonalEscala < 4)
                            {
                                GLSettings.projecaoOrtogonalEscala *= 2;
                            }
                        }
                        else if (e.Delta == 120)
                        {
                            if (GLSettings.projecaoOrtogonalEscala > 0.5)
                            {
                                GLSettings.projecaoOrtogonalEscala /= 2;
                            }
                        }
                    }
                    this.GLrender.glControl1.Refresh();
                }
                //this.GLrender.projecaoSuperior(false);
            }
            catch 
            {
            }        
        }

        /// <summary>
        /// Controle via comandos de teclado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                //if (e.KeyCode == Keys.Escape)
                bool flag = false;
                if (e.KeyCode == Keys.W)
                {
                    this.GLrender.center -= (double)this.GLrender.zFar * 0.002 * this.GLrender.front;
                    this.GLrender.eye -= (double)this.GLrender.zFar * 0.002 * this.GLrender.front;
                    flag = true;
                }
                if (e.KeyCode == Keys.S)
                {
                    this.GLrender.center += (double)this.GLrender.zFar * 0.002 * this.GLrender.front;
                    this.GLrender.eye += (double)this.GLrender.zFar * 0.002 * this.GLrender.front;
                    flag = true;
                }
                if (e.KeyCode == Keys.A)
                {
                    this.GLrender.center = (double)this.GLrender.zFar * 0.001 * this.GLrender.esq;
                    this.GLrender.eye -= (double)this.GLrender.zFar * 0.001 * this.GLrender.esq;
                    flag = true;
                }
                if (e.KeyCode == Keys.D)
                {
                    this.GLrender.center += (double)this.GLrender.zFar * 0.001 * this.GLrender.esq;
                    this.GLrender.eye += (double)this.GLrender.zFar * 0.001 * this.GLrender.esq;
                    flag = true;
                }

                if (e.KeyCode == Keys.P)
                {
                    Vector3d colorForm1 = new Vector3d(0, 0, 0);
                    //Model3D modeloForm1 = Models.StandardModels3D.Cuboid_AllLines("Form1", 1000, 500, 100, colorForm1, null);
                    Bitmap texturaCubo = new Bitmap(512, 512);
                    Model3D.TriangulateVertices_Stark(GLrender.Models3D[0]);
                    convexHullTool();

                    // GLrender.Models3D[0].CreateVerticesFromFunction(new Model3D.CoordFuncXYZ(StandardModels3D.Cuboid_AllLines), 0.0f, 12.566370f, 5, 5, 100, 2, colorForm1, null); // -= (double)this.GLrender.zFar * 0.002 * this.GLrender.front;
                    //this.GLrender.eye -= (double)this.GLrender.zFar * 0.002 * this.GLrender.front;
                    flag = true;
                }

                double num1 = Math.Cos(0.01);
                double num2 = Math.Sin(0.01);
                if (e.KeyCode == Keys.NumPad4)
                {
                    Vector3d vector1 = new Vector3d(this.GLrender.front);
                    Vector3d vector2 = new Vector3d(this.GLrender.esq);

                    this.GLrender.front = num1 * vector1 + num2 * vector2;
                    this.GLrender.esq = -num2 * vector1 + num1 * vector2;
                    this.GLrender.center = this.GLrender.eye - this.GLrender.front * this.GLrender.distEye;
                    flag = true;
                }
                if (e.KeyCode == Keys.NumPad6)
                {
                    Vector3d vector1 = new Vector3d(this.GLrender.front);
                    Vector3d vector2 = new Vector3d(this.GLrender.esq);
                    this.GLrender.front = num1 * vector1 - num2 * vector2;
                    this.GLrender.esq = num2 * vector1 + num1 * vector2;
                    this.GLrender.center = this.GLrender.eye - this.GLrender.front * this.GLrender.distEye;
                    flag = true;
                }
                if (e.KeyCode == Keys.NumPad2)
                {
                    Vector3d vector1 = new Vector3d(this.GLrender.front);
                    Vector3d vector2 = new Vector3d(this.GLrender.up);
                    this.GLrender.front = num1 * vector1 + num2 * vector2;
                    this.GLrender.up = -num2 * vector1 + num1 * vector2;
                    this.GLrender.center = this.GLrender.eye - this.GLrender.front * this.GLrender.distEye;
                    flag = true;
                }
                if (e.KeyCode == Keys.NumPad8)
                {
                    Vector3d vector1 = new Vector3d(this.GLrender.front);
                    Vector3d vector2 = new Vector3d(this.GLrender.up);
                    this.GLrender.front = num1 * vector1 - num2 * vector2;
                    this.GLrender.up = num2 * vector1 + num1 * vector2;
                    this.GLrender.center = this.GLrender.eye - this.GLrender.front * this.GLrender.distEye;
                    flag = true;
                }
                if (!flag)
                    return;
                this.glControl1.Invalidate();
            }
            catch 
            {
             
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double MarcacaoPolygon()
        {
            if ((modelsListSelect - 1) >= 0)
            {
                coeficienteAngular[0] = (double)(endingPointPolygon[0].Y - startPointPolygon[0].Y) / (double)(endingPointPolygon[0].X - startPointPolygon[0].X);
                coeficienteAngular[1] = (double)(endingPointPolygon[1].Y - startPointPolygon[1].Y) / (double)(endingPointPolygon[1].X - startPointPolygon[1].X);
                coeficienteAngular[2] = (double)(endingPointPolygon[2].Y - startPointPolygon[2].Y) / (double)(endingPointPolygon[2].X - startPointPolygon[2].X);               

                double mX0 = coeficienteAngular[0] * startPointPolygon[0].X;
                double mX1 = coeficienteAngular[1] * startPointPolygon[1].X;
                double mX2 = coeficienteAngular[2] * startPointPolygon[2].X;

                switch (GLSettings.selecionarTipoProjecao)
                {
                    /***************************************************************************************************************************************************************************************************************************************************** XZ */
                    case (int)GLSettings.tipoProjecao.frontal:
                    case (int)GLSettings.tipoProjecao.fundo:
                    case (int)GLSettings.tipoProjecao.ZX:
                        if (endingPointPolygon[1].X > startPointPolygon[1].X)
                        {
                            if (endingPointPolygon[1].X > startPointPolygon[0].X && startPointPolygon[1].X > startPointPolygon[0].X)
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (endingPointPolygon[1].X > startPointPolygon[0].X && startPointPolygon[1].X < startPointPolygon[0].X)
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (endingPointPolygon[1].X < startPointPolygon[0].X)
                        {
                            if (startPointPolygon[1].X > startPointPolygon[0].X)
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                            {
                                if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                {
                                    if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (!GLSettings.desmarcar)
                                            {
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                GLSettings.vertexMarcacao.Add(i);
                                            }
                                            else
                                            {
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                GLSettings.vertexMarcacao.Remove(i);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                        /*****************************************************************************************************************************************************************************************************************************************************YZ*/
                    case (int)GLSettings.tipoProjecao.esquerda:
                    case (int)GLSettings.tipoProjecao.direita:
                        if (endingPointPolygon[1].X > startPointPolygon[1].X)
                        {
                            if (endingPointPolygon[1].X > startPointPolygon[0].X && startPointPolygon[1].X > startPointPolygon[0].X)
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX0) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX1) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX2) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (endingPointPolygon[1].X > startPointPolygon[0].X && startPointPolygon[1].X < startPointPolygon[0].X)
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX0) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX1) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX2) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX0) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX1) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX2) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (endingPointPolygon[1].X < startPointPolygon[0].X)
                        {
                            if (startPointPolygon[1].X > startPointPolygon[0].X)
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX0) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX1) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX2) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX0) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX1) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX2) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                            {
                                if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX0) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                {
                                    if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX1) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                    {
                                        if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y) - mX2) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z))
                                        {
                                            if (!GLSettings.desmarcar)
                                            {
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                GLSettings.vertexMarcacao.Add(i);
                                            }
                                            else
                                            {
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                GLSettings.vertexMarcacao.Remove(i);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                        /*****************************************************************************************************************************************************************************************************************************************************XY*/
                    case (int)GLSettings.tipoProjecao.superior:
                    case (int)GLSettings.tipoProjecao.inferior:
                        if (endingPointPolygon[1].X > startPointPolygon[1].X)
                        {
                            if (endingPointPolygon[1].X > startPointPolygon[0].X && startPointPolygon[1].X > startPointPolygon[0].X)
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (endingPointPolygon[1].X > startPointPolygon[0].X && startPointPolygon[1].X < startPointPolygon[0].X)
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (endingPointPolygon[1].X < startPointPolygon[0].X)
                        {
                            if (startPointPolygon[1].X > startPointPolygon[0].X)
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                                {
                                    if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                    {
                                        if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                        {
                                            if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                            {
                                                if (!GLSettings.desmarcar)
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                    GLSettings.vertexMarcacao.Add(i);
                                                }
                                                else
                                                {
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                    this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                    GLSettings.vertexMarcacao.Remove(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                            {
                                if (startPointPolygon[0].Y + ((double)(coeficienteAngular[0] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX0) > (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                {
                                    if (startPointPolygon[1].Y + ((double)(coeficienteAngular[1] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX1) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                    {
                                        if (startPointPolygon[2].Y + ((double)(coeficienteAngular[2] * (double)this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X) - mX2) < (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y))
                                        {
                                            if (!GLSettings.desmarcar)
                                            {
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.4f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.8f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 0.8f;
                                                GLSettings.vertexMarcacao.Add(i);
                                            }
                                            else
                                            {
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                                this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                                GLSettings.vertexMarcacao.Remove(i);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                }               
           
                GLSettings.selecionarTipoProjecaoAux = GLSettings.selecionarTipoProjecao;
                ChangeDisplayMode(GLrender.Models3D[modelsListSelect - 1].ModelRenderStyle);              
                GLSettings.selecionarTipoProjecao = GLSettings.selecionarTipoProjecaoAux;
                if (!GLSettings.projectionFree) GLSettings.atualizarProjecao(this);
            }
            return 1000;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Marcacao_()
        {
         
          if ((modelsListSelect - 1) >= 0)
            {
                for (int i = 0; i < this.GLrender.Models3D[modelsListSelect - 1].VertexList.Count; i++)
                {
                    switch (GLSettings.selecionarTipoProjecao)
                    {

                        case (int)GLSettings.tipoProjecao.frontal:
                        case (int)GLSettings.tipoProjecao.fundo:
                        case (int)GLSettings.tipoProjecao.ZX:
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X > startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X < endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z > startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z < endingPoint.Y)
                                {                                   
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X < startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X > endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z < startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z > endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X > startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X < endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z < startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z > endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X < startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X > endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z > startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z < endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            break;
                        case (int)GLSettings.tipoProjecao.esquerda:
                        case (int)GLSettings.tipoProjecao.direita:
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y > startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y < endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z > startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z < endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y < startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y > endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z < startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z > endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y > startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y < endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z < startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z > endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y < startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y > endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z > startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Z < endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            break;
                        case (int)GLSettings.tipoProjecao.superior:
                        case (int)GLSettings.tipoProjecao.inferior:
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X > startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X < endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y > startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y < endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;//0.2
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X < startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X > endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y < startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y > endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X > startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X < endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y < startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y > endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X < startPoint.X && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.X > endingPoint.X)
                            {
                                if (this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y > startPoint.Y && this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Vector.Y < endingPoint.Y)
                                {
                                    if (!GLSettings.desmarcar)
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 0.2f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 0.4f;
                                    }
                                    else
                                    {
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[0] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[1] = 1.0f;
                                        this.GLrender.Models3D[modelsListSelect - 1].VertexList[i].Color[2] = 1.0f;
                                        GLSettings.vertexMarcacao.Remove(i);
                                    }
                                }
                            }
                            break;
                    }
                }
                GLSettings.selecionarTipoProjecaoAux = GLSettings.selecionarTipoProjecao;
                ChangeDisplayMode(GLrender.Models3D[modelsListSelect - 1].ModelRenderStyle);
                GLSettings.selecionarTipoProjecao = GLSettings.selecionarTipoProjecaoAux;
                if (!GLSettings.projectionFree) GLSettings.atualizarProjecao(this);
            }                        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Marcacao(object sender, PaintEventArgs e)
        {        
        }   
    }
}
