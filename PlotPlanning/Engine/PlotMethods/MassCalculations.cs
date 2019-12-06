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
        public static Dictionary<string, double> MassBalance(SingleFamily house, Mesh site, int divisions)
        {
            //Mesh siteMesh = Mesh.CreateFromSurface(site);  
            double tol = Tolerance.Distance;
            Brep gardenBrep = Brep.CreatePlanarBreps(new[] { house.Garden.ToPolylineCurve() }, tol)[0];
            Surface gardenSrf = gardenBrep.Surfaces[0];
            double stackArea = AreaMassProperties.Compute(gardenSrf, true, false, false, false).Area / (divisions * divisions);

            double cut = 0;
            double fill = 0;

            Point3d[,] srfPts = Query.SurfaceGrid(gardenSrf, divisions, divisions);

            foreach (Point3d pt in srfPts)
            {
                Point3d projectedPt = new Point3d();
                Ray3d ray = new Ray3d(pt, Vector3d.ZAxis);
                double rayParameter = Intersection.MeshRay(site, ray);

                if (rayParameter >= 0)
                    projectedPt = ray.PointAt(rayParameter);
                else
                {
                    ray = new Ray3d(pt, -Vector3d.ZAxis);
                    rayParameter = Intersection.MeshRay(site, ray);
                    if (rayParameter >= 0)
                        projectedPt = ray.PointAt(rayParameter);
                }

                if (projectedPt != new Point3d())
                {
                    if ((pt.Z - projectedPt.Z) > 0)
                        fill += (pt.Z - projectedPt.Z);
                    else
                        cut += (projectedPt.Z - pt.Z);
                }
            }

            fill *= stackArea;
            cut *= stackArea;
            double massBalance = cut - fill;

            Dictionary<string, double> values = new Dictionary<string, double>();
            values.Add("cut", cut);
            values.Add("fill", fill);
            values.Add("massBalance", massBalance);
            return values;
        }

        //====================================================================//


    }
}