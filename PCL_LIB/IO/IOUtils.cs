/******************************************************************************
 *
 *    MIConvexHull, Copyright (C) 2013 David Sehnal, Matthew Campbell
 *
 *  This library is free software; you can redistribute it and/or modify it 
 *  under the terms of  the GNU Lesser General Public License as published by 
 *  the Free Software Foundation; either version 2.1 of the License, or 
 *  (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful, 
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of 
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser 
 *  General Public License for more details.
 *  
 *****************************************************************************/

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
using System.Windows.Forms;

namespace PCLLib
{
    public class IOUtils
    {
        public static List<Vertex> ReadXYZFile_ToVertices(string fileNameLong, bool rotatePoints)
        {
            List<Vector3d> listVector3d = ReadXYZFile_ToVector3d(fileNameLong, rotatePoints);
            List<Vertex> listVertices = Vertices.ConvertVector3DListToVertexList(listVector3d);

            return listVertices;

        }
        public static List<Vector3d> ReadXYZFile_ToVector3d(string fileNameLong, bool rotatePoints)
        {

            string[] lines;
            try
            {

                lines = System.IO.File.ReadAllLines(fileNameLong);
            }
            catch
            {
                MessageBox.Show("File read error - e.g. cannot be found:  " + fileNameLong);
                return null;
            }
            return ConvertLinesToVector3d(lines, rotatePoints);
        }
        private static List<Vector3d> ConvertLinesToVector3d(string[] lines, bool rotatePoints)
        {
            List<Vector3d> listOfVectors = new List<Vector3d>();

            for (int i = 0; i < lines.GetLength(0); i++)
            {
                string[] arrStr1 = lines[i].Split(new Char[] { ' ' });
                try
                {

                    if (arrStr1.GetLength(0) > 2)
                        listOfVectors.Add(new Vector3d(Convert.ToDouble(arrStr1[0], GeneralSettings.CurrentCulture), Convert.ToDouble(arrStr1[1], GeneralSettings.CurrentCulture), Convert.ToDouble(arrStr1[2], GeneralSettings.CurrentCulture)));

                }
                catch
                {
                    MessageBox.Show("Error parsing file at line: " + i.ToString());
                }

            }

            //if (rotatePoints)
            //{
            //    listOfVectors = RotatePointCloud(listOfVectors);
            //}


            return listOfVectors;
        }

        /// <summary>
        /// Reads only position and color information (No normals, texture, triangles etc. etc)
        /// </summary>
        /// <param name="fileOBJ"></param>
        /// <param name="myNewModel"></param>
        public static List<Vertex> ReadObjFile_ToVertices(string fileOBJ)
        {
            List<Vertex> myVertices = new List<Vertex>();
            string line = string.Empty;
            int indexInModel = -1;
            try
            {

                using (StreamReader streamReader = new StreamReader(fileOBJ))
                {
                    //Part p = new Part();
                    Vertex vertex = new Vertex();
                    //myNewModel.Part = new List<Part>();
                    while (!streamReader.EndOfStream)
                    {
                        line = streamReader.ReadLine().Trim();
                        while (line.EndsWith("\\"))
                            line = line.Substring(0, line.Length - 1) + streamReader.ReadLine().Trim();
                        string str1 = GeneralSettings.TreatLanguageSpecifics(line);
                        string[] strArrayRead = str1.Split();
                        if (strArrayRead.Length >= 0)
                        {
                            switch (strArrayRead[0].ToLower())
                            {
                                case "v"://Vertex
                                    vertex = HelperReadVertex(strArrayRead);
                                    indexInModel++;
                                    vertex.IndexInModel = indexInModel;
                                    myVertices.Add(vertex);
                                    break;
                           
                            }
                        }
                    }
            
                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error reading obj file - Vertices: " + line + " ; " + err.Message);
            }
            return myVertices;

        }

        public static Vertex HelperReadVertex(double X, double Y, double Z)
        {

            Vertex vertex = new Vertex();
            vertex.Vector = new Vector3d(X, Y, Z);
            vertex.IndTriangles = new List<int>();
            vertex.IndPart = new List<int>();
            vertex.IndexNormals = new List<int>();

            vertex.Color = new float[4] { 1f, 1f, 1f, 1f };

            return vertex;
        }

        public static Vertex HelperReadVertex(string[] strArrayRead)
        {

            Vertex vertex = new Vertex();
            vertex.Vector = new Vector3d(0, 0, 0);
            vertex.IndTriangles = new List<int>();
            vertex.IndPart = new List<int>();
            vertex.IndexNormals = new List<int>();

            if (strArrayRead.Length > 3)
            {
                //double dx, dy, dz;
                double.TryParse(strArrayRead[1], NumberStyles.Float | NumberStyles.AllowThousands, GeneralSettings.CurrentCulture, out vertex.Vector.X);
                double.TryParse(strArrayRead[2], NumberStyles.Float | NumberStyles.AllowThousands, GeneralSettings.CurrentCulture, out vertex.Vector.Y);
                double.TryParse(strArrayRead[3], NumberStyles.Float | NumberStyles.AllowThousands, GeneralSettings.CurrentCulture, out vertex.Vector.Z);
            }

            vertex.Color = new float[4] { 1f, 1f, 1f, 1f };
            if (strArrayRead.Length > 7)
            {
                float.TryParse(strArrayRead[7], NumberStyles.Float | NumberStyles.AllowThousands, GeneralSettings.CurrentCulture, out vertex.Color[3]);
            }

            if (strArrayRead.Length > 6)
            {
                float.TryParse(strArrayRead[4], NumberStyles.Float | NumberStyles.AllowThousands, GeneralSettings.CurrentCulture, out vertex.Color[0]);
                float.TryParse(strArrayRead[5], NumberStyles.Float | NumberStyles.AllowThousands, GeneralSettings.CurrentCulture, out vertex.Color[1]);
                float.TryParse(strArrayRead[6], NumberStyles.Float | NumberStyles.AllowThousands, GeneralSettings.CurrentCulture, out vertex.Color[2]);

            }
            return vertex;
        }
    }
}
