using System;
using System.Collections.Generic;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;
using PlotPlanning.Engine.Geometry;

namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static List<Point3d> PossiblePoints(Line line, HouseRow row, Random random, Carport carport) //TODO: we want to have carport as an optional parameter later
        {
            Polyline houseGardenBoundary = row.Houses[0].Garden;
            Point3d houseAccessPt = row.Houses[0].AccessPoint;
            bool hasCarPort = row.Houses[0].HasCarPort;
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

                if (i == row.MaxAmount)
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
        public static List<Point3d> PossiblePoints(Curve crv, HouseRow row)
        {
            SingleFamily house = row.Houses[0]; 
            Polyline houseGardenBoundary = house.Garden;
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
                    Polyline prevGarden = Methods.Adjust.Translate(house, pointPos[i - 1], crv.TangentAt(tParam[i - 1])).Garden;
                    Polyline currGarden = Methods.Adjust.Translate(house, pointPos[i], crv.TangentAt(tParam[i])).Garden;
                    Curve[] overlap = Curve.CreateBooleanIntersection(prevGarden.ToPolylineCurve(), currGarden.ToPolylineCurve(), ObjectModel.Tolerance.Distance);

                    if (overlap != null && overlap.Length != 0 && AreaMassProperties.Compute(overlap[0]).Area >= 10)
                    {
                        break;
                    }
                }


                i++;

                if (i == row.MaxAmount)
                    break;


            }

            return pointPos;
        }
    }
}


