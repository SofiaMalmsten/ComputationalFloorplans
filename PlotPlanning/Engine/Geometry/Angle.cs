using System;
using Rhino.Geometry;
using System.Collections.Generic; 

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Adjust
    {
        public static List<Line> CullDuplicates(List<Line> lines)
        {
            List<Line> culledList = new List<Line>();
            double tol = 0.01;

            if (lines.Count > 0)
            {
                culledList.Add(lines[0]);

                foreach (Line check_line in lines)
                {
                    bool addLine = true;
                    foreach (Line added_line in culledList)
                    {
                        if (WithinTol(check_line.To, added_line.To, tol) && WithinTol(check_line.From, added_line.From, tol))
                        {
                            addLine = false;
                            break;
                        }

                        else if (WithinTol(check_line.From, added_line.To, tol) && WithinTol(check_line.To, added_line.From, tol))
                        {
                            addLine = false;
                            break;
                        }
                    }
                    if (addLine) culledList.Add(check_line);
                }
            }
            return culledList;
        }

        //====================================================================//

        private static bool WithinTol(Point3d pt1, Point3d pt2, double tol)
        {
            return pt1.DistanceTo(pt2) < tol;
        }

        //====================================================================//

    }
}
