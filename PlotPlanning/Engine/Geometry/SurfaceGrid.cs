using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Rhino.Collections; 

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Query
    {
        //====================================================================//        
        public static Point3d[,] SurfaceGrid(Surface surface, int uCount, int vCount)
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

            Point3d[,] ptArray2d = new Point3d[uCount, vCount];

            for (int i = 0; i < uCount; i++)
            {
                for (int j = 0; j < vCount; j++)
                {
                    ptArray2d[i, j] = surface.PointAt(uValues[i], vValues[j]); 
                }
            }

            return ptArray2d; 
        }

        //====================================================================//    
    }
}
