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
        public static ObjectModel.SingleFamily Convert(this ObjectModel.SingleFamily house)
        {
            ObjectModel.SingleFamily newHouse = new ObjectModel.SingleFamily();
            newHouse.Type = house.Type;
            newHouse.HouseGeom = house.HouseGeom.DuplicateBrep();

            return newHouse;
        }

        /***************************************************/
    }
}
