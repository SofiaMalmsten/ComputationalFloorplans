using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
    
        public static List<Line> SegmentBounds(Polyline siteBound, Rectangle3d rectangle, int seed)
        {

            Random r = new Random(seed);

            //Check if clockwise
            if (!PlotPlanning.Methods.Calculate.IsClockwise(siteBound, new Vector3d(0, 0, -1)))
            {
                siteBound.Reverse();
            }

            List<double> lengths = new List<double>();
            List<Line> segments = new List<Line>();

            double segmentWidth = rectangle.Width;
            double segmentHeight = rectangle.Height;

            double shortestSegm = Math.Min(segmentWidth, segmentHeight);

            foreach (var segm in siteBound.GetSegments())
            {
                    if (segm.Length > shortestSegm*2)
                    {
                        segments.Add(segm);
                    }

            }

            IEnumerable<Line> shuffledSegments = PlotPlanning.Methods.Generate.Shuffle(segments, new Random(r.Next()));

            return shuffledSegments.ToList();
        }
    }

    //====================================================================

}
