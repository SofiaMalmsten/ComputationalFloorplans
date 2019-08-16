using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class MFHComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public MFHComponent()
          : base("MultiFamilyHouse", "MultiFamilyHouse",
              "MultiFamilyHouse",
              "PlotPlanningTool", "1.Objects")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("tag", "tag", "tag", GH_ParamAccess.item);
            pManager.AddIntegerParameter("minFloor", "minFloor", "Minimum amount of floors", GH_ParamAccess.item);
            pManager.AddIntegerParameter("maxFloor", "maxFloor", "Maximum amount of floors", GH_ParamAccess.item);
            pManager.AddIntegerParameter("minShift", "minShift", "Minimum horisontal shift", GH_ParamAccess.item);
            pManager.AddIntegerParameter("maxShift", "maxShift", "Maximum horisontal shift", GH_ParamAccess.item);
            pManager.AddIntegerParameter("leveDifference", "levelDifference", "Difference in height between units in the same block", GH_ParamAccess.item);
            pManager.AddIntegerParameter("levelHeight", "levelHeight", "Height between levels", GH_ParamAccess.item);
            pManager.AddBrepParameter("houseGeom", "houseGeom", "houseGeom", GH_ParamAccess.item);
    }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("MultiFamilyHouse", "MFH", "MFH", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            string tag = "";
            int minFloor = 1;
            int maxFloor = 1;
            int minShift = 1;
            int maxShift = 1;
            int levelDifference = 1;
            int levelHeight = 1;
            Brep houseGeom = new Brep();

            //Get Data
            if (!DA.GetData(0, ref tag))
                return;
            if (!DA.GetData(1, ref minFloor))
                return;
            if (!DA.GetData(2, ref maxFloor))
                return;
            if (!DA.GetData(3, ref minShift))
                return;
            if (!DA.GetData(4, ref maxShift))
                return;
            if (!DA.GetData(5, ref levelDifference))
                return;
            if (!DA.GetData(6, ref levelHeight))
                return;
            if (!DA.GetData(7, ref houseGeom))
                return;


            //Set properties
            PlotPlanning.ObjectModel.MultiFamily house = new ObjectModel.MultiFamily();
            house.Tag = tag;
            house.minFloors = minFloor;
            house.maxFloors = maxFloor;
            house.minShift = minShift;
            house.maxShift = maxShift;
            house.levelDifference = levelDifference;
            house.levelHeight = levelHeight;
            house.houseGeom = houseGeom;

            //Set data
            DA.SetData(0, house);
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
                return Properties.Resources.MFH;
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
            get { return new Guid("d921a823-6059-4f89-8326-7341ff81a8c1"); }
        }
    }


}
