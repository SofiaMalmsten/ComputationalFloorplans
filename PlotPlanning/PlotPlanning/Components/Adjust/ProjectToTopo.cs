using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using PlotPlanning.ObjectModel; 

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class ProjectToTopo : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ProjectToTopo()
          : base("ProjectToTopo", "ProjectToTopo",
              "projects houses to the topograpthy",
              "PlotPlanningTool", "Adjust")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("houses", "houses", "the houses you want to project", GH_ParamAccess.list);
            pManager.AddBrepParameter("topology", "topology", "the topology you want to project the houses onto", GH_ParamAccess.item);
            pManager.AddNumberParameter("possibleValues", "possibleValues", "the alowed valuses for difference in vertical placement for two neighbouring houses", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("houses", "houses", "the projected houses", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            List<SingleFamily> houses = new List<SingleFamily>();
            Brep plot = new Brep();
            List<double> possibleValues = new List<double>();

            //Get Data
            if (!DA.GetDataList(0, houses))
                return;
            if (!DA.GetData(1, ref plot))
                return;
            if (!DA.GetDataList(2, possibleValues))
                return;

            //Calculate
            List<SingleFamily> projectedHouses = PlotPlanning.Methods.Adjust.ProjectToTopo(houses , plot, possibleValues);
           
            //Set data
            DA.SetDataList(0, projectedHouses);
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
            get { return new Guid("15e6459f-611a-424c-926c-c528a240847b"); }
        }
    }


}
