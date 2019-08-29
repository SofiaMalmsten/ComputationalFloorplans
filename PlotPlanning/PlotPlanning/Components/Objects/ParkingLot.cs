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
    public class ParkingLot : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ParkingLot()
          : base("ParkingLot", "Prk",
              "ParkingLot",
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
                return Properties.Resources.ParkingLot;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ffe5484e-03bf-4c0c-a9ae-9f97a9487f59"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("type", "T", "type", GH_ParamAccess.item);
            pManager.AddCurveParameter("carportBound", "B", "carportBound", GH_ParamAccess.item);
            pManager.AddIntegerParameter("floors", "F", "floors", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Carport", "C", "Crp", GH_ParamAccess.item);
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
            string type = "";
            Curve parkingBound = new PolylineCurve();
            int floors = 0;

            //Get Data
            if (!DA.GetData(0, ref type))
                return;
            if (!DA.GetData(1, ref parkingBound))
                return;
            if (!DA.GetData(2, ref floors))
                return;


            //Set properties
            PlotPlanning.ObjectModel.ParkingLot parking = new ObjectModel.ParkingLot();
            parking.Type = type;
            parking.ParkingBound = parkingBound;
            parking.Floors = floors;

            //Set data
            DA.SetData(0, parking);
        }

        #endregion
    }
}
