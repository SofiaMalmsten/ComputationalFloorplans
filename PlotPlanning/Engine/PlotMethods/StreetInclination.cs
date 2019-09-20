using System;
using System.Collections.Generic;
using PlotPlanning.ObjectModel;
using Rhino.Geometry;
using System.Linq;

namespace PlotPlanning.Methods
{
    public static partial class Evaluate
    {
        public static double StreetInclination(this Curve street, double distanceBetweenPoints = 1)
        {
            Point3d[] evaluationPoints = street.DivideEquidistant(distanceBetweenPoints);           
            List<double> zValues = evaluationPoints.Select(x => x.Z).ToList();
            int ptCount = zValues.Count;

            double inclination = 0;
            for (int i = 0; i < ptCount - 1; i++)
                inclination += Math.Abs(zValues[i] - zValues[i + 1]);

            inclination /= ((ptCount-1) * distanceBetweenPoints);

            double avgAngle = Math.Asin(inclination) * 180 / Math.PI;
            return avgAngle;
        }

        //====================================================================//

        public static double StreetInclination(this List<Curve> streets, double distanceBetweenPoints = 1)
        {
            double inclination = 0;
            int totalPtCount = 0;

            foreach (Curve street in streets)
            {
                Point3d[] evaluationPoints = street.DivideEquidistant(distanceBetweenPoints);
                List<double> zValues = evaluationPoints.Select(x => x.Z).ToList();
                int ptCount = zValues.Count;

                for (int i = 0; i < ptCount - 1; i++)
                    inclination += Math.Abs(zValues[i] - zValues[i + 1]);

                totalPtCount += ptCount; 
            }

            inclination /= ((totalPtCount-1)*distanceBetweenPoints);

            double avgAngle = Math.Asin(inclination)*180/ Math.PI;             
            return avgAngle;
        }
    }
}


