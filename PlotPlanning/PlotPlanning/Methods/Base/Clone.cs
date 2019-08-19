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
                AccessPoint = new Point3d(house.AccessPoint),
                MinAmount = house.MinAmount,
                Offset = house.Offset,
                RowPosition = house.RowPosition
            };
        }

        /***************************************************/
    }
}
