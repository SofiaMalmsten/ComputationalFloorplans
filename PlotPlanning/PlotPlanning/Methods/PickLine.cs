using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using isc = Rhino.Geometry.Intersect;
using nic = Rhino.NodeInCode.Components;



namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static Line PickLine(this List<Line> lines, string method, Curve originalBound, Random random)
        {
            if (method == "random") //selects line randomly
            {
                Line line = lines[random.Next(lines.Count)];
                return line;
            }
            else if (method == "shortest") //selects shortest line 
            {
                return lines.OrderBy(x => x.Length).ToList()[0];
            }
            else if (method == "boundary") //selects line on boundary
            {
                originalBound.TryGetPolyline(out Polyline originalBoundPl);
                List<Line> originalSegments = originalBoundPl.GetSegments().ToList();
                List<Line> posLines = new List<Line>();

                foreach (Line l in lines)
                {
                    isc.CurveIntersections i = isc.Intersection.CurveCurve(originalBound, l.ToNurbsCurve(), DistanceTol(), DistanceTol());
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
            else if (method == "boundary first") //selects line on boundary first and then randomly is there are none
            {
                originalBound.TryGetPolyline(out Polyline originalBoundPl);
                List<Line> originalSegments = originalBoundPl.GetSegments().ToList();
                List<Line> posLines = new List<Line>();

                foreach (Line l in lines)
                {
                    isc.CurveIntersections i = isc.Intersection.CurveCurve(originalBound, l.ToNurbsCurve(), DistanceTol(), DistanceTol());
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
                    return lines.PickLine("random", originalBound, random);
                }
                return posLines[random.Next(lines.Count)];
            }
            else if(method == "longest") //returns longest line 
            {
                return lines.OrderBy(x => x.Length).ToList()[lines.Count - 1];
            }
            else
            {
                throw new NotImplementedException("The methods yo can choose from are 'shortest', 'longest', 'random', 'boundary' and 'boundary first'."); 
            }
        }

        //====================================================================

    }
}
