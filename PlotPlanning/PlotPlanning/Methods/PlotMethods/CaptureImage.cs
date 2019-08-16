using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Display;


namespace PlotPlanning.Methods
{
    public static partial class Generate
    {
        public static string CaptureImage(string pathToFile, string fileName, bool transparent, bool activate)
        {
            string result = "Failure";

            if (activate == false)
                return "Not Activated";
            else if (pathToFile == "")
                return "Path does not exist";
            else if (!System.IO.Directory.Exists(pathToFile)) 
                return "Path does not exist";


            RhinoView view = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;
              if (null == view)
                return result;

            var view_capture = new ViewCapture
            {
                Width = view.ActiveViewport.Size.Width,
                Height = view.ActiveViewport.Size.Height,
                ScaleScreenItems = false,
                DrawAxes = false,
                DrawGrid = false,
                DrawGridAxes = false,
                TransparentBackground = transparent,
            };

            System.Drawing.Bitmap bitmap = view_capture.CaptureToBitmap(view);

          if (null != bitmap)
          {
                if (!pathToFile.EndsWith("\\"))
                    pathToFile = pathToFile + "\\";
                
                var filename = pathToFile + fileName + ".png";
                bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                    return "Success";
          }
            return result;
        }
    }
  }

 //====================================================================



