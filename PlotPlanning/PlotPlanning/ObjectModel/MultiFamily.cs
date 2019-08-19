using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class MultiFamily: House
    {
        public int minFloors { get; set; } = 1;
        public int maxFloors { get; set; } = 2;
        public int minShift { get; set; } = 0;
        public int maxShift { get; set; } = 1;
        public int levelDifference { get; set; } = 1;
        public int levelHeight { get; set; } = 3;

    }

    //====================================================================

}
