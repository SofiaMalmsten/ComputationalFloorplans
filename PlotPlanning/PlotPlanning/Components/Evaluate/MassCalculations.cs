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
    public class MassCalculations : GH_Component
    {

        #region Register Node
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

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.MassCalculations;
            }
        }


        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("dfd8d506-3ac5-4785-8515-472179549439"); }
        }
        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("House", "H", "The house to evaluate.", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Topology", "T", "The topology of the site to evaluate.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Divisions", "D", "The resolution with which the calculation is made", GH_ParamAccess.item, 10); 
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Cut", "C", "The amount of soil having to be cut away.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Fill", "F", "The amout of soil having to be added.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Mass balance", "Mb", "The net product of the cut and fill.", GH_ParamAccess.item);
        }
        #endregion

        #region Solution
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            ObjectModel.SingleFamily house = new ObjectModel.SingleFamily();
            Surface site = null;
            int divisions = 0; 
            

            //Get Data
            if (!DA.GetData(0, ref house))
                return;
            if (!DA.GetData(1, ref site))
                return;
            if (!DA.GetData(2, ref divisions))
                return;

            //Calculate
            Dictionary<string, double> values = PlotPlanning.Methods.Evaluate.MassBalance(house, site, divisions); 

            //Set data            
            DA.SetData(0, values["cut"]); 
            DA.SetData(1, values["fill"]);
            DA.SetData(2, values["massBalance"]); 

        }
        #endregion

    }
}
