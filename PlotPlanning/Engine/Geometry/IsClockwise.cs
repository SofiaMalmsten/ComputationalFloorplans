using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.Engine.Base;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Query
    {
        public static bool IsClockwise(this Polyline polyline, Vector3d viewVector, double tolerance = 0.001) //TODO: Change this to defalut ObjModel tol
        {
            if (!polyline.IsClosed)
                throw new Exception("The polyline is not closed. IsClockwise method is relevant only to closed curves.");

            List<Point3d> pts = polyline.DiscontinuityPoints(tolerance);
            Vector3d dir1 = (pts[0] - pts.Last()).Normalise();
            Vector3d dir2;
            double angleTot = 0;

            for (int i = 1; i < pts.Count; i++)
            {
                dir2 = (pts[i] - pts[i - 1]).Normalise();
                double signedAngle = dir1.SignedAngle(dir2, viewVector);
                dir1 = dir2.Clone();

                if (Math.PI - Math.Abs(signedAngle) <= 0.1)
                {
                    dir1 *= -1;
                    continue;
                }
                else
                    angleTot += signedAngle;
            }

            return angleTot > 0;
        }

        //====================================================================//

    }
}
