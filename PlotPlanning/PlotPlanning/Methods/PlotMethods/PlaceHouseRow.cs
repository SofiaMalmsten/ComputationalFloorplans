using System;
using System.Collections.Generic;
using pp = PlotPlanning.Methods;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.ObjectModel;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static void PlaceHouseRow(List<SingleFamily> baseHouses, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method, Carport carport, out List<SingleFamily> houseList, out List<PolylineCurve> cutBound, out List<Carport> carportList)
        {
            //1. pick house type to place
            SingleFamily baseHouse = baseHouses[random.Next(baseHouses.Count)];

            try
            {//2. Get boundaries. 
                bound.TryGetPolyline(out Polyline boundPL);
                List<Line> lines = SegmentBounds(boundPL.ClosePolyline(), baseHouse);                                                                                                                                          
                Line currLine = lines.PickLine(method, random, roads, originalBound);
                currLine.Extend(-FilletOffset(), -FilletOffset());

                houseList = new List<SingleFamily>();
                carportList = new List<Carport>();
            
                List<Point3d> pos = AccessPoints(currLine, baseHouse, random);
                List<Vector3d> tan = Tangent(pos, currLine);

                //4. Create gardens for each position
                for (int i = 0; i < pos.Count; i++)
                {
                    SingleFamily movedHouse = Adjust.Translate(baseHouse, pos[i], tan[i]);

                    if (IsInside(movedHouse, bound))
                        houseList.Add(movedHouse);
                    else if (houseList.Count != 0) //already places houses
                        break;

                    if (movedHouse.HasCarPort)
                    {
                        i++;
                        Carport movedCarport = Adjust.Translate(carport, pos[i], tan[i]);
                        carportList.Add(movedCarport);
                    }
                }

                if (houseList.Count < baseHouse.MinAmount)
                    houseList = new List<SingleFamily>();
                
                Polyline cutRegion = Calculate.ConvexHull(houseList.Select(x => x.GardenBound).ToList()); //Här blir det fel eftersom vi har rectangles.count == 0 ibland
                Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
                Curve offsetRegion = cutCrv.OffsetOut(baseHouse.Offset, Plane.WorldXY);
                List<Curve> cutRegions = Curve.CreateBooleanDifference(bound, offsetRegion, DistanceTol()).ToList();
                cutRegions = cutRegions.Where(x => AreaMassProperties.Compute(x).Area >= CellSize(baseHouse.GardenBound.ToNurbsCurve())).ToList();
                cutBound = cutRegions.CurvesToPolylineCurves();
            }
            catch
            {
                cutBound = new List<PolylineCurve>() { bound.CurveToPolylineCurve() };
                houseList = new List<SingleFamily>();
                carportList = new List<Carport>();
            }
        }
    }
}
