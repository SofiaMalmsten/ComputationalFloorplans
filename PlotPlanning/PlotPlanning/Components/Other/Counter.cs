﻿using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class Counter : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Counter()
          : base("Counter", "Countr",
              "counter",
              "PlotPlanningTool", "Other")
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
                return Properties.Resources.Counter;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2b088e34-ec05-4547-abc5-f7772f9f3ff5"); }
        }
        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("reset", "R", "default false", GH_ParamAccess.item);
            pManager.AddBooleanParameter("run", "R", "default false", GH_ParamAccess.item);
            pManager.AddIntegerParameter("steps", "S", "default 1", GH_ParamAccess.item);
            pManager.AddIntegerParameter("start", "S", "default 0", GH_ParamAccess.item);
            pManager.AddIntegerParameter("stop", "S", "default 9999", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("int", "I", "int", GH_ParamAccess.item);
        }

        #endregion

        #region Solution
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        /// 

        int n = 0;
        int A = 0;

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            bool reset = false;
            bool run = false;
            int steps = 1;
            int start = 0;
            int stop = 9999;

            //Get Data
            if (!DA.GetData(0, ref reset))
                return;
            if (!DA.GetData(1, ref run))
                return;
            if (!DA.GetData(2, ref steps))
                return;
            if (!DA.GetData(3, ref start))
                return;
            if (!DA.GetData(4, ref stop))
                return;

            //Calculate
            if (reset)
                n = start;
            else if (run)
            {
                n = n + steps;

                if (n > stop)
                {
                    n = start;
                }

                A = n;
            }

            //Set data
            DA.SetData(0, A);
        }

        #endregion
    }
}
