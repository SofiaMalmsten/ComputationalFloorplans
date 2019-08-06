﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class House
    {
        public string Type { get; set; } = "";
        public bool HasCarPort { get; set; } = false;
        public Rectangle3d gardenBound { get; set; } = new Rectangle3d();
        public Brep houseGeom { get; set; } = new Brep();
        public Vector3d orientation { get; set; } = new Vector3d();
        public Point3d accessPoint { get; set; } = new Point3d();

    }

    //====================================================================

}
