using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        public static Rectangle3d BoundingRect(this Polyline polyline)
        {
            List<Point3d> ptList = polyline.GetControlPoints();
            List<double> xVal = new List<double>();
            List<double> yVal = new List<double>();

            foreach (var pt in ptList)
            {
                xVal.Add(pt.X);
                yVal.Add(pt.Y);
            }

            Point3d maxPt = new Point3d(xVal.Max(), yVal.Max(), 0);
            Point3d minPt = new Point3d(xVal.Min(), yVal.Min(), 0);

            Plane plane = new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));

            Rectangle3d rec = new Rectangle3d(plane, maxPt, minPt);

            return rec;
        }


        //====================================================================//

        public static Rectangle3d BoundingRect(this Polyline polyline, Vector3d vector)
        {
            List<Point3d> ptList = polyline.GetControlPoints();
            List<double> xVal = new List<double>();
            List<double> yVal = new List<double>();

            foreach (var pt in ptList)
            {
                xVal.Add(pt.X);
                yVal.Add(pt.Y);
            }

            Point3d maxPt = new Point3d(xVal.Max(), yVal.Max(), 0);
            Point3d minPt = new Point3d(xVal.Min(), yVal.Min(), 0);

            Plane plane = new Plane(new Point3d(0, 0, 0), new Vector3d(vector.X, vector.Y, 1));

            Rectangle3d rec = new Rectangle3d(plane, maxPt, minPt);

            return rec;
        }

        //====================================================================//
    }
}