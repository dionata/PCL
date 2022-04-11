using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace PCLLib
{
    public partial class Model3D
    {
       
        /// <summary>Returns a Bitmap containing a text drawn. Useful to set as texture.</summary>
        /// <param name="s">String to be written</param>
        /// <param name="TextFont">Font to use</param>
        /// <param name="TextLeftColor">Left color of Text.</param>
        /// <param name="TextRightColor">Right color of Text.</param>
        /// <param name="BackgroundLeftColor">Left color of Background.</param>
        /// <param name="BackgroundRightColor">Right color of Background.</param>
        public static Bitmap DrawString(string s, Font TextFont,Color TextLeftColor,Color TextRightColor,Color BackgroundLeftColor,Color BackgroundRightColor)
        {
            if (s == "")
                return (Bitmap)null;
            Bitmap bitmap1 = new Bitmap(10, 10);
            SizeF sizeF = Graphics.FromImage((Image)bitmap1).MeasureString(s, TextFont);
            Bitmap bitmap2 = new Bitmap((int)sizeF.Width, (int)sizeF.Height);
            Graphics graphics = Graphics.FromImage((Image)bitmap2);
            Brush brush1 = (Brush)new LinearGradientBrush(new PointF(0.0f, 0.0f), new PointF(sizeF.Width, sizeF.Height), BackgroundLeftColor, BackgroundRightColor);
            graphics.FillRectangle(brush1, 0, 0, bitmap2.Width, bitmap2.Height);
            Brush brush2 = (Brush)new LinearGradientBrush(new PointF(0.0f, 0.0f), new PointF(sizeF.Width, sizeF.Height), TextLeftColor, TextRightColor);
            graphics.DrawString(s, TextFont, brush2, 0.0f, 0.0f);
            bitmap1.Dispose();
            return bitmap2;
        }

        public static Bitmap DrawString(string s, Font TextFont,Color TextColor,Color BackgroundColor)
        {
            return Model3D.DrawString(s, TextFont, TextColor, TextColor, BackgroundColor, BackgroundColor);
        }

        public static Bitmap DrawString(string s, Font TextFont)
        {
            return Model3D.DrawString(s, TextFont,Color.Black,Color.Black,Color.White,Color.White);
        }

        public static void AdjustCompatibility(DataTable OpenTKLibTbl)
        {
            if (!(OpenTKLibTbl.Rows[0]["OpenTKLibVersion"].ToString() != ((object)Assembly.GetExecutingAssembly().GetName().Version).ToString()))
                return;
            if (OpenTKLibTbl.Columns.IndexOf("intWireframe") >= 0)
                OpenTKLibTbl.Columns["intWireframe"].ColumnName = "intRenderStyle";
            if (OpenTKLibTbl.Columns.IndexOf("ModelName") < 0)
            {
                DataColumn column = new DataColumn("ModelName", Type.GetType("System.String"));
                OpenTKLibTbl.Columns.Add(column);
                OpenTKLibTbl.Rows[0]["ModelName"] = (object)"";
            }
        }      
    }
}
