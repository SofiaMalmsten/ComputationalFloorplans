using System;
using Rhino.Geometry;
using Grasshopper.Kernel;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Adjust
    {
        //Moves a point along a vecotr inside a boundary. If the point moves outside the new point will be the closest point on the boundary.
        public static Point3d MoveInside(this Point3d point, Vector3d vector, Curve bound) //Change the name of this method
        {
            Point3d testPt = point + vector;
            Point3d movedPt = new Point3d();

            //Check wheter the point is inside the building boundary
            if (bound.Contains(testPt, Plane.WorldXY, 0.01) == Rhino.Geometry.PointContainment.Inside)
                movedPt = testPt;
            else
            {
                Line l = new Line(point, testPt);
                Point3d boundPt = Rhino.Geometry.Intersect.Intersection.CurveCurve(bound, l.ToNurbsCurve(), ObjectModel.Tolerance.Distance, ObjectModel.Tolerance.Distance)[0].PointA;

                movedPt = boundPt;
            }

            return movedPt;
        }
    }

    //====================================================================//
}
