using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
    
        public static List<Line> SegmentBounds(Polyline siteBound, Rectangle3d rectangle, int seed, double minAmount)
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
                    if (segm.Length > shortestSegm*minAmount)
                    {
                        segments.Add(segm);
                    }

            }

            //IEnumerable<Line> shuffledSegments = PlotPlanning.Methods.Generate.Shuffle(segments, new Random(r.Next()));
            IEnumerable<Line> shuffledSegments = segments.OrderBy(x => x.Length).ToList();

            return shuffledSegments.ToList();
        }

        public static List<Line> SegmentBounds(Polyline siteBound, Polyline pline, Point3d pt, int seed, double minAmount)
        {

            Random r = new Random(seed);

            //Check if clockwise
            if (!PlotPlanning.Methods.Calculate.IsClockwise(siteBound, new Vector3d(0, 0, -1)))
            {
                siteBound.Reverse();
            }

            List<double> lengths = new List<double>();
            List<Line> segments = new List<Line>();

            double gardenLength = PlotPlanning.Methods.Calculate.GetAccessLine(pt, pline).Length;

            foreach (var segm in siteBound.GetSegments())
            {
                if (segm.Length > gardenLength * minAmount)
                {
                    segments.Add(segm);
                }

            }

            //IEnumerable<Line> shuffledSegments = PlotPlanning.Methods.Generate.Shuffle(segments, new Random(r.Next()));
            IEnumerable<Line> shuffledSegments = segments.OrderBy(x => x.Length).ToList();

            return shuffledSegments.ToList();
        }
    }

    //====================================================================

}
