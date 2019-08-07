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
        public static ObjectModel.House Convert(this ObjectModel.House house)
        {
            ObjectModel.House newHouse = new ObjectModel.House();
            newHouse.Type = house.Type;
            newHouse.houseGeom = house.houseGeom.DuplicateBrep();

            return newHouse;
        }

        /***************************************************/
    }
}
