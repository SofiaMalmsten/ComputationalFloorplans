using System;
using System.Collections.Generic;
using Rhino.Geometry;
using PlotPlanning.Engine.Base;
using System.Linq;

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

        public static Brep[] Sweep(this Curve centreCrv, double thickness, int CornerStyle)
        {
            //1. Distans div by 2
            double offsetDist = thickness / 2;
            Curve [] offsetCrv1 = centreCrv.Offset(Plane.WorldXY, offsetDist, 1, CurveOffsetCornerStyle.Sharp);
            Curve[] offsetCrv2 = centreCrv.Offset(Plane.WorldXY, -offsetDist, 1, CurveOffsetCornerStyle.Sharp);

            List<Curve> crvList = new List<Curve>();
            crvList.AddRange(offsetCrv1.ToList());
            crvList.AddRange(offsetCrv2.ToList());

            //Brep[] b = Brep.CreateDevelopableLoft(offsetCrv1[0], offsetCrv2[0], false, false, 1);
            Brep[] b = Brep.CreateFromLoft(crvList, Point3d.Unset, Point3d.Unset, LoftType.Normal, false);

            return b;
        }
    }
}
