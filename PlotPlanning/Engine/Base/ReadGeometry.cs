using System.Reflection;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace PlotPlanning.Engine.Base
{
    public class ReadGeometry
    {
        public static (Brep, Point3d, Rectangle3d) ReadAllHouseGeometry(string houseType)
        {


            string fileName = ReadResourceFile("Engine.Resources." + houseType + ".txt");
            char[] separator = { '$' };

            string[] splitStrings = fileName.Split(separator);
            string brep_string = splitStrings[0];
            string point_string = splitStrings[1];
            string rec_string = splitStrings[2];

            Brep brep = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(brep_string)) as Brep;
            Point point = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(point_string)) as Point;
            Point3d point3d = point.Location; 
            PolylineCurve rec_plc = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(rec_string)) as PolylineCurve;

            Polyline rec_pl = new Polyline();
            rec_plc.TryGetPolyline(out rec_pl);
            Rectangle3d rec =  new Rectangle3d(Plane.WorldXY, rec_pl[0], rec_pl[2]);


            return (brep, point3d, rec);

        }

        //====================================================================//
        public static Brep ReadHouseGeometry(string houseType)
        {
            string fileName = ReadResourceFile("Engine.Resources." + houseType + ".txt");
            GeometryBase geometry = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(fileName));
            return geometry as Brep;
        }

        //============            Private Methods               ==============//        

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
