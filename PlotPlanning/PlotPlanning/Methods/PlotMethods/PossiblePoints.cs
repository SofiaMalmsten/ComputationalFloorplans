using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static List<Point3d> PossiblePoints(Line line, SingleFamily house, Random random, Carport carport) //we want to have carport as an optional parameter later
        {
            Polyline houseGardenBoundary = house.GardenBound;
            Point3d houseAccessPt = house.AccessPoint;
            bool hasCarPort = house.HasCarPort; 

            //========================================================
            //Declaration - fixed values
            //========================================================
            double lineLength = line.Length;
            double houseWidth = Calculate.GetAccessLine(houseAccessPt, houseGardenBoundary).Length;

            Point3d startPt = line.From;
            Vector3d vec = (line.Direction) / lineLength;
            Vector3d husVec = vec * houseWidth;

            //========================================================
            //Declaration - lists and new objects
            //========================================================

            List<Point3d> pointPos = new List<Point3d>();

            Point3d currPt = startPt;
            double currLength = houseWidth;
            int i = 0;

            while (currLength < lineLength)
            {
                Line currLine = new Line(currPt, husVec);
                currPt = currLine.To;
                pointPos.Add(currPt);
                currLength += houseWidth;
                i ++;

                if (i == house.MaxAmount)
                    break;
            }

            Vector3d move_vec = random.NextDouble() * (lineLength - currLength) * vec.Normalise();
            List<Point3d> move_pts = new List<Point3d>();
            foreach (Point3d p in pointPos) move_pts.Add(Point3d.Add(p, move_vec));

            return move_pts;
        }

        //====================================================================
    }
}


