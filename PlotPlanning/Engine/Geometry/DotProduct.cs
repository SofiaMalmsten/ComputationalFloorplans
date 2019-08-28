using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        /***************************************************/
        public static double DotProduct(this Vector3d a, Vector3d b)
        {
            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }

        /***************************************************/
    }
}
