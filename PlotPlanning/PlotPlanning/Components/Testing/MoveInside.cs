﻿using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper.Kernel;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class MoveInside : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public MoveInside()
          : base("MoveInside", "MoveInside",
              "MoveInside",
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
            get { return new Guid("25e523c8-2855-4412-b05f-20e5b911f153"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "type hint: string", GH_ParamAccess.item);
            pManager.AddVectorParameter("Vector", "V", "type hint: string", GH_ParamAccess.item);
            pManager.AddCurveParameter("Boundary", "B", "boundary", GH_ParamAccess.item);
            pManager.AddLineParameter("RefLine", "L", "ref line", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Point", GH_ParamAccess.item);
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
            Point3d pt = new Point3d();
            Vector3d vec = new Vector3d();
            Curve crv = new PolylineCurve();
            Line ln = new Line();

            //Get Data
            if (!DA.GetData(0, ref pt))
                return;
            if (!DA.GetData(1, ref vec))
                return;
            if (!DA.GetData(2, ref crv))
                return;
            if (!DA.GetData(3, ref ln))
                return;

            //Calculate

            Point3d movedPt = PlotPlanning.Engine.Geometry.Adjust.MoveInside(pt, vec, crv);

            //Set data
            DA.SetData(0, movedPt);
        }

        #endregion
    }
}
