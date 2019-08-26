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

            int i = 0; 
            List<Point3d> asseccPts = new List<Point3d>(); 
            List<Point3d> planePts = new List<Point3d>();
            List<Point3d> surfacePts = new List<Point3d>(); 

            List<SingleFamily> currList = new List<SingleFamily>();
            for (i < houseList.Count; i++)
            {
                currList.Add(houseList[i]);
                if (houseList[i].RowPosition == "left" || houseList[i].RowPosition == "freestanding") break; 
            }
            asseccPts = houseList.Select(x => x.AccessPoint).ToList();
            asseccPts.CopyTo(surfacePts.ToArray());
            asseccPts.CopyTo(planePts.ToArray());
            surfacePts.ForEach(x => Intersection.ProjectPointsToBreps(plotList, asseccPts, Vector3d.ZAxis, PlotPlanning.Methods.Generate.DistanceTol()));
            planePts.ForEach(x => x.Z = 0);

            List<Point3d> projectPts = PlotPlanning.Methods.Calculate.SnapToTopo(planePts, surfacePts, possibleValues); 
           
                for (int i = 0; i < currList.Count; i++)
            {
                Vector3d moveVec = PlotPlanning.Methods.Calculate.createVector(asseccPts[i], projectPts[i]);
                projectedHuses.Add(Move(currList[i], moveVec)); 
            }

            return projectedHuses; 





        }

        //====================================================================      

    }
}


