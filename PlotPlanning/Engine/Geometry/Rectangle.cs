using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Adjust
    {
        public static Rectangle3d Rectangle(Plane pl, double u, double v)
        {
            Interval uInt = new Interval(-u / 2, u / 2);
            Interval vInt = new Interval(-v / 2, v / 2);
            Rectangle3d rec = new Rectangle3d(pl, uInt, vInt);

            return rec;
        }

        //====================================================================//
    }
}


