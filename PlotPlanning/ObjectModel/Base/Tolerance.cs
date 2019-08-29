using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public static class Tolerance
    {
        #region Properties
        public static double Distance { get; } = 0.01;
        public static double Angle { get; } = 0.01;
        public static double Garden { get; } = 0.01;
        public static double FilletOffset { get; } = 2;
        public static double Area { get; } = 0.0001;
        #endregion 
    }

    //====================================================================//
}
