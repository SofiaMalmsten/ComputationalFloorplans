using System;
using System.Collections.Generic;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;
using System.Linq;
using Rhino.Geometry.Intersect;
using PlotPlanning.Engine.Geometry;
using PlotPlanning.Engine.Base;


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
                surfacePts = Intersection.ProjectPointsToBreps(plot, accessPts, Vector3d.ZAxis, Tolerance.Distance).ToList();
                if (surfacePts.Count != accessPts.Count) throw new Exception("The surface has to be directly below or above the houses you are tyring to project. Seems like the houses are outside the topography."); 
                double firstZ = surfacePts[0].Z;
                foreach (Point3d pt in accessPts)
                {
                    Point3d planePt = pt.Clone();
                    planePt.Z = firstZ;
                    planePts.Add(planePt); 
                }

                
                //If 1 m is a possible displacement we want to move the house either -1 or 1 depending on the topography
                List<double> possibleVals = Modify.MirrorList(possibleValues);
                List<Point3d> projectPts = Engine.Geometry.Adjust.AttractTo(surfacePts, planePts, possibleVals);

                for (int k = 0; k < currList.Count; k++)
                {
                    Vector3d moveVec = Compute.CreateVector(accessPts[k], projectPts[k]);                    
                    projectedHuses.Add(Move(currList[k], moveVec));
                }
            }
            return projectedHuses;
        }

        //====================================================================//

    }
}


