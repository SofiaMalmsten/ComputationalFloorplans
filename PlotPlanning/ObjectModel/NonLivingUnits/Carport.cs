using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class Carport
    {
        #region Properties
        public Polyline Garden { get; set; } = new Polyline();
        public Brep CarportGeometry { get; set; } = new Brep();
        public Point3d AccessPoint { get; set; } = new Point3d();
        public double Width { get; set; } = 0;
        public Point3d ReferencePoint { get; set; } = new Point3d();
        #endregion

        public Carport() { }
        #region Constructors
        public Carport(Rectangle3d garden, Brep carportGeometry, Point3d referencePoint)
        {
            Garden = garden.ToPolyline();
            CarportGeometry = carportGeometry;
            AccessPoint = garden.PointAt(1); 
            Width = Math.Abs(garden.Corner(0).DistanceTo(garden.Corner(1)));
            ReferencePoint = referencePoint;
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
