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
using OpenTK;

namespace PCLLib
{
    public class TransformPointsUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static List<Vector3d> CalculatePointsShiftedByCentroid(List<Vector3d> a)
        {
            Vector3d centroid = CalculateCentroid(a);
            List<Vector3d> b = CalculatePointsShiftedByCentroid(a, centroid);
            return b;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="centroid"></param>
        /// <returns></returns>
        public static List<Vector3d> CalculatePointsShiftedByCentroid(List<Vector3d> a, Vector3d centroid)
        {
            
            List<Vector3d> b = new List<Vector3d>();
            for (int i = 0; i < a.Count; i++)
            {
                Vector3d v = a[i];
                Vector3d vNew = new Vector3d(v.X - centroid.X, v.Y - centroid.Y, v.Z - centroid.Z);
                b.Add(vNew);

            }
            return b;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Matrix3d CalculateCorrelationMatrix(List<Vector3d> b, List<Vector3d> a)
        {
            //consists of elementx 
            //axbx axby axbz
            //aybx ayby aybz
            //azbx azby azbz
            Matrix3d H = new Matrix3d();
            for (int i = 0; i < b.Count; i++)
            {
                //H[0, 0] += b[i].X * a[i].X;
                //H[1, 0] += b[i].X * a[i].Y;
                //H[2, 0] += b[i].X * a[i].Z;

                //H[0, 1] += b[i].Y * a[i].X;
                //H[1, 1] += b[i].Y * a[i].Y;
                //H[2, 1] += b[i].Y * a[i].Z;

                //H[0, 2] += b[i].Z * a[i].X;
                //H[1, 2] += b[i].Z * a[i].Y;
                //H[2, 2] += b[i].Z * a[i].Z;

                H[0, 0] += b[i].X * a[i].X;
                H[0, 1] += b[i].X * a[i].Y;
                H[0, 2] += b[i].X * a[i].Z;

                H[1, 0] += b[i].Y * a[i].X;
                H[1, 1] += b[i].Y * a[i].Y;
                H[1, 2] += b[i].Y * a[i].Z;

                H[2, 0] += b[i].Z * a[i].X;
                H[2, 1] += b[i].Z * a[i].Y;
                H[2, 2] += b[i].Z * a[i].Z;


            }
            H = MatrixUtilsOpenTK.MultiplyScalar3D(H, 1.0D / b.Count);
            return H;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double[,] DoubleArrayFromMatrix(Matrix3d m)
        {
            double[,] doubleArray = new double[3,3];
            for(int i = 0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                    doubleArray[i,j] = m[i,j];

            return doubleArray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double[,] DoubleArrayFromMatrix(Matrix2d m)
        {
            double[,] doubleArray = new double[2, 2];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    doubleArray[i, j] = m[i, j];

            return doubleArray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[,] GetTransformMatrixAsDoubleArray(Matrix4d matrix)
        {
            double[,] matrixAsDouble = new double[4, 4];


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrixAsDouble[i, j] = matrix[i, j];
                }
            }

            return matrixAsDouble;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static float[,] GetTransformMatrixAsFloatArray(Matrix4d matrix)
        {
            float[,] matrixAsDouble = new float[4, 4];


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrixAsDouble[i, j] = Convert.ToSingle(matrix[i, j]);
                }
            }

            return matrixAsDouble;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsTarget"></param>
        /// <returns></returns>
        public static Vector3d CalculateCentroid(List<Vector3d> pointsTarget)
        {
            
            
            Vector3d centroid = new Vector3d();
            for(int i = 0; i < pointsTarget.Count; i++)
            {
                Vector3d v = pointsTarget[i];
                centroid.X += v.X;
                centroid.Y += v.Y;
                centroid.Z += v.Z;

                
            }
            centroid.X /= pointsTarget.Count;
            centroid.Y /= pointsTarget.Count;
            centroid.Z /= pointsTarget.Count;
            
            return centroid;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projection"></param>
        /// <param name="view"></param>
        /// <param name="viewport"></param>
        /// <param name="mouse"></param>
        public void MouseToWorldRay(Matrix4 projection, Matrix4 view, System.Drawing.Size viewport, Vector2 mouse)
        {
            // these mouse.Z values are NOT scientific. 
            // Near plane needs to be < -1.5f or we have trouble selecting objects right in front of the camera. (why?)
         //   Vector3 pos1 = UnProject(ref projection, view, viewport, new Vector3(mouse.X, mouse.Y, -1.5f)); // near
         //   Vector3 pos2 = UnProject(ref projection, view, viewport, new Vector3(mouse.X, mouse.Y, 1.0f));  // far
         //   return SSRay.FromTwoPoints(pos1, pos2);
        }
    }
}
