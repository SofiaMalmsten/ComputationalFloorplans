using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static Polyline HouseFootprint(Rectangle3d baseRectangle, Point3d pts, Vector3d tan)
        {
            double wDim = baseRectangle.Width;
            double hDim = baseRectangle.Height;
            Vector3d unitZ = new Vector3d(0, 0, 1);

            List<Point3d> movedPts = new List<Point3d>();

            //======================================================
            //Create Polyline
            //======================================================
                //create points
                Point3d pt0 = pts;
                Point3d pt1 = pt0 + tan * wDim;
                Point3d pt2 = pt1 + Vector3d.CrossProduct(tan, unitZ) * (hDim);
                Point3d pt3 = pt2 - tan * wDim;
                Point3d pt4 = pt0;

                //add points
                movedPts.Add(pt0);
                movedPts.Add(pt1);
                movedPts.Add(pt2);
                movedPts.Add(pt3);
                movedPts.Add(pt4);

                Polyline pLine = new Polyline(movedPts);

            return pLine;
        }

    }

    //====================================================================

}
