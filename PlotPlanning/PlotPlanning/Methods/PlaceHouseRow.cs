using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static void PlaceHouseRow(Rectangle3d baseRec, Curve bound, double min, double max, double space, double offset, Random random, string method,
            out List<Polyline> outRecs, out List<Vector3d> out_tan, out PolylineCurve cutBound)
        {
            try
            {
                List<Line> lines = PlotPlanning.Methods.Generate.SegmentBounds(Methods.Calculate.ConvertToPolyline(bound as PolylineCurve).ClosePolyline(), baseRec, 1); //1 is just a seed to make it work for now                                                                                                                                                    
                Line this_line = lines.PickLine(method, random);
                List<Point3d> pos = PlotPlanning.Methods.Generate.AccessPoints(this_line, min, max, baseRec, space);
                out_tan = new List<Vector3d>();
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
                    }
                }

                Polyline cutRegion = PlotPlanning.Methods.Calculate.ConvexHull(rectangles);
                Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
                Curve offsetRegion = new PolylineCurve();
                offsetRegion = cutCrv.OffsetOut(offset, Plane.WorldXY);
                Curve[] cutRegions = Curve.CreateBooleanDifference(bound, offsetRegion, PlotPlanning.Methods.Generate.DistanceTol()).ToArray();
                double max_area = cutRegions.Max(x => Rhino.Geometry.AreaMassProperties.Compute(x).Area);
                Polyline largest_region = new Polyline();
                cutRegions.First(x => Rhino.Geometry.AreaMassProperties.Compute(x).Area == max_area).TryGetPolyline(out largest_region);
                cutBound = largest_region.ToPolylineCurve();
                outRecs = rectangles;
            }
            catch
            {
                outRecs = new List<Polyline>();
                out_tan = new List<Vector3d>();
                cutBound = new PolylineCurve();
            }


        }

        public static Line PickLine(this List<Line> lines, string method, Random random)
        {
            if (method == "random")
            {
                Line line = lines[random.Next(lines.Count)];
                return line;
            }
            else if (method == "shortest")
            {
                return lines.OrderBy(x => x.Length).ToList()[0];
            }
            else //(method == "longest")
            {
                return lines.OrderBy(x => x.Length).ToList()[lines.Count - 1];
            }

        }


        //====================================================================

    }
}
