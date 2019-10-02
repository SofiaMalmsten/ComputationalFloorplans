using System;
using System.Collections.Generic;
using Rhino.Geometry; 

namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static List<List<Line>> FindSubgraphs(List<Line> voronoiNetwork)
        {
            List<List<Line>> subGraphs = new List<List<Line>>();
            int nrLines = voronoiNetwork.Count;
            int nrCheckedLines = 0;
            double tol = PlotPlanning.ObjectModel.Tolerance.Distance;

            for (int i = 0; i < voronoiNetwork.Count; i++)
            {
                if (nrCheckedLines != voronoiNetwork.Count)
                {   
                    List<Line> connectedLines = new List<Line>();
                    List<Line> lines_to_check = new List<Line>();

                    lines_to_check.Add(voronoiNetwork[i]);
                    connectedLines.Add(voronoiNetwork[i]);

                    while (lines_to_check.Count != 0)
                    {
                        Line this_line = lines_to_check[0];
                        lines_to_check.RemoveAt(0);

                        foreach (Line l in voronoiNetwork)
                        {
                            if (!this_line.Equals(l))
                            {
                                if (Engine.Geometry.Query.WithinTolerance(l.To, this_line.To, tol) || Engine.Geometry.Query.WithinTolerance(l.From, this_line.To, tol) ||
                                    Engine.Geometry.Query.WithinTolerance(l.To, this_line.From, tol) || Engine.Geometry.Query.WithinTolerance(l.From, this_line.From, tol))
                                {
                                    if (!connectedLines.Contains(l))
                                    {
                                        connectedLines.Add(l);
                                        lines_to_check.Add(l);
                                    }

                                }
                            }
                        }
                    }

                    bool graphExists = false;
                    for (int j = 0; j < subGraphs.Count; j++)
                    {
                        if (Engine.Base.question.ListEqualsIgnoreOrder(subGraphs[j], connectedLines))
                        {
                            graphExists = true;
                        }
                    }

                    if (!graphExists)
                    {
                        nrCheckedLines += connectedLines.Count;
                        subGraphs.Add(connectedLines);
                    }
                }
            }
            return subGraphs;
        }
    }

    //====================================================================//


}


