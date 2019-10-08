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
        public static (List<Point3d>, Curve) VoronoiPoints(Curve siteBoundary, List<Point3d> accessPoints, double offset, int density, int seed)
        {
            double tol = PlotPlanning.ObjectModel.Tolerance.Distance;
            double radius = siteBoundary.GetLength() * 0.04; // 4% of the total length seems to work fine

            List<Curve> offset_crvs = siteBoundary.Offset(Plane.WorldXY, -offset, tol, CurveOffsetCornerStyle.Sharp).ToList();
            Curve offset_crv = offset_crvs.OrderByDescending(c => c.GetLength()).FirstOrDefault();

            List<Point3d> voroni_pts = new List<Point3d>();
            List<Curve> remove_crvs = new List<Curve>();

            foreach (Point3d p in accessPoints)
            {
                double t; 
                offset_crv.ClosestPoint(p, out t);
                Point3d closestPoint = offset_crv.PointAt(t); 

                Curve c = new ArcCurve(new Circle(Plane.WorldXY, closestPoint, radius));
                remove_crvs.Add(c);
                CurveIntersections crv_intersections = Intersection.CurveCurve(offset_crv, c, tol, tol);

                for (int i = 0; i < crv_intersections.Count; i++)
                {
                    IntersectionEvent ie = crv_intersections[i];
                    if (ie.PointA != null) voroni_pts.Add(ie.PointA);
                }
            }

            Curve trimed_crv = new PolylineCurve();
            try { trimed_crv = Curve.CreateBooleanDifference(offset_crv, remove_crvs, tol)[0]; }
            catch { trimed_crv = offset_crv; }

            Polyline mesh_pl = new Polyline();
            trimed_crv.ToPolyline(tol, 1, 5, double.PositiveInfinity).TryGetPolyline(out mesh_pl);
            if (!mesh_pl.IsClosed) mesh_pl.Add(mesh_pl.First);

            Mesh population_mesh = Mesh.CreateFromClosedPolyline(mesh_pl);
            voroni_pts.AddRange(PopulateGeometry(population_mesh, voroni_pts, density, seed));


            return (voroni_pts, offset_crv);
            }

        //====================================================================//

        private static List<Point3d> PopulateGeometry(Mesh mesh, List<Point3d> existing_pts, int nrPts, int seed)
        {
            List<Point3d> points = existing_pts.Select(x => new Point3d(x)).ToList();
            ComponentFunctionInfo pop_geometry = Components.FindComponent("PopulateGeometry");

            object[] arguments = new object[]
              {(object) mesh,
             (object) nrPts,
             (object) seed,
             existing_pts.Cast<object>().ToArray()};

            string[] warnings;
            object[] result = pop_geometry.Evaluate(arguments, false, out warnings);

            IList<object> resultList = (IList<object>)result[0];
            List<Point3d> pt_list = resultList.Select(x => (Point3d)x).ToList();
            return pt_list;
        }

    }
}


