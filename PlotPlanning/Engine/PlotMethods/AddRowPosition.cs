using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using PlotPlanning.ObjectModel;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static void AddRowPos(this List<SingleFamily> houseList)
        {
            if (houseList.Count == 1)
            {
                houseList[0].RowPosition = "freestanding";
            }
            else if (houseList.Count != 0)
            {
                for (int i = 0; i < houseList.Count; i++)
                {
                    if (houseList[i].Type == "A") houseList[i].RowPosition = "freestanding"; //this is just to make it work for now. Should be changed so that every house type can be freestanding.
                    else if (i == 0) houseList[i].RowPosition = "left";
                    else if (i == houseList.Count - 1) houseList[i].RowPosition = "right";
                    else houseList[i].RowPosition = "middle";

                }
            }

        }
        //====================================================================


    }
}


