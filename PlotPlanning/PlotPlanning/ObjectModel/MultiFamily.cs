using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class MultiFamily: House
    {
        public int MinFloors { get; set; } = 1;
        public int MaxFloors { get; set; } = 2;
        public int MinShift { get; set; } = 0;
        public int MaxShift { get; set; } = 1;
        public int LevelDifference { get; set; } = 1;
        public int LevelHeight { get; set; } = 3;

    }

    //====================================================================

}
