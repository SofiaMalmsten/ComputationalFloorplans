﻿using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using PlotPlanning.Methods;
using PlotPlanning.ObjectModel; 

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class TranslateHouse : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public TranslateHouse()
          : base("TranslateHouse", "HTrans",
              "Moves a house along a given vector",
              "PlotPlanningTool", "Adjust")
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
            get { return new Guid("effe649f-73c6-4d7f-89f0-a1e96bc37458"); }
        }
        #endregion

        #region Input/Outputs
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
            pManager.AddGenericParameter("House", "H", "House to move", GH_ParamAccess.item);
            pManager.AddVectorParameter("Vector", "V", "Vector to move the house along", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "P", "Point to place the house at", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("House", "H", "Moved house", GH_ParamAccess.item);
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

            //define instances
            SingleFamily house = new SingleFamily();
            Vector3d vec = new Vector3d();
            Point3d pt = new Point3d();
            
            //Get data
            if (!DA.GetData(0, ref house))
                return;
            if (!DA.GetData(1, ref vec))
                return;
            if (!DA.GetData(2, ref pt))
                return;

            //Calculate
            Vector3d tan = Engine.Geometry.Compute.CrossProduct(vec, Vector3d.ZAxis);
            SingleFamily movedHouse = Adjust.Translate(house, pt, tan);

            //Set data
            DA.SetData(0, movedHouse);

        }
        #endregion
    }
}