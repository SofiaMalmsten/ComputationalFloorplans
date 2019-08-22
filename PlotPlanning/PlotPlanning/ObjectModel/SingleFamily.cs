using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class SingleFamily: House, IHouse
    {

        //====================================================================
        // Properties
        //====================================================================
       
        public bool HasCarPort { get; set; } = false;
        public Vector3d Orientation { get; set; } = new Vector3d();
        public string RowPosition { get; set; } = "";

        //====================================================================
        // Constructors
        //====================================================================

        //Default constructor
        public SingleFamily() { }

        //====================================================================
        public SingleFamily(string type, bool hasCarPort, Polyline gardenBound, Brep houseGeom,
            Point3d accessPoint, int minAmount, int maxAmount, int offset)
        {
            Type = type;
            HasCarPort = hasCarPort;
            GardenBound = gardenBound;
            HouseGeom = houseGeom;
            AccessPoint = accessPoint;
            MinAmount = minAmount;
            Offset = offset;
            MidPoint = gardenBound.CenterPoint();
            MaxAmount = maxAmount;
            //TODO: Add orientation
        }

        //====================================================================

    }

    //====================================================================

}
