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

namespace PCLLib
{
  public class floatVector : IComparable<floatVector>
  {
    public float x;
    public float y;
    public float z;

    public floatVector()
    {
      this.x = 0.0f;
      this.y = 0.0f;
      this.z = 0.0f;
    }

    public floatVector(float xComponent, float yComponent, float zComponent)
    {
      this.x = xComponent;
      this.y = yComponent;
      this.z = zComponent;
    }

    public floatVector(floatVector v)
    {
      this.x = v.x;
      this.y = v.y;
      this.z = v.z;
    }

    public static floatVector operator +(floatVector v1, floatVector v2)
    {
      return new floatVector()
      {
        x = v1.x + v2.x,
        y = v1.y + v2.y,
        z = v1.z + v2.z
      };
    }

    public static floatVector operator -(floatVector v1, floatVector v2)
    {
      return new floatVector()
      {
        x = v1.x - v2.x,
        y = v1.y - v2.y,
        z = v1.z - v2.z
      };
    }

    public static floatVector operator *(float num, floatVector v)
    {
      return new floatVector()
      {
        x = v.x * num,
        y = v.y * num,
        z = v.z * num
      };
    }

    public static floatVector operator *(floatVector v, float num)
    {
      return new floatVector()
      {
        x = v.x * num,
        y = v.y * num,
        z = v.z * num
      };
    }

    public static floatVector operator /(float num, floatVector v)
    {
      return new floatVector()
      {
        x = v.x / num,
        y = v.y / num,
        z = v.z / num
      };
    }

    public static floatVector operator /(floatVector v, float num)
    {
      return new floatVector()
      {
        x = v.x / num,
        y = v.y / num,
        z = v.z / num
      };
    }

    public override string ToString()
    {
      return "(" + this.x.ToString() + ";" + this.y.ToString() + ";" + this.z.ToString() + ")";
    }

    public int CompareTo(floatVector v)
    {
      return (double) this.x == (double) v.x && (double) this.y == (double) v.y && (double) this.z == (double) v.z ? 0 : 1;
    }

    public static float DotProduct(floatVector v1, floatVector v2)
    {
      return (float) ((double) v1.x * (double) v2.x + (double) v1.y * (double) v2.y + (double) v1.z * (double) v2.z);
    }

    public static floatVector CrossProduct(floatVector v1, floatVector v2)
    {
      return new floatVector()
      {
        x = (float) ((double) v1.y * (double) v2.z - (double) v2.y * (double) v1.z),
        y = (float) (-(double) v1.x * (double) v2.z + (double) v2.x * (double) v1.z),
        z = (float) ((double) v1.x * (double) v2.y - (double) v2.x * (double) v1.y)
      };
    }

    public float norm()
    {
      return (float) Math.Sqrt((double) floatVector.DotProduct(this, this));
    }

    public void normalize()
    {
      float num = 1f / this.norm();
      this.x *= num;
      this.y *= num;
      this.z *= num;
    }
  }
}
