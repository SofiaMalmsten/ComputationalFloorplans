using System;
using System.Collections.Generic;
using System.Linq;

namespace PlotPlanning.Engine.Base
{
    public partial class Modify
    {
        public static List<double> MirrorList(List<double> inpList)
        {
            List<double> outpList = new List<double>();
            inpList.Sort();
            inpList.Reverse();

            //Add negative values
            foreach (var item in inpList)
                outpList.Add(-item);

            //Add 0
            if (outpList.Last()!=0)
                outpList.Add(0);
      
            //Add positive values
            foreach (var item in inpList)
                    outpList.Add(item);

            return outpList;
        }
    }

    //====================================================================//
}
