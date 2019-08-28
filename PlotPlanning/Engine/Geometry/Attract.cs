using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.Engine.Base;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Adjust
    {
        //TODO: Only works for points in the XY plane - add plane as input?
        public static List<Point3d> AttractTo(this List<Point3d> topoPts, List<Point3d> attractorPts, List<double> possibleValues)
        {
            double displ;
            double valueToCheck;
            List<Point3d> movePts = new List<Point3d>();

            for (int i = 0; i < attractorPts.Count; i++)
            {
                List<Point3d> refPts = new List<Point3d>();
                if (i == 0)
                    displ = 0;
                else
                {
                    valueToCheck = topoPts[i].Z - attractorPts[i].Z;
                    displ = Modify.ClosestValue(valueToCheck, possibleValues);
                }

                Point3d currPt = new Point3d(attractorPts[i].X, attractorPts[i].Y, attractorPts[i].Z + displ);

                //Project all the points in the planePts to the new reference plane. It doesnt matter that we add the value in z.
                for (int k = 0; k < attractorPts.Count; k++)
                {
                    //refPts.Add(currPt);
                    //Point3d p = planePts[k];
                    //p.Z= currPt.Z;


                    attractorPts[k] = new Point3d(attractorPts[k].X, attractorPts[k].Y, currPt.Z);
                }
                //planePts = refPts;
                movePts.Add(currPt);
            }

            return movePts;
        }
    }    
}
