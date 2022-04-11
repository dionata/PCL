using System;
using System.Collections.Generic;
using System.IO;

 
 
using OpenTK;
namespace PCLLib
{
    public partial class OpenGLControl
    {
        public void ShowPointCloud(string name, List<Vector3d> vectors, List<float[]> colors )
        {
            string errorText = string.Empty;

            List<Vertex> myVertexList = Model3D.CreateVertexList(vectors, colors);

            ShowPointCloud(name, myVertexList);
        }
        public void ShowPointCloud(string name, List<Vertex> myVertexList)
        {
            string errorText = string.Empty;
                       
            Model3D model3D = new Model3D(name, myVertexList);
            AddModel(model3D);
            ShowModels();
        }
        public void RemoveAllModels()
        {
            for (int i = GLrender.Models3D.Count - 1; i >= 0; i--)
            {
                RemoveModel(i);
            }
        }
        public void OpenTwoTrialPointClouds()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Models";
            path = AppDomain.CurrentDomain.BaseDirectory + "Models";
            string errorText = string.Empty;
            string fileName = path + "\\KinectFace1.obj";
            Model3D model = this.GLrender.LoadModel(fileName, errorText);
            AddModel(model);
            ShowModels();

            fileName = path + "\\KinectFace2.obj";
            model = this.GLrender.LoadModel(fileName, errorText);
            AddModel(model);
            ShowModels();
        }
    }
}
