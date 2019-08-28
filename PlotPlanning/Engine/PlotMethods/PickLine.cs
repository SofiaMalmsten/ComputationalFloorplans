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

        public static Line PickLine(this List<Line> lines, string method, Random random = null, List<Curve> roads = null, Curve originalBound = null)
        {
            if (lines.Count == 0)
            {
                return new Line();
            }

            //====================================================================

            else if (method == "random") //selects line randomly
            {
                Line line = lines[random.Next(lines.Count)];
                return line;
            }


            //====================================================================

            else if (method == "shortest") //selects shortest line 
            {
                return lines.OrderBy(x => x.Length).ToList()[0];
            }

            //====================================================================

            else if (method == "boundary") //selects line on boundary
            {
                List<Line> posLines = new List<Line>();

                foreach (Line l in lines)
                {
                    isc.CurveIntersections i = isc.Intersection.CurveCurve(originalBound, l.ToNurbsCurve(), Tolerance.Distance, Tolerance.Distance);
                    if (i.Count > 0)
                    {
                        isc.IntersectionEvent ie = i[0];
                        if (ie.IsOverlap)
                        {
                            posLines.Add(l);
                        }
                    }
                }
                return posLines[random.Next(lines.Count)];
            }
            //====================================================================

            else if (method == "roads") //select line on road
            {
                List<Line> posLines = new List<Line>();

                foreach (Line l in lines)
                {
                    foreach (Curve road in roads)
                    {
                        isc.CurveIntersections i = isc.Intersection.CurveCurve(road, l.ToNurbsCurve(), Tolerance.Distance, Tolerance.Distance);
                        if (i.Count > 0)
                        {
                            isc.IntersectionEvent ie = i[0];
                            if (ie.IsOverlap)
                            {
                                posLines.Add(l);
                                goto nextL;

                            }
                        }
                    }
                nextL:
                    int a = 1; //TODO: make it nicer, this is just to break out of the nested for loop when l is added 
                }
                if (posLines.Count != 0)
                    return posLines[random.Next(posLines.Count)];
                else
                    return new Line();
            }

            //====================================================================

            else if (method == "boundary first") //selects line on boundary first and then randomly is there are none. TODO: Make it work. :)
            {
                List<Line> posLines = new List<Line>();

                foreach (Line l in lines)
                {
                    isc.CurveIntersections i = isc.Intersection.CurveCurve(originalBound, l.ToNurbsCurve(), Tolerance.Distance, Tolerance.Distance);
                    if (i.Count > 0)
                    {
                        isc.IntersectionEvent ie = i[0];
                        if (ie.IsOverlap)
                        {
                            posLines.Add(l);
                        }
                    }
                }
                if (posLines.Count == 0)
                {
                    return lines.PickLine("random", random, roads, originalBound);
                }
                return posLines[random.Next(lines.Count)];
            }

            //====================================================================

            else if (method == "longest") //returns longest line 
            {
                return lines.OrderBy(x => x.Length).ToList()[lines.Count - 1];
            }
            else // TODO: Fix issue with try/catch in PlaceHouseRow. 
            {
                throw new NotImplementedException("The methods yo can choose from are 'shortest', 'longest', 'random', 'boundary' and 'boundary first'.");
            }
        }

        //====================================================================

    }
}
