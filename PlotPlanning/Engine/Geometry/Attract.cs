using System;
using System.Collections.Generic;
using Rhino.Geometry;
using PlotPlanning.Engine.Base;
using System.Linq;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Adjust
    {
        //TODO: Only works for points in the XY plane - add plane as input?
        public static List<Point3d> AttractTo(this List<Point3d> originalPts, List<Point3d> attractorPts, List<double> possibleValues)
        {
            double displacement;
            double valueToCheck;
            List<Point3d> movePts = new List<Point3d>();

            for (int i = 0; i < attractorPts.Count; i++)
            {
                if (i == 0)
                    displacement = 0;
                else
                {
                    valueToCheck = originalPts[i].Z - attractorPts[i].Z;
                    displacement = Modify.ClosestValue(valueToCheck, possibleValues);
                }

                Point3d currPt = new Point3d(attractorPts[i].X, attractorPts[i].Y, attractorPts[i].Z + displacement);

                //Project all the points in the planePts to the new reference plane. It doesn't matter that we add the value in z.
                for (int k = 0; k < attractorPts.Count; k++)
                    attractorPts[k] = new Point3d(attractorPts[k].X, attractorPts[k].Y, currPt.Z);

                movePts.Add(currPt);
            }
            return movePts;
        }

        //====================================================================//
        public static List<Point3d> AttractTo(this List<Point3d> originalPts, List<Point3d> attractorPts, double attractionRange)
        {
            List<Point3d> movePts = new List<Point3d>();

            for (int i = 0; i < originalPts.Count; i++)
            {
                List<double> distances = new List<double>();
                for (int j = 0; j < attractorPts.Count; j++)
                {
                    distances.Add(originalPts[i].DistanceTo(attractorPts[j]));
                }

                double minDistance = distances.Min();
                int index = distances.IndexOf(minDistance);

                if (minDistance <= attractionRange)
                    movePts.Add(attractorPts[index]); //select closest attractor point
                else
                    movePts.Add(originalPts[i]);

            }
            return movePts;
        }
        //====================================================================//
    }
}