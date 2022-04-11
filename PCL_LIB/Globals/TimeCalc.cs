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

namespace PCLLib
{
    
    public class TimeCalc
    {
        public static DateTime CurrentTime;

        public static void ShowLastTimeSpan(string name)
        {

            DateTime now = DateTime.Now;
            TimeSpan ts = now - CurrentTime;
            System.Diagnostics.Debug.WriteLine("--Duration for " + name + " : " + ts.TotalMilliseconds.ToString() + " - miliseconds");
            CurrentTime = now;
        }
        public static void ResetTime()
        {
            CurrentTime = DateTime.Now;

        }
        

    }
}
