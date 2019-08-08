using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Calculate
    {
        //TODO: Only works for points in the XY plane - add plane as input?
        public static List<Point3d> SnapToTopo(List<Point3d> planePts, List<Point3d> topoPts, List<double> possibleValues)
        {
            double displ;
            double valueToCheck;
            List<Point3d> movePts = new List<Point3d>();

            for (int i = 0; i < planePts.Count; i++)
            {
                List<Point3d> refPts = new List<Point3d>();
                if (i == 0)
                {
                    displ = 0;
                }
                else
                {
                    valueToCheck = topoPts[i].Z - planePts[i].Z;
                    displ = getClosestValue(valueToCheck, possibleValues);
                }

                Point3d currPt = new Point3d(planePts[i].X, planePts[i].Y, planePts[i].Z + displ);

                //Project all the points in the planePts to the new reference plane. It doesnt matter that we add the value in z.
                for (int k = 0; k < planePts.Count; k++)
                {
                    //refPts.Add(currPt);
                    //Point3d p = planePts[k];
                    //p.Z= currPt.Z;


                    planePts[k] = new Point3d(planePts[k].X, planePts[k].Y, currPt.Z);
                }
                //planePts = refPts;
                movePts.Add(currPt);
            }


            return movePts;
        }
    }    
}
