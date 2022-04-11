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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Windows.Media;
using OpenTK;

namespace PCLLib
{
    public class PointUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsTarget"></param>
        /// <returns></returns>
        public static List<Vector3d> CopyVector3(List<Vector3d> pointsTarget)
        {
            List<Vector3d> tempPoints = new List<Vector3d>();


            for (int i = (pointsTarget.Count - 1); i >= 0; i--)
            {
                Vector3d point1 = pointsTarget[i];
                tempPoints.Add(point1); 

            }
            return tempPoints;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsTarget"></param>
        /// <returns></returns>
        public static List<Vertex> CopyVertices(List<Vertex> pointsTarget)
        {
            List<Vertex> tempPoints = new List<Vertex>();


            for (int i = (pointsTarget.Count - 1); i >= 0; i--)
            {
                Vertex point1 = pointsTarget[i];
                tempPoints.Add(point1);

            }
            return tempPoints;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsTarget"></param>
        /// <param name="pointsSource"></param>
        /// <param name="indices"></param>
        public static void RemoveVector3d(ref List<Vector3d> pointsTarget, ref List<Vector3d> pointsSource, List<int> indices)
        {
            List<Vector3d> temp1 = new List<Vector3d>();
            List<Vector3d> temp2 = new List<Vector3d>();

            //temp.ShallowCopy(this.PointsTarget.GetPoints());

            indices.Sort();
            int indexNew = -1;
            for (int iPoint = (pointsTarget.Count - 1); iPoint >= 0; iPoint--)
            {
                Vector3d point1 = pointsTarget[iPoint];
                Vector3d point2 = pointsSource[iPoint];
                bool bfound = false;
                for (int i = (indices.Count - 1); i >= 0; i--)
                {
                    if (indices[i] == iPoint)
                    {
                        bfound = true;
                        break;
                    }
                }
                if (!bfound)
                {
                    indexNew++;
                    temp1.Add(point1);
                    temp2.Add(point2);
                    
                }
            }
            pointsTarget = temp1;
            pointsSource = temp2;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsTarget"></param>
        /// <param name="pointsSource"></param>
        /// <param name="indices"></param>
        public static void RemoveVertex(ref List<Vertex> pointsTarget, ref List<Vertex> pointsSource, List<int> indices)
        {
            List<Vertex> temp1 = new List<Vertex>();
            List<Vertex> temp2 = new List<Vertex>();

            //temp.ShallowCopy(this.PointsTarget.GetPoints());

            indices.Sort();
            int indexNew = -1;
            for (int iPoint = (pointsTarget.Count - 1); iPoint >= 0; iPoint--)
            {
                Vertex point1 = pointsTarget[iPoint];
                Vertex point2 = pointsSource[iPoint];
                bool bfound = false;
                for (int i = (indices.Count - 1); i >= 0; i--)
                {
                    if (indices[i] == iPoint)
                    {
                        bfound = true;
                        break;
                    }
                }
                if (!bfound)
                {
                    indexNew++;
                    temp1.Add(point1);
                    temp2.Add(point2);

                }
            }
            pointsTarget = temp1;
            pointsSource = temp2;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double CalculateTotalDistance(List<Vertex> a, List<Vertex> b)
        {

            double totaldist = 0;
            for (int i = 0; i < a.Count; i++)
            {
                Vertex p1 = a[i];
                Vertex p2 = b[i];
                double dist = (Vector3d.Subtract(p1.Vector, p2.Vector)).Length;
                
                totaldist += dist;

            }

            return totaldist;
        }
      
    }
}
