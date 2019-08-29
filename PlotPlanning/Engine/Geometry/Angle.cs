using System;
using Rhino.Geometry;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        public static double SignedAngle(this Vector3d v1, Vector3d v2, Vector3d normal)
        {
            double angle = Angle(v1, v2);

            Vector3d crossproduct = v1.CrossProduct(v2);
            if (crossproduct.DotProduct(normal) < 0)
                return -angle;
            else
                return angle;
        }

        //====================================================================//

        public static double SignedAngle(this Vector3d v1, Vector3d v2, Plane plane)
        {
            Vector3d normal = plane.Normal;
            return v1.SignedAngle(v2, normal);
        }

        //====================================================================//
        public static double Angle(this Vector3d v1, Vector3d v2)
        {
            double dotProduct = v1.DotProduct(v2);
            double length = v1.Length * v2.Length;

            return (Math.Abs(dotProduct) < length) ? Math.Acos(dotProduct / length) : (dotProduct < 0) ? Math.PI : 0;
        }

        //====================================================================//
    }
}
