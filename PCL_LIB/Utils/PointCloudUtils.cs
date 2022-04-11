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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCLLib;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using OpenTK;

namespace PCLLib
{
    public class PointCloudUtils
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldList"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static List<Vector3d> RotatePointCloudXY(List<Vector3d> oldList, int Width, int Height)
        {
           
            List<Vector3d> listOfVectors = new List<Vector3d>();


            for (int i = 0; i < oldList.Count; i++)
            {
                Vector3d v = oldList[i];
                
                double newX = Width - v.X;
                double newY = Height - v.Y;

                listOfVectors.Add(new Vector3d(newX, newY, v.Z));

            }

            return listOfVectors;

        }   
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrayColor"></param>
        /// <param name="arrayDepth"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static List<float[]> CreateColorInfo(byte[] arrayColor, ushort[] arrayDepth, int width, int height)
        {

            int BYTES_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

            List<float[]> listOfColors = new List<float[]>();
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    int depthIndex = (y * width) + x;
                    int colorIndex = depthIndex * BYTES_PER_PIXEL;
                    ushort z = arrayDepth[depthIndex];
                    if (z > 0)
                    {
                        float[] color = new float[4]{0,0,0,0};
                        color[0] = arrayColor[colorIndex    ] / 255F  ;
                        color[1] = arrayColor[colorIndex +1 ] / 255F;
                        color[2] = arrayColor[colorIndex +2 ] / 255F;
                        color[3] = 1F;
                        listOfColors.Add(color);
                    }
                }
            }

            return listOfColors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfItems"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="transparency"></param>
        /// <returns></returns>
        public static List<float[]> CreateColorList(int numberOfItems, byte red, byte green, byte blue, byte transparency)
        {

            int BYTES_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

            List<float[]> listOfColors = new List<float[]>();
            float[] color = new float[4] { 0, 0, 0, 0 };
            color[0] = red / 255F;
            color[1] = green / 255F;
            color[2] = blue / 255F;
            color[3] = transparency / 255F;

            for (int i = 0; i < numberOfItems; ++i)
            {
                listOfColors.Add(color);
            }
           
            return listOfColors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myListVertices"></param>
        /// <returns></returns>
        public static List<Point3D> CreatePoint3DListFromVertices(List<Vertex> myListVertices)
        {
            List<Point3D> myListPoint3D = new List<Point3D>();
            for (int i = 0; i < myListVertices.Count; i++)
            {
                Vertex myVertex = myListVertices[i];
                Point3D p3D = new Point3D(myVertex.Vector.X, myVertex.Vector.Y, myVertex.Vector.Z);
                myListPoint3D.Add(p3D); 
            }
            return myListPoint3D;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointList"></param>
        /// <param name="pointsTarget"></param>
        /// <param name="pointOther"></param>
        /// <returns></returns>
        public static List<Vertex> CreateVerticesFromDrawingPoints_IncludingCheck(List<System.Drawing.Point> pointList, List<Vertex> pointsTarget, List<System.Drawing.Point> pointOther)
        {

            List<Vertex> pointNew = new List<Vertex>();
            bool pointFound = false;

            for (int i = pointList.Count - 1; i >= 0; i--)
            {
                System.Drawing.Point pNew = pointList[i];
                for (int j = 0; j < pointsTarget.Count; j++)
                {
                    Vertex p = pointsTarget[j];
                    //add point only if it is found in the original point list
                    if (pNew.X == Convert.ToInt32(p.Vector[0]) && pNew.Y == Convert.ToInt32(p.Vector[1]))
                    {
                        pointFound = true;
                        pointNew.Add(p);
                        break;
                    }
                }
                //some error - have to check!
                if (!pointFound)
                {
                    MessageBox.Show("Error in identifying point from cloud with the stitched result: " + i.ToString());
                    pointOther.RemoveAt(i);
                }
            }
            return pointNew;
        }
    }
}
