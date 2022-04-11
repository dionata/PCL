
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
 
  
    public static class CLEnum
    {
        public enum CLModelType
        {
            Curve,
            Surface,
        }

        public enum CLModelMovement
        {
            Static,
            Dynamic,
        }

        public enum CLModelTextureType
        {
            None,
            FromText,
            FromImage,
        }

        public enum CLModelQuality
        {
            Low,
            Medium,
            High,
            VeryHigh,
            Custom,
        }

        public enum CLRenderStyle
        {
            Solid,
            Wireframe,
            Point,
        }
    }

 
 
}
