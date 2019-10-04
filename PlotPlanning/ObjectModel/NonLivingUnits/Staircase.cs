using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PlotPlanning.ObjectModel
{
    public class Staircase
    {
        #region Properties
        public Curve LandingPerimeter { get; set; } = new PolylineCurve();
        public Curve StairCasePerimeter { get; set; } = new PolylineCurve();

        #endregion

        #region Constructors

        public Staircase() { }

        //==================================================

        /// <summary>
        /// The staircase consists of the landing geometry and the shaft for the starir + evelvator.
        /// These are represented by rectangles and are created by dimensions widht and height. width is along backbone, height is perpedicular.
        /// </summary>
        public Staircase(Plane pl, Interval widthStair, Interval heightStarir, Interval widthLanding, Interval heightLanding, bool flip = true)
        {
            StairCasePerimeter = (new Rectangle3d(pl, widthStair, heightStarir)).ToNurbsCurve();

            double factor = (heightStarir.Length + heightLanding.Length) / 2;
            if (flip)
                factor *= -1;

            Plane p = pl.Clone();
            p.Translate(pl.YAxis * factor);

            LandingPerimeter = (new Rectangle3d(p, widthLanding, heightLanding)).ToNurbsCurve();
        }

        //==================================================

        public Staircase(Plane pl, Interval widthStair, Interval heightStarir, Interval widthLanding, Interval heightLanding, Vector3d dir)
        {
            StairCasePerimeter = (new Rectangle3d(pl, widthStair, heightStarir)).ToNurbsCurve();

            double factor = (heightStarir.Length + heightLanding.Length) / 2;
            if (dir == null || dir.IsZero)
                dir = pl.YAxis;

            Plane p = pl.Clone();
            p.Translate(dir/dir.Length * factor);

            LandingPerimeter = (new Rectangle3d(p, widthLanding, heightLanding)).ToNurbsCurve();
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