using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using g = Grasshopper.Kernel.Components; 


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
    
        public static List<Polyline> CullSmallAreas(Curve rec, Curve bound)
        {
            Point3d origin = new Point3d(0, 0, 0);
            Vector3d normal = new Vector3d(0, 0, 1);
            Plane p = new Plane(origin, normal);
            List<Polyline> crvList = new List<Polyline>();

            //Calculate start area
            //for (int i = 0; i < rec.Count; i++)
            //{

            Curve[] X = Rhino.Geometry.Curve.CreateBooleanIntersection(rec, bound, 0.001);
            List<Polyline> x = CurvesToPolylines(X); 

            if (x.Count != 0)
            {
            
            
                double a1 = Rhino.Geometry.AreaMassProperties.Compute(X).Area;
                double a2 = Rhino.Geometry.AreaMassProperties.Compute(rec).Area;

                double r1 = Math.Round(a1);
                double r2 = Math.Round(a2);

                if (r1 == r2)
                {
                    crvList.AddRange(x);
                }
            //}
            }
            return crvList;
            
        }

    }

    //====================================================================

}
