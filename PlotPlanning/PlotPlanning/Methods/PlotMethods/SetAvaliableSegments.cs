using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using isc = Rhino.Geometry.Intersect;
using nic = Rhino.NodeInCode.Components;
using PlotPlanning.ObjectModel;



namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static void SetAvaliableSegments(this Cell cell, List<Curve> roads = null)
        {
            if (cell.Method == "roads") //select line on road
            {
                List<Line> lines = cell.BoundaryCurve.GetSegments().ToList();
                List<Line> posLines = new List<Line>();

                foreach (Line l in lines)
                {
                    foreach (Curve road in roads)
                    {
                        isc.CurveIntersections i = isc.Intersection.CurveCurve(road, l.ToNurbsCurve(), DistanceTol(), DistanceTol());
                        if (i.Count > 0)
                        {
                            isc.IntersectionEvent ie = i[0];
                            if (ie.IsOverlap)
                            {
                                cell.AvaliableSegments.Add(l);
                                goto nextL;

                            }
                        }
                    }
                nextL:
                    int a = 1; //TODO: make it nicer, this is just to break out of the nested for loop when l is added 
                }
                //return cell;
            }
            //else return cell; 
        }       

        //====================================================================

    }
}
