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
        public static List<Line> VoronoiNetwork(List<Point3d> voronoiPoints, Curve offsetBoundary)
        {
            double tol = 0.01;

            List<Curve> voroni_crvs = Voronoi(voronoiPoints);
            List<Curve> split_voronoi = new List<Curve>();

            foreach (Curve c in voroni_crvs)
            {
                CurveIntersections crv_intersections = Intersection.CurveCurve(offsetBoundary, c, tol, tol);

                if (crv_intersections.Count == 0)
                    split_voronoi.Add(c);
                else
                {
                    List<double> ts = new List<double>();
                    for (int i = 0; i < crv_intersections.Count; i++)
                    {
                        IntersectionEvent ie = crv_intersections[i];
                        ts.Add(ie.ParameterB);
                    }
                    split_voronoi.AddRange(c.Split(ts));
                }
            }

            Engine.Geometry.Adjust.Reparametrize(split_voronoi);
            List<Curve> trimed_voronoi = split_voronoi.Where(crv => offsetBoundary.Contains(crv.PointAt(0.5), Plane.WorldXY, tol) == PointContainment.Inside).ToList();

            List<Line> trimed_voronoi_lines = new List<Line>();
            foreach (Curve c in trimed_voronoi)
            {
                Polyline p = null;
                bool success = c.TryGetPolyline(out p);
                if (success)
                    trimed_voronoi_lines.AddRange(p.GetSegments());
            }

            List<Line> culled_line_list = Engine.Geometry.Adjust.CullDuplicates(trimed_voronoi_lines);
            return culled_line_list;

        }

        //====================================================================//

        private static List<Curve> Voronoi(List<Point3d> pts)
        {
            ComponentFunctionInfo voronoi = Components.FindComponent("Voronoi");

            object[] arguments = new object[]
              {(object) pts,
              };

            string[] warnings;
            object[] result = voronoi.Evaluate(arguments, false, out warnings);

            IList<object> resultList = (IList<object>)result[0];
            List<Curve> crv_list = resultList.Select(x => (Curve)x).ToList();

            return crv_list;
        }
    }
}


