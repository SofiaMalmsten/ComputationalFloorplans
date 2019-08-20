using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Carport
    {
        public Polyline gardenBound { get; set; } = new Polyline();
        public Brep carportGeom { get; set; } = new Brep();
        public Point3d accessPoint { get; set; } = new Point3d();

    }

    //====================================================================

}
