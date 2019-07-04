using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using nic = Rhino.NodeInCode.Components; 


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static void PlaceHouseRow(Rectangle3d baseRec, Curve bound, Curve originalBound, List<Curve> roads, double min, double max, double offset, Random random, string method,
            out List<Polyline> outRecs, out List<Vector3d> out_tan, out PolylineCurve cutBound, out List<Point3d> midPts)
        {
            try
            {
                bound.TryGetPolyline(out Polyline boundPL); 
                List<Line> lines = PlotPlanning.Methods.Generate.SegmentBounds(boundPL.ClosePolyline(), baseRec, 1, min); //1 is just a seed to make it work for now                                                                                                                                                    
                Line this_line = lines.PickLine(method, random, roads, originalBound); 
                List <Point3d> pos = PlotPlanning.Methods.Generate.AccessPoints(this_line, min, max, baseRec);
                out_tan = new List<Vector3d>();
                midPts = new List<Point3d>(); 
                List<Vector3d> tan = PlotPlanning.Methods.Generate.GetTanVect(pos, this_line);

                List<Polyline> rectangles = new List<Polyline>();
                for (int i = 0; i < pos.Count; i++)
                {
                    Polyline pLines = PlotPlanning.Methods.Generate.HouseFootprint(baseRec, pos[i], tan[i]);
                    Curve rec = Curve.CreateControlPointCurve(pLines.ToList(), 1);
                    List<Polyline> this_rec = PlotPlanning.Methods.Generate.CullSmallAreas(rec, bound);
                    if (this_rec.Count != 0)
                    {
                        rectangles.Add(this_rec[0]);
                        out_tan.Add(tan[i]);                        
                        midPts.Add(rec.CurveToPolyline().CenterPoint()); 
                    }
                }

                if (rectangles.Count < min)
                    rectangles = new List<Polyline>();

                Polyline cutRegion = PlotPlanning.Methods.Calculate.ConvexHull(rectangles);
                Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
                Curve offsetRegion = cutCrv.OffsetOut(offset, Plane.WorldXY);
                Curve[] cutRegions = Curve.CreateBooleanDifference(bound, offsetRegion, PlotPlanning.Methods.Generate.DistanceTol()).ToArray();
                double max_area = cutRegions.Max(x => Rhino.Geometry.AreaMassProperties.Compute(x).Area);
                cutRegions.First(x => Rhino.Geometry.AreaMassProperties.Compute(x).Area == max_area).TryGetPolyline(out Polyline largest_region);
                cutBound = largest_region.ToPolylineCurve();
                outRecs = rectangles;
            }
            catch
            {
                outRecs = new List<Polyline>();
                out_tan = new List<Vector3d>();
                cutBound = bound as PolylineCurve;
                midPts = new List<Point3d>(); 
            }
        }
        //====================================================================

    }
}
