﻿using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class Site : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Site()
          : base("GenerateSite", "Site",
              "Generate site",
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
                return Properties.Resources.Site;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8199e623-443c-4caf-8c91-d3e80b658542"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("accessPoint", "P", "accessPt", GH_ParamAccess.item);
            pManager.AddCurveParameter("Boundary", "B", "boundary", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Topography", "T", "gardenBound", GH_ParamAccess.item);
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("site", "S", "site", GH_ParamAccess.item);
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
            Point3d accesspoint = new Point3d();
            Curve bounday = new PolylineCurve();
            Brep topography = new Brep();

            //Get Data
            if (!DA.GetData(0, ref accesspoint))
                return;
            if (!DA.GetData(1, ref bounday))
                return;
            if (!DA.GetData(2, ref topography))
                return;

            //Set properties
            PlotPlanning.ObjectModel.Site site = new ObjectModel.Site();
            site.AccessPoint = accesspoint;
            site.Boundary = bounday;
            site.Topography = topography;


            //Set data
            DA.SetData(0, site);
        }

        #endregion
    }
}
