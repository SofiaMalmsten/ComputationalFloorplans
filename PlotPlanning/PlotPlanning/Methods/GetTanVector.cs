using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {

        public static List<Vector3d> GetTanVect(List<Point3d> Points, Line line)
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

            //Set data for the outputs
            return tanList;
        }
    }
    //====================================================================

}
