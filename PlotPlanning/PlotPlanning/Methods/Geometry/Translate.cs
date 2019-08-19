using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.ObjectModel;


namespace PlotPlanning.Methods
{
    public static partial class Adjust
    {
            public static Polyline Translate(Rectangle3d baseRectangle, Point3d pts, Vector3d tan)
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

        //================================

        public static Polyline Translate(Polyline pline, Point3d basePt, Point3d boundPt, Vector3d tan)
        {
            Line accessLine = Calculate.GetAccessLine(basePt, pline);
            Vector3d accessVec = Calculate.createVector(accessLine.To, accessLine.From);


            Polyline pLineToMove = new Polyline(pline);
            pLineToMove.Transform(Transform.Translation(Calculate.createVector(basePt, boundPt)));

            pLineToMove.Transform(Transform.Rotation(accessVec, tan, boundPt));


            return pLineToMove;
        }

        //================================

        public static ObjectModel.SingleFamily Translate(SingleFamily house, Point3d boundPt, Vector3d tan)
        {
            Point3d basePt = house.AccessPoint;
            Line accessLine = Calculate.GetAccessLine(basePt, house.GardenBound);
            Vector3d accessVec = Calculate.createVector(accessLine.To, accessLine.From);

            SingleFamily movedHouse = house.Clone();

            movedHouse.GardenBound.Transform(Transform.Translation(Calculate.createVector(basePt, boundPt)));
            movedHouse.GardenBound.Transform(Transform.Rotation(accessVec, tan, boundPt));

            movedHouse.HouseGeom.Transform(Transform.Translation(Calculate.createVector(basePt, boundPt)));
            movedHouse.HouseGeom.Transform(Transform.Rotation(accessVec, tan, boundPt));
            movedHouse.Orientation = Calculate.CrossProduct(new Vector3d(0,0,1), tan);

            movedHouse.AccessPoint = boundPt;

            return movedHouse;
        }

        //================================
    }
}
