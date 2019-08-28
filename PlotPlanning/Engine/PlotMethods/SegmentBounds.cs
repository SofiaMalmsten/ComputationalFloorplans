using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.Engine.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
    
        public static List<Line> SegmentBounds(Polyline siteBound, Rectangle3d rectangle, int seed, double minAmount)
        {

            Random r = new Random(seed);

            //Check if clockwise
            if (!Query.IsClockwise(siteBound, new Vector3d(0, 0, -1)))
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

        public static List<Line> SegmentBounds(Polyline siteBound, ObjectModel.SingleFamily house)
        {

            double minAmount = house.MinAmount;
            Polyline pline = house.GardenBound;
            Point3d pt = house.AccessPoint;

            //Check if clockwise
            if (!Query.IsClockwise(siteBound, new Vector3d(0, 0, -1)))
            {
                siteBound.Reverse();
            }

            List<double> lengths = new List<double>();
            List<Line> segments = new List<Line>();

            double gardenLength = Query.ClosestSegmentToPoint(pt, pline).Length;

            foreach (var segm in siteBound.GetSegments())
            {
                if (segm.Length > gardenLength * minAmount)
                {
                    segments.Add(segm);
                }

            }

            IEnumerable<Line> shuffledSegments = segments.OrderBy(x => x.Length).ToList();

            return shuffledSegments.ToList();
        }

        public static List<Line> SegmentBounds(Polyline siteBound, ObjectModel.MultiFamily house)
        {

            double minAmount = house.MinAmount;
            Polyline pline = house.GardenBound;
            Point3d pt = house.AccessPoint;

            //Check if clockwise
            if (!Query.IsClockwise(siteBound, new Vector3d(0, 0, -1)))
            {
                siteBound.Reverse();
            }

            List<double> lengths = new List<double>();
            List<Line> segments = new List<Line>();

            double gardenLength = Query.ClosestSegmentToPoint(pt, pline).Length;

            foreach (var segm in siteBound.GetSegments())
            {
                if (segm.Length > gardenLength * minAmount)
                {
                    segments.Add(segm);
                }

            }

            IEnumerable<Line> shuffledSegments = segments.OrderBy(x => x.Length).ToList();

            return shuffledSegments.ToList();
        }
    }

    //====================================================================

}
