﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.ObjectModel;


namespace PlotPlanning.Methods
{
    public static partial class Adjust
    {            
        public static ObjectModel.SingleFamily Move(SingleFamily house, Vector3d vector)
        {            

            SingleFamily movedHouse = house.Clone();
            Transform t = Transform.Translation(vector); 

            movedHouse.GardenBound.Transform(t);
            movedHouse.HouseGeom.Transform(t);
            movedHouse.AccessPoint = movedHouse.AccessPoint + new Point3d(vector);
            movedHouse.MidPoint = movedHouse.MidPoint + new Point3d(vector);

            return movedHouse;
        }

        //================================

        public static ObjectModel.Carport Move(Carport carport, Vector3d vector)
        {

            Carport movedCarport = carport.Clone();
            Transform t = Transform.Translation(vector);

            movedCarport.gardenBound.Transform(t);
            movedCarport.carportGeom.Transform(t);
            movedCarport.accessPoint = movedCarport.accessPoint + new Point3d(vector);

            return movedCarport;
        }
    }
}
