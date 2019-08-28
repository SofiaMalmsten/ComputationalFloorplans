using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Query
    {
        public static bool IsInside(Curve rec, Curve bound)
        {
            Point3d origin = new Point3d(0, 0, 0);
            Vector3d normal = new Vector3d(0, 0, 1);
            Plane p = new Plane(origin, normal);
            List<Polyline> crvList = new List<Polyline>();

            Curve[] splitCurves = Curve.CreateBooleanIntersection(rec, bound, 0.001);
            List<Polyline> plList = Convert.ToPolylines(splitCurves);

            if (plList.Count >= 1)
            {
                Polyline biggest_pl = plList.OrderBy(x => AreaMassProperties.Compute(new PolylineCurve(x.GetControlPoints())).Area).ToList().Last();

                double recArea = AreaMassProperties.Compute(rec).Area;
                double diff = recArea - AreaMassProperties.Compute(new PolylineCurve(biggest_pl.GetControlPoints())).Area;

                if (diff < ObjectModel.Tolerance.Garden * recArea) return true;
                else return false;
            }
            else return false;

        }
    }
}
