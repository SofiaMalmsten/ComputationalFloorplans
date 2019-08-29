using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.Engine.Base;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Adjust
    {
        public static Vector3d Normalise(this Vector3d vector)
        {
            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;
            double d = Math.Sqrt(x * x + y * y + z * z);

            if (d == 0)
                return vector.Clone();

            return new Vector3d { X = x / d, Y = y / d, Z = z / d };
        }

        /***************************************************/
    }
}
