using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static Curve OffsetOut(this Curve curve, double distance, Plane plane, CurveOffsetCornerStyle cornerStyle = CurveOffsetCornerStyle.Sharp)
        {

            if (distance == 0) return curve; 


            double original_area = Rhino.Geometry.AreaMassProperties.Compute(curve).Area;
            Curve[] offset_1 = curve.Offset(plane, distance, DistanceTol(),cornerStyle);
            if (offset_1 != null)
            {
                double area_1 = Rhino.Geometry.AreaMassProperties.Compute(offset_1[0]).Area;
                if (area_1 > original_area)
                    return offset_1[0]; 
                else
                    return curve.Offset(plane, -distance, DistanceTol(), cornerStyle)[0];
            }
            else
                return curve.Offset(plane, -distance, DistanceTol(), cornerStyle)[0]; 
        }
        
    }
}

    //====================================================================
