using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;
using System.Linq;
using Rhino.Geometry.Intersect;
using PlotPlanning.Methods; 


namespace PlotPlanning.Methods
{
    public static partial class Adjust
    {
        public static List<SingleFamily> ProjectToTopo(List<SingleFamily> houseList, List<Brep> plot, List<double> possibleValues)
        {
            
            List<SingleFamily> projectedHuses = new List<SingleFamily>();
            
            for (int j = -1; j < houseList.Count-1;)
            {
                List<Point3d> accessPts = new List<Point3d>();
                List<Point3d> planePts = new List<Point3d>();
                List<Point3d> surfacePts = new List<Point3d>();
                List<SingleFamily> currList = new List<SingleFamily>();

                while (j < houseList.Count-1)
                {
                    j++;
                    currList.Add(houseList[j]);                    
                    if (houseList[j].RowPosition == "right" || houseList[j].RowPosition == "freestanding") break;
                }
                accessPts = currList.Select(x => x.AccessPoint).ToList();
                planePts = new List<Point3d>();
                surfacePts = Intersection.ProjectPointsToBreps(plot, accessPts, Vector3d.ZAxis, Generate.DistanceTol()).ToList();
                if (surfacePts.Count != accessPts.Count) throw new Exception("The surface has to be directly below or above the houses you are tyring to project."); 
                double firstZ = surfacePts[0].Z;
                foreach (Point3d pt in accessPts)
                {
                    Point3d planePt = pt.Clone();
                    planePt.Z = firstZ;
                    planePts.Add(planePt); 
                }               

                List<Point3d> projectPts = Calculate.SnapToTopo(planePts, surfacePts, possibleValues);

                for (int k = 0; k < currList.Count; k++)
                {
                    Vector3d moveVec = Calculate.createVector(accessPts[k], projectPts[k]);                    
                    projectedHuses.Add(Move(currList[k], moveVec));
                }
            }
            return projectedHuses; 





        }

        //====================================================================      

    }
}


