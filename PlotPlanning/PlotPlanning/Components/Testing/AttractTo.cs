using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper.Kernel;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class AttractTo : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public AttractTo()
          : base("AttractTo", "AttractTo",
              "AttractTo",
              "PlotPlanningTool", "Testing")
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
            get { return new Guid("6b158ab0-8bc1-4dd0-93da-b3e8d232703d"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "points to move", GH_ParamAccess.list);
            pManager.AddPointParameter("AttractorPoint", "A", "attractor points", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "T", "Toleance", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Point", GH_ParamAccess.list);
        }

        #endregion

        #region Solution
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        /// 

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            List<Point3d> pts = new List<Point3d>();
            List<Point3d> attractors = new List<Point3d>();
            double tol = 0.1;
            List<Point3d> movedPt = new List<Point3d>();

            //Get Data
            if (!DA.GetDataList(0, pts))
                return;
            if (!DA.GetDataList(1, attractors))
                return;
            if (!DA.GetData(2, ref tol))
                return;

            //Calculate
            movedPt = PlotPlanning.Engine.Geometry.Adjust.AttractTo(pts, attractors, tol);

            //Set data
            DA.SetDataList(0, movedPt);
        }

        #endregion
    }
}
