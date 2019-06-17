using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
    
        public static List<Curve> CullSmallAreas(List<Curve> A, Curve B)
        {
            Point3d origin = new Point3d(0, 0, 0);
            Vector3d normal = new Vector3d(0, 0, 1);
            Plane p = new Plane(origin, normal);
            List<Curve> crvList = new List<Curve>();

            //Calculate start area
            for (int i = 0; i < A.Count; i++)
            {

                Curve[] x = Rhino.Geometry.Curve.CreateBooleanIntersection(A[i], B, 0.001);


                double a1 = Rhino.Geometry.AreaMassProperties.Compute(x).Area;
                double a2 = Rhino.Geometry.AreaMassProperties.Compute(A[i]).Area;

                double r1 = Math.Round(a1);
                double r2 = Math.Round(a2);


                if (r1 == r2)
                {
                    crvList.AddRange(x.ToList());
                }
            }
            
            return crvList;
        }
    }

    //====================================================================

}
