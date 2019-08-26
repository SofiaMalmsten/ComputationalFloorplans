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
        public static List<SingleFamily> ProjectToTopo(List<SingleFamily> houseList, Brep plot, List<double> possibleValues)
        {
            List<Brep> plotList = new List<Brep> { plot }; 
            List<SingleFamily> projectedHuses = new List<SingleFamily>();

            //int j = 0;
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
                planePts = new List<Point3d>(); // (accessPts);
                surfacePts = Intersection.ProjectPointsToBreps(plotList, accessPts, Vector3d.ZAxis, Generate.DistanceTol()).ToList();
                double firstZ = surfacePts[0].Z;
                foreach (Point3d pt in accessPts)
                {
                    Point3d planePt = pt.Clone();
                    planePt.Z = firstZ;
                    planePts.Add(planePt); 
                }
                //planePts.ForEach(x => x.Z = firstZ);

                List<Point3d> projectPts = Calculate.SnapToTopo(planePts, surfacePts, possibleValues);

                for (int k = 0; k < currList.Count; k++)
                {
                    Vector3d moveVec = Calculate.createVector(accessPts[k], projectPts[k]);
                    //moveVec += Calculate.createVector()
                    projectedHuses.Add(Move(currList[k], moveVec));
                }
            }
            return projectedHuses; 





        }

        //====================================================================      

    }
}


