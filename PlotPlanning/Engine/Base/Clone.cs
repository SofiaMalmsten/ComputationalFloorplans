using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq; 


namespace PlotPlanning.Engine.Base
{
    public static partial class Modify
    {
        //====================================================================//
        public static Vector3d Clone(this Vector3d vector)
        {
            return new Vector3d { X = vector.X, Y = vector.Y, Z = vector.Z };
        }

        //====================================================================//

        public static Point3d Clone(this Point3d point)
        {
            return new Point3d { X = point.X, Y = point.Y, Z = point.Z };
        }

        //====================================================================//

        public static Brep Clone(this Brep brep)
        {
            return brep.DuplicateBrep();
        }

        //====================================================================//

        public static ObjectModel.SingleFamily Clone(this ObjectModel.SingleFamily house)
        {
            return new ObjectModel.SingleFamily
            {
                Type = house.Type,
                HasCarPort = house.HasCarPort,
                Garden = house.Garden.Duplicate(),
                HouseGeometry = house.HouseGeometry.Clone(),
                Orientation = new Vector3d(house.Orientation),
                AccessPoint = house.AccessPoint.Clone(),
                ReferencePoint = house.ReferencePoint.Clone(),
                Width = house.Width,
                Carport = house.Carport.Clone()
            };
        }

        //====================================================================//

        public static ObjectModel.HouseRow Clone(this ObjectModel.HouseRow row)
        {
            if (row == null) return null; 
            return new ObjectModel.HouseRow
            {
                Houses = row.Houses.Select(x => x.Clone()).ToList(),
                Offset = row.Offset,
                MinAmount = row.MinAmount, 
                MaxAmount = row.MaxAmount,
                widthDiff = row.widthDiff                
            };
        }

        //====================================================================//

        public static ObjectModel.MultiFamily Clone(this ObjectModel.MultiFamily house)
        {
            return new ObjectModel.MultiFamily
            {
                Type = house.Type,
                GardenBound = house.GardenBound.Duplicate(),
                HouseGeom = house.HouseGeom.Clone(),
                Orientation = new Vector3d(house.Orientation),
                AccessPoint = house.AccessPoint.Clone(),
                MinAmount = house.MinAmount,
                MaxAmount = house.MaxAmount,
                Offset = house.Offset,
                MidPoint = house.MidPoint.Clone(),
            };
        }

        //====================================================================//

        public static ObjectModel.Carport Clone(this ObjectModel.Carport carport)
        {
            if (carport == null) return null; 
            return new ObjectModel.Carport
            {
                AccessPoint = carport.AccessPoint,
                Garden = carport.Garden.Duplicate(),
                CarportGeometry = carport.CarportGeometry.Clone(),
                ReferencePoint = carport.ReferencePoint.Clone(),
                Width = carport.Width
                
            };
        }

        //====================================================================//
    }
}
