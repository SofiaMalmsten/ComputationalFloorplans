using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
    
        public static List<Line> SegmentBounds(Polyline siteBound, Rectangle3d rectangle, double cornerRadius)
        {

            List<double> lengths = new List<double>();
            List<Line> segments = new List<Line>();

            double segmentWidth = rectangle.Width;
            double segmentHeight = rectangle.Height;

            double shortestSegm = Math.Min(segmentWidth, segmentHeight);

            foreach (var segm in siteBound.GetSegments())
            {
                if (segm.Length > shortestSegm)
                {
                    segm.Extend(cornerRadius * -1, cornerRadius * -1);
                    if (segm.Length > shortestSegm*2)
                    {
                        segments.Add(segm);
                    }
                }

            }

            return segments;
        }
    }

    //====================================================================

}
