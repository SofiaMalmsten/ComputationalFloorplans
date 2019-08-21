using System;
using System.Collections.Generic;
using pp = PlotPlanning.Methods;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.ObjectModel;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        //IHouse
        public static void IPlaceHouseRow(List<IHouse> baseHouses, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method, Carport carport, out List<IHouse> houseList, out List<PolylineCurve> cutBound, out List<Carport> carportList)
        {

            PlaceHouseRow(baseHouses as dynamic, bound, originalBound, roads, random, method, carport, out houseList, out cutBound, out carportList);
            //throw new NotImplementedException();

        }

        
        //SFH
        public static void PlaceHouseRow(List<SingleFamily> baseHouses, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method, Carport carport, out List<SingleFamily> houseList, out List<PolylineCurve> cutBound, out List<Carport> carportList)
        {
            houseList = new List<SingleFamily>();
            carportList = new List<Carport>();

            SingleFamily baseHouse = baseHouses[random.Next(baseHouses.Count)]; //1. pick house type to place

            //2. Get boundaries. 
            bound.TryGetPolyline(out Polyline boundPL);
            List<Line> lines = SegmentBounds(boundPL.ClosePolyline(), baseHouse);

            if (lines.Count == 0)
                goto end;

            Line currLine = lines.PickLine(method, random, roads, originalBound);
            currLine.Extend(-FilletOffset(), -FilletOffset());
            List<Point3d> possiblePts = PossiblePoints(currLine, baseHouse, random, carport);

            //3. Place houses for each position
            for (int i = 0; i < possiblePts.Count; i++)
            {
                SingleFamily movedHouse = Adjust.Translate(baseHouse, possiblePts[i], currLine.Direction);

                if (IsInside(movedHouse, bound)) //TODO: Include carport
                    houseList.Add(movedHouse);
                else if (houseList.Count != 0) //already places houses
                    break;

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

                cutBound = new List<PolylineCurve>() { bound.CurveToPolylineCurve() };
                houseList = new List<SingleFamily>();
                carportList = new List<Carport>();
            }
        }

        //==============
        //MFH
        public static void PlaceHouseRow(List<MultiFamily> baseHouses, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method, Carport carport, out List<MultiFamily> houseList, out List<PolylineCurve> cutBound, out List<Carport> carportList)
        {
            houseList = new List<MultiFamily>();
            carportList = new List<Carport>();

            MultiFamily baseHouse = baseHouses[random.Next(baseHouses.Count)]; //1. pick house type to place

            //2. Get boundaries. 
            bound.TryGetPolyline(out Polyline boundPL);
            List<Line> lines = SegmentBounds(boundPL.ClosePolyline(), baseHouse);

            if (lines.Count == 0)
                goto end;

            Line currLine = lines.PickLine(method, random, roads, originalBound);
            currLine.Extend(-FilletOffset(), -FilletOffset());
            List<Point3d> possiblePts = PossiblePoints(currLine, baseHouse, random, carport);

            //3. Place houses for each position
            for (int i = 0; i < possiblePts.Count; i++)
            {
                MultiFamily movedHouse = Adjust.Translate(baseHouse, possiblePts[i], currLine.Direction);

                if (IsInside(movedHouse, bound)) //TODO: Include carport
                    houseList.Add(movedHouse);
                else if (houseList.Count != 0) //already places houses
                    break;
            }

        end:
            if (houseList.Count >= baseHouse.MinAmount)
                cutBound = UpdateBoundaries(houseList, baseHouse, bound); //TODO: Include carport
            else
            {

                cutBound = new List<PolylineCurve>() { bound.CurveToPolylineCurve() };
                houseList = new List<MultiFamily>();
                carportList = new List<Carport>();
            }
        }
    }
}
