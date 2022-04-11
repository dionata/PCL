
using OpenCLTemplate;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK;
using System.Text;

namespace PCLLib.Models
{
    public partial class Model3DAUX
    {        
        public static CultureInfo CultureInfo = new CultureInfo("en-US");      
        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Save_ListVertices_Obj(List<Vertex> listVertices, string path, string fileName)
        {
            StringBuilder sb = new StringBuilder();


            List<string> lines = new List<string>();
            lines.Add("####");
            lines.Add("#");
            lines.Add("# Projeto Tecpost ");
            lines.Add("# Autor: Dionata Nunes <dionata.nunes@senairs.org.br>");
            lines.Add("# Data de criacao: " + System.DateTime.Today.ToString());
            lines.Add("#");
            lines.Add("# OBJ File");
            lines.Add("#");
            lines.Add("####");
            lines.Add("#");
            lines.Add("# Vertices: " + listVertices.Count.ToString());
            lines.Add("#");
            lines.Add("####");



            //vertices

            WriteVertices(listVertices, lines);
            
            System.IO.File.WriteAllLines(path + "\\" + fileName, lines);

            
            return true;

        }
        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Save_OBJ(Model3D myModel, string path, string fileName)
        {
            StringBuilder sb = new StringBuilder();

            
            List<string> lines = new List<string>();
            lines.Add("####");
            lines.Add("#");
            lines.Add("# Projeto Tecpost ");
            lines.Add("# Autor: Dionata Nunes <dionata.nunes@senairs.org.br>");
            lines.Add("# Data de criacao: " + System.DateTime.Today.ToString());
            lines.Add("#");
            lines.Add("# OBJ File");
            lines.Add("#");
            lines.Add("####");
            lines.Add("#");
            lines.Add("# Vertices: " + myModel.VertexList.Count.ToString());
            lines.Add("#");
            lines.Add("####");

            //vertices

            WriteVertices(myModel.VertexList, lines);
            WriteTexture(myModel, lines);
            WriteNormals(myModel, lines);
            WriteTriangles(myModel, lines);

            File.WriteAllLines(@path + fileName, lines, Encoding.UTF8);

            return true;
        }
        private static void WriteNormals(Model3D myModel, List<string> lines)
        {
          

            if (myModel.Normals != null)
            {
                int i ;
                for (i = 0; i < myModel.Normals.Count; i++)
                {
                    Vector3d vn = myModel.Normals[i];
                    string line = string.Format(CultureInfo.InvariantCulture, "vn {0} {1} {2}", vn.X, vn.Y, vn.Z);
                    //if (line.Contains("NaN"))
                    //    System.Windows.Forms.MessageBox.Show("NaN");

                    //dio lines.Add(string.Format(CultureInfo.InvariantCulture, "vn {0} {1} {2}", vn.X, vn.Y, vn.Z));
                }

                lines.Add(string.Format("# {0} normals", i));
            }

        }
        private static void WriteTexture(Model3D myModel, List<string> lines)
        {
            if (myModel.TextureCoords != null)
            {
                int i ;
                for(i = 0; i< myModel.TextureCoords.Count; i++)
                {
                    float[] texCoord = myModel.TextureCoords[i];
                    //textureIndexMap.Add(i, this.textureIndex++);

                    //dio lines.Add(string.Format(CultureInfo.InvariantCulture, "vt {0} {1}", texCoord[0], 1 - texCoord[2]));
                }

                lines.Add(string.Format("# {0} texture coordinates", i));
            }

        }
        private static void WriteVertices(List<Vertex> listVertices, List<string> lines)
        {
            for (int i = 0; i < listVertices.Count; ++i)
            {

                Vertex v = listVertices[i];
                Vector3d vect = v.Vector;
                string coordinate = vect.X.ToString(CultureInfo) + " " + vect.Y.ToString(CultureInfo) + " " + vect.Z.ToString(CultureInfo);
                if (v.Color != null)
                {

                    string color = v.Color[0].ToString(CultureInfo) + " " + v.Color[1].ToString(CultureInfo) + " " + v.Color[2].ToString(CultureInfo);

                    //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
                    lines.Add("v " + coordinate + " " + color);
                }
                else
                {
                    lines.Add("v " + coordinate);

                }


            }

        }
        private static void WriteTriangles(Model3D myModel, List<string> lines)
        {
            int i;
            if(myModel.Parts != null)
            {
                for(i = 0; i < myModel.Parts.Count; i++)
                {
                    //the index vertex starts with 1 (instead of 0)
                    for (int j = 0; j < myModel.Parts[i].Triangles.Count; j++)
                    {
                       // if(j<8)
                       // {
                           // Triangle a = myModel.Parts[i].Triangles[j];
                           // lines.Add(string.Format("f {0}", a.IndVertices[0]));
                       // }
                       // else
                       // {
                            Triangle a = myModel.Parts[i].Triangles[j];
                        try
                        {
                            lines.Add(string.Format("f {0} {1} {2}", a.IndVertices[0] + 1, a.IndVertices[1] + 1, a.IndVertices[2] + 1));
                        }
                        catch 
                        {
                        }
                           
                       // }
                    }
                }

            }
            //lines.Add(string.Format("# {0} faces", m.TriangleIndices.Count / 3));

        }

        private static string formatIndices(Model3D myModel, int i0)
        {
            bool hasTextureIndex = false;
            if(myModel.TextureCoords != null && myModel.TextureCoords.Count > i0)
                hasTextureIndex = true;

            bool hasNormalIndex = false;
            if (myModel.Normals != null && myModel.Normals.Count > i0)
                hasNormalIndex = true;

            
            if (hasTextureIndex && hasNormalIndex)
            {
                return string.Format("{0}/{1}/{2}", i0, i0, i0);
            }

            if (hasTextureIndex)
            {
                return string.Format("{0}/{1}", i0, i0);
            }

            if (hasNormalIndex)
            {
                return string.Format("{0}//{1}", i0, i0);
            }

            return i0.ToString();
        }

        public struct SSRay
        {
            public Vector3 pos;
            public Vector3 dir;

            public SSRay(Vector3 pos, Vector3 dir)
            {
                this.pos = pos;
                this.dir = dir;
            }

            public static SSRay FromTwoPoints(Vector3 p1, Vector3 p2)
            {

                Vector3 pos = p1;
                Vector3 dir = (p2 - p1).Normalized();

                return new SSRay(pos, dir);
            }

            public SSRay Transformed(Matrix4 mat)
            {
                // a point is directly transformed
                // however, a ray is only rotationally transformed.
                return new SSRay(Vector3.Transform(pos, mat), Vector3.Transform(dir, mat.ExtractRotation()).Normalized());
            }

            public override string ToString()
            {
                return String.Format("({0}) -> v({1})", pos, dir);
            }
        }

    }
}
