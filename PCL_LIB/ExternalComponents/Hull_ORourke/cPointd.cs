/*----------------------------------------------------------------------------
 * Class cPointd  -- point with double coordinates
 *
 * PrintPoint() -- prints point to the console;
 *
 *---------------------------------------------------------------------------*/


using OpenTK;
using PCLLib;
using System;

namespace PCLLib
{

    public class cPointd
    {
        public double x;
        public double y;

        public cPointd()
        {
            x = y = 0;
        }

        public cPointd(int myx, int myy)
        {
            x = myx;
            y = myy;
        }

        public void PrintPoint()
        {
            System.Diagnostics.Debug.WriteLine(" (" + x + "," + y + ")");
        }
    }
}