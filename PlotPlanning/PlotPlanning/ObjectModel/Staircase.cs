using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Staircase
    {
        public Curve Footprint { get; set; } = new PolylineCurve();
        public List<Point3d> AccessPoints { get; set; } = new List<Point3d>();
        public int Floors { get; set; } = 1;

    }

    //====================================================================

}
