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

            double inclination = 0;
            for (int i = 0; i < zValues.Count - 1; i++)
                inclination += Math.Abs(zValues[i] - zValues[i + 1]);

            inclination /= street.GetLength();

            return inclination;
        }

        //====================================================================//

        public static double StreetInclination(this List<Curve> streets, double distanceBetweenPoints = 1)
        {
            double inclination = 0;
            double totalLength = 0;

            foreach (Curve street in streets)
            {
                Point3d[] evaluationPoints = street.DivideEquidistant(distanceBetweenPoints);
                List<double> zValues = evaluationPoints.Select(x => x.Z).ToList();

                for (int i = 0; i < zValues.Count - 1; i++)
                    inclination += Math.Abs(zValues[i] - zValues[i + 1]);

                totalLength += street.GetLength();
            }

            inclination /= totalLength;
            return inclination;

        }
    }
}


