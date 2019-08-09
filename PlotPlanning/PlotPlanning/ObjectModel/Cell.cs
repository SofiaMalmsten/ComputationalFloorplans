using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Cell
    {
        public Polyline BoundaryCurve { get; set; } = new Polyline();
        public List<Line> AvaliableSegments { get; set; } = new List<Line>(); 
        public Polyline OriginalBoundary { get; set;  } = new Polyline();
    }

    




    //====================================================================

}
