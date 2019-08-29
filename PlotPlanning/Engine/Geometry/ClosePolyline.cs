using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Adjust
    {
        public static Polyline ClosePolyline(this Polyline pl)
        {

            if (pl.IsClosed) return pl;
            else
            {
                List<Point3d> ptList = pl.ToList();
                ptList.Add(ptList.First());
                return new Polyline(ptList);
            }
        }

        //====================================================================//
        public static List<Polyline> ClosePolyline(this List<Polyline> pls)
        {
            List<Polyline> closed_list = new List<Polyline>();
            foreach (Polyline pl in pls)
            {
                closed_list.Add(pl.ClosePolyline());
            }
            return closed_list;
        }

        //====================================================================//
    }
}


