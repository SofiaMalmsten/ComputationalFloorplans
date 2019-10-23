using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Engine.Base
{
    public static partial class Modify
    {
        public static double ClosestValue(double valueToCheck, List<double> possibleValues)
        {
           
            double displacement = 0;

            if (possibleValues.Count == 1) //if we only have one possible value that value will always be the closest value. 
                displacement = possibleValues[0];
            else
            {
                for (int i = 0; i < possibleValues.Count - 1; i++)
                {
                    if (valueToCheck == possibleValues[i])
                        displacement = possibleValues[i];
                    else if (valueToCheck > possibleValues[i] && valueToCheck < possibleValues[i + 1])
                    {
                        double halfDistance = (possibleValues[i + 1] - possibleValues[i]) / 2;
                        double difference = possibleValues[i] + halfDistance;
                        if (valueToCheck < difference)
                            displacement = possibleValues[i];
                        else
                            displacement = possibleValues[i + 1];
                    }
                    else if (valueToCheck >= possibleValues.Max())
                        displacement = possibleValues.Max();
                    else if (valueToCheck <= possibleValues.Min())
                        displacement = possibleValues.Min();
                }
            }
            return displacement;
        }

        //====================================================================//

    }
}
