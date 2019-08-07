﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

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

    }
}
