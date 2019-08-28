using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Appartment
    {
        public Curve Footprint { get; set; } = new PolylineCurve();
        public Point3d AccessPoint { get; set; } = new Point3d();
        public int Rooms { get; set; } = 1;

    }

    //====================================================================

}
