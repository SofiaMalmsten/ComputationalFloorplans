using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        public static Vector3d CreateVector(this Point3d a, Point3d b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double dz = b.Z - a.Z;

            return new Vector3d(dx, dy, dz);
        }

        //====================================================================//
    }
}
