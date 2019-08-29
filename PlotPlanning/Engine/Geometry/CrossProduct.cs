using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        //====================================================================//        
        public static Vector3d CrossProduct(this Vector3d v1, Vector3d v2)
        {
            return new Vector3d { X = v1.Y * v2.Z - v1.Z * v2.Y, Y = v1.Z * v2.X - v1.X * v2.Z, Z = v1.X * v2.Y - v1.Y * v2.X };
        }

        //====================================================================//    
    }
}
