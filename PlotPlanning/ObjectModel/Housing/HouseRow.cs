using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.ObjectModel
{
    public class HouseRow
    {
        #region Properties
        public List<SingleFamily> Houses { get; set; } = new List<SingleFamily>();
        public int MinAmount { get; set; } = 1;
        public int MaxAmount { get; set; } = int.MaxValue;
        public double Offset { get; set; } = 0; 
        #endregion

        #region Constructors
        public HouseRow() { }

        //====================================================================//

        public HouseRow(List<SingleFamily> houses, int minAmount, int maxAmount, double offset)
        {
            Houses = houses;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            Offset = offset;
        }

        #endregion

        #region Public methods
        //TODO:Add constructors
        #endregion

        #region Private methods
        //TODO:Add constructors
        #endregion
    }

    //====================================================================//

}
