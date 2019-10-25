using System;
using System.Collections.Generic;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;
using PlotPlanning.Engine.Geometry;

namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static List<Point3d> PossiblePoints(Line line, SingleFamily house, Random random, Carport carport) //TODO: we want to have carport as an optional parameter later
        {
            Polyline houseGardenBoundary = house.Garden;
            Point3d houseAccessPt = house.AccessPoint;
            bool hasCarPort = house.HasCarPort;
            double lineLength = line.Length;

            double houseWidth = Query.ClosestSegmentToPoint(houseAccessPt, houseGardenBoundary).Length;

            Point3d startPt = line.From;
            Vector3d vec = (line.Direction) / lineLength;
            Vector3d husVec = vec * houseWidth;

            Vector3d cpVec = new Vector3d();
            double cpWidth = 0;
            if (hasCarPort)
            {
                cpWidth = Query.ClosestSegmentToPoint(carport.AccessPoint, carport.GardenBound).Length;
                cpVec = vec * cpWidth;
            }

            List<Point3d> pointPos = new List<Point3d>();

            Point3d currPt = startPt;
            double currLength = houseWidth;
            Line currLine = new Line();
            int i = 0;

            while (currLength < lineLength)
            {
                currLine = new Line(currPt, husVec);
                currPt = currLine.To;
                pointPos.Add(currPt);
                currLength += houseWidth;
                i++;

                if (hasCarPort)
                {
                    currLine = new Line(currPt, cpVec);
                    currPt = currLine.To;
                    pointPos.Add(currPt);
                    currLength += cpWidth;
                }

                if (i == house.MaxAmount)
                    break;
            }

            Vector3d move_vec = random.NextDouble() * (lineLength - currLength) * vec.Normalise();
            List<Point3d> move_pts = new List<Point3d>();
            foreach (Point3d p in pointPos) move_pts.Add(Point3d.Add(p, move_vec));

            return move_pts;
        }

        //====================================================================//

        public static List<Point3d> PossiblePoints(Line line, MultiFamily house, Random random, Carport carport) //we want to have carport as an optional parameter later
        {
            Polyline houseGardenBoundary = house.GardenBound;
            Point3d houseAccessPt = house.AccessPoint;

            double lineLength = line.Length;
            double houseWidth = Query.ClosestSegmentToPoint(houseAccessPt, houseGardenBoundary).Length;

            Point3d startPt = line.From;
            Vector3d vec = (line.Direction) / lineLength;
            Vector3d husVec = vec * houseWidth;

            List<Point3d> pointPos = new List<Point3d>();

            Point3d currPt = startPt;
            double currLength = houseWidth;
            Line currLine = new Line();
            int i = 0;

            while (currLength < lineLength)
            {
                currLine = new Line(currPt, husVec);
                currPt = currLine.To;
                pointPos.Add(currPt);
                currLength += houseWidth;
                i++;

                if (i == house.MaxAmount)
                    break;
            }

            Vector3d move_vec = random.NextDouble() * (lineLength - currLength) * vec.Normalise();
            List<Point3d> move_pts = new List<Point3d>();
            foreach (Point3d p in pointPos) move_pts.Add(Point3d.Add(p, move_vec));

            return move_pts;
        }

        //============

        //Test for curves
        public static List<Point3d> PossiblePoints(Curve crv, SingleFamily house)
        {
            Polyline houseGardenBoundary = house.GardenBound;
            Point3d houseAccessPt = house.AccessPoint;
            double crvLength = crv.GetLength();
            double houseWidth = Query.ClosestSegmentToPoint(houseAccessPt, houseGardenBoundary).Length;

            List<Point3d> pointPos = new List<Point3d>();

            Point3d currPt = crv.PointAtStart;
            double currLength = houseWidth;
            int i = 0;

            while (currLength < crvLength)
            {
                double[] tParam = crv.DivideByLength(houseWidth, true);
                currPt = crv.PointAt(tParam[i]);
                pointPos.Add(currPt);
                currLength += houseWidth;

                //if the garden overlaps the previous garden the loop will break.
                if (i != 0)
                {
                    Polyline prevGarden = Methods.Adjust.Translate(house, pointPos[i - 1], crv.TangentAt(tParam[i - 1])).GardenBound;
                    Polyline currGarden = Methods.Adjust.Translate(house, pointPos[i], crv.TangentAt(tParam[i])).GardenBound;
                    Curve[] overlap = Curve.CreateBooleanIntersection(prevGarden.ToPolylineCurve(), currGarden.ToPolylineCurve(), ObjectModel.Tolerance.Distance);

                    if (overlap != null && overlap.Length != 0 && AreaMassProperties.Compute(overlap[0]).Area >= 10)
                    {
                        break;
                    }
                }


                i++;

                if (i == house.MaxAmount)
                    break;


            }

            return pointPos;
        }
    }
}


