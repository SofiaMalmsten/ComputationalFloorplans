using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.ObjectModel
{
    public class MultiFamily : House, IHouse
    {

        #region Properties
        public int Floors { get; set; } = 1;
        public double Thickness { get; set; } = 0;
        public double LevelHeight { get; set; } = 3;
        public Vector3d Orientation { get; set; } = new Vector3d();

        #endregion

        #region Constructors
        public MultiFamily(){}

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