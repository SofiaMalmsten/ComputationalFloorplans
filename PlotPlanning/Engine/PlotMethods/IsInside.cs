﻿using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Query
    {
        public static bool IsInside(ObjectModel.SingleFamily sfh, Curve bound)
        {
            Curve garden = Curve.CreateControlPointCurve(sfh.GardenBound.ToList(), 1);
            return IsInside(garden, bound);
        }

        //====================================================================//

        public static bool IsInside(ObjectModel.Carport sfh, Curve bound)
        {
            Curve garden = Curve.CreateControlPointCurve(sfh.GardenBound.ToList(), 1);
            return IsInside(garden, bound);
        }

        //====================================================================//

        public static bool IsInside(ObjectModel.MultiFamily mfh, Curve bound)
        {
            Curve garden = Curve.CreateControlPointCurve(mfh.GardenBound.ToList(), 1);
            return IsInside(garden, bound);
        }
    }

    //====================================================================//

}