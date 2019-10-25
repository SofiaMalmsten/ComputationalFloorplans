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
        public string Type { get; set; } = "";
        public int MinAmount { get; set; } = 1;
        public int MaxAmount { get; set; } = 999;
        public int Offset { get; set; } = 0;
        public Point3d MidPoint { get; set; } = new Point3d();
        public Point3d AccessPoint { get; set; } = new Point3d();
        public Polyline GardenBound { get; set; } = new Polyline();
        public Brep HouseGeom { get; set; } = new Brep(); 

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