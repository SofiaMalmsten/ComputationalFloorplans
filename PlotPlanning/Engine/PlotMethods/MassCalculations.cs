using System;
using System.Collections.Generic;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;
using System.Linq;
using PlotPlanning.Engine.Geometry;
using Rhino.Geometry.Intersect; 


namespace PlotPlanning.Methods
{
    public static partial class Evaluate
    {
        public static Dictionary<string, double> MassBalance(SingleFamily house, Surface site, int divisions)
        {
            double tol = Tolerance.Distance;
            Brep gardenBrep = Brep.CreatePlanarBreps(new[] { house.GardenBound.ToPolylineCurve() }, tol)[0];
            Surface gardenSrf = gardenBrep.Surfaces[0];
            double stackArea = AreaMassProperties.Compute(gardenSrf, true, false, false, false).Area/(divisions * divisions); 
            
            double cut = 0;
            double fill = 0;
            double massBalance = 0;

            List<Point3d> srfPts = Query.SurfaceGrid(gardenSrf, divisions, divisions);

            foreach (Point3d pt in srfPts)
            {
                Ray3d ray = new Ray3d(pt, Vector3d.ZAxis);

                Point3d[] projectedPt = Intersection.RayShoot(ray, new Surface[] { site }, 1);
                if (projectedPt == null)
                {
                    ray = new Ray3d(pt, -Vector3d.ZAxis);
                    projectedPt = Intersection.RayShoot(ray, new Surface[] { site }, 1);
                    if (projectedPt != null)
                        cut += (pt.Z - projectedPt[0].Z);
                }
                else
                    fill += (pt.Z - projectedPt[0].Z);
            }
            
            //TODO: Change cut to fill and vice versa
            fill = fill*stackArea;
            cut = cut*stackArea;
            massBalance = cut - fill;

            Dictionary<string, double> values = new Dictionary<string, double>();
            values.Add("cut", cut);
            values.Add("fill", fill);
            values.Add("massBalance", massBalance);
            return values; 
        }

        //====================================================================//


    }
}