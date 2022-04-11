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
    public class RandomUtils
    {
        /// <summary>
        /// Generates random indices 
        /// </summary>
        /// <param name="MaxIndex"></param>
        /// <param name="numIndices"></param>
        /// <returns></returns>
        public static List<int> UniqueRandomIndices(int numIndices, int MaxIndex)
        {
            List<int> indices = new List<int>();
            try
            {
                //generatePart Number unique random indices from 0 to MAX


                //SeedRandom();
                //cannot generatePart more unique numbers than than the size of the set we are sampling
                if (numIndices > MaxIndex )
                {
                    MessageBox.Show("SW Call error for UniqueRandomIndices");
                    return indices;
                }
                Random rnd = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < 100000; i++)
                {
                    double newRnd = rnd.NextDouble() * (MaxIndex - 1) -0.5;
                    int newIndex = Convert.ToInt32(newRnd);
                    if (newIndex < 0)
                        newIndex = 0;
                    if (newIndex == MaxIndex)
                        newIndex = MaxIndex - 1;

                    if (!indices.Contains(newIndex))
                        indices.Add(newIndex);

                    if (indices.Count == numIndices)
                        return indices;

                }
                MessageBox.Show("No random Indices are found - please check routine UniqueRandomIndices");
                return indices;
            }
            catch(Exception err)
            {
                MessageBox.Show("Error in UniqueRandomIndices " + err.Message);
                return indices;
            }

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static List<Vertex> ExtractPoints(List<Vertex> points, List<int> indices)
        {
            try
            {

                List<Vertex> output = new List<Vertex>();
                for (int i = 0; i < indices.Count; i++)
                {
                    int indexPoint = indices[i];
                    Vertex p = points[indexPoint];
                    output.Add(p);
                }
                return output;
            }
            catch (Exception err)
            {
                MessageBox.Show("Error in RandomUtils.ExtractPoints " + err.Message);
                return null;
            }
        }

        /// <summary>
        /// Generates random indices 
        /// </summary>
        /// <param name="MaxIndex"></param>
        /// <param name="numIndices"></param>
        /// <returns></returns>
        public static List<Vertex> GetRandomPoints(int numPoints, List<Vertex> pointsInput)
        {

            List<int> randomIndices = RandomUtils.UniqueRandomIndices(numPoints, Convert.ToInt32(pointsInput.Count));

            List<Vertex> output = RandomUtils.ExtractPoints(pointsInput, randomIndices);

            return output;
        }


    }
}
