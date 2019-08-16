using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Carport
    {
        public Rectangle3d gardenBound { get; set; } = new Rectangle3d();
        public Brep carportGeom { get; set; } = new Brep();
        public Point3d accessPoint { get; set; } = new Point3d();

    }

    //====================================================================

}
