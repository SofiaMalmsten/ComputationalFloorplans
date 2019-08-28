using System.Reflection;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace PlotPlanning.Methods
{
    public class ReadGeometry
    {
        public static Brep ReadHouseGeometry(string type)
        {
            string test = ReadResourceFile("PlotPlanning.Resources." + type + ".txt");
            GeometryBase geometry = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(test));
            return geometry as Brep;

        }

        //====================================================================

        public static string ReadResourceFile(string filename)
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

    }
}
