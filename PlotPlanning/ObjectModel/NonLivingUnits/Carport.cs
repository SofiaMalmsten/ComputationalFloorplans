using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Carport
    {
        #region Properties
        public Polyline GardenBound { get; set; } = new Polyline();
        public Brep CarportGeom { get; set; } = new Brep();
        public Point3d AccessPoint { get; set; } = new Point3d();
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
