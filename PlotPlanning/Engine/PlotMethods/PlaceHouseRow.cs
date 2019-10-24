using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.ObjectModel;
using PlotPlanning.Engine.Geometry;
using PlotPlanning.Engine.Base; 


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static (List<IHouse>, List<PolylineCurve>, List<Carport>) IPlaceHouseRow(List<IHouse> baseHouses, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method, Carport carport)
        {
            if (baseHouses[0].GetType() == new SingleFamily().GetType()) //TODO: Replace this if statement with interface method
                return PlaceHouseRow(baseHouses.Cast<SingleFamily>().ToList(), bound, originalBound, roads, random, method, carport);

            else
                return PlaceHouseRow(baseHouses.Cast<MultiFamily>().ToList(), bound, originalBound, roads, random, method, carport);
        }

        //====================================================================//

        public static (List<IHouse>, List<PolylineCurve>, List<Carport>) PlaceHouseRow(List<SingleFamily> baseHouses, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method, Carport carport)
        {
            List<SingleFamily> houseList = new List<SingleFamily>();
            List<Carport> carportList = new List<Carport>();
            List<PolylineCurve> cutBound = new List<PolylineCurve>();

            SingleFamily baseHouse = baseHouses[random.Next(baseHouses.Count)]; //1. pick house type to place

            //2. Get boundaries. 
            bound.TryGetPolyline(out Polyline boundPL);
            List<Line> lines = SegmentBounds(boundPL.ClosePolyline(), baseHouse);

            if (lines.Count == 0)
                goto end;

            Line currLine = lines.PickLine(method, random, roads, originalBound);
            currLine.Extend(-Tolerance.FilletOffset, -Tolerance.FilletOffset);
            List<Point3d> possiblePts = PossiblePoints(currLine, baseHouse, random, carport);

            //3. Place houses for each position
            for (int i = 0; i < possiblePts.Count; i++)
            {
                SingleFamily movedHouse = new SingleFamily(); 
                if (houseList.Count == 0)
                {
                    SingleFamily rightHouse = baseHouse.Clone(); 
                    string type = baseHouse.Type.Replace("M", "R");
                    (Brep newHouseGeometry, Point3d referencePoint, Rectangle3d garden) = ReadGeometry.ReadAllHouseGeometry(type);
                    garden = PlotPlanning.Engine.Geometry.Convert.ExpandRectangle(garden, baseHouse.Front, baseHouse.Back);
                    rightHouse.GardenBound = garden.ToPolyline();
                    rightHouse.AccessPoint = garden.Corner(1);
                    rightHouse.HouseGeom = newHouseGeometry;
                    rightHouse.RowPosition = "right";
                    rightHouse.Type = type;
                    movedHouse = Adjust.Translate(rightHouse, possiblePts[i], currLine.Direction);
                }
                else
                {
                    movedHouse = Adjust.Translate(baseHouse, possiblePts[i], currLine.Direction);
                }

                if (Query.IsInside(movedHouse, bound)) //TODO: Include carport
                    houseList.Add(movedHouse);
                if (houseList.Count != 0 && (!Query.IsInside(movedHouse, bound) || i == possiblePts.Count-1)) //already places houses
                {
                    if (!Query.IsInside(movedHouse, bound))
                        houseList.RemoveAt(i - 1); 
                    SingleFamily leftHouse = baseHouse.Clone();
                    string type = baseHouse.Type.Replace("M", "L");
                    (Brep newHouseGeometry, Point3d referencePoint, Rectangle3d garden) = ReadGeometry.ReadAllHouseGeometry(type);
                    garden = PlotPlanning.Engine.Geometry.Convert.ExpandRectangle(garden, baseHouse.Front, baseHouse.Back);
                    leftHouse.GardenBound = garden.ToPolyline();
                    leftHouse.AccessPoint = garden.Corner(1);
                    leftHouse.HouseGeom = newHouseGeometry;
                    leftHouse.RowPosition = "left";
                    leftHouse.Type = type;
                    movedHouse = Adjust.Translate(leftHouse, possiblePts[i], currLine.Direction);
                    houseList.Add(movedHouse); 
                    break;
                }
                    

                if (movedHouse.HasCarPort)
                {
                    i++;
                    Carport movedCarport = Adjust.Translate(carport, possiblePts[i], currLine.Direction);
                    carportList.Add(movedCarport);
                }
            }

        end:
            if (houseList.Count >= baseHouse.MinAmount)
                cutBound = UpdateBoundaries(houseList, baseHouse, bound); //TODO: Include carport
            else
            {

                cutBound = new List<PolylineCurve>() { bound.ToPolylineCurve() };
                houseList = new List<SingleFamily>();
                carportList = new List<Carport>();
            }
            houseList.AddRowPos();
            List<IHouse> IHouseList = houseList.Cast<IHouse>().ToList();
            return (IHouseList, cutBound, carportList);
        }

        //====================================================================//

        public static (List<IHouse>, List<PolylineCurve>, List<Carport>) PlaceHouseRow(List<MultiFamily> baseHouses, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method, Carport carport)
        {
            List<MultiFamily> houseList = new List<MultiFamily>();
            List<Carport> carportList = new List<Carport>();
            List<PolylineCurve> cutBound = new List<PolylineCurve>();

            MultiFamily baseHouse = baseHouses[random.Next(baseHouses.Count)]; //1. pick house type to place

            //2. Get boundaries. 
            bound.TryGetPolyline(out Polyline boundPL);
            List<Line> lines = SegmentBounds(boundPL.ClosePolyline(), baseHouse);

            if (lines.Count == 0)
                goto end;

            Line currLine = lines.PickLine(method, random, roads, originalBound);
            currLine.Extend(-Tolerance.FilletOffset, -Tolerance.FilletOffset);
            List<Point3d> possiblePts = PossiblePoints(currLine, baseHouse, random, carport);

            //3. Place houses for each position
            for (int i = 0; i < possiblePts.Count; i++)
            {
                MultiFamily movedHouse = Adjust.Translate(baseHouse, possiblePts[i], currLine.Direction);

                if (Query.IsInside(movedHouse, bound))
                    houseList.Add(movedHouse);
                else if (houseList.Count != 0) //already places houses
                    break;
            }

        end:
            if (houseList.Count >= baseHouse.MinAmount)
                cutBound = UpdateBoundaries(houseList, baseHouse, bound); //TODO: Include carport
            else
            {

                cutBound = new List<PolylineCurve>() { bound.ToPolylineCurve() };
                houseList = new List<MultiFamily>();
                carportList = new List<Carport>();
            }

            List<IHouse> IHouseList = houseList.Cast<IHouse>().ToList();
            return (IHouseList, cutBound, carportList);
        }
    }
}