using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Calculate
    {
        //TODO: Only works for points in the XY plane - add plane as input?
        public static Polyline ConvexHull(List<Point3d> points)
        {
            List<Point3d> hull = new List<Point3d>();
            hull.Add(points[0]);

            for (int x = 1; x < points.Count; x++)
            {
                if (hull[0].X > points[x].X)
                    hull[0] = points[x];
                else if (hull[0].X == points[x].X)
                {
                    if (hull[0].Y > points[x].Y)
                        hull[0] = points[x];
                }
            }

            Point3d nextPt = new Point3d();
            int counter = 0;
            while (counter < hull.Count)
            {
                nextPt = NextHullPoint(points, hull[counter]);
                if (nextPt != hull[0])
                    hull.Add(nextPt);
                counter++;
            }

            hull.Add(hull[0]);

            Polyline hullBoundary = new Polyline(hull) ;
            return hullBoundary;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Point3d NextHullPoint(List<Point3d> points, Point3d currentPt)
        {
            int right = -1;
            int none = 0;

            Point3d nextPt = currentPt;
            int t;
            foreach (Point3d pt in points)
            {
                t = ((nextPt.X - currentPt.X) * (pt.Y - currentPt.Y) - (pt.X - currentPt.X) * (nextPt.Y - currentPt.Y)).CompareTo(0);
                if (t == right || t == none && currentPt.DistanceTo(pt) > currentPt.DistanceTo(nextPt))
                    nextPt = pt;
            }

            return nextPt;
        }

        /***************************************************/
    }
}
