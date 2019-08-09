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
        public string Method { get; set; } = "random";

       // public static Cell Copy(this Cell cell)
       // {
       //     return new Cell { AvaliableSegments = cell.AvaliableSegments, Method = cell.Method, BoundaryCurve = cell.BoundaryCurve, OriginalBoundary = cell.OriginalBoundary };
      //  }


    }

    




    //====================================================================

}
