using System.Drawing;
using OpenCLTemplate;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using SharpGL;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph;

namespace PCLLib
{
    public class OpenGLRenderer
    {
        public GLControl glControl1;
        public List<Model3D> Models3D;
        public int SelectedModelIndex;   
        public bool DrawStereo;
        public float StereoDistance;
        public float[] ClearColor;
        public int MODE_ROT;
        public int MODE_TRANSL;
        public Vector3d center;
        public Vector3d eye;
        public Vector3d front;
        public Vector3d up;
        public Vector3d esq;
        public double distEye;
        public float zFar;
        /*Original
        private Vector3d frontCpy;
        private Vector3d upCpy;
        private Vector3d esqCpy;
        */
        public Vector3d frontCpy;
        public Vector3d upCpy;
        public Vector3d esqCpy;

        private Vector3d centerCpy;
        private float paramAnt;
        private float param;
        public List<Vertex> LinesFrom;
        public List<Vertex> LinesTo;
        public static bool habilitadaPerspectivasAlternativas = false;
        private static Vector3d frontUp_;        
        private static Point point_anterior;

        TextRenderer textRenderer;
        Font serif = new Font(FontFamily.GenericSerif, 24);
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="openGlControl"></param>
        public OpenGLRenderer(GLControl openGlControl)
        {
            Models3D = new List<Model3D>();
            float[] numArray = new float[3];

            /*
             * Cor de fundo da renderizacao 3D
             */
            numArray[0] = 0.9f;//0.0f; //92; 153; 214
            numArray[1] = 0.9f;//0.0f;
            numArray[2] = 0.9f;//0.1f;

            this.ClearColor = numArray;
            this.MODE_ROT = 0;
            this.MODE_TRANSL = 3;
            this.center = new Vector3d(0, 0, 0);
            this.eye = new Vector3d(0, 0, 0);
            this.front = new Vector3d(0, 0, 1);
            this.up = new Vector3d(0, 1, 0);
            this.esq = new Vector3d(1, 0, 0);
            this.frontCpy = new Vector3d(0, 0, 1);
            this.upCpy = new Vector3d(0, 1, 0);
            this.esqCpy = new Vector3d(1, 0, 0);
            this.distEye = 215.0;
            this.zFar = 1000f;                                 
            this.centerCpy = new Vector3d(0, 0, 0);
            this.paramAnt = 0.0f;
            this.param = 0.0f;            
            // ISSUE: explicit constructor call
            this.glControl1 = openGlControl;
            this.glControl1.MakeCurrent();
            
            
            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.DontCare);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.ClearDepth(1.0);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.DepthTest);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.DontCare);       
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
            GL.Enable(EnableCap. ColorMaterial);
                       
            float[] params1 = new float[4] { 0.5f, 0.5f, 0.5f, 1f };
            float[] params2 = new float[4] { 0.3f, 0.3f, 0.3f, 1f };
            float[] params3 = new float[4] { 0.1f, 0.1f, 0.1f, 1f };
            float[] params4 = new float[4] { 0.0f, -400f, 0.0f, 1f };            
 
            GL.Light(LightName.Light1, LightParameter.Ambient, params1);
            GL.Light(LightName.Light1, LightParameter.Diffuse, params2);
            GL.Light(LightName.Light1, LightParameter.Specular, params3);
            GL.Light(LightName.Light1, LightParameter.Position, params4);
            GL.Enable(EnableCap.Light1);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Texture2D);           
            
            try
            {
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.VertexArray);
            }
            catch
            {
                Model3D.HardwareSupportsBufferObjects = false;
            }
            
            CreateTextRenderer();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void CreateTextRenderer()
        {
            textRenderer = new TextRenderer(500, 500);
            PointF position = PointF.Empty;
            textRenderer.Clear(Color.MidnightBlue);
            textRenderer.DrawString("MyText", serif, Brushes.White, position);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ind"></param>
        public void RemoveModel(int ind)
        {
            try
            {
                foreach (Part parts in this.Models3D[ind].Parts)
                {
                    if (Model3D.HardwareSupportsBufferObjects)
                    {
                        for (int index = 0; index < parts.GLBuffers.Length; ++index)
                            GL.DeleteBuffers(1, ref parts.GLBuffers[index]);
                    }
                    else
                        GL.DeleteLists(parts.GLListNumber, 1);
                }
                if (this.Models3D[ind].GLTexture > 0)
                    GL.DeleteTexture(this.Models3D[ind].GLTexture);
            }
            catch
            {
            }
            Model3D Model = this.Models3D[ind];
           
            this.Models3D.RemoveAt(ind);
        }

        /// <summary>
        /// projecao Superior
        /// </summary>
        /// <param name="LookAt"></param>
        public void projecaoSuperior(bool LookAt)
        {            

            Vector3d front_ = new Vector3d(0, 0, 1);
            //Vector3d frontCenter = new Vector3d(300, 300, 600);
            Vector3d frontCenter = new Vector3d(190, 300, 100);
            frontUp_ = new Vector3d(0, 100, 0);//-1,100,0

            this.center = frontCenter;
            this.front = front_;

            this.centerCpy = this.center;
            this.frontCpy = this.front;

            this.eye = this.center + this.front * this.distEye;

            GLSettings.selecionarTipoProjecao = (int)GLSettings.tipoProjecao.superior;

            if (!LookAt)
            {                 
                this.RepositionLight();                           
                this.perspectivaConsolidacao();
                this.glControl1.Refresh();
                this.perspectivaConsolidacao();                     
            }
        }

        /// <summary>
        /// projecao Inverior
        /// </summary>
        /// <param name="LookAt"></param>
        public void projecaoInverior(bool LookAt)
        {
            Vector3d front_ = new Vector3d(0, 0, -1);
           // Vector3d frontCenter = new Vector3d(300, 300, 0);
            Vector3d frontCenter = new Vector3d(180, 300, 100);
            frontUp_ = new Vector3d(0, 100, 0); //-1,100,0

            this.center = frontCenter;
            this.front = front_;

            this.centerCpy = this.center;
            this.frontCpy = this.front;

            this.eye = this.center + this.front * this.distEye;

            GLSettings.selecionarTipoProjecao = (int)GLSettings.tipoProjecao.inferior;

            if (!LookAt)
            {
                this.RepositionLight();                
                this.perspectivaConsolidacao();
                this.glControl1.Refresh();
                this.perspectivaConsolidacao();
            }
        }

        /// <summary>
        /// projecao Frontal
        /// </summary>
        /// <param name="LookAt"></param>
        public void projecaoFrontal(bool LookAt) 
        {
            Vector3d front_ = new Vector3d(0, -1, 0);
            Vector3d frontCenter = new Vector3d(190, 0, 100);//200,0 ,200
            frontUp_ = new Vector3d(0, 100, 1);

            this.center = frontCenter;
            this.front = front_;
            this.eye = this.center + this.front * this.distEye;

            GLSettings.selecionarTipoProjecao = (int)GLSettings.tipoProjecao.frontal;

            if (!LookAt)
            {
                this.RepositionLight();                
                this.perspectivaConsolidacao();
                this.glControl1.Refresh();
                this.perspectivaConsolidacao();
            }
        }

        /// <summary>
        /// projecaoZX
        /// </summary>
        /// <param name="LookAt"></param>
        public void projecaoZX(bool LookAt)
        {
            Vector3d front_ = new Vector3d(1, -1, 1);
            Vector3d frontCenter = new Vector3d(150, 300, 50); //300 0 300
            frontUp_ = new Vector3d(0, 100, 1);

            this.center = frontCenter;
            this.front = front_;
            this.eye = this.center + this.front * this.distEye;

            GLSettings.selecionarTipoProjecao = (int)GLSettings.tipoProjecao.ZX;

            if (!LookAt)
            {
                this.RepositionLight();                
                this.perspectivaConsolidacao();
                this.glControl1.Refresh();
                this.perspectivaConsolidacao();
            }
        }

        /// <summary>
        /// projecao Fundo
        /// </summary>
        /// <param name="LookAt"></param>
        public void projecaoFundo(bool LookAt)
        {
            Vector3d front_ = new Vector3d(0, 1, 0);
            //Vector3d frontCenter = new Vector3d(300, 600, 300);
            Vector3d frontCenter = new Vector3d(180, 600, 100); //300 0 300
            frontUp_ = new Vector3d(0, 100, 1);

            this.center = frontCenter;
            this.front = front_;
            this.eye = this.center + this.front * this.distEye;

            GLSettings.selecionarTipoProjecao = (int)GLSettings.tipoProjecao.fundo;

            if (!LookAt)
            {
                this.RepositionLight();                
                this.perspectivaConsolidacao();
                this.glControl1.Refresh();
                this.perspectivaConsolidacao();
            }
        }

        /// <summary>
        /// projecao Direita
        /// </summary>
        /// <param name="LookAt"></param>
        public void projecaoDireita(bool LookAt)
        {
            Vector3d frontXY = new Vector3d(1, 0, 0);
            //Vector3d frontCenter = new Vector3d(600, 300, 300);
            Vector3d frontCenter = new Vector3d(190, 225, 100);
            frontUp_ = new Vector3d(0, 0, 100); //0,-1,100

            this.center = frontCenter;
            this.front = frontXY;
            this.eye = this.center + this.front * this.distEye;

            GLSettings.selecionarTipoProjecao = (int)GLSettings.tipoProjecao.direita;

            if (!LookAt)
            {
                this.RepositionLight();                
                this.perspectivaConsolidacao();
                this.glControl1.Refresh();
                this.perspectivaConsolidacao();
            }
        }

        /// <summary>
        /// projecao Esquerda
        /// </summary>
        /// <param name="LookAt"></param>
        public void projecaoEsquerda(bool LookAt)
        {
            Vector3d frontXY = new Vector3d(-1, 0, 0);   
            Vector3d frontCenter = new Vector3d(190, 225, 100);
            frontUp_ = new Vector3d(0, 0, 100);

            this.center = frontCenter;
            this.front = frontXY;
            this.eye = this.center + this.front * this.distEye;

            GLSettings.selecionarTipoProjecao = (int)GLSettings.tipoProjecao.esquerda;

            if (!LookAt)
            {
                this.RepositionLight();                
                this.perspectivaConsolidacao();
                this.glControl1.Refresh();
                this.perspectivaConsolidacao();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouseDX"></param>
        /// <param name="mouseDY"></param>
        /// <param name="modo"></param>
        public void RepositionCamera(float mouseDX, float mouseDY, int modo)
        {            
            if (modo == this.MODE_ROT)
            {
                double num1 = -3.0 * Math.PI * (double)mouseDX / (double)this.glControl1.Width;
                double num2 = -3.0 * Math.PI * (double)mouseDY / (double)this.glControl1.Height;

                double num3 = Math.Cos(num2);
                double num4 =  Math.Sin(num2);
                double num5 = Math.Cos(num1);
                double num6 = Math.Sin(num1);
                               
                this.front = this.frontCpy * num3 + this.upCpy * -num4;
                this.up = num4 * this.frontCpy + this.upCpy * num3;
                Vector3d vector = new Vector3d(this.front);
                this.front = vector * num5 + num6 * this.esqCpy;
                this.esq = -num6 * vector + this.esqCpy * num5;                
            }
            else if (modo == this.MODE_TRANSL)
            {
                double dx = -distEye * mouseDX / (float)glControl1.Width;
                double dy = distEye * mouseDY / (float)glControl1.Height;

                center = centerCpy + esqCpy * dx + upCpy * dy;
            }
            this.eye = this.center + this.front * this.distEye;
            this.RepositionLight();            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Distance"></param>
        public void Fly(Vector3d Distance)
        {
            this.center += Distance.X * this.front + Distance.Y * this.esq + Distance.Z * this.up;
            this.eye += Distance.X * this.front + Distance.Y * this.esq + Distance.Z * this.up;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ConsolidateMove()
        {
            this.frontCpy = new Vector3d(this.front);
            this.upCpy = new Vector3d(this.up);
            this.esqCpy = new Vector3d(this.esq);
            this.centerCpy = new Vector3d(this.center);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RepositionLight()
        {
            try
            {
                //GL.LoadIdentity();
                GL.Light(LightName.Light1, LightParameter.Position, new float[4] { (float)this.distEye, (float)this.distEye, 0.0f, 1f });        
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in Reposition light : " + err.Message);
            }
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="NoOpenCLMessage"></param>
        /// <returns></returns>
        public Model3D LoadModel(string file, string NoOpenCLMessage)
        {
            Model3D model3D = null;
            try
            {              
                model3D = new Model3D(file);                
                return model3D;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower() == "opencldisabledexception")
                {
                    int num1 = (int)MessageBox.Show(NoOpenCLMessage, "PCLLib", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    int num2 = (int)MessageBox.Show("Exception: " + ex.ToString(), "PCLLib", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                return model3D;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectModelo"></param>
        private void DrawStereoImages(string selectModelo)
        {
            Vector3d vectorLeftEye = this.eye - this.distEye * (double)this.StereoDistance * this.esq;
            GL.DrawBuffer(DrawBufferMode.BackRight);
            GL.ClearColor(this.ClearColor[0], this.ClearColor[1], this.ClearColor[2], 0.0f);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();
            Matrix4d perspectiveFieldOfView1 = Matrix4d.CreatePerspectiveFieldOfView(Math.PI / 4.0, (double)this.glControl1.Width / (double)this.glControl1.Height, (double)this.zFar * 0.001, (double)this.zFar + 1000.0);
            GL.LoadMatrix(ref perspectiveFieldOfView1);
            Matrix4d mat1 = Matrix4d.LookAt(vectorLeftEye.X, vectorLeftEye.Y, vectorLeftEye.Z, this.center.X, this.center.Y, this.center.Z, this.up.X, this.up.Y, this.up.Z);
            GL.MultMatrix(ref mat1);

            this.DrawAll3DObjects(selectModelo);
            
            Vector3d vectorRightEye = this.eye + this.distEye * (double)this.StereoDistance * this.esq;
            GL.DrawBuffer(DrawBufferMode.BackLeft);
            GL.ClearColor(this.ClearColor[0], this.ClearColor[1], this.ClearColor[2], 0.0f);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();
            Matrix4d perspectiveFieldOfView2 = Matrix4d.CreatePerspectiveFieldOfView(Math.PI / 4.0, (double)this.glControl1.Width / (double)this.glControl1.Height, (double)this.zFar * 0.001, (double)this.zFar + 1000.0);
            GL.LoadMatrix(ref perspectiveFieldOfView2);
            Matrix4d mat2 = Matrix4d.LookAt(vectorRightEye.X, vectorRightEye.Y, vectorRightEye.Z, this.center.X, this.center.Y, this.center.Z, this.up.X, this.up.Y, this.up.Z);
            GL.MultMatrix(ref mat2);

            this.DrawAll3DObjects(selectModelo);
            
            this.glControl1.SwapBuffers();        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectModelo"></param>
        private void Draw3D(string selectModelo)
        {           
            GL.DrawBuffer(DrawBufferMode.FrontAndBack);
            GL.ClearColor(this.ClearColor[0], this.ClearColor[1], this.ClearColor[2], 0.0f);                      
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);           
            GL.LoadIdentity();          
            
            Matrix4d perspectiveFieldOfView;

            if (!GLSettings.projecaoPerpectiva)
            {
                perspectiveFieldOfView = Matrix4d.CreateOrthographic((double)this.glControl1.Width * GLSettings.projecaoOrtogonalEscala, (double)this.glControl1.Height * GLSettings.projecaoOrtogonalEscala, (double)this.zFar * 0.0001, (double)this.zFar + 2000.0);
            }
            else
            {
                perspectiveFieldOfView = Matrix4d.CreatePerspectiveFieldOfView(Math.PI / 4.0, (double)this.glControl1.Width / (double)this.glControl1.Height, (double)this.zFar * 0.001, (double)this.zFar + 1000.0);
            }

            GL.LoadMatrix(ref perspectiveFieldOfView);

            Matrix4d mat;

            if (!GLSettings.projectionFree)
            {
                GLSettings.projectionFreeAjuste = false;

                switch (GLSettings.selecionarTipoProjecao)
                {
                    case (int)GLSettings.tipoProjecao.superior:
                        this.projecaoSuperior(true);
                        break;
                    case (int)GLSettings.tipoProjecao.inferior:
                        this.projecaoInverior(true);
                        break;
                    case (int)GLSettings.tipoProjecao.frontal:
                        this.projecaoFrontal(true);
                        break;
                    case (int)GLSettings.tipoProjecao.direita:
                        this.projecaoDireita(true);
                        break;
                    case (int)GLSettings.tipoProjecao.esquerda:
                        this.projecaoEsquerda(true);
                        break;
                    case (int)GLSettings.tipoProjecao.fundo:
                        this.projecaoFundo(true);
                        break;
                    case (int)GLSettings.tipoProjecao.ZX:
                        this.projecaoZX(true);
                        break;
                    default:
                        this.projecaoZX(true);
                        break;
                }
                
                mat = Matrix4d.LookAt(this.eye.X, this.eye.Y, this.eye.Z, this.center.X, this.center.Y, this.center.Z, frontUp_.X, frontUp_.Y, frontUp_.Z);                                                                                                                                                                                             
            }
            else
            {
                if (!GLSettings.projectionFreeAjuste)
                {
                    switch (GLSettings.selecionarTipoProjecao)
                    {
                        case (int)GLSettings.tipoProjecao.superior:
                            this.projecaoSuperior(true);
                            break;
                        case (int)GLSettings.tipoProjecao.inferior:
                            this.projecaoInverior(true);
                            break;
                        case (int)GLSettings.tipoProjecao.frontal:
                            this.projecaoFrontal(true);
                            break;
                        case (int)GLSettings.tipoProjecao.direita:
                            this.projecaoDireita(true);
                            break;
                        case (int)GLSettings.tipoProjecao.esquerda:
                            this.projecaoEsquerda(true);
                            break;
                        case (int)GLSettings.tipoProjecao.fundo:
                            this.projecaoFundo(true);
                            break;
                        case (int)GLSettings.tipoProjecao.ZX:
                            this.projecaoZX(true);
                            break;
                        default:
                            break;
                    }
                    mat = Matrix4d.LookAt(this.eye.X, this.eye.Y, this.eye.Z, this.center.X, this.center.Y, this.center.Z, this.up.X, this.up.Y, this.up.Z);
                    GLSettings.projectionFreeAjuste = !GLSettings.projectionFreeAjuste;
                }
                else
                {                
                    mat = Matrix4d.LookAt(this.eye.X, this.eye.Y, this.eye.Z, this.center.X, this.center.Y, this.center.Z, this.up.X, this.up.Y, this.up.Z);
                }
            }

            GL.MultMatrix(ref mat);

            this.DrawAll3DObjects(selectModelo);            
            
            GL.Material(MaterialFace.Front, MaterialParameter.AmbientAndDiffuse, new float[4] { 0.9f, 0.9f, 0.9f, 1f });
            GL.PolygonMode(MaterialFace.Back, PolygonMode.Fill);         

            this.glControl1.SwapBuffers();       
        }

        /// <summary>
        /// 
        /// </summary>
        public void perspectivaConsolidacao()
        {
            if (habilitadaPerspectivasAlternativas)
            {               
                switch (GLSettings.selecionarTipoProjecao)
                {
                    case (int)GLSettings.tipoProjecao.superior:
                        this.eye = new Vector3d(0, 0, 0);//0, 215, 0
                        this.front = new Vector3d(0, 0, 1); 
                        this.up = new Vector3d(0, 1, 0); 
                        this.esq = new Vector3d(1, 0, 0);
                        this.frontCpy = new Vector3d(0, 0, 1);
                        this.upCpy = new Vector3d(0, 1, 0);
                        this.esqCpy = new Vector3d(1, 0, 0);
                        break;
                    case (int)GLSettings.tipoProjecao.inferior:
                        this.eye = new Vector3d(0, 0, 0);//0, 215, 0
                        this.front = new Vector3d(0, 0, -1);
                        this.up = new Vector3d(0, 1, 0); //eixo principal 
                        this.esq = new Vector3d(-1, 0, 0); //1 gira em um sentido / -1 gira em outra sentido 
                        this.frontCpy = new Vector3d(0, 0, -1);
                        this.upCpy = new Vector3d(0, 1, 0);
                        this.esqCpy = new Vector3d(-1, 0, 0);
                        break;                  
                    case (int)GLSettings.tipoProjecao.frontal:
                        this.eye = new Vector3d(0, 0, 0); //215,0,0             Vector3d frontCenter = new Vector3d(190, 0, 100);//200,0 ,200
                        this.front = new Vector3d(0, -1, 0);
                        this.up = new Vector3d(0, 0, 1); 
                        this.esq = new Vector3d(1, 0, 0);
                        this.frontCpy = new Vector3d(0, -1, 0);
                        this.upCpy = new Vector3d(0, 0, 1);
                        this.esqCpy = new Vector3d(1, 0, 0);
                        break;
                    case (int)GLSettings.tipoProjecao.direita:
                        this.eye = new Vector3d(0, 0, 0); //215,0,0
                        this.front = new Vector3d(1, 0, 0);
                        this.up = new Vector3d(0, 0, 1);
                        this.esq = new Vector3d(0, 1, 0);
                        this.frontCpy = new Vector3d(1, 0, 0);
                        this.upCpy = new Vector3d(0, 0, 1);
                        this.esqCpy = new Vector3d(0, 1, 0);
                        break;
                    case (int)GLSettings.tipoProjecao.esquerda:
                        this.eye = new Vector3d(0, 0, 0); //215,0,0
                        this.front = new Vector3d(-1, 0, 0);
                        this.up = new Vector3d(0, 0, 1);
                        this.esq = new Vector3d(0, -1, 0);
                        this.frontCpy = new Vector3d(-1, 0, 0);
                        this.upCpy = new Vector3d(0, 0, 1);
                        this.esqCpy = new Vector3d(0, -1, 0);
                        break;
                    case (int)GLSettings.tipoProjecao.fundo:
                        this.eye = new Vector3d(0, 0, 0); //215,0,0
                        this.front = new Vector3d(0, 1, 0);
                        this.up = new Vector3d(0, 0, 1);
                        this.esq = new Vector3d(-1, 0, 0);
                        this.frontCpy = new Vector3d(0, 1, 0);
                        this.upCpy = new Vector3d(0, 0, 1);
                        this.esqCpy = new Vector3d(-1, 0, 0);
                        break;
                    case (int)GLSettings.tipoProjecao.ZX:
                        this.eye = new Vector3d(0, 0, 0);//0, 215, 0
                        this.front = new Vector3d(1, -1, 1);
                        this.up = new Vector3d(0, 1, 0); //eixo principal 
                        this.esq = new Vector3d(-1, 0, 0); //1 gira em um sentido / -1 gira em outra sentido 
                        this.frontCpy = new Vector3d(1, -1, 1);
                        this.upCpy = new Vector3d(0, 1, 0);
                        this.esqCpy = new Vector3d(-1, 0, 0);



                        /*
                        Vector3d front_ = new Vector3d(0, -1, 0);
                        Vector3d frontCenter = new Vector3d(200, 0, 200);//300,0 ,400
                        frontUp_ = new Vector3d(0, 100, 1);

                        Vector3d front_ = new Vector3d(1, -1, 1);
                        Vector3d frontCenter = new Vector3d(200, 300, 200); //300 0 300
                        frontUp_ = new Vector3d(0, 100, 1);
                        */

                        break;
                }               
            }
            habilitadaPerspectivasAlternativas = !habilitadaPerspectivasAlternativas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectModelo"></param>
        public void Draw(string selectModelo)
        {

            if ((double)this.zFar <= 0.0)
                this.zFar = 300f;
            if ((double)this.zFar > 1.00000004091848E+35)
                this.zFar = 1E+35f;            

            if (this.DrawStereo)
            {
                DrawStereoImages(selectModelo);
            }
            else
            {
                Draw3D(selectModelo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertexPointer"></param>
        /// <param name="colorPointer"></param>
        /// <param name="verticesLength"></param>
        /// <param name="projection"></param>
        public void DrawPointCloud(int vertexPointer, int colorPointer, int verticesLength, Matrix4 projection)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit |
                       ClearBufferMask.DepthBufferBit |
                       ClearBufferMask.StencilBufferBit);
          
            Matrix4 lookat = Matrix4.LookAt(0, 128, 256, 0, 0, 0, 0, 1, 0);
            Vector3d scale = new Vector3d(4, 4, 4);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
            GL.Scale(scale);
            GL.PointSize(GLSettings.PointSize_);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes, new IntPtr(0));
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorPointer);
            GL.ColorPointer(3,ColorPointerType.UnsignedByte, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexPointer);
            GL.DrawArrays(PrimitiveType.Points, 0, verticesLength);
            glControl1.SwapBuffers();
            glControl1.Invalidate();            
        }
      
        /// <summary>
        /// 
        /// </summary>
        public void DrawAxis()
        {                                   
            GL.Begin(PrimitiveType.Lines);
            //GL.Normal3(90.0f, 90.0f, 90.0f);
            GL.Normal3(0.0f, 0.0f, 0.0f);
            GL.Color3(Color.Red);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(370f, 0.0f, 0.0f);
            GL.Vertex3(370f, 0.0f, 150f);
            GL.Vertex3(0.0f, 0.0f, 150f);
            GL.Vertex3(0.0f, 450f, 0.0f);
            GL.Vertex3(370f, 450f, 0.0f);
            GL.Vertex3(370f, 450f, 150f);
            GL.Vertex3(0.0f, 450f, 150f);
            GL.Color3(Color.Green);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 450f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 150f);
            GL.Vertex3(0.0f, 450f, 150f);
            GL.Vertex3(370f, 0.0f, 0.0f);
            GL.Vertex3(370f, 450f, 0.0f);
            GL.Vertex3(370f, 0.0f, 150f);
            GL.Vertex3(370f, 450f, 150f);
            GL.Color3(Color.Blue);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 150f);
            GL.Vertex3(370f, 0.0f, 0.0f);
            GL.Vertex3(370f, 0.0f, 150f);
            GL.Vertex3(0.0f, 450f, 0.0f);
            GL.Vertex3(0.0f, 450f, 150f);
            GL.Vertex3(370f, 450f, 0.0f);
            GL.Vertex3(370f, 450f, 150f);
            GL.End();
            GL.LineWidth(GLSettings.PointSizeAxis);
            GL.PointSize(GLSettings.PointSize_);
         
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(Color.Aqua);
            GL.Vertex3(0.0f, 0.0f, 0.0f);           
            GL.Vertex3(0.0f, 40.0f, 0.0f);            
            GL.Vertex3(40.0f, 0.0f, 0.0f);            
            GL.End();          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        public void teste2(Point point)
        {
            point_anterior = point;
            GL.Begin(PrimitiveType.Points);
            //GL.Normal3(90.0f, 90.0f, 90.0f);
           // GL.Normal3(0.0f, 0.0f, 0.0f);
            GL.Color3(Color.Red);
            GL.Vertex3(point.X, point.Y, 0.0f);           
            GL.End();

            GL.Begin(PrimitiveType.Polygon);
            //GL.Normal3(90.0f, 90.0f, 90.0f);
            // GL.Normal3(0.0f, 0.0f, 0.0f);
            GL.Color3(Color.Red);
            GL.Vertex3(point.X, point.Y, 0.0f);
            GL.End();
        }

        /// <summary>
        /// perspectivaConsolidacao
        /// </summary>
        private void teste3()
        {
            // Habilita o uso do Stencil neste programa 
            GL.Enable(EnableCap.StencilTest);
            // Define que "0" será usado para limpar o Stencil 
            GL.ClearStencil(0);
            // limpa o Stencil 
            GL.Clear(ClearBufferMask.StencilBufferBit);
            //////////////////////////////////////////////
            //GL.StencilFunc(StencilFunction.Notequal, 1, 1);
            //GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            GL.StencilMask(0xFF);
            //////////////////////////////////////////////
            GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
           // GL.PushMatrix();
            //Triangulo desenhado
            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(Color.Green);
            GL.Vertex3(60.0f, 0.0f, 0.0f);
            GL.Color3(Color.Green);
            GL.Vertex3(0.0f, 60f, 0.0f);
            GL.Color3(Color.Green);
            GL.Vertex3(0.0f, 0.0f, 60f);
            GL.End();
           // GL.PopMatrix();
          //  GL.LoadIdentity();
          //  GL.PushMatrix();
            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(Color.Red);
            GL.Vertex3(10.0f, 0.0f, 0.0f);
            GL.Color3(Color.Red);
            GL.Vertex3(0.0f, 30f, 0.0f);
            GL.Color3(Color.Red);
            GL.Vertex3(0.0f, 0.0f, 20f);
            GL.End();
           // GL.PopMatrix();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawAxisLabels()
        {
            PointF position = PointF.Empty;
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textRenderer.Texture);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1f, -1f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1f, -1f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1f, 1f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1f, 1f);
            GL.End();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteLinesForNormals()
        {
            LinesFrom = new List<Vertex>();
            LinesTo = new List<Vertex>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myModel"></param>
        public void CreateLinesForNormals(Model3D myModel)
        {
            if (myModel.Normals == null || myModel.VertexList.Count != myModel.Normals.Count)
            {
                MessageBox.Show("Normals not calculated right ");
                return;
            }
            LinesFrom = new List<Vertex>();
            LinesTo = new List<Vertex>();

            for (int i = 0; i < myModel.VertexList.Count; i++)
            {
                LinesFrom.Add(myModel.VertexList[i]);
                LinesTo.Add(new Vertex(myModel.Normals[i]));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowLines()
        {       
            if (this.LinesFrom != null)
            {
                GL.Begin(PrimitiveType.Lines);
               
                for (int i = 0; i < LinesFrom.Count; i++)
                {
                    Vertex vStart = LinesFrom[i];
                    Vertex vEnd = LinesTo[i];
                    if (vStart.Color == null)
                    {
                        GL.Color4(1f,1f,1f,1f);
                    }
                    else
                    {
                        GL.Color4(vStart.Color[0], vStart.Color[1], vStart.Color[2], vStart.Color[3]);
                    }
                    GL.Vertex3(vStart.Vector.X, vStart.Vector.Y, vStart.Vector.Z);
                    GL.Vertex3(vEnd.Vector.X, vEnd.Vector.Y, vEnd.Vector.Z);
                }
                GL.End();
                GL.LineWidth(GLSettings.PointSizeAxis);
                GL.PointSize(GLSettings.PointSize_);
                //dio GL.Scale(GLSettings.scaleX, GLSettings.scaleY, GLSettings.scaleZ);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="vStart"></param>
        /// <param name="vEnd"></param>
        private void DrawLine(float[] color, Vertex vStart, Vertex vEnd)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(color[0], color[1], color[2], color[3]);
            GL.Vertex3(vStart.Vector.X, vStart.Vector.Y, vStart.Vector.Z);
            GL.Vertex3(vEnd.Vector.X, vEnd.Vector.Y, vEnd.Vector.Z);
            GL.End();

            GL.LineWidth(GLSettings.PointSizeAxis);
            GL.PointSize(GLSettings.PointSize_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectName"></param>
        private void DrawAll3DObjects(string selectName)
        {
            if(GLSettings.ShowAxis)
                DrawAxis();

            if (this.Models3D != null && this.Models3D.Count > 0)
            {

                ShowLines();

                lock (this.Models3D)
                {
                    foreach (Model3D model in this.Models3D)
                    {
                        lock (model)
                        {
                            if(selectName == "*")
                            { 
                                model.Render(false, model.Name);
                            }
                            else model.Render(false, selectName);
                        }
                    }
                }             
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ind"></param>
        public void SelectModel(int ind)
        {
            
            try
            {
                this.SelectedModelIndex = ind;
                this.center = new Vector3d(this.Models3D[ind].CenterOfGravity + this.Models3D[ind].vetTransl);
                this.distEye = this.Models3D[ind].MaxPoint.Vector.X;
                if (this.distEye < this.Models3D[ind].MaxPoint.Vector.Y)
                    this.distEye = this.Models3D[ind].MaxPoint.Vector.Y;
                if (this.distEye < this.Models3D[ind].MaxPoint.Vector.Z)
                    this.distEye = this.Models3D[ind].MaxPoint.Vector.Z;
                this.distEye *= 1.5;
                this.zFar = (float)((this.distEye + 30.0) * 3.0);
              //  this.RepositionCamera(180.0f, 0.0f, this.MODE_ROT);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Exception: " + ex.ToString(), "Select Model");
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ind"></param>
        /// <param name="modo"></param>
        /// <param name="mouseCount"></param>
        /// <param name="OpenGLControl"></param>
        public void MoveModel(int ind, int modo, int mouseCount, OpenGLControl OpenGLControl)
        {
            try
            {          
                this.paramAnt = this.param;
                if (modo <= 2)                 
                    this.param = (float)this.distEye * (float)mouseCount / (float)this.glControl1.Width;
                if (modo > 2)
                    this.param = /*9.424778f*/ 4.0f * (float)mouseCount / (float)this.glControl1.Width;
                //this.Models3D[ind].Translate(modo, (double)this.param - (double)this.paramAnt);
                this.Models3D[ind].Translate(modo, (double)this.param - (double)this.paramAnt);
                this.Models3D[ind].Render(true, this.Models3D[ind].Name);                   
                this.Draw(this.Models3D[ind].Name);
                this.glControl1.Refresh();          
            }
            catch 
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ConsolidaMoveModel()
        {
            this.param = 0.0f;
        }      
    }
}
