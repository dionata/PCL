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

namespace PCLLib
{
 
    /// <summary>
    /// a part of a 3D model
    /// </summary>
    public class Part
    {
        public string Name;
        public List<Triangle> Triangles;
        public Vector3d ColorOverall;
        public float Transparency;
        public bool Selected;
        public int GLListNumber;
        public int[] GLBuffers;
        
        private int gLNumElements;

        public Part(Part p)
        {
            
            this.Name = p.Name;
            this.Triangles = new List<Triangle>((IEnumerable<Triangle>)p.Triangles);
            this.ColorOverall = new Vector3d(p.ColorOverall);
            this.Transparency = p.Transparency;
            this.Selected = p.Selected;
            this.GLListNumber = p.GLListNumber;
        }

        public Part()
        {
            this.Name = "";
            this.Triangles = new List<Triangle>();
            this.ColorOverall = new Vector3d();
            this.Transparency = 0.0f;
            this.Selected = false;
            this.GLListNumber = -1;
        }
        public int GLNumElements
        {
            get
            {
                return gLNumElements;
            }
            set
            {
                gLNumElements = value;
            }
        }
    }



}
