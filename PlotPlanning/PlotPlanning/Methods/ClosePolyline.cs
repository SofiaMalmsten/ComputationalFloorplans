using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using pp = PlotPlanning.Methods.Generate; 


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static Polyline ClosePolyline(this Polyline pl)
        {

            if (pl.IsClosed) { return pl; }
            else
            {
                List<Point3d> ptList = pl.ToList();
                ptList.Add(ptList.First());
                return new Polyline(ptList);
            }
        }

        public static List<Polyline> ClosePolyline(this List<Polyline> pls)
        {
            List<Polyline> closed_list = new List<Polyline>();
            foreach (Polyline pl in pls)
            {
                closed_list.Add(pl.ClosePolyline()); 
            }
            return closed_list; 
        }
    }
}

    //====================================================================
