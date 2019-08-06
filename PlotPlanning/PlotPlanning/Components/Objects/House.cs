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
    public class House : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public House()
          : base("GenerateHouse", "GenerateHouse",
              "Generate house",
              "PlotPlanningTool", "Objects")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("type", "type", "house type", GH_ParamAccess.item);
            pManager.AddBooleanParameter("carport", "carport", "has car port", GH_ParamAccess.item);
            pManager.AddRectangleParameter("gardenBound", "gardenBound", "gardenBound", GH_ParamAccess.item);
            pManager.AddBrepParameter("houseGeom", "houseGeom", "houseGeom", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("house", "house", "house", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            string type = "";
            bool carport = false;
            Rectangle3d gardenBound = new Rectangle3d();
            Brep houseGeom = new Brep();

            //Get Data
            if (!DA.GetData(0, ref type))
                return;
            if (!DA.GetData(1, ref carport))
                return;
            if (!DA.GetData(2, ref gardenBound))
                return;
            if (!DA.GetData(3, ref houseGeom))
                return;

            //Set properties
            PlotPlanning.ObjectModel.House house = new ObjectModel.House();
            house.Type = type;
            house.HasCarPort = carport;
            house.gardenBound = gardenBound;
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
                return Properties.Resources.SnapToTopo;
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
            get { return new Guid("d1ce43a2-a700-4147-824f-26e734eb3c4d"); }
        }
    }


}
