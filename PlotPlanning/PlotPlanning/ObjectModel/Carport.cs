using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Carport
    {
        public Polyline GardenBound { get; set; } = new Polyline();
        public Brep CarportGeom { get; set; } = new Brep();
        public Point3d AccessPoint { get; set; } = new Point3d();

    }

    //====================================================================

}
