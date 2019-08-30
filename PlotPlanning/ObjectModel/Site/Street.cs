using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.ObjectModel
{
    public class Street
    {

        #region Properties
        public Curve CentreCurve { get; set; } = new PolylineCurve();
        public double Width { get; set; } = 1;
        public double CornerFillet { get; set; } = 0;
        public string type { get; set; } = "Primary";

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
