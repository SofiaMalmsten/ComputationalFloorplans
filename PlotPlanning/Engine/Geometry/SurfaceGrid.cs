using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Rhino.Collections; 

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Query
    {
        //====================================================================//        
        public static List<Point3d> SurfaceGrid(Surface surface, int uCount, int vCount)
        {
            Interval uDomain = surface.Domain(0);
            Interval vDomain = surface.Domain(1);

            double deltaU = (uDomain.Max - uDomain.Min)/(uCount-1);
            double deltaV = (vDomain.Max - vDomain.Min)/(vCount-1);

            List<double> uValues = new List<double>();
            List<double> vValues = new List<double>();

            for (int i = 0; i < uCount; i++)
                uValues.Add(uDomain.Min + i * deltaU);

            for (int i = 0; i < vCount; i++)
                vValues.Add(vDomain.Min + i * deltaV);

            List<Point3d> ptList = new List<Point3d>();
            foreach  (double u in uValues)
            {
                foreach (double v in vValues)
                {
                    ptList.Add(surface.PointAt(u, v)); 
                }
            }
            return ptList; 

        }

        //====================================================================//    
    }
}
