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
    //Extensios attached to the object which folloes the "this" 
    public static class Matrix3DExtension
    {
        
        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3d TransformVector(this Matrix3d mat, Vector3d vec)
        {
            Vector3d vNew = new Vector3d(mat.Row2);
            double val = Vector3d.Dot(vNew, vec);

            return new Vector3d(
                Vector3d.Dot(new Vector3d(mat.Row0), vec),
                Vector3d.Dot(new Vector3d(mat.Row1), vec),
                Vector3d.Dot(new Vector3d(mat.Row2), vec));
        }
        public static  Matrix3d CreateRotation30Degrees(this Matrix3d mat)
        {
            Matrix3d result = Matrix3d.Identity;
            //rotation 30 degrees
            result[0, 0] = 1F;
            result[1, 1] = result[2, 2] = 0.86603;
            result[1, 2] = -0.5;
            result[2, 1] = 0.5;
           
            return result;
        }
        //public static int Multiply(this int valToMultiply, int value)
        //{
        //    //if (x <= 1) return 1;
        //    //if (x == 2) return 2;
        //    //else
        //    //    return x * factorial(x - 1);

        //    return valToMultiply * value;

        //} 
        
    }
}
