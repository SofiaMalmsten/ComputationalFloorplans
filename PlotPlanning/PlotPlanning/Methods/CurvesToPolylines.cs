using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq; 


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static List<Polyline> CurvesToPolylines(this List<Curve> crvList)          
        {
            List<Polyline> x = new List<Polyline>();

            foreach (Curve c in crvList)
            {
                Polyline pl = new Polyline();
                c.TryGetPolyline(out pl);
                x.Add(pl);
            }

            return x; 
        }
        public static List<Polyline> CurvesToPolylines(this Curve[] crvArray)
        {
            return CurvesToPolylines(crvArray.ToList()); 
        }

        public static List<PolylineCurve> CurvesToPolylineCurves(this List<Curve> crvList)
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
        public static List<PolylineCurve> CurvesToPolylineCurves(this Curve[] crvArray)
        {
            return CurvesToPolylineCurves(crvArray.ToList());
        }
        public static Polyline CurveToPolyline(this Curve curve)
        {
            Polyline x = new Polyline();
            curve.TryGetPolyline(out x);             
            return x;
        }
        public static PolylineCurve CurveToPolylineCurve(this Curve curve)
        {
            curve.TryGetPolyline(out Polyline x);
            return new PolylineCurve(x.GetControlPoints()); 
        }
    }

}
