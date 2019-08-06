using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static List<Point3d> AccessPoints(Line line, double minAmount, double maxAmount, Rectangle3d rectangle, Random random)
        {
            //========================================================
            //Declaration - fixed values
            //========================================================
            double lineLength = line.Length;
            double segmentWidth = rectangle.Width;
            double segmentHeight = rectangle.Height;
            double segmentLength = Math.Min(segmentWidth, segmentHeight);

            Point3d startPt = line.From;
            Vector3d vec = (line.Direction) / lineLength;
            Vector3d husVec = vec * segmentLength;

            //========================================================
            //Declaration - lists and new objects
            //========================================================
            Line currLine = new Line();
            Point3d currPt = new Point3d();
            List<Point3d> pointPos = new List<Point3d>();
            List<Line> lineCombination = new List<Line>();

            currPt = startPt;
            double currLength = segmentLength;
            int i = 0;

            while (currLength < lineLength)
            {
                currLine = new Line(currPt, husVec);
                currPt = currLine.To;
                pointPos.Add(currPt);
                lineCombination.Add(currLine);
                currLength = currLength + segmentLength;
                i = i + 1;

                if (i == maxAmount)
                {
                    i = 0;
                    break;
                }
            }

            Vector3d move_vec = random.NextDouble() * (lineLength - currLength) * vec.Normalise();
            List<Point3d> move_pts = new List<Point3d>();
            foreach (Point3d p in pointPos) move_pts.Add(Rhino.Geometry.Point3d.Add(p, move_vec));

            return move_pts;
        }

        //====================================================================

        public static List<Point3d> AccessPoints(Line line, double minAmount, double maxAmount, Rectangle3d rectangle)
        {
            //========================================================
            //Declaration - fixed values
            //========================================================
            double lineLength = line.Length;
            double segmentWidth = rectangle.Width;
            double segmentHeight = rectangle.Height;
            double segmentLength = Math.Min(segmentWidth, segmentHeight);

            Point3d startPt = line.From;
            Vector3d vec = (line.Direction) / lineLength;
            Vector3d husVec = vec * segmentLength;

            //========================================================
            //Declaration - lists and new objects
            //========================================================
            Line currLine = new Line();
            Point3d currPt = new Point3d();
            List<Point3d> pointPos = new List<Point3d>();
            List<Line> lineCombination = new List<Line>();

            currPt = startPt;
            double currLength = segmentLength;
            int i = 0;

            while (currLength < lineLength)
            {
                currLine = new Line(currPt, husVec);
                currPt = currLine.To;
                pointPos.Add(currPt);
                lineCombination.Add(currLine);
                currLength = currLength + segmentLength;
                i = i + 1;

                if (i == maxAmount)
                {
                    i = 0;
                    break;
                }
            }
            return pointPos;             
        }
    }
}


