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
        public static List<Point3d> SnapToTopo(List<Point3d> planePts, List<Point3d> topoPts, List<double> possibleValues, ref object A)
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


            // <Custom additional code> 

            //========================================================
            //Get Closest Value
            //========================================================
            public static double getClosestValue(double valueToCheck, List<double> possibleValues)
            {
                double displ = 0;
                for (int i = 0; i < possibleValues.Count - 1; i++)
                {
                    if (valueToCheck > possibleValues[i] && valueToCheck < possibleValues[i + 1])
                    {
                        double halfDistance = (possibleValues[i + 1] - possibleValues[i]) / 2;
                        double difference = possibleValues[i] + halfDistance;
                        if (valueToCheck < difference)
                        {
                            displ = possibleValues[i];
                        }
                        else
                        {
                            displ = possibleValues[i + 1];
                        }
                    }
                    else if (valueToCheck >= possibleValues.Max())
                    {
                        displ = possibleValues.Max();
                    }
                    else if (valueToCheck <= possibleValues.Min())
                    {
                        displ = possibleValues.Min();
                    }
                }
                return displ;
            }
        }
    }
