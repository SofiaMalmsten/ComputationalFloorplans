using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class CaptureImage : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CaptureImage()
          : base("CaptureImage", "CaptureImage",
              "CaptureImage",
              "PlotPlanningTool", "Other")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("PathToFile", "pathToFile", "type hint: string", GH_ParamAccess.item);
            pManager.AddTextParameter("FileName", "FileName", "type hint: string", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Transparent", "Transparent", "transparent background or not", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Activate", "Activate", "type hint: boolean /n true or false", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Success", "Success", "Success", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        /// 

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            string PathToFIle = "";
            string FileName = "";
            bool Transparent = false;
            bool Activate = false;



            //Get Data
            if (!DA.GetData(0, ref PathToFIle))
                return;
            if (!DA.GetData(1, ref FileName))
                return;
            if (!DA.GetData(2, ref Transparent))
                return;
            if (!DA.GetData(3, ref Activate))
                return;

            //Calculate
            string A = PlotPlanning.Methods.Generate.CaptureImage(PathToFIle, FileName, Transparent, Activate);

            //Set data
            DA.SetData(0, A);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                return Properties.Resources.Capture;
                //return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2d450a9b-604f-4931-a436-097a468d04e7"); }
        }
    }


}
