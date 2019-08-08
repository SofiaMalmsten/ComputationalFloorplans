﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static List<Point3d> GetControlPoints(this Polyline pLine)
        {
            var pts = new List<Point3d>();
            pts = pLine.ToList();
            return pts;
        }

    }
}

    //====================================================================