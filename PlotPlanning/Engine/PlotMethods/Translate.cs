using PlotPlanning.ObjectModel;
using Rhino.Geometry;
using System.Collections.Generic;
using PlotPlanning.Engine.Geometry;
using PlotPlanning.Engine.Base;


namespace PlotPlanning.Methods
{
    public static partial class Adjust
    {
        public static Polyline Translate(Rectangle3d baseRectangle, Point3d point, Vector3d tan) //TODO: This method should return a rectangle
        {
            Point3d pt0 = point;
            Point3d pt1 = pt0 + tan * baseRectangle.Width;
            Point3d pt2 = pt1 + Vector3d.CrossProduct(tan, Vector3d.ZAxis) * (baseRectangle.Height);
            Point3d pt3 = pt2 - tan * baseRectangle.Width;

            List<Point3d> movedPts = new List<Point3d> { pt0, pt1, pt2, pt3, pt0 };
            Polyline pLine = new Polyline(movedPts);

            return pLine;
        }

        //====================================================================//

        public static Polyline Translate(Polyline pline, Point3d basePt, Point3d boundPt, Vector3d tan)
        {
            Line accessLine = Query.ClosestSegmentToPoint(basePt, pline);
            Vector3d accessVec = Compute.CreateVector(accessLine.To, accessLine.From);

            Polyline pLineToMove = new Polyline(pline);

            pLineToMove.Transform(Transform.Translation(Compute.CreateVector(basePt, boundPt)));
            pLineToMove.Transform(Transform.Rotation(accessVec, tan, boundPt));
            return pLineToMove;
        }

        //====================================================================//

        public static ObjectModel.SingleFamily Translate(SingleFamily house, Point3d boundPt, Vector3d tan)
        {
            Point3d basePt = house.AccessPoint;
            Line accessLine = Query.ClosestSegmentToPoint(basePt, house.GardenBound);
            Vector3d accessVec = Compute.CreateVector(accessLine.To, accessLine.From);

            SingleFamily movedHouse = house.Clone();

            Transform t = Transform.Translation(Compute.CreateVector(basePt, boundPt));
            Transform r = Transform.Rotation(accessVec, tan, boundPt);

            movedHouse.GardenBound.Transform(t);
            movedHouse.GardenBound.Transform(r);

            movedHouse.HouseGeom.Transform(t);
            movedHouse.HouseGeom.Transform(r);

            movedHouse.Orientation = Compute.CrossProduct(Vector3d.ZAxis, tan);

            movedHouse.MidPoint = movedHouse.GardenBound.CenterPoint(); // add a real translation here maybe? 
            movedHouse.AccessPoint = boundPt;

            return movedHouse;
        }

        //====================================================================//

        public static ObjectModel.Carport Translate(Carport carport, Point3d boundPt, Vector3d tan)
        {
            Point3d basePt = carport.AccessPoint;
            Line accessLine = Query.ClosestSegmentToPoint(basePt, carport.GardenBound);
            Vector3d accessVec = Compute.CreateVector(accessLine.To, accessLine.From);

            Carport movedCarport = carport.Clone();

            Transform t = Transform.Translation(Compute.CreateVector(basePt, boundPt));
            Transform r = Transform.Rotation(accessVec, tan, boundPt);

            movedCarport.GardenBound.Transform(t);
            movedCarport.GardenBound.Transform(r);

            movedCarport.CarportGeom.Transform(t);
            movedCarport.CarportGeom.Transform(r);

            movedCarport.AccessPoint = boundPt;

            return movedCarport;
        }

        //====================================================================//

        public static ObjectModel.MultiFamily Translate(MultiFamily house, Point3d boundPt, Vector3d tan)
        {
            Point3d basePt = house.AccessPoint;
            Line accessLine = Query.ClosestSegmentToPoint(basePt, house.GardenBound);
            Vector3d accessVec = Compute.CreateVector(accessLine.To, accessLine.From);

            MultiFamily movedHouse = house.Clone();

            Transform t = Transform.Translation(Compute.CreateVector(basePt, boundPt));
            Transform r = Transform.Rotation(accessVec, tan, boundPt);

            movedHouse.GardenBound.Transform(t);
            movedHouse.GardenBound.Transform(r);

            movedHouse.HouseGeom.Transform(t);
            movedHouse.HouseGeom.Transform(r);

            movedHouse.Orientation = Compute.CrossProduct(Vector3d.ZAxis, tan);

            movedHouse.MidPoint = movedHouse.GardenBound.CenterPoint();  // add a real translation here maybe? 

            movedHouse.AccessPoint = boundPt;

            return movedHouse;
        }
    }
}