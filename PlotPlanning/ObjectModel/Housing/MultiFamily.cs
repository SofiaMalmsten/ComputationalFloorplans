using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.ObjectModel
{
    public class MultiFamily : House, IHouse
    {

        #region Properties
        public Brep Geometry { get; set; } = new Brep();
        public int Floors { get; set; } = 1;
        public int Shift { get; set; } = 0;
        public int LevelHeight { get; set; } = 3;
        public Vector3d Orientation { get; set; } = new Vector3d();

        #endregion

        #region Constructors
        public MultiFamily(){}

        public MultiFamily(Polyline centreCrv, double thickness, int floors, double levelHeight)
        {
           
        }

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