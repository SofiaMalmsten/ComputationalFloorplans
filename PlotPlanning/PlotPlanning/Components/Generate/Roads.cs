using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using PlotPlanning.Engine.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class Roads : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Roads()
          : base("Roads", "Road",
              "Generates streets",
              "PlotPlanningTool", "Generate")
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
                return Properties.Resources.roads;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6b676486-a9d3-4851-86b2-4cc9c429d863"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("planePts", "P", "line to place accesspoints on", GH_ParamAccess.list);
            pManager.AddPointParameter("topoPts", "T", "min amount of houses in a row", GH_ParamAccess.list);
            pManager.AddNumberParameter("possibleValues", "P", "max amount of houses in a row", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("projectedPts", "P", "projected Points", GH_ParamAccess.list);
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
            List<Point3d> topoPts = new List<Point3d>();
            List<Point3d> planePts = new List<Point3d>();
            List<double> possibleValues = new List<double>();

            //Get Data
            if (!DA.GetDataList(0, planePts))
                return;
            if (!DA.GetDataList(1, topoPts))
                return;
            if (!DA.GetDataList(2, possibleValues))
                return;

            //Calculate
            List<Point3d> projectedPts = Adjust.AttractTo(topoPts, planePts, possibleValues);
           
            //Set data
            DA.SetDataList(0, projectedPts);
        }

        #endregion
    }
}
