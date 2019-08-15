using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class ParkingLot
    {
        public string type { get; set; } = "";
        public Curve parkingBound { get; set; } = new PolylineCurve();
        public int floors { get; set; } = 0;

    }

    //====================================================================

}
