using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Calculate
    {
        public static bool IsClockwise(this Polyline polyline, Vector3d normal, double tolerance = 0.001)
        {
            if (!polyline.IsClosed)
                throw new Exception("The polyline is not closed. IsClockwise method is relevant only to closed curves.");

            List<Point3d> cc = polyline.DiscontinuityPoints(tolerance);
            Vector3d dir1 = (cc[0] - cc.Last()).Normalise();
            Vector3d dir2;
            double angleTot = 0;

            for (int i = 1; i < cc.Count; i++)
            {
                dir2 = (cc[i] - cc[i - 1]).Normalise();
                double signedAngle = dir1.SignedAngle(dir2, normal);
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

        /***************************************************/

        public static Vector3d Normalise(this Vector3d vector)
        {
            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;
            double d = Math.Sqrt(x * x + y * y + z * z);

            if (d == 0)
                return vector.Clone();

            return new Vector3d { X = x / d, Y = y / d, Z = z / d };
        }


        /***************************************************/

        public static Vector3d Clone(this Vector3d vector)
        {
            return new Vector3d { X = vector.X, Y = vector.Y, Z = vector.Z };
        }

        /***************************************************/

        public static double SignedAngle(this Vector3d a, Vector3d b, Vector3d normal)
        {
            double angle = Angle(a, b);

            Vector3d crossproduct = a.CrossProduct(b);
            if (crossproduct.DotProduct(normal) < 0)
                return -angle;
            else
                return angle;
        }
        /***************************************************/

        public static double DotProduct(this Vector3d a, Vector3d b)
        {
            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }

        /***************************************************/

        public static Vector3d CrossProduct(this Vector3d a, Vector3d b)
        {
            return new Vector3d { X = a.Y * b.Z - a.Z * b.Y, Y = a.Z * b.X - a.X * b.Z, Z = a.X * b.Y - a.Y * b.X };
        }

        /***************************************************/

        public static double Angle(this Vector3d v1, Vector3d v2)
        {
            double dotProduct = v1.DotProduct(v2);
            double length = v1.Length * v2.Length;

            return (Math.Abs(dotProduct) < length) ? Math.Acos(dotProduct / length) : (dotProduct < 0) ? Math.PI : 0;
        }

        /***************************************************/

        public static List<Point3d> DiscontinuityPoints(this Polyline curve, double distanceTolerance = 0.001, double angleTolerance = 0.1)
        {
            List<Point3d> ctrlPts = curve.GetControlPoints();

            if (ctrlPts.Count < 3)
                return ctrlPts;

            double sqTol = distanceTolerance * distanceTolerance;
            int j = 0;
            if (!curve.IsClosed)
                j += 2;

            for (int i = j; i < ctrlPts.Count; i++)
            {
                int cc = ctrlPts.Count;
                int i1 = (i - 1 + cc) % cc;
                int i2 = (i - 2 + cc) % cc;
                Vector3d v1 = ctrlPts[i1] - ctrlPts[i2];
                Vector3d v2 = ctrlPts[i] - ctrlPts[i1];
                double angle = v1.Angle(v2);

                if (angle <= angleTolerance || angle >= (2 * Math.PI) - angleTolerance || ctrlPts[i2].SquareDistance(ctrlPts[i1]) <= sqTol)
                {
                    ctrlPts.RemoveAt(i1);
                    i--;
                }
            }

            return ctrlPts;
        }

        /***************************************************/

        public static List<Point3d> GetControlPoints(this Polyline pLine)
        {
                var pts = new List<Point3d>();
                pts = pLine.ToList();
                return pts;
        }

        /***************************************************/

        public static double SquareDistance(this Point3d a, Point3d b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            double dz = a.Z - b.Z;
            return dx * dx + dy * dy + dz * dz;
        }

        /***************************************************/

    }
}
