using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Query
    {
        public static List<Point3d> GetControlPoints(this Polyline polyline)
        {
            return polyline.ToList();
        }
    }

    //====================================================================//
}