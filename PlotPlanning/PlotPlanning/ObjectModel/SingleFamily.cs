using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class SingleFamily
    {

        //====================================================================
        // Properties
        //====================================================================

        public string Type { get; set; } = "";
        public bool HasCarPort { get; set; } = false;
        public Polyline GardenBound { get; set; } = new Polyline();
        public Brep HouseGeom { get; set; } = new Brep();
        public Vector3d Orientation { get; set; } = new Vector3d();
        public Point3d AccessPoint { get; set; } = new Point3d();
        public int MinAmount { get; set; } = 0;
        public int MaxAmount { get; set; } = 999;
        public int Offset { get; set; } = 0;
        public string RowPosition { get; set; } = "";

        //====================================================================
        // Properties
        //====================================================================

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
