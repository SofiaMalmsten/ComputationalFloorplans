using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


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
        public static List<Polyline> CurvesToPolylines(this Curve[] crvList)
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
    }

}
