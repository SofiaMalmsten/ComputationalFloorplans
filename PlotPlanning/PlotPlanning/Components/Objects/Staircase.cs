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
    public class Staircase : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Staircase()
          : base("Staircase", "StrC",
              "Staircase",
              "PlotPlanningTool", "1.Objects")
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
                return Properties.Resources.Empty;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("044c36b0-c2a9-446b-b800-7f34e0890cc3"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("footprint", "F", "footprint", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Floors", "Fl", "Floors", GH_ParamAccess.item);
            pManager.AddPointParameter("accessPoint", "P", "AccessPoint", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Staircase", "S", "Staircase", GH_ParamAccess.item);
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
            Curve footprint = new PolylineCurve();
            List<Point3d> accessPoint = new List<Point3d>();
            int floors = 0;

            //Get Data
            if (!DA.GetData(0, ref footprint))
                return;
            if (!DA.GetData(1, ref floors))
                return;
            if (!DA.GetDataList(2, accessPoint))
                return;


            //Set properties
            PlotPlanning.ObjectModel.Staircase staircase = new ObjectModel.Staircase();
            staircase.AccessPoints = accessPoint;
            staircase.Footprint = footprint;
            staircase.Floors = floors;

            //Set data
            DA.SetData(0, staircase);
        }

        #endregion
    }
}