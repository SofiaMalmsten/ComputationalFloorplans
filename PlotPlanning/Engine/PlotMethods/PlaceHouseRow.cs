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

        public static (HouseRow, List<PolylineCurve>, List<Carport>) PlaceHouseRow(List<HouseRow> baseRows, Curve bound, Curve originalBound, List<Curve> roads, Random random, string method, Carport carport)
        {

            List<Carport> carportList = new List<Carport>();
            List<PolylineCurve> cutBound = new List<PolylineCurve>();

            HouseRow baseRow = baseRows[random.Next(baseRows.Count)]; //1. pick house type to place
            HouseRow createdHouseRow = baseRow.Clone();
            createdHouseRow.Houses = new List<SingleFamily>();

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
                SingleFamily movedHouse = new SingleFamily();
                if (createdHouseRow.Houses.Count == 0)
                {
                    movedHouse = Adjust.Translate(baseRow.Houses[0].Clone(), possiblePts[i], currLine.Direction);
                }
                else if (i < possiblePts.Count - 1)
                {
                    movedHouse = Adjust.Translate(baseRow.Houses[1].Clone(), possiblePts[i], currLine.Direction);
                }
                else
                {
                    movedHouse = Adjust.Translate(baseRow.Houses[2].Clone(), possiblePts[i], currLine.Direction);
                }

                if (Query.IsInside(movedHouse, bound)) //TODO: Include carport
                    createdHouseRow.Houses.Add(movedHouse);
                else if (createdHouseRow.Houses.Count != 0) //already places houses
                {
                    createdHouseRow.Houses.RemoveAt(createdHouseRow.Houses.Count - 1);
                    movedHouse = Adjust.Translate(baseRow.Houses[2].Clone(), possiblePts[i - 1], currLine.Direction);
                    createdHouseRow.Houses.Add(movedHouse);
                }
                if (baseRow.Houses.Count == 1) break;
            }

        end:
            if (createdHouseRow.Houses.Count >= baseRow.MinAmount)
                cutBound = UpdateBoundaries(createdHouseRow, baseRow, bound); //TODO: Include carport
            else
            {

                cutBound = new List<PolylineCurve>() { bound.ToPolylineCurve() };
                createdHouseRow = new HouseRow();
                carportList = new List<Carport>();
            }
            return (createdHouseRow, cutBound, carportList);
        }

        //====================================================================//


    }
}
