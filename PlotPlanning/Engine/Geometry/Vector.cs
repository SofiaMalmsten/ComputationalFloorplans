using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        public static Vector3d CreateVector(this Point3d a, Point3d b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double dz = b.Z - a.Z;

            Vector3d vec = new Vector3d(dx, dy, dz);
            return vec;
        }
        /***************************************************/
    }
}
