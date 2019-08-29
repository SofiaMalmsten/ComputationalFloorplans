using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;


namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        public static Polyline ConcaveHull(this IEnumerable<Point3d> pts, double factor)
        {
            //1. make mesh and get faces
            Mesh mesh = new Mesh();
            mesh = Compute.DelaunayMesh(pts.ToList());
            mesh.Vertices.AddVertices(pts);


            List<Point3d> ptsList = pts.ToList();
            List<Point3f> vertices = mesh.Vertices.ToList();
            List<MeshFace> faces = mesh.Faces.ToList();

            //2 get max distance of edges of all faces
            List<double> maxDists = new List<double>();

            foreach (var face in faces)
            {
                double dist1 = ptsList[face.A].DistanceTo(ptsList[face.B]);
                double dist2 = ptsList[face.B].DistanceTo(ptsList[face.C]);
                double dist3 = ptsList[face.C].DistanceTo(ptsList[face.A]);
                double maxD = new List<double>() { dist1, dist2, dist3 }.Max();
                maxDists.Add(maxD);
            }


            //3 get median
            List<double> sortedDists = maxDists.OrderBy(d => d).ToList();
            int medIndex = (sortedDists.Count) / 2;
            double median = sortedDists[medIndex];

            //4 keep faces with edges within treshold
            double distTreshold = median * (1.0 + factor);
            Mesh newMesh = new Mesh();
            int i = 0;
            foreach (var face in faces)
            {
                if (maxDists[i] < distTreshold)
                    newMesh.Faces.AddFace(face);
                i += 1;
            }

            //5 create final meshes and outlines
            //Mesh finalMesh = ConstructMesh(vertices, newMesh);
            Mesh finalMesh = new Mesh();
            finalMesh = newMesh;
            finalMesh.Vertices.AddVertices(pts);
            Polyline[] test = finalMesh.GetNakedEdges();
            Polyline concaveHull = finalMesh.GetNakedEdges().ToList()[0].ClosePolyline();

            return concaveHull;
        }

        //====================================================================//
    }
}
