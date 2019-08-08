using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq; 


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static Curve PickLargest(this List<Curve> crvList)
        {
      
            return crvList.OrderBy(x => AreaMassProperties.Compute(x).Area).ToList().Last(); 
        }
        public static Curve PickLargest(this Curve[] crvList)
        {

            return crvList.OrderBy(x => AreaMassProperties.Compute(x).Area).ToList().Last();
        }
    }
}

    //====================================================================
