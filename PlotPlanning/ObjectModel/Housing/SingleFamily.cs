using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class SingleFamily: House, IHouse
    {
        #region Properties
        public string Type { get; set; } = ""; 
        public bool HasCarPort { get; set; } = false;
        public Vector3d Orientation { get; set; } = new Vector3d();
        public Polyline Garden { get; set; } = new Polyline();
        public Point3d ReferencePoint { get; set; } = new Point3d();
        public Point3d AccessPoint { get; set; } = new Point3d();
        public Brep HouseGeometry { get; set; } = new Brep(); 

        #endregion

        #region Constructors
        public SingleFamily() { }

        //====================================================================//

        public SingleFamily(string type, bool hasCarPort, Polyline garden, Point3d referencePoint, Point3d accessPoint, Brep houseGeometry, Vector3d orientation)
        {
            Type = type;
            HasCarPort = hasCarPort;
            Garden = garden;
            ReferencePoint = referencePoint;
            AccessPoint = accessPoint;
            HouseGeometry = houseGeometry;
            Orientation = orientation;
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
