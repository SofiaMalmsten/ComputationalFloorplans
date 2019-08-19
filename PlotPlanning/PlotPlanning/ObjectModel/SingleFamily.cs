using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class SingleFamily: House
    {

        //====================================================================
        // Properties
        //====================================================================
       
        public bool HasCarPort { get; set; } = false;
        public Polyline GardenBound { get; set; } = new Polyline();
        public Vector3d Orientation { get; set; } = new Vector3d();
        public Point3d AccessPoint { get; set; } = new Point3d();
        public string RowPosition { get; set; } = "";

        //====================================================================
        // Constructors
        //====================================================================

        //Default constructor
        public SingleFamily()
        {

        }

        //====================================================================
        public SingleFamily(string type, bool hasCarPort, Polyline gardenBound, Brep houseGeom, Vector3d orientation,
            Point3d accessPoint, int minAmount, int maxAmount, int offset, string rowPosition)
        {
            Type = type;
            HasCarPort = hasCarPort;
            GardenBound = gardenBound;
            HouseGeom = houseGeom;
            Orientation = orientation;
            AccessPoint = accessPoint;
            MinAmount = minAmount;
            Offset = offset;
            RowPosition = rowPosition;
        }

        //====================================================================

    }

    //====================================================================

}
