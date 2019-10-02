using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.Engine.Base
{
    public static partial class Query
    {
        //====================================================================//
        public static bool listEqualsIgnoreOrder(List<Line> list1, List<Line> list2)
        {
            return new HashSet<Line>(list1).SetEquals(new HashSet<Line>(list2));
        }

        //====================================================================//

    }
}
