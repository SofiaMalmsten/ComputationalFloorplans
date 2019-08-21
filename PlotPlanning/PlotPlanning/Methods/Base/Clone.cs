using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Methods
{
    public static partial class Calculate
    {
        /***************************************************/
        public static Vector3d Clone(this Vector3d vector)
        {
            return new Vector3d { X = vector.X, Y = vector.Y, Z = vector.Z };
        }

        /***************************************************/

        public static Point3d Clone(this Point3d point)
        {
            return new Point3d { X = point.X, Y = point.Y, Z = point.Z };
        }

        /***************************************************/

        public static Brep Clone(this Brep brep)
        {
            return brep.DuplicateBrep();
        }

        /***************************************************/

        public static ObjectModel.SingleFamily Clone(this ObjectModel.SingleFamily house)
        {
            return new ObjectModel.SingleFamily
            {
                Type = house.Type,
                HasCarPort = house.HasCarPort,
                GardenBound = house.GardenBound.Duplicate(),
                HouseGeom = house.HouseGeom.Clone(),
                Orientation = new Vector3d(house.Orientation),
                AccessPoint = house.AccessPoint.Clone(),
                MinAmount = house.MinAmount,
                MaxAmount = house.MaxAmount,
                Offset = house.Offset,
                RowPosition = house.RowPosition,
                MidPoint = house.MidPoint.Clone(),                
            };
        }

        /***************************************************/

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

        /***************************************************/


        public static ObjectModel.Carport Clone(this ObjectModel.Carport carport)
        {
            return new ObjectModel.Carport
            {
                AccessPoint = carport.AccessPoint,
                GardenBound = carport.GardenBound.Duplicate(),
                CarportGeom = carport.CarportGeom.Clone(),
            };
        }
    }
}
