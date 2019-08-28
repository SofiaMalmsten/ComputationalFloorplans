using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static List<PolylineCurve> UpdateBoundaries(List<SingleFamily> houseList, SingleFamily baseHouse, Curve bound)
        {
            Polyline cutRegion = Calculate.ConvexHull(houseList.Select(x => x.GardenBound).ToList()); //Här blir det fel eftersom vi har rectangles.count == 0 ibland
            Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
            Curve offsetRegion = cutCrv.OffsetOut(baseHouse.Offset, Plane.WorldXY);
            List<Curve> cutRegions = Curve.CreateBooleanDifference(bound, offsetRegion, DistanceTol()).ToList();
            cutRegions = cutRegions.Where(x => AreaMassProperties.Compute(x).Area >= CellSize(baseHouse.GardenBound.ToNurbsCurve())).ToList();
            //cutBound = cutRegions.CurvesToPolylineCurves();
            return cutRegions.CurvesToPolylineCurves();
        }

        //====================================================================

        public static List<PolylineCurve> UpdateBoundaries(List<MultiFamily> houseList, MultiFamily baseHouse, Curve bound)
        {
            Polyline cutRegion = Calculate.ConvexHull(houseList.Select(x => x.GardenBound).ToList()); //Här blir det fel eftersom vi har rectangles.count == 0 ibland
            Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
            Curve offsetRegion = cutCrv.OffsetOut(baseHouse.Offset, Plane.WorldXY);
            List<Curve> cutRegions = Curve.CreateBooleanDifference(bound, offsetRegion, DistanceTol()).ToList();
            cutRegions = cutRegions.Where(x => AreaMassProperties.Compute(x).Area >= CellSize(baseHouse.GardenBound.ToNurbsCurve())).ToList();
            //cutBound = cutRegions.CurvesToPolylineCurves();
            return cutRegions.CurvesToPolylineCurves();
        }

    }
}


