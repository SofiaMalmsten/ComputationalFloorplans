using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class MassCalculations : GH_Component
    {
        //====================================================================//
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public MassCalculations()
          : base("MassCalculations", "MCalc",
              "Calculates volumes for cut, fill and mass balance",
              "PlotPlanningTool", "Evaluate")
        {
        }

        //====================================================================//
        #region Outputs
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("SFH", "S", "SFH", GH_ParamAccess.list);
        }

        //====================================================================//
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("NumberOfHouses", "N", "Number Of Houses", GH_ParamAccess.item);
        }

        //====================================================================//
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            List<ObjectModel.SingleFamily> SFH = new List<ObjectModel.SingleFamily>();

            //Get Data
            if (!DA.GetDataList(0, SFH))
                return;

            //Calculate
            int numerOfHouses = SFH.Count;
           
            //Set data
            DA.SetData(0, numerOfHouses);
        }

        //====================================================================//
        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Empty;
            }
        }

        //====================================================================//
        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("dfd8d506-3ac5-4785-8515-472179549439"); }
        }
        //====================================================================//
    }


}
