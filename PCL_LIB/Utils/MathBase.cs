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
    
    public class MathBase
    {
        public static float Pi_Float = 3.14159265358979f;
        public static float DegreesToRadians_Float = 0.017453292f;
        public static float RadiansToDegrees_Float = 57.2957795131f;
        public static double DegreesToRadians = 0.017453292519943295;
        public static double Pi = 3.1415926535897932384626;
        public static double RadiansToDegrees = 57.29577951308232;
        
         /// <summary>
         /// 
         /// </summary>
         /// <param name="v1"></param>
         /// <param name="v2"></param>
         /// <returns></returns>
         public static double DistanceBetweenVectors(Vector3d v1, Vector3d v2)
        {

            return (Vector3d.Subtract(v1, v2)).Length;
        }

        /// <summary>
        /// Rectum equation
        /// </summary>
        /// <param name="z0">Initial Z</param>
        /// <param name="z"></param>
        /// <param name="y0"></param>
        /// <param name="y"></param>
        /// <returns>value pairs: Z - Y</returns>
        public static List<Tuple<double, double>> rectumEquation(double z0, double z, double y0, double y, int level)
        {
            double m, inc;
            //List<double> newZs = new List<double>();
            List<Tuple<double, double>> newZs = new List<Tuple<double, double>>();                                 
            
            m = (z - z0) / (y - y0);

            inc = (y - y0) / level;

            for (double i = (y0 + inc); i < y; i += inc)
            {
                //newZs.Add((z0 + m * (i - y0)));
                newZs.Add(new Tuple<double, double>((z0 + m * (i - y0)), i));
            }            

            return newZs;
        }
    }
}
