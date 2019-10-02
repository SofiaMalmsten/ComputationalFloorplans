using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;

namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static List<Line> ConnectSubgraphs(List<List<Line>> subgraphs)
        {
            List<Line> connectedGraph = subgraphs.SelectMany(x => x).ToList();
            double tol = PlotPlanning.ObjectModel.Tolerance.Distance; 
            int nrBranches = subgraphs.Count;

            if (nrBranches == 2)
            {
                for (int i = 0; i < nrBranches - 1; i++)
                {
                    List<Line> branch_1 = subgraphs[i];
                    List<Line> branch_2 = subgraphs[i + 1]; 
                    List<Point3d> pts_1 = EndPointSet(branch_1, tol);
                    List<Point3d> pts_2 = EndPointSet(branch_2, tol);

                    List<Point3d> closest_pair = ClosestPair(pts_1, pts_2);

                    connectedGraph.Add(new Line(closest_pair[0], closest_pair[1]));
                }
            }

            else if (nrBranches > 2)
            {
                for (int i = 0; i < nrBranches; i++)
                {
                    List<Line> branch_1 = subgraphs[i];
                    List<double> dists = new List<double>();
                    List<List<Point3d>> possible_pairs = new List<List<Point3d>>();

                    for (int j = 0; j < nrBranches; j++)
                    {
                        if (i != j)
                        {
                            List<Line> branch_2 = subgraphs[j];
                            List<Point3d> pts_1 = EndPointSet(branch_1, tol);
                            List<Point3d> pts_2 = EndPointSet(branch_2, tol);
                            List<Point3d> possible_closest_pair = ClosestPair(pts_1, pts_2);

                            dists.Add(possible_closest_pair[0].DistanceTo(possible_closest_pair[1]));
                            int current_Nr_Branches = possible_pairs.Count;
                            possible_pairs.Add(possible_closest_pair);
                        }
                    }
                    List<Point3d> closest_pair = possible_pairs[dists.IndexOf(dists.Min())];

                    connectedGraph.Add(new Line(closest_pair[0], closest_pair[1]));

                }
            }

            return connectedGraph;

        }

        //====================================================================//

        private static List<Point3d> EndPointSet(List<Line> lines, double tol)
        {
            List<Point3d> pts = new List<Point3d>();
            foreach (Line l in lines)
            {
                pts.Add(l.From);
                pts.Add(l.To);
            }
            return Point3d.CullDuplicates(pts, tol).ToList();
        }

        //====================================================================//

        private static List<Point3d> ClosestPair(List<Point3d> list_1, List<Point3d> list_2)
        {

            PointCloud p = new PointCloud(list_2);
            List<double> distances = new List<double>();
            List<Point3d> possible_points = new List<Point3d>();

            for (int i = 0; i < list_1.Count; i++)
            {
                Point3d this_point = list_1[i];
                int idx = p.ClosestPoint(this_point);
                distances.Add(this_point.DistanceTo(p.PointAt(idx)));
                possible_points.Add(p.PointAt(idx));
            }


            int minIndex = distances.IndexOf(distances.Min());

            return new List<Point3d>() { list_1[minIndex], possible_points[minIndex] };

        }



    }
}


