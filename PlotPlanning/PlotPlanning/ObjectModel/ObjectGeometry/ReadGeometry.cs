using System;
using System.Reflection;
using System.Collections.Generic;
using System.Resources; 

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
    public class ReadGeometry
    {
        //public static Brep ReadHouseGeometry(string type)
        //{
        //    string pt1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //    string path = pt1 + "\\A1.txt"; //only a placeholder for now as I have only one geometry in binary

        //    string A1 = System.IO.File.ReadAllText(path); 
        //    GeometryBase geometry = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(A1));           
        //    return geometry as Brep;

        //}


        public static Brep ReadHouseGeometry(string type)
        {
            string pt1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = pt1 + "\\A1.txt"; //only a placeholder for now as I have only one geometry in binary
            string test = ReadResourceFile("PlotPlanning.Resources.A1.txt"); 
            //string A1 = System.IO.File.ReadAllText(path);
            //string houseString = ReadResourceFile("A1.txt");
            GeometryBase geometry = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(test));
            return geometry as Brep;

        }

        //public static Brep ReadHouseGeometry(string type)
        //{
        //    //string pt1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //    //string path = pt1 + "\\A1.txt"; //only a placeholder for now as I have only one geometry in binary
        //    //string test = Properties.Resources.A;
        //    //string A1 = System.IO.File.ReadAllText(path);
        //    ResourceManager rm = new ResourceManager("items", Assembly.GetExecutingAssembly());
        //    //rm.GetString()
        //    Assembly assembly = Assembly.GetExecutingAssembly(); 
        //    string[] names = assembly.GetManifestResourceNames();
        //    String str = rm.GetString("PlotPlanning.Resources.A1.txt");
        //    GeometryBase geometry = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(str));
        //    return geometry as Brep;
        //}

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

    //====================================================================

}
