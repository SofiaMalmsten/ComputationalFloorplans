﻿using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        public static double SquareDistance(this Point3d a, Point3d b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            double dz = a.Z - b.Z;
            return dx * dx + dy * dy + dz * dz;
        }
    }

    //====================================================================//
}
