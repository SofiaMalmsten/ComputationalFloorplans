﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
    
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
            while (currLength < lineLength)
            {
                currLine = new Line(currPt, husVec);
                currPt = currLine.To;
                pointPos.Add(currPt);
                lineCombination.Add(currLine);
                currLength = currLength + segmentLength;
            }

            return pointPos;
        }
    }

    //====================================================================

}
