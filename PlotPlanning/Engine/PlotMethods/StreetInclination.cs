﻿using System;
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
            for (int i = 0; i < zValues.Count-1; i++)            
                inclination += Math.Abs(zValues[i] - zValues[i + 1]);

            inclination /= street.GetLength();   

            return inclination; 
        }

        //====================================================================//

    }
}


