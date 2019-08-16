using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class SingleFamily
    {
        public string Type { get; set; } = "";
        public bool HasCarPort { get; set; } = false;
        public Rectangle3d gardenBound { get; set; } = new Rectangle3d();
        public Brep houseGeom { get; set; } = new Brep();
        public Vector3d orientation { get; set; } = new Vector3d();
        public Point3d accessPoint { get; set; } = new Point3d();
        public int MinAmount { get; set; } = 0;
        public int MaxAmount { get; set; } = 999;
        public int Offset { get; set; } = 0;

    }

    //====================================================================

}
