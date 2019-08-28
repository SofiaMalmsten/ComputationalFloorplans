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
        public static List<Polyline> ToPolylines(this List<Curve> crvList)          
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

        //====================================================================
        public static List<Polyline> ToPolylines(this Curve[] crvArray)
        {
            return ToPolylines(crvArray.ToList()); 
        }

        //====================================================================

        public static Polyline ToPolyline(this Curve curve)
        {
            Polyline x = new Polyline();
            curve.TryGetPolyline(out x);             
            return x;
        }

      
        //====================================================================
        public static Polyline ToPolyline(this PolylineCurve pCurve)
        {

            int point_count = pCurve.PointCount;
            Polyline pLine = new Polyline(point_count);
            for (int i = 0; i < pCurve.PointCount; ++i)
            {
                pLine.Add(pCurve.Point(i));
            }
            return pLine;
        }
    }

}
