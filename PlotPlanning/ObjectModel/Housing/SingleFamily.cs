using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.ObjectModel
{
    public class SingleFamily : House, IHouse
    {
        #region Properties
        public string Type { get; set; } = "";
        public bool HasCarPort { get; set; } = false;
        public Vector3d Orientation { get; set; } = new Vector3d();
        public Polyline Garden { get; set; } = new Polyline();
        public Point3d ReferencePoint { get; set; } = new Point3d();
        public Point3d AccessPoint { get; set; } = new Point3d();
        public Brep HouseGeometry { get; set; } = new Brep();
        public double Width { get; set; } = 0;
        public Carport Carport { get; set; } = null;

        #endregion

        #region Constructors
        public SingleFamily() { }

        //====================================================================//

        public SingleFamily(string type, bool hasCarPort, Rectangle3d garden, Point3d referencePoint, Point3d accessPoint, Brep houseGeometry, Vector3d orientation, Carport carport)
        {
            Type = type;
            HasCarPort = hasCarPort;
            Garden = garden.ToPolyline(); 
            ReferencePoint = referencePoint;
            AccessPoint = accessPoint;
            HouseGeometry = houseGeometry;
            Orientation = orientation;
            Width = Math.Abs(garden.Corner(0).DistanceTo(garden.Corner(1)) + (carport != null ? carport.Width : 0));
            if (carport != null)
            {
                Garden = ExpandRectangleWidth(garden, carport.Width).ToPolyline();
                Vector3d moveVector = Vector3d.CrossProduct(orientation, Vector3d.ZAxis);
                moveVector.Unitize();                
                AccessPoint += new Point3d(moveVector*carport.Width);
                Carport = carport;
                Carport.AccessPoint = new Point3d(AccessPoint);
            }
            
        }

        #endregion

        #region Public methods
        //TODO:Add constructors
        #endregion

        #region Private methods
        private static Rectangle3d ExpandRectangleWidth(Rectangle3d rec, double width)
        {
            rec.MakeIncreasing();
            Point3d pt1 = rec.Corner(1);
            pt1 = new Point3d(pt1.X + width, pt1.Y, pt1.Z);
            Point3d pt2 = rec.Corner(3);
            Rectangle3d expandedRectangle = new Rectangle3d(new Plane(rec.Center, Vector3d.ZAxis), pt1, pt2);

            return expandedRectangle;
        }

        //====================================================================//
    }
    #endregion
}

