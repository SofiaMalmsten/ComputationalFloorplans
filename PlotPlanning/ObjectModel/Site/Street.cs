using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Street
    {
        public Curve CentreCurve { get; set; } = new PolylineCurve();
        public double Width { get; set; } = 1;

    }

    //====================================================================

}
