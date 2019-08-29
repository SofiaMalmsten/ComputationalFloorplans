using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Site
    {
        #region Properties
        public Point3d AccessPoint { get; set; } = new Point3d();
        public Curve Boundary { get; set; } = new PolylineCurve();
        public Brep Topography { get; set; } = new Brep();
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
