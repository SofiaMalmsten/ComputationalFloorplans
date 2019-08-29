using System.Reflection;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace PlotPlanning.Engine.Base
{
    public class ReadGeometry
    {
        public static Brep ReadHouseGeometry(string houseType)
        {
            string fileName = ReadResourceFile("Engine.Resources." + houseType + ".txt");
            GeometryBase geometry = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(fileName));
            return geometry as Brep;

        }

        //====================================================================//

        private static string ReadResourceFile(string filename)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            using (var stream = thisAssembly.GetManifestResourceStream(filename))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        //====================================================================//

    }
}
