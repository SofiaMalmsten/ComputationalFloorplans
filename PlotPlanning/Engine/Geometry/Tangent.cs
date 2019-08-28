using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {

        public static List<Vector3d> Tangent(List<Point3d> Points, Line line)
        {
            Vector3d unitZ = new Vector3d(0, 0, 1);
            List<Vector3d> tanList = new List<Vector3d>();

            //======================================================
            //Curve closest point
            //======================================================
            for (int i = 0; i < Points.Count; i++)
            {
                Vector3d tan = line.UnitTangent;
                tanList.Add(tan);
            }

            return tanList;
        }
    }
    //====================================================================

}
