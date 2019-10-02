using System;
using Rhino.Geometry;
using System.Collections.Generic;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Query
    { 
        public static bool WithinTolerance(Point3d pt1, Point3d pt2, double tol)
        {
            return pt1.DistanceTo(pt2) < tol;
        }

        //====================================================================//

        public static bool ContainsWithinTolerance(List<Point3d> pts, Point3d pt, double tol)
        {
            return pts.Any(x => WithinTol(x, pt, tol));
        }

        //====================================================================//

    }
}
