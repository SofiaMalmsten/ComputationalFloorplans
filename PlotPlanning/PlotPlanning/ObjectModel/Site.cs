using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Site
    {
        public Point3d AccessPoint { get; set; } = new Point3d();
        public Curve Boundary { get; set; } = new PolylineCurve();
        public Brep Topography { get; set; } = new Brep();

    }

    //====================================================================

}
