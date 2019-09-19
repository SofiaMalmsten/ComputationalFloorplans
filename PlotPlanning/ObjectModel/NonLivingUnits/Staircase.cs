using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.ObjectModel
{
    public class Staircase
    {
        #region Properties
        public Curve TotalPerimeter { get; set; } = new PolylineCurve();
        public Curve LandingPerimeter { get; set; } = new PolylineCurve();
        public Curve StairCasePerimeter { get; set; } = new PolylineCurve();
        public double uStair { get; set; } = 1;
        public double vStair { get; set; } = 1;
        public double uLanding { get; set; } = 1;
        public double vLanding { get; set; } = 1;
        #endregion

        #region Constructors
        //TODO:Add constructors

        public Staircase() { }

        //==================================================
        public Staircase(Plane pl, double u, double v)
        {
                Interval uInt = new Interval(-u / 2, u / 2);
                Interval vInt = new Interval(-v / 2, v / 2);
                Rectangle3d rec = new Rectangle3d(pl, uInt, vInt);

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