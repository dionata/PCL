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

namespace VoronoiFortune
{
	public class PointFortune
	{
		public double X, Y;
        public int IndexInList;

        public PointFortune()
        {
           
        }

		public PointFortune (double x, double y, int indexInList)
		{
            X = x;
            Y = y;
            IndexInList = indexInList;
		}
		
       
	}
	
	
	public class Halfedge
	{
        public PointFortune vertex;
		public Halfedge ELleft, ELright;
        public Halfedge PQnext;
        public EdgeF ELedge;
		public bool deleted;
		public int ELpm;
		
		public double ystar;
		
		
		public Halfedge ()
		{
			PQnext = null;
		}
	}
    public class EdgeF
    {
        public double a = 0, b = 0, c = 0;
        public PointFortune[] ep;
        public PointFortune[] reg;
        public int edgenbr;

        public EdgeF()
        {
            ep = new PointFortune[2];
            reg = new PointFortune[2];
        }
    }
	
	
	public class EdgeFortune
	{
        public double x1, y1, x2, y2;
		public int PointIndex1, PointIndex2;
	}
	
	public class PointFortuneSorterYX : IComparer<PointFortune>
	{
		public int Compare ( PointFortune p1, PointFortune p2 )
		{
			
			if ( p1.Y < p2.Y )	return -1;
			if ( p1.Y > p2.Y ) return 1;
			if ( p1.X < p2.X ) return -1;
			if ( p1.X > p2.X ) return 1;
			return 0;
		}
	}
}
