/*using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class Regulations : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Regulations()
          : base("GenerateRegulations", "GenerateRegulations",
              "Generate regulations",
              "PlotPlanningTool", "Objects")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("minAmount", "minAmount", "minAmount", GH_ParamAccess.item);
            pManager.AddIntegerParameter("maxAmount", "maxAmount", "maxAmount", GH_ParamAccess.item);
            pManager.AddIntegerParameter("offset", "offset", "offset", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("regulations", "regulations", "regulations", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            int minAmount = 0;
            int maxAmount = 999;
            int offset = 0;

            //Get Data
            if (!DA.GetData(0, ref minAmount))
                return;
            if (!DA.GetData(1, ref maxAmount))
                return;
            if (!DA.GetData(2, ref offset))
                return;

            //Set properties
            PlotPlanning.ObjectModel.Regulation regulation = new ObjectModel.Regulation();
            regulation.MinAmount = minAmount;
            regulation.MaxAmount = maxAmount;
            regulation.Offset = offset;

            //Set data
            DA.SetData(0, regulation);
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
                return Properties.Resources.SnapToTopo;
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
            get { return new Guid("e3e64100-c5d8-41ce-b139-0ce2e60d7844"); }
        }
    }


}
*/