using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class House
    {
        public string Type { get; set; } = "";
        public Brep HouseGeom { get; set; } = new Brep();
        public int MinAmount { get; set; } = 0;
        public int MaxAmount { get; set; } = 999;
        public int Offset { get; set; } = 0;

    }

    //====================================================================

}
