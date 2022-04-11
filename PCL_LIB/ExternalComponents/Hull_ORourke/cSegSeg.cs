/*----------------------------------------------------------------------------
 * Class cSegSeg  -- used for computing intersection of
 *                   (currently) two segments
 *
 * ClearSegments -- used to clear previous intesection
 * SegSegTopLevel returns code of intersection
 * DrawSegments, DrawInters -- drawing routines;
 * 
 *---------------------------------------------------------------------------*/

using OpenTK;
using PCLLib;
using System;

namespace PCLLib
{

    public class cSegSeg
    {
        public cPointd p;                           /* main point of intersection */
        private cVertexList list;            /* list, in which the segements are stored */
        private cPointd q = new cPointd();   /* 2nd pt. of intersection, if it is a line*/
        char code = '0';                     /* intersection code returned by SegSegInt*/

        public cSegSeg(cVertexList list)
        {
            p = new cPointd(0, 0);
            this.list = list;
        }

        public void ClearSegments()
        {
            code = '0';
            p.x = p.y = q.x = p.y = 0;
        }

        /* -------------------------------------------------------------------------
         *The following set of routines compute the (real) intersection point between
         *two segments.  The two segments are taken to be the first four edges of
         *the input "polygon" list.  A character "code" is returned and printed out.
         */
        public char SegSegTopLevel()
        {
            // Set the segments ab and cd to be the first four points in the list.
            cVertex temp = list.head;
            cPointi a = temp.Point;
            temp = temp.NextVertex;
            cPointi b = temp.Point;
            temp = temp.NextVertex;
            cPointi c = temp.Point;
            temp = temp.NextVertex;
            cPointi d = temp.Point;
            // Store the results in class data field p, and returns code.
            code = a.SegSegInt(a, b, c, d, this.p, q);
            return code;
        }

        /* Draws segments
         */
        public void DrawSegments(System.Drawing.Graphics g, int w, int h)
        {
            if (list.n == 0)
                System.Diagnostics.Debug.WriteLine("No drawing is possible.");
            else
            {
                cVertex v1 = list.head;

                do
                {
                    v1 = v1.NextVertex;
                } while (v1 != list.head.PrevVertex);
            }
        }

        /* Draws Intersection
         */
        public void DrawInters(System.Drawing.Graphics g, int w, int h)
        {
        }
    }
}