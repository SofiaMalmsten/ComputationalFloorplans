using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Calculate
    {
        //========================================================
        //Get Closest Value
        //========================================================
        public static double getClosestValue(double valueToCheck, List<double> possibleValues)
        {
            double displ = 0;
            for (int i = 0; i < possibleValues.Count - 1; i++)
            {
                if (valueToCheck > possibleValues[i] && valueToCheck < possibleValues[i + 1])
                {
                    double halfDistance = (possibleValues[i + 1] - possibleValues[i]) / 2;
                    double difference = possibleValues[i] + halfDistance;
                    if (valueToCheck < difference)
                    {
                        displ = possibleValues[i];
                    }
                    else
                    {
                        displ = possibleValues[i + 1];
                    }
                }
                else if (valueToCheck >= possibleValues.Max())
                {
                    displ = possibleValues.Max();
                }
                else if (valueToCheck <= possibleValues.Min())
                {
                    displ = possibleValues.Min();
                }
            }
            return displ;
        }
    }
}
