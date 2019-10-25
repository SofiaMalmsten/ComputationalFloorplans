using System;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.ObjectModel;
using PlotPlanning.Engine.Geometry;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {               

        public static (List<IHouse>, List<PolylineCurve>, List<Carport>) PlaceHouseRow(List<HouseRow> baseRows, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method, Carport carport)
        {
            List<SingleFamily> houseList = new List<SingleFamily>();
            List<Carport> carportList = new List<Carport>();
            List<PolylineCurve> cutBound = new List<PolylineCurve>();

            HouseRow baseRow = baseRows[random.Next(baseRows.Count)]; //1. pick house type to place

            //2. Get boundaries. 
            bound.TryGetPolyline(out Polyline boundPL);
            List<Line> lines = SegmentBounds(boundPL.ClosePolyline(), baseRow);

            if (lines.Count == 0)
                goto end;

            Line currLine = lines.PickLine(method, random, roads, originalBound);
            currLine.Extend(-Tolerance.FilletOffset, -Tolerance.FilletOffset);
            List<Point3d> possiblePts = PossiblePoints(currLine, baseRow, random, carport);

            //3. Place houses for each position
            for (int i = 0; i < possiblePts.Count; i++)
            {
                SingleFamily movedHouse = Adjust.Translate(baseRow, possiblePts[i], currLine.Direction);
                if (i == 0) movedHouse.RowPosition = "left";

                if (Query.IsInside(movedHouse, bound)) //TODO: Include carport
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
            if (houseList.Count >= baseRow.MinAmount)
                cutBound = UpdateBoundaries(houseList, baseRow, bound); //TODO: Include carport
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

       
        }
    }
}