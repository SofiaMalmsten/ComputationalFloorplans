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

            int j = 0;
            for (int i = 0; i < houseList.Count; i++)
            {
                List<Point3d> asseccPts = new List<Point3d>();
                List<Point3d> planePts = new List<Point3d>();
                List<Point3d> surfacePts = new List<Point3d>();
                List<SingleFamily> currList = new List<SingleFamily>();

                while (j < houseList.Count)
                {
                    j++;
                    i++; 
                    currList.Add(houseList[i]);
                    if (houseList[j].RowPosition == "left" || houseList[i].RowPosition == "freestanding") break;
                }
                asseccPts = houseList.Select(x => x.AccessPoint).ToList();
                asseccPts.CopyTo(surfacePts.ToArray());
                asseccPts.CopyTo(planePts.ToArray());
                surfacePts.ForEach(x => Intersection.ProjectPointsToBreps(plotList, asseccPts, Vector3d.ZAxis, PlotPlanning.Methods.Generate.DistanceTol()));
                planePts.ForEach(x => x.Z = 0);

                List<Point3d> projectPts = PlotPlanning.Methods.Calculate.SnapToTopo(planePts, surfacePts, possibleValues);

                for (int k = 0; k < currList.Count; k++)
                {
                    Vector3d moveVec = PlotPlanning.Methods.Calculate.createVector(asseccPts[k], projectPts[k]);
                    projectedHuses.Add(Move(currList[k], moveVec));
                }
            }
            return projectedHuses; 





        }

        //====================================================================      

    }
}


