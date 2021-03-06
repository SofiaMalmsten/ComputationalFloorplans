﻿using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.ObjectModel
{
    public class House : IHouse
    {
        #region Properties
        public string Type { get; set; } = "";
        public Brep HouseGeom { get; set; } = new Brep();
        public int MinAmount { get; set; } = 1;
        public int MaxAmount { get; set; } = 999;
        public int Offset { get; set; } = 0;
        public Point3d MidPoint { get; set; } = new Point3d();
        public Point3d AccessPoint { get; set; } = new Point3d();
        public Polyline GardenBound { get; set; } = new Polyline();
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