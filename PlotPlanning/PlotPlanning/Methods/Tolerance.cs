using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static double DistanceTol()
        {
            return 0.01;
        }
        public static double AngleTol()
        {
            return 0.01;
        }
        public static double GardenTol()
        {
            return 0.1;
        }
        public static double FilletOffset()
        {
            return 2;
        }
    }
}

    //====================================================================
