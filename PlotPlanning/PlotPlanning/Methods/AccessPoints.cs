using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using GeometryBase;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
    
        public static List<Point3d> AccessPoints(Line line, double minAmount, double maxAmount, Rectangle3d rectangle, double spaceDist)
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
            Vector3d spaceVec = vec * (spaceDist + segmentLength);

            //========================================================
            //Declaration - lists and new objects
            //========================================================
            Line currLine = new Line();
            Point3d currPt = new Point3d();
            List<Point3d> pointPos = new List<Point3d>();
            List<Vector3d> vectList = new List<Vector3d>();
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

                //if i = max amunt: add spaceDist and reset counter. // Todo: add a new branch
                if (i == maxAmount)
                {
                    currLine = new Line(currPt, spaceVec);
                    currPt = currLine.To;
                    currLength = currLength + spaceDist + segmentLength;
                    i = 0;
                }
            }

            //========================================================
            // Remove lines that are too short. minAmount * segmentLength
            //========================================================
            if (i < minAmount)
            {
                lineCombination.RemoveRange(lineCombination.Count - i, i);
            }

            return pointPos;
        }
    }

    //====================================================================

}
