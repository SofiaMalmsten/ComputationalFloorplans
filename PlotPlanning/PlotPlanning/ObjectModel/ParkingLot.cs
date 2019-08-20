using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class ParkingLot
    {
        public string Type { get; set; } = "";
        public Curve ParkingBound { get; set; } = new PolylineCurve();
        public int Floors { get; set; } = 0;

    }

    //====================================================================

}
