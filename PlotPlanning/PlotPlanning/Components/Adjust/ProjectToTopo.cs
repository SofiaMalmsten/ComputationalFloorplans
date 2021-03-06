﻿using System;
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
        #region Register node
        //====================================================================//
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ProjectToTopo()
          : base("ProjectToTopo", "TProj",
              "Projects houses to the topograpthy but within given displacements between each house in a row",
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
                return Properties.Resources.Adjust;
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
        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Houses", "H", "Houses that will be projected", GH_ParamAccess.list);
            pManager.AddBrepParameter("Topography", "T", "Topography that the houses will be projected onto", GH_ParamAccess.list);
            pManager.AddNumberParameter("PossibleValues", "V", "Allowed values for vertical displacement between houses", GH_ParamAccess.list, new List<double>() { 0 });
        }

 
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Houses", "H", "Projected houses", GH_ParamAccess.list);
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
            List<SingleFamily> houses = new List<SingleFamily>();
            List<Brep> plot = new List<Brep>();
            List<double> possibleValues = new List<double>();

            //Get Data
            if (!DA.GetDataList(0, houses))
                return;
            if (!DA.GetDataList(1, plot))
                return;
            if (!DA.GetDataList(2, possibleValues))
                return;

            //Calculate
            List<SingleFamily> projectedHouses = Methods.Adjust.ProjectToTopo(houses, plot, possibleValues);
           
            //Set data
            DA.SetDataList(0, projectedHouses);
        }

        #endregion
       
    }


}