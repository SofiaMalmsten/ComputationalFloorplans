using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.ObjectModel
{
    public class Staircase
    {
        #region Properties
        public Curve Footprint { get; set; } = new PolylineCurve();
        public List<Point3d> AccessPoints { get; set; } = new List<Point3d>();
        public int Floors { get; set; } = 1;
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