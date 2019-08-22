﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class Orientation : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Orientation()
          : base("Orientation", "Orientation",
              "Calculates the orentation for each house on the plot",
              "PlotPlanningTool", "Evaluate")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("SFH", "SFH", "SFH", GH_ParamAccess.list);
            pManager.AddVectorParameter("ReferenceVector", "RefVec", "RefVec", GH_ParamAccess.item);
            pManager.AddVectorParameter("NorthVector", "NorthVec", "NorthVEc", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Mean", "Mean", "Mean", GH_ParamAccess.item); //mean value of dot product
            pManager.AddNumberParameter("Variance", "Variance", "Variance", GH_ParamAccess.item);
            pManager.AddNumberParameter("Distubution", "Distrubution", "Distrubution", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            List<ObjectModel.SingleFamily> SFH = new List<ObjectModel.SingleFamily>();
            Vector3d nortVec = new Vector3d();
            Vector3d refVec = new Vector3d();

            //Get Data
            if (!DA.GetDataList(0, SFH))
                return;
            if (!DA.GetData(1, ref refVec))
                return;
            if (!DA.GetData(2, ref nortVec))
                return;

            //Calculate
            List<double> distrList = new List<double>();
            List<double> dotProdList = new List<double>();

            foreach (var s in SFH)
            {
                Vector3d houseVec = s.Orientation;
                double angle = Methods.Calculate.DotProduct(houseVec, refVec);
                dotProdList.Add(angle);
            }

            double average = dotProdList.Average();
            double variance = dotProdList.Average(v=>Math.Pow(v-average,2));

            //Set data
            DA.SetData(0, average);
            DA.SetData(1, variance);
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
                return Properties.Resources.Houses;
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
            get { return new Guid("b02e53b7-ec4b-479d-bc48-7e43e1799c0f"); }
        }
    }


}
