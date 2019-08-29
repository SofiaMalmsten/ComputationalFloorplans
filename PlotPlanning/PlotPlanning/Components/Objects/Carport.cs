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
    public class Carport : GH_Component //change the name of the component so it doesn't collide with the name of the object
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Carport()
          : base("Carport", "Crp",
              "Carport",
              "PlotPlanningTool", "1.Objects")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddRectangleParameter("gardenBound", "G", "gardenBOund", GH_ParamAccess.item);
            pManager.AddBrepParameter("carportGeom", "C", "carportGeom", GH_ParamAccess.item);
            pManager.AddPointParameter("accessPoint", "P", "accessPoint", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Carport", "C", "Crp", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            Rectangle3d gardenBound = new Rectangle3d();
            Brep carportGeom = new Brep();
            Point3d accessPoint = new Point3d();

            //Get Data
            if (!DA.GetData(0, ref gardenBound))
                return;
            if (!DA.GetData(1, ref carportGeom))
                return;
            if (!DA.GetData(2, ref accessPoint))
                return;


            //Set properties
            PlotPlanning.ObjectModel.Carport carport = new ObjectModel.Carport();
            carport.AccessPoint = accessPoint;
            carport.CarportGeom = carportGeom;
            carport.GardenBound = gardenBound.ToPolyline();

            //Set data
            DA.SetData(0, carport);
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
                return Properties.Resources.CarPort;
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
            get { return new Guid("1f358b10-f626-4d8a-ab05-eb737935abcb"); }
        }
    }


}
