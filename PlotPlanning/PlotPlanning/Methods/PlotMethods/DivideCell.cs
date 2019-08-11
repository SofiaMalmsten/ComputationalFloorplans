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

        public static List<Cell> DivideCell(this Cell cell, List<Polyline> boundaries)
        {
            if (boundaries.Count == 1) return new List<Cell> { cell }; 
            
            List<Line> avaliableSegments = cell.AvaliableSegments;
            List<Cell> cells = new List<Cell>(); 
            foreach (Polyline p in boundaries)
            {
                Cell splitCell = new Cell() { BoundaryCurve = p, OriginalBoundary = cell.OriginalBoundary };
                foreach (Line l in p.GetSegments().ToList())
                {
                    foreach (Line avaliable in avaliableSegments)
                    {
                        isc.CurveIntersections i = isc.Intersection.CurveCurve(avaliable.ToNurbsCurve(), l.ToNurbsCurve(), DistanceTol(), DistanceTol());
                        if (i.Count > 0)
                        {
                            isc.IntersectionEvent ie = i[0];
                            if (ie.IsOverlap)
                            {
                                splitCell.AvaliableSegments.Add(l);
                                goto nextL;

                            }
                        }
                    }
                nextL:
                    int a = 1; //TODO: make it nicer, this is just to break out of the nested for loop when l is added 
                }
                cells.Add(splitCell); 
            }
            return cells; 
        }

    }

    //====================================================================

}

