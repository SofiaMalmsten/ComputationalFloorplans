using System;
using Rhino.Geometry;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Adjust
    {
        //Moves a point along a vecotr inside a boundary. If the point moves outside the new point will be the closest point on the boundary.
        public static Point3d MoveInside(this Point3d pt, Vector3d v, PolyCurve bound)
        {
            Point3d testPt = pt + v;
            Line testLine = new Line(pt, pt + v);
            Point3d movedPt = new Point3d();

            //Check wheter the point is inside the building boundary

            if (bound.Contains(testPt, Plane.WorldXY, 0.01) == Rhino.Geometry.PointContainment.Inside)
            {
                movedPt = testPt;
            }
            else
            {
                Rhino.Geometry.Intersect.CurveIntersections intSec = Rhino.Geometry.Intersect.Intersection.CurveLine(bound, testLine, 0.001, 0.001);
                movedPt = intSec[0].PointA;
            }

            return movedPt;
        }
    }
}
