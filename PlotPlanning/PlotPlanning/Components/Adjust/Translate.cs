﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using PlotPlanning.Methods;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class TranslateTest : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public TranslateTest()
          : base("Translate", "Translate",
              "Translate",
              "PlotPlanningTool", "Adjust")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
            pManager.AddCurveParameter("pline", "pline", "pline", GH_ParamAccess.item);
            pManager.AddPointParameter("basePt", "basePt", "pt1", GH_ParamAccess.item);
            pManager.AddPointParameter("boundPt", "boundPt", "pt2", GH_ParamAccess.item);
            pManager.AddVectorParameter("tan", "tan", "tan", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("pline", "plien", "pline", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //define instances
            Point3d basePt = new Point3d();
            Point3d boundPt = new Point3d();
            Curve crv = new PolylineCurve();
            Vector3d tan = new Vector3d();

            
            //Get data
            if (!DA.GetData(0, ref crv))
                return;
            if (!DA.GetData(1, ref basePt))
                return;
            if (!DA.GetData(2, ref boundPt))
                return;
            if (!DA.GetData(3, ref tan))
                return;

            //Calculate
            Polyline movedPline = Calculate.Translate(crv.CurveToPolyline(), basePt, boundPt, tan);

            //Set data
            DA.SetData(0, movedPline);

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
                return Properties.Resources.Evaluate;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b2e1c9de-48de-4c6f-b3a2-4dab648a5027"); }
        }
    }


}
