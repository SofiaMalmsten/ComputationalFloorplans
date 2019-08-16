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
                    Polyline pLines = Calculate.Translate(baseRec, pos[i], tan[i]);
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

        public static void PlaceHouseRow(List<SingleFamily> baseHouses, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method,
            out List<Polyline> outRecs, out List<SingleFamily> houseList, out List<PolylineCurve> cutBound)
        {
            //1. Declare list
            houseList = new List<SingleFamily>();
            List<Polyline> rectangles = new List<Polyline>();

            //2. pick random house type to place
            int index = random.Next(baseHouses.Count);
            SingleFamily baseHouse = baseHouses[index];

            //3. Get boundaries
            bound.TryGetPolyline(out Polyline boundPL);
            List<Line> lines = SegmentBounds(boundPL.ClosePolyline(), baseHouse.gardenBound, baseHouse.accessPoint, 1, baseHouse.MinAmount); //1 is just a seed to make it work for now                                                                                                                                                    

            //3. Could we have a while loop here testing all the lines in the list??
            // Could we shuffle the list in the pick line method and then pick the first item we can itterate over the list of lines?
            try
            {
                Line currLine = lines.PickLine(method, random, roads, originalBound);
                currLine.Extend(-FilletOffset(), -FilletOffset());
                List<Point3d> pos = AccessPoints(currLine, baseHouse.MinAmount, baseHouse.MaxAmount, baseHouse.gardenBound, baseHouse.accessPoint, random);
                List<Vector3d> tan = Tangent(pos, currLine);
            
                //4. Create gardens for each position. if the garden overlaps the boundary it will not be created
                for (int i = 0; i < pos.Count; i++)
                {
                    Polyline pLines = Calculate.Translate(baseHouse.gardenBound, pos[i], tan[i]);
                    Curve rec = Curve.CreateControlPointCurve(pLines.ToList(), 1);
                    List<Polyline> currGarden = CullSmallAreas(rec, bound); //returns 0 when the garden overlaps the boundary
                    if (currGarden.Count != 0)
                    {
                        SingleFamily outHouse = new SingleFamily();
                        outHouse.gardenBound = pp.Calculate.BoundingRect(currGarden[0]);
                        outHouse.Type = baseHouse.Type;
                        outHouse.orientation = tan[i];
                        outHouse.houseGeom = baseHouse.houseGeom.DuplicateBrep();
                        outHouse.houseGeom.Translate(Calculate.createVector(pLines.CenterPoint(), baseHouse.gardenBound.Center));
                        outHouse.accessPoint = pLines.CenterPoint();
                        outHouse.HasCarPort = baseHouse.HasCarPort;
                        houseList.Add(outHouse);
                        rectangles.Add(currGarden[0]);
                    }
                }

                if (rectangles.Count < baseHouse.MinAmount)
                {
                    //Test another line in the set. If it still doesnt work after all the lines are tested we return an empty list of rectangles. 
                        rectangles = new List<Polyline>();
                }
                
                Polyline cutRegion = PlotPlanning.Methods.Calculate.ConvexHull(rectangles); //Här blir det fel eftersom vi har rectangles.count == 0 ibland
                Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
                Curve offsetRegion = cutCrv.OffsetOut(baseHouse.Offset, Plane.WorldXY);
                List<Curve> cutRegions = Curve.CreateBooleanDifference(bound, offsetRegion, DistanceTol()).ToList();
                cutRegions = cutRegions.Where(x => AreaMassProperties.Compute(x).Area >= CellSize(baseHouse.gardenBound.ToNurbsCurve())).ToList();
                cutBound = cutRegions.CurvesToPolylineCurves();
                outRecs = rectangles;
            }
            catch
            {
                outRecs = new List<Polyline>();
                cutBound = new List<PolylineCurve>() { bound.CurveToPolylineCurve() };
                houseList = new List<SingleFamily>();
            }
        }
    }
}
