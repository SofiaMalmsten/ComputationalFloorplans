using System;
using Rhino.Geometry;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        public static Brep [] Sweep(this Curve centreCrv, double thickness)
        {
            Vector3d tan = centreCrv.TangentAtStart;
            Vector3d projTan = new Vector3d(tan.X, tan.Y, 0);
            Vector3d norm = Engine.Geometry.Compute.CrossProduct(projTan / projTan.Length, Vector3d.ZAxis);

            Line crossSection = new Line(centreCrv.PointAtStart - norm * thickness / 2, norm * thickness);

            Rhino.Geometry.SweepOneRail sweepOne = new SweepOneRail();
            Brep[] b = sweepOne.PerformSweep(centreCrv, crossSection.ToNurbsCurve());

            return b;
        }

        //====================================================================//
    }
}
