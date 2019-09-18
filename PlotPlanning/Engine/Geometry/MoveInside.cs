using System;
using Rhino.Geometry;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Adjust
    {
        //Moves a point along a vecotr inside a boundary. If the point moves outside the new point will be the closest point on the boundary.
        public static Point3d MoveInside(this Point3d point, Vector3d vector, Curve bound) //Change the name of this method to something like MoveWithin or MovePointInsideCurve.
        {
            Point3d testPt = point + vector;
            Line testLine = new Line(point, point + vector);
            Point3d movedPt = new Point3d();

            //Check wheter the point is inside the building boundary

            if (bound.Contains(testPt, Plane.WorldXY, 0.01) == Rhino.Geometry.PointContainment.Inside)
            {
                movedPt = testPt;
            }
            else
            {
                //Rhino.Geometry.Intersect.CurveIntersections intSec = Rhino.Geometry.Intersect.Intersection.CurveLine(bound, testLine, 0.001, 0.001);
                //movedPt = intSec[0].PointA;

                bound.ClosestPoint(testPt, out double t);
                movedPt = bound.PointAt(t);
            }

            return movedPt;
        }
    }
}
