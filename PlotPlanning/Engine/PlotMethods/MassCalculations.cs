using System;
using System.Collections.Generic;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;
using System.Linq;
using PlotPlanning.Engine.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Evaluate
    {
        public static Dictionary<string, double> MassBalance(SingleFamily house, Surface site, int divisions)
        {
            double tol = Tolerance.Distance;
            Brep gardenBrep = Brep.CreatePlanarBreps(new[] { house.GardenBound.ToPolylineCurve() }, tol)[0];
            Surface gardenSrf = gardenBrep.Surfaces[0];
            double stackArea = AreaMassProperties.Compute(gardenSrf, true, false, false, false).Area/(divisions ^ 2); 
            
            double cut = 0;
            double fill = 0;
            double massBalance = 0;

            Interval interval = new Interval(0, divisions); 

            gardenSrf.SetDomain(0, interval);
            gardenSrf.SetDomain(1, interval);

            for (int i = 0; i < divisions; i++)
            {
                for (int j = 0; j < divisions; j++)
                {
                    Point3d pt = gardenSrf.PointAt(i, j);
                    Point3d projectedPt = gardenBrep.ClosestPoint(pt);
                    double difference = pt.Z - projectedPt.Z;
                    if (difference < 0) cut += difference;
                    else fill += difference; 
                }
            }

            fill *= stackArea;
            cut *= stackArea;
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