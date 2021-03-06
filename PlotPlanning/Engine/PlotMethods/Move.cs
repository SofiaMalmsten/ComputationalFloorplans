﻿using System;
using System.Collections.Generic;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;
using PlotPlanning.Engine.Base;


namespace PlotPlanning.Methods
{
    public static partial class Adjust
    {            
        public static SingleFamily Move(SingleFamily house, Vector3d vector)
        {            

            SingleFamily movedHouse = house.Clone();
            Transform t = Transform.Translation(vector); 

            movedHouse.GardenBound.Transform(t);
            movedHouse.HouseGeom.Transform(t);
            movedHouse.AccessPoint +=  new Point3d(vector);
            movedHouse.MidPoint += new Point3d(vector);

            return movedHouse;
        }

        //====================================================================//

        public static MultiFamily Move(MultiFamily house, Vector3d vector)
        {

            MultiFamily movedHouse = house.Clone();
            Transform t = Transform.Translation(vector);

            movedHouse.GardenBound.Transform(t);
            movedHouse.HouseGeom.Transform(t);
            movedHouse.AccessPoint += new Point3d(vector);
            movedHouse.MidPoint +=  new Point3d(vector);

            return movedHouse;
        }

        //====================================================================//
        public static ObjectModel.Carport Move(Carport carport, Vector3d vector)
        {
            Carport movedCarport = carport.Clone();
            Transform t = Transform.Translation(vector);

            movedCarport.GardenBound.Transform(t);
            movedCarport.CarportGeom.Transform(t);
            movedCarport.AccessPoint +=  new Point3d(vector);

            return movedCarport;
        }
    }
}
