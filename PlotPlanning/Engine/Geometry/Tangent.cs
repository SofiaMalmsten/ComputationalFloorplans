using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        public static List<Vector3d> Tangent(List<Point3d> points, Line line)
        {
            List<Vector3d> tanList = new List<Vector3d>();

            for (int i = 0; i < points.Count; i++)
                tanList.Add(line.UnitTangent);

            return tanList;
        }

        //====================================================================//
    }

}
