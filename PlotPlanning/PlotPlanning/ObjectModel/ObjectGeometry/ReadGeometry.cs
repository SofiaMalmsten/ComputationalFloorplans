using System;
using System.Reflection;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;
using Rhino.Collections;
using GH_IO;
using GH_IO.Serialization;

using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;


namespace PlotPlanning.ObjectModel.Geometry
{
    public static class ReadGeometry
    {
        public static Brep ReadHouseGeometry(string type)
        {
            string pt1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = pt1 + "\\A1.txt"; //only a placeholder for now as I have only one geometry in binary
            byte[] A1 = System.IO.File.ReadAllBytes(path);
            GeometryBase geometry = GH_Convert.ByteArrayToCommonObject<GeometryBase>(A1);
            return geometry as Brep;
        }

    }

    //====================================================================

}
