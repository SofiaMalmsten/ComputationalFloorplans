using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq; 


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
      
        //====================================================================
        public static List<PolylineCurve> ToPolylineCurves(this List<Curve> crvList)
        {
            List<PolylineCurve> x = new List<PolylineCurve>();

            foreach (Curve c in crvList)
            {
                Polyline pl = new Polyline();
                c.TryGetPolyline(out pl);
                x.Add(new PolylineCurve(pl.GetControlPoints()));
            }
            return x;
        }

        //====================================================================
        public static List<PolylineCurve> ToPolylineCurves(this Curve[] crvArray)
        {
            return ToPolylineCurves(crvArray.ToList());
        }

        //====================================================================
      
        public static PolylineCurve ToPolylineCurve(this Curve curve)
        {
            curve.TryGetPolyline(out Polyline x);
            return new PolylineCurve(x.GetControlPoints()); 
        }

        //====================================================================
    }

}
