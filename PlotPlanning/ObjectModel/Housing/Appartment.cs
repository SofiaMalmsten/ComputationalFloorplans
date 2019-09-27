using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.ObjectModel
{
    public class Appartment
    {
        #region Properties
        public Curve Perimeter { get; set; } = new PolylineCurve();
        public Point3d AccessPoint { get; set; } = new Point3d();
        public int Rooms { get; set; } = 1;
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