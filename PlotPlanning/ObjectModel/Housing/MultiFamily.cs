using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.ObjectModel
{
    public class MultiFamily : House, IHouse
    {

        #region Properties
        public int MinFloors { get; set; } = 1;
        public int MaxFloors { get; set; } = 2;
        public int MinShift { get; set; } = 0;
        public int MaxShift { get; set; } = 1;
        public int LevelDifference { get; set; } = 1;
        public int LevelHeight { get; set; } = 3;
        public Vector3d Orientation { get; set; } = new Vector3d();

        #endregion

        #region Constructors
        //TODO:Add constructors
        #endregion

        #region Public methods
        //TODO:Add constructors
        #endregion

        #region Private methods
        //TODO:Add constructors
        #endregion

    }

    //====================================================================//

}