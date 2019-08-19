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
        //====================================================================
        public static void PlaceHouseRow(Rectangle3d baseRec, Curve bound, Curve originalBound, List<Curve> roads, double min, double max, double offset, Random random, string method,
            out List<Polyline> outRecs, out List<Vector3d> out_tan, out List<PolylineCurve> cutBound, out List<Point3d> midPts)
        {
            try
            {
                bound.TryGetPolyline(out Polyline boundPL); 
                List<Line> lines = SegmentBounds(boundPL.ClosePolyline(), baseRec, 1, min); //1 is just a seed to make it work for now                                                                                                                                                    
                Line this_line = lines.PickLine(method, random, roads, originalBound);
                this_line.Extend(-FilletOffset(), -FilletOffset()); 
                List <Point3d> pos = AccessPoints(this_line, min, max, baseRec, random);
                out_tan = new List<Vector3d>();
                midPts = new List<Point3d>(); 
                List<Vector3d> tan = Tangent(pos, this_line);

                List<Polyline> rectangles = new List<Polyline>();
                for (int i = 0; i < pos.Count; i++)
                {
                    Polyline pLines = Adjust.Translate(baseRec, pos[i], tan[i]);
                    Curve rec = Curve.CreateControlPointCurve(pLines.ToList(), 1);
                    List<Polyline> this_rec = CullSmallAreas(rec, bound); //this_rec finns inte. 
                    if (this_rec.Count != 0)
                    {
                        rectangles.Add(this_rec[0]);
                        out_tan.Add(tan[i]);                        
                        midPts.Add(rec.CurveToPolyline().CenterPoint()); 
                    }
                    //else: Testa med annan hustyp!
                    //om det ta bort linjen från listan över linjer att testa
                }

                if (rectangles.Count < min)
                    rectangles = new List<Polyline>();

                Polyline cutRegion = PlotPlanning.Methods.Calculate.ConvexHull(rectangles); //Här blir det fel eftersom vi har rectangles.count == 0 ibland
                Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
                Curve offsetRegion = cutCrv.OffsetOut(offset, Plane.WorldXY);
                List<Curve> cutRegions = Curve.CreateBooleanDifference(bound, offsetRegion, DistanceTol()).ToList();
                //double max_area = cutRegions.Max(x => Rhino.Geometry.AreaMassProperties.Compute(x).Area);
                cutRegions = cutRegions.Where(x => AreaMassProperties.Compute(x).Area >= CellSize(baseRec.ToNurbsCurve())).ToList();
                cutBound = cutRegions.CurvesToPolylineCurves(); 
                outRecs = rectangles;
            }
            catch
            {
                outRecs = new List<Polyline>();
                out_tan = new List<Vector3d>();
                cutBound = new List<PolylineCurve>() { bound.CurveToPolylineCurve() };
                midPts = new List<Point3d>(); 
            }
        }
        //====================================================================

        public static void PlaceHouseRow(List<SingleFamily> baseHouses, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method, out List<SingleFamily> houseList, out List<PolylineCurve> cutBound)
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
            
                List<Point3d> pos = AccessPoints(currLine, baseHouse, random);
                List<Vector3d> tan = Tangent(pos, currLine);
            
                //4. Create gardens for each position
                for (int i = 0; i < pos.Count; i++)
                {
                    SingleFamily movedHouse = Adjust.Translate(baseHouse, pos[i], tan[i]);

                    Curve garden = Curve.CreateControlPointCurve(movedHouse.GardenBound.ToList(), 1);
                    List<Polyline> currGarden = CullSmallAreas(garden, bound); //returns 0 when the garden overlaps the boundary
                    if (currGarden.Count != 0)
                        houseList.Add(movedHouse);
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
            }
        }
    }
}
