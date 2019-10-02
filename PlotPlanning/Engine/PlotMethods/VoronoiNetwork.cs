using System;
using System.Collections.Generic;
using PlotPlanning.ObjectModel;
using Rhino.Geometry;
using Rhino.NodeInCode;
using System.Linq; 

namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static void VoronoiNetwork(List<Point3d> voronoiPoints, Curve offsetBoundary)
        {
            


        }

        //====================================================================//

        private static List<Curve> Voronoi(List<Point3d> pts)
        {
            ComponentFunctionInfo voronoi = Components.FindComponent("Voronoi");

            object[] arguments = new object[]
              {(object) pts,
              };

            string[] warnings;
            object[] result = voronoi.Evaluate(arguments, false, out warnings);

            IList<object> resultList = (IList<object>)result[0];
            List<Curve> crv_list = resultList.Select(x => (Curve)x).ToList();

            return crv_list;
        }
    }
}


