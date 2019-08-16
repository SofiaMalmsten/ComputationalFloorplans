using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Calculate
    {
        public static Line GetAccessLine(Point3d pt, Polyline pline)
        {
            

            List<Line> segments = pline.GetSegments().ToList();
            List<double> closestPtsDist = new List<double>();

            foreach (var segm in segments)
            {
                double dist = segm.ClosestPoint(pt, true).DistanceTo(pt);
                closestPtsDist.Add(dist);
            }

            int index = closestPtsDist.IndexOf(closestPtsDist.Min());
            Line accessLine = segments[index];
            return accessLine;
        }

        /***************************************************/
       
    }
}
