using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.ObjectModel;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Query
    {
        public static bool IsInside(this Curve subjectCurve, Curve boundCurve)
        {
            double areaTol = Tolerance.Area;

            Curve[] splitCurves = Curve.CreateBooleanIntersection(subjectCurve, boundCurve, areaTol);
            List<Polyline> plList = Convert.ToPolylines(splitCurves);

            if (plList.Count >= 1) //Is this really right??
            {
                Polyline biggest_pl = plList.OrderBy(x => AreaMassProperties.Compute(new PolylineCurve(x.GetControlPoints())).Area).ToList().Last();

                double recArea = AreaMassProperties.Compute(subjectCurve).Area;
                double diff = recArea - AreaMassProperties.Compute(new PolylineCurve(biggest_pl.GetControlPoints())).Area;

                if (diff < ObjectModel.Tolerance.Garden * recArea) return true;
                else return false;
            }
            else return false;

        }
    }

    //====================================================================//

}
