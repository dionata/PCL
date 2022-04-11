
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

namespace PCLLib.Models
{
    public static class StandardModels3D
    {
        private static float rCyl = 1f;
        private static float rCon = 1f;
        private static float rSph = 1f;
        private static Model3D myModelPoygon = new Model3D();
        private static string Name = "polygon";
        private static  List<Vertex> points = new List<Vertex>();
        private static int indexInModel = -1;
        private static Part p = new Part();
        private static Point fimPolygon;
        private static Vector3d Color = new Vector3d(0.2, 0.4, 1.0);

        /// <summary>Generates a 3D Model for a cylinder.</summary>
        /// <param name="Name">Model name.</param>
        /// <param name="Radius">Cylinder radius.</param>
        /// <param name="Height">Cylinder height.</param>
        /// <param name="numPoints">Number of points for circular section.</param>
        /// <param name="Color">Color vector.</param>
        /// <param name="Texture">Texture bitmap. Null uses no texture</param>
        public static Model3D Cylinder(string Name, float Radius, float Height, int numPoints, Vector3d Color, Bitmap Texture)
        {
            rCyl = Radius;
            return new Model3D(Name, new Model3D.CoordFuncXYZ(CylFunction), 0.0f, 12.566370f, 0.0f, Height, numPoints, 2, Color, Texture);
        }

        private static float[] CylFunction(float u, float v)
        {
            float[] listOfPoints = new float[6] { 0.0f, 0.0f, 0.0f, (float)Math.Cos((double)u), (float)Math.Sin((double)u), 0.0f };
            listOfPoints[0] = rCyl * listOfPoints[3];
            listOfPoints[1] = rCyl * listOfPoints[4];
            listOfPoints[2] = v;
            return listOfPoints;
        }
        /// <summary>Generates a 3D Model for a cone.</summary>
        /// <param name="Name">Model name.</param>
        /// <param name="Radius">Cone outer radius.</param>
        /// <param name="Height">Cone height.</param>
        /// <param name="numPoints">Number of points for circular section.</param>
        /// <param name="Color">Color vector.</param>
        /// <param name="Texture">Texture bitmap. Null uses no texture</param>
        public static Model3D Cone(string Name, float Radius, float Height, int numPoints, Vector3d Color, Bitmap Texture)
        {
            StandardModels3D.rCon = Radius / Height;
            return new Model3D(Name, new Model3D.CoordFuncXYZ(StandardModels3D.ConeFunc), 0.0f, 12.566370f, 0.0f, -Height, numPoints, 2, Color, Texture);
        }

        private static float[] ConeFunc(float u, float v)
        {

            float[] listOfPoints = new float[6]{ StandardModels3D.rCon * (float) Math.Sin((double) u) * v,
                StandardModels3D.rCon * (float) Math.Cos((double) u) * v,v,v * (float) Math.Sin((double) u),
                    v * (float) Math.Cos((double) u),-StandardModels3D.rCon * v};

            float num = (float)(1.0 / Math.Sqrt((double)listOfPoints[3] * (double)listOfPoints[3] + (double)listOfPoints[4] * (double)listOfPoints[4] + (double)listOfPoints[5] * (double)listOfPoints[5]));
            listOfPoints[3] *= num;
            listOfPoints[4] *= num;
            listOfPoints[5] *= num;
            return listOfPoints;
        }
        /// <summary>Generates a 3D Model for a sphere.</summary>
        /// <param name="Name">Model name.</param>
        /// <param name="numPoints">Number of points to use for each coordinate. At least 4.</param>
        /// <param name="Radius">Sphere radius.</param>
        /// <param name="Color">Color vector.</param>
        /// <param name="Texture">Texture bitmap. Null uses no texture</param>
        public static Model3D Sphere(string Name, float Radius, int numPoints, Vector3d Color, Bitmap Texture)
        {
            StandardModels3D.rSph = Radius;
            if (numPoints < 4)
                numPoints = 4;
            return new Model3D(Name, new Model3D.CoordFuncXYZ(StandardModels3D.SphereFunction), -12.566370f, 12.566370f, -3.141592f, 3.141592f, numPoints, numPoints, Color, Texture);
        }

        private static float[] SphereFunction(float u, float v)
        {
            float[] listOfPoints = new float[6]{0.0f,0.0f,0.0f,(float) (Math.Cos((double) u) * Math.Cos((double) v)),(float) (Math.Sin((double) u) * Math.Cos((double) v)),
          (float) Math.Sin((double) v)};
            listOfPoints[0] = StandardModels3D.rSph * listOfPoints[3];
            listOfPoints[1] = StandardModels3D.rSph * listOfPoints[4];
            listOfPoints[2] = StandardModels3D.rSph * listOfPoints[5];
            return listOfPoints;
        }
        /// <summary>Generates a 3D Model for a disk.</summary>
        /// <param name="Name">Model name.</param>
        /// <param name="numPoints">Number of points to use in circumference.</param>
        /// <param name="InnerRadius">Inner disk radius.</param>
        /// <param name="OuterRadius">Outer disk radius.</param>
        /// <param name="Color">Color vector.</param>
        /// <param name="Texture">Texture bitmap. Null uses no texture</param>
        public static Model3D Disk(string Name, float InnerRadius, float OuterRadius, int numPoints, Vector3d Color, System.Drawing.Bitmap Texture)
        {
            if (numPoints < 4)
                numPoints = 4;
            return new Model3D(Name, new Model3D.CoordFuncXYZ(StandardModels3D.DiskFunction), 0.0f, 12.566370f, InnerRadius, OuterRadius, numPoints, 2, Color, Texture);
        }

        private static float[] DiskFunction(float u, float v)
        {
            float[] listOfPoints = new float[6] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1f };

            listOfPoints[0] = v * (float)Math.Cos((double)u);
            listOfPoints[1] = v * (float)Math.Sin((double)u);
            listOfPoints[2] = 0.0f;
            return listOfPoints;
        }
        /// <summary>
        /// Generates a 3D Model for a cuboid
        /// </summary>
        /// <param name="Name">Model name</param>
        /// <param name="u">Length of the lower part</param>
        /// <param name="v">Length of the high part</param>
        /// <param name="numberOfPoints">Number of points to use in circumference</param>
        /// <param name="Color">Color vector</param>
        /// <param name="Texture">Texture bitmap. Null uses no texture</param>
        /// <returns></returns>
        public static Model3D Cuboid(string Name, float u, float v, int numberOfPoints, Vector3d Color, Bitmap Texture)
        {

            List<Vertex> points = Vertices.CreateCuboid(u, v, numberOfPoints);

            Vertices.SetColorOfListTo(points, Color);

            Model3D myModel = new Model3D();
            myModel.Create3DModel("Cube", points);

            return myModel;

        }
        /// <summary>
        /// Generates a 3D Model for a cuboid, by setting all lines with points
        /// </summary>
        /// <param name="Name">Model name</param>
        /// <param name="u">Length of the lower part</param>
        /// <param name="v">Length of the high part</param>
        /// <param name="numberOfPoints">Number of points to use in circumference</param>
        /// <param name="Color">Color vector</param>
        /// <param name="Texture">Texture bitmap. Null uses no texture</param>
        /// <returns></returns>
        public static Model3D Cuboid_AllLines(string Name, int numberOfPoints, Vector3d Color, Bitmap Texture)
        {
            //GLSettings.Bloco1XEncosto

            List<Vertex> points = new List<Vertex>();
            Model3D myModel = new Model3D();
            Part p = new Part();
            float y0 = 0f;
            p.Triangles.Clear();
            int indexInModel = -1;

            #region Corte X

            if (GLSettings.modoFuncionamentoCorte == "Corte X")
            {
                switch (GLSettings.numberDivblocoExecutado)
                {
                    case 0:
                    case 1:
                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, 0, y0, 0));
                            points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            y0 += (float)GLSettings.Bloco1YEncosto / numberOfPoints;
                        }
                        break;
                    case 2:
                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, 0, y0, 0));
                            points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            y0 += (float)GLSettings.Bloco1YEncosto / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, 0));
                            y0 += ((float)GLSettings.Bloco2YEncosto) / numberOfPoints;
                        }

                        break;
                    case 3:

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, 0, y0, 0));
                            points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            y0 += (float)GLSettings.Bloco1YEncosto / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, 0));
                            y0 += ((float)GLSettings.Bloco2YEncosto) / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco3ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, y0, GLSettings.Bloco3ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, y0, 0));
                            y0 += ((float)GLSettings.Bloco3YEncosto) / numberOfPoints;
                        }

                        break;
                    case 4:

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, 0, y0, 0));
                            points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            y0 += (float)GLSettings.Bloco1YEncosto / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, 0));
                            y0 += ((float)GLSettings.Bloco2YEncosto) / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco3ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, y0, GLSettings.Bloco3ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, y0, 0));
                            y0 += ((float)GLSettings.Bloco3YEncosto) / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, y0, GLSettings.Bloco4ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto + GLSettings.Bloco4XEncosto, y0, GLSettings.Bloco4ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto + GLSettings.Bloco4XEncosto, y0, 0));
                            y0 += ((float)GLSettings.Bloco4YEncosto) / numberOfPoints;
                        }
                        break;
                    case 5:

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, 0, y0, 0));
                            points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            y0 += (float)GLSettings.Bloco1YEncosto / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, 0));
                            y0 += ((float)GLSettings.Bloco2YEncosto) / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco3ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, y0, GLSettings.Bloco3ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, y0, 0));
                            y0 += ((float)GLSettings.Bloco3YEncosto) / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, y0, GLSettings.Bloco4ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto + GLSettings.Bloco4XEncosto, y0, GLSettings.Bloco4ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto + GLSettings.Bloco4XEncosto, y0, 0));
                            y0 += ((float)GLSettings.Bloco4YEncosto) / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto + GLSettings.Bloco4XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto + GLSettings.Bloco4XEncosto, y0, GLSettings.Bloco5ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto + GLSettings.Bloco4XEncosto + GLSettings.Bloco5XEncosto, y0, GLSettings.Bloco5ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto + GLSettings.Bloco4XEncosto + GLSettings.Bloco5XEncosto, y0, 0));
                            y0 += ((float)GLSettings.Bloco5YEncosto) / numberOfPoints;
                        }
                        break;
                }
            }
            #endregion Corte X
            #region Corte Z
            else if (GLSettings.modoFuncionamentoCorte == "Corte Z")
            {
                switch (GLSettings.numberDivblocoExecutado)
                {
                    case 0:
                    case 1:
                        
                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, 0, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto));
                            y0 += (float)GLSettings.Bloco1YEncosto / numberOfPoints;
                        }
                        break;
                    case 2:
                    case 3:
                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, 0, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto));
                            y0 += (float)GLSettings.Bloco1YEncosto / numberOfPoints;
                        }

                        y0 = 0;

                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            indexInModel++;
                            points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto));

                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto - GLSettings.Bloco3XEncosto, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto - GLSettings.Bloco3XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));                
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco1ZEncosto));

                            points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto));
                            points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                            points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                            y0 += ((float)GLSettings.Bloco2YEncosto) / numberOfPoints;
                        }

                        break;

                    //case 4:
                    //case 5:
                    //    for (int i = 0; i < numberOfPoints; i++)
                    //    {
                    //        indexInModel++;
                    //        points.Add(new Vertex(indexInModel, 0, y0, 0));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, 0));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco1XEncosto, y0, GLSettings.Bloco1ZEncosto));
                    //        points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto));
                    //        y0 += (float)GLSettings.Bloco1YEncosto / numberOfPoints;
                    //    }

                    //    y0 = 0;

                    //    for (int i = 0; i < numberOfPoints; i++)
                    //    {
                    //        indexInModel++;
                    //        points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto + GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto + GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco2XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                    //        points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                    //        y0 += ((float)GLSettings.Bloco2YEncosto) / numberOfPoints;
                    //    }

                    //    y0 = 0;

                    //    for (int i = 0; i < numberOfPoints; i++)
                    //    {
                    //        indexInModel++;
                    //        points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco4XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco4XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto + GLSettings.Bloco3ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco4XEncosto + GLSettings.Bloco4XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto + GLSettings.Bloco3ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco4XEncosto + GLSettings.Bloco4XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco4XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto));
                    //        points.Add(new Vertex(indexInModel, GLSettings.Bloco4XEncosto, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto + GLSettings.Bloco3ZEncosto));
                    //        points.Add(new Vertex(indexInModel, 0, y0, GLSettings.Bloco1ZEncosto + GLSettings.Bloco2ZEncosto + GLSettings.Bloco3ZEncosto));
                    //        y0 += ((float)GLSettings.Bloco3YEncosto) / numberOfPoints;
                    //    }
                    //    break;
                }
            }
#endregion Corte Z

            Vertices.SetColorOfListTo(points, Color);
            Vertices.ChangeTransparency(points, 0.5f);
            myModel.Create3DModel(Name, points);

            return myModel;
        }

        /// <summary>
        /// Generates a 3D Model for a cuboid, by setting all lines with points
        /// </summary>
        /// <param name="Name">Model name</param>
        /// <param name="u">Length of the lower part</param>
        /// <param name="v">Length of the high part</param>
        /// <param name="numberOfPoints">Number of points to use in circumference</param>
        /// <param name="Color">Color vector</param>
        /// <param name="Texture">Texture bitmap. Null uses no texture</param>
        /// <returns></returns>
        public static Model3D Cuboid_AllLines2(string Name, int numberOfPoints, Vector3d Color, Bitmap Texture)
        {
            //GLSettings.Bloco1XEncosto

            List<Vertex> points = new List<Vertex>();
            Model3D myModel = new Model3D();
            Part p = new Part();
            float y0 = 0f;
            p.Triangles.Clear();
            int indexInModel = -1;
            numberOfPoints = 10;
            int tamanho = 100; 

            for (int j = 0; j < tamanho; j += 1)
           {
                for (int i = 0; i < numberOfPoints; i++)
                {
                    indexInModel++;
                    points.Add(new Vertex(indexInModel, 0, y0, 0));
                    points.Add(new Vertex(indexInModel, 0, y0, j));
                    points.Add(new Vertex(indexInModel, j, y0, 0));
                    y0 += (float)tamanho / numberOfPoints;
                }
                y0 = 0;
            }

            for (int j = 0; j < tamanho; j +=1)
            {
                for (int i = 0; i < numberOfPoints; i++)
                {
                    indexInModel++;
                    points.Add(new Vertex(indexInModel, 0, 0, y0));
                    points.Add(new Vertex(indexInModel, j, 0, y0));
                    y0 += (float)tamanho / numberOfPoints;
                }

                y0 = 0;
            }
            Vertices.SetColorOfListTo(points, Color);
            Vertices.ChangeTransparency(points, 0.5f);
            myModel.Create3DModel(Name, points);
            myModel.ModelRenderStyle = CLEnum.CLRenderStyle.Wireframe;

            p.ColorOverall = new Vector3d(1.0, 1.0, 1.0);
            p.Name = Name;

            myModel.Parts.Add(p);

            return myModel;
        }

        /// <summary>
        /// Generates a 3D Model for a cuboid, by setting all lines with points
        /// </summary>
        /// <param name="Name">Model name</param>
        /// <param name="u">Length of the lower part</param>
        /// <param name="v">Length of the high part</param>
        /// <param name="numberOfPoints">Number of points to use in circumference</param>
        /// <param name="Color">Color vector</param>
        /// <param name="Texture">Texture bitmap. Null uses no texture</param>
        /// <returns></returns>
        public static void polygon(Point point, OpenGLControl OpenGLControl_, int estado)
        {            
            switch (estado)
            {
                case 0: //Inicio do polygono
                    fimPolygon = point;                                                                               
                    p.Triangles.Clear();                   
                    points.Clear();                 
                    points.Add(new Vertex(indexInModel, point.X, point.Y, 1000));           
                    Vertices.SetColorOfListTo(points, Color);
                    Vertices.ChangeTransparency(points, 1f);                    
                    myModelPoygon.Create3DModel(Name, points);
                    myModelPoygon.ModelRenderStyle = CLEnum.CLRenderStyle.Wireframe;              
                    p.Name = Name;
                    myModelPoygon.Parts.Add(p);
                    OpenGLControl_.AddModel(myModelPoygon);                              
                    break;
                case 1://Demarcações polygono            
                    points.Add(new Vertex(indexInModel, point.X, point.Y, 1000));
                    Vertices.SetColorOfListTo(points, Color);
                    Vertices.ChangeTransparency(points, 1f);
                    myModelPoygon.Create3DModel(Name, points);
                    myModelPoygon.ModelRenderStyle = CLEnum.CLRenderStyle.Wireframe;                   
                    p.Name = Name;
                    myModelPoygon.Parts.Add(p);                   
                    break;
                case 2://Fim do polygono              
                    points.Add(new Vertex(indexInModel, fimPolygon.X, fimPolygon.Y, 1000));
                    Vertices.SetColorOfListTo(points, Color);
                    Vertices.ChangeTransparency(points, 1f);
                    myModelPoygon.Create3DModel(Name, points);
                    myModelPoygon.ModelRenderStyle = CLEnum.CLRenderStyle.Wireframe;
                    p.Name = Name;
                    myModelPoygon.Parts.Add(p);                
                    break;

            }
        }
    }
}
