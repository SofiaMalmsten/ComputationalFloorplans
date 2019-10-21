using System;
using Rhino.Geometry;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Convert
    {
        public static Rectangle3d ExpandRectangle(Rectangle3d rec, double front, double back)
        {
            rec.MakeIncreasing();
            Point3d pt1 = rec.Corner(0);
            pt1 = new Point3d(pt1.X - front, pt1.Y, pt1.Z);
            Point3d pt2 = rec.Corner(2);
            pt2 = new Point3d(pt2.X + back, pt2.Y, pt2.Z);
            Rectangle3d expandedRectangle = new Rectangle3d(new Plane(rec.Center, Vector3d.ZAxis), pt1, pt2);           

            return expandedRectangle; 
        }

        //====================================================================//
    }
}
