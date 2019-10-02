using System;
using System.Collections.Generic;
using PlotPlanning.ObjectModel;
using Rhino.Geometry;
using Rhino.NodeInCode;
using System.Linq;
using Rhino.Geometry.Intersect; 

namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static List<Line> CullSegments(List<Line> segments, Interval wantedInterval)
        {
            List<Line> outLines = new List<Line>();
            double tol = 0.01;

            for (int i = 0; i < segments.Count; i++)
            {
                Line this_line = segments[i];
                List<Line> this_list = new List<Line>(segments);
                this_list.RemoveAt(i);
                List<Point3d> endPoints = EndPointSet(this_list, 0.01);

                bool addLine = false;
                bool notEndLine = Engine.Geometry.Query.ContainsWithinTolerance(endPoints, this_line.To, tol) && Engine.Geometry.Query.ContainsWithinTolerance(endPoints, this_line.From, tol);

                if (this_line.Length >= wantedInterval.Min && this_line.Length <= wantedInterval.Max)
                    addLine = true;

                else if (notEndLine)
                    addLine = true;

                if (addLine)
                    outLines.Add(this_line);
            }

            return outLines;
        }

        //====================================================================//       

    }
}


