using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;

namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static List<List<Curve>> FindEndSegments(List<Line> streetSegments)
        {
            List<Line> endLines = new List<Line>();
            List<Line> midLines = new List<Line>();

            double tol = 0.01;

            for (int i = 0; i < streetSegments.Count; i++)
            {
                Line this_line = streetSegments[i];
                List<Line> this_list = new List<Line>(streetSegments);
                this_list.RemoveAt(i);
                List<Point3d> endPoints = EndPointSet(this_list, 0.01);

                bool EndLine = !(Engine.Geometry.Query.ContainsWithinTolerance(endPoints, this_line.To, tol) && Engine.Geometry.Query.ContainsWithinTolerance(endPoints, this_line.From, tol));

                if (EndLine)
                    endLines.Add(this_line);
                else
                    midLines.Add(this_line);
            }

            List<Curve> out_endCurves = endLines.Select(l => (Curve)l.ToNurbsCurve()).ToList();

            List<Curve> midCurves = midLines.Select(l => (Curve)l.ToNurbsCurve()).ToList();
            List<Curve> out_midCurves = Curve.JoinCurves(midCurves).ToList();

            List<List<Curve>> dividedStreetSegments = new List<List<Curve>>();
            dividedStreetSegments.Add(out_midCurves);
            dividedStreetSegments.Add(out_endCurves);

            return dividedStreetSegments;           

        }

        //====================================================================//  

    }
}


