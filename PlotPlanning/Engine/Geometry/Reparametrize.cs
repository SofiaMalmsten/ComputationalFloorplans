using System;
using Rhino.Geometry;
using System.Collections.Generic; 

namespace PlotPlanning.Engine.Geometry
{
    public static partial class Compute
    {
        public static void Reparametrize(Curve c)
        {
            c.Domain = new Interval(0, 1);
        }

        public static void Reparametrize(List<Curve> crvs)
        {
            foreach (Curve c in crvs)
                Reparametrize(c);
        }

        //====================================================================//

    }
}
