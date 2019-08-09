using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.ObjectModel; 

namespace PlotPlanning.Methods
{
    public static partial class Calculate
    {
        /***************************************************/
        public static Vector3d Clone(this Vector3d vector)
        {
            return new Vector3d { X = vector.X, Y = vector.Y, Z = vector.Z };
        }

        /***************************************************/

        public static Brep Clone(this Brep brep)
        {
            return brep.DuplicateBrep();
        }

        public static Cell Clone(this Cell cell)
        {
            return new Cell() { AvaliableSegments = cell.AvaliableSegments, BoundaryCurve = cell.BoundaryCurve, OriginalBoundary = cell.OriginalBoundary }; 

        }
    }
}
