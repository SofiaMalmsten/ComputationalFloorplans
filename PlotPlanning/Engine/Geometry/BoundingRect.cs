using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Calculate
    {
        public static Rectangle3d BoundingRect(this Polyline pLine)
        {
            List<Point3d> ptList = pLine.GetControlPoints();
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



        /***************************************************/

        public static Rectangle3d BoundingRect(this Polyline pLine, Vector3d vec)
        {
            List<Point3d> ptList = pLine.GetControlPoints();
            List<double> xVal = new List<double>();
            List<double> yVal = new List<double>();

            foreach (var pt in ptList)
            {
                xVal.Add(pt.X);
                yVal.Add(pt.Y);
            }

            Point3d maxPt = new Point3d(xVal.Max(), yVal.Max(), 0);
            Point3d minPt = new Point3d(xVal.Min(), yVal.Min(), 0);

            Plane plane = new Plane(new Point3d(0, 0, 0), new Vector3d(vec.X, vec.Y, 1));

            Rectangle3d rec = new Rectangle3d(plane, maxPt, minPt);

            return rec;
        }

    }
}
