using System;
using System.Collections.Generic;
using Rhino.Geometry;


namespace PlotPlanning.Engine.Base
{

    public class Compare : IEqualityComparer<Line>
    {
        public int GetHashCode(Line co)
        {
            if (co == null)
            {
                return 0;
            }
            return co.GetHashCode();
        }

        //====================================================================//

        public bool Equals(Line x1, Line x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) ||
                object.ReferenceEquals(x2, null))
            {
                return false;
            }
            return x1.GetHashCode() == x2.GetHashCode();
        }

        //====================================================================//
    }
}


