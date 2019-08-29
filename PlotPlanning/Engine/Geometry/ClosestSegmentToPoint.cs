using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Query
    {
        public static Line ClosestSegmentToPoint(Point3d pt, Polyline pline)
        {
            List<Line> segments = pline.GetSegments().ToList();
            List<double> closestPtsDist = new List<double>();

            foreach (Line l in segments)
            {
                double dist = l.ClosestPoint(pt, true).DistanceTo(pt);
                closestPtsDist.Add(dist);
            }

            int idx = closestPtsDist.IndexOf(closestPtsDist.Min());
            Line accessLine = segments[idx];
            return accessLine;
        }

        //====================================================================//

    }
}
