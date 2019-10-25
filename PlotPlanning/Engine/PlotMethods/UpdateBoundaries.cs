using System;
using System.Collections.Generic;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;
using System.Linq;
using PlotPlanning.Engine.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static List<PolylineCurve> UpdateBoundaries(HouseRow row, HouseRow baseRow, Curve bound)
        {
            if (row.Houses.Count == 0) return new List<PolylineCurve>();
            Polyline cutRegion = Compute.ConvexHull(row.Houses.Select(x => x.Garden).ToList());

            Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
            Curve offsetRegion = cutCrv.OffsetOut(row.Offset, Plane.WorldXY);
            List<Curve> cutRegions = Curve.CreateBooleanDifference(bound, offsetRegion, Tolerance.Distance).ToList();
            double cellSize = AreaMassProperties.Compute(baseRow.Houses[0].Garden.ToPolylineCurve()).Area * 2;
            cutRegions = cutRegions.Where(x => AreaMassProperties.Compute(x).Area >= cellSize).ToList();

            return cutRegions.ToPolylineCurves();
        }

        //====================================================================//

        public static List<PolylineCurve> UpdateBoundaries(List<MultiFamily> houseList, MultiFamily baseHouse, Curve bound)
        {
            if (houseList.Count == 0) return new List<PolylineCurve>();
            Polyline cutRegion = Compute.ConvexHull(houseList.Select(x => x.GardenBound).ToList());

            Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
            Curve offsetRegion = cutCrv.OffsetOut(baseHouse.Offset, Plane.WorldXY);
            List<Curve> cutRegions = Curve.CreateBooleanDifference(bound, offsetRegion, Tolerance.Distance).ToList();
            double cellSize = AreaMassProperties.Compute(baseHouse.GardenBound.ToPolylineCurve()).Area * 2;
            cutRegions = cutRegions.Where(x => AreaMassProperties.Compute(x).Area >= cellSize).ToList();

            return cutRegions.ToPolylineCurves();
        }

        //====================================================================//
    }
}