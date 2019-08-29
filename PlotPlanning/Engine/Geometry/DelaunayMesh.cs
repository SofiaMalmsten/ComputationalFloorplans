using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        public static Mesh DelaunayMesh(List<Point3d> points)
        {
            var nodes = new Grasshopper.Kernel.Geometry.Node2List();

            for (int i = 0; i < points.Count; i++)
                nodes.Append(new Grasshopper.Kernel.Geometry.Node2(points[i].X, points[i].Y));  //notice how we only read in the X and Y coordinates, this is why points should be mapped onto the XY plane

            var faces = Grasshopper.Kernel.Geometry.Delaunay.Solver.Solve_Faces(nodes, 1);
            Mesh delMesh = Grasshopper.Kernel.Geometry.Delaunay.Solver.Solve_Mesh(nodes, 1, ref faces);

            return delMesh;
        }
    }

    //====================================================================// 

}
