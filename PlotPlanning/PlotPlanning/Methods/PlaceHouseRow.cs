using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static void PlaceHouseRow(Rectangle3d baseRec,Curve bound, double min, double max, double space, double offset, int seed, out List<Polyline> outRecs, out List<Vector3d> tan, out PolylineCurve cutBound)
        {
            List<Line> line = PlotPlanning.Methods.Generate.SegmentBounds(Methods.Calculate.ConvertToPolyline(bound as PolylineCurve), baseRec, seed);
            if (line.Count != 0)
            {
                List<Point3d> pos = PlotPlanning.Methods.Generate.AccessPoints(line.Last(), min, max, baseRec, space);
                tan = PlotPlanning.Methods.Generate.GetTanVect(pos, line.Last());


                List<Polyline> rectangles = new List<Polyline>();
                for (int i = 0; i < pos.Count; i++)
                {
                    Polyline pLines = PlotPlanning.Methods.Generate.HouseFootprint(baseRec, pos[i], tan[i]);
                    Curve rec = Curve.CreateControlPointCurve(pLines.ToList(), 1);
                    rectangles.AddRange(PlotPlanning.Methods.Generate.CullSmallAreas(rec, bound));
                }

                Polyline cutRegion = PlotPlanning.Methods.Calculate.ConvexHull(rectangles);
                Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
                Curve offsetRegion = cutCrv.Offset(Plane.WorldXY, offset, PlotPlanning.Methods.Generate.DistanceTol(), CurveOffsetCornerStyle.Sharp)[0];
                Curve.CreateBooleanDifference(bound, offsetRegion, PlotPlanning.Methods.Generate.DistanceTol()).PickLargest().TryGetPolyline(out Polyline cutPolyline);
                cutBound = cutPolyline.ToPolylineCurve();
                outRecs = rectangles;
            }
            else
            {
                outRecs = new List<Polyline>();
                tan = new List<Vector3d>();
                cutBound = new PolylineCurve(); 
            }

            
        }

        //====================================================================

    }
}
