using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using static System.Math;



namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static bool IsInside(Curve rec, Curve bound)
        {
            Point3d origin = new Point3d(0, 0, 0);
            Vector3d normal = new Vector3d(0, 0, 1);
            Plane p = new Plane(origin, normal);
            List<Polyline> crvList = new List<Polyline>();

            Curve[] splitCurves = Curve.CreateBooleanIntersection(rec, bound, 0.001);
            List<Polyline> plList = CurvesToPolylines(splitCurves);

            if (plList.Count >= 1)
            {
                Polyline biggest_pl = plList.OrderBy(x => AreaMassProperties.Compute(new PolylineCurve(x.GetControlPoints())).Area).ToList().Last();

                double recArea = AreaMassProperties.Compute(rec).Area;
                double diff = recArea - AreaMassProperties.Compute(new PolylineCurve(biggest_pl.GetControlPoints())).Area;

                if (diff < GardenTol() * recArea) return true;
                else return false;
            }
            else return false; 

        }

        public static bool IsInside (ObjectModel.SingleFamily sfh, Curve bound)
        {
            Curve garden = Curve.CreateControlPointCurve(sfh.GardenBound.ToList(), 1);
            return IsInside(garden, bound);
        }

    }

    //====================================================================

}
