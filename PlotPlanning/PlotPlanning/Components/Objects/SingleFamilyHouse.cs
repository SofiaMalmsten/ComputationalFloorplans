using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using PlotPlanning.Engine.Base;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class HouseComponent : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public HouseComponent()
          : base("SingleFamilyHouse", "SFH",
              "SingleFamilyHouse",
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
                return Properties.Resources.SFH;
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

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("type", "T", "house type", GH_ParamAccess.item, "");
            pManager.AddBooleanParameter("carport", "C", "has car port", GH_ParamAccess.item, false); 
            pManager.AddIntegerParameter("minAmount", "minA", "minAmount in a row of houses", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("maxAmount", "maxA", "max amount in a row of houses (1 means free standing)", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("offset", "O", "buffer distance", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("frontyard", "f", "front yard", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("backyard", "b", "back yard", GH_ParamAccess.item, 0);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SingleFamilyHouse", "S", "SFH", GH_ParamAccess.list);
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
            bool carport = false;
            int minAmount = 1;
            int maxAmount = int.MaxValue;
            int offset = 1;
            double front = 0;
            double back = 0; 
           
            //Get Data
            if (!DA.GetData(0, ref type))
                return;
            if (!DA.GetData(1, ref carport))
                return;
            if (!DA.GetData(2, ref minAmount))
                return;
            if (!DA.GetData(3, ref maxAmount))
                return;
            if (!DA.GetData(4, ref offset))
                return;
            if (!DA.GetData(5, ref front))
                return;
            if (!DA.GetData(6, ref back))
                return;

            //Set properties
            List<PlotPlanning.ObjectModel.SingleFamily> houses = new List<ObjectModel.SingleFamily>(); 
            if(minAmount == 1 || minAmount == 0)
            {                
                (Brep houseGeometry, Point3d referencePoint, Rectangle3d garden) = PlotPlanning.Engine.Base.ReadGeometry.ReadAllHouseGeometry(type + "S");
                garden = PlotPlanning.Engine.Geometry.Convert.ExpandRectangle(garden, front, back); 
                PlotPlanning.ObjectModel.SingleFamily freestanding = new PlotPlanning.ObjectModel.SingleFamily(type + "S", carport, garden.ToPolyline(), houseGeometry, garden.Corner(1), 1, 1, offset);
                freestanding.Front = front; 
                freestanding.Back = back; 
                houses.Add(freestanding); 
            }
            if(maxAmount > 1)
            {
                (Brep houseGeometry, Point3d referencePoint, Rectangle3d garden) = PlotPlanning.Engine.Base.ReadGeometry.ReadAllHouseGeometry(type + "M");
                garden = PlotPlanning.Engine.Geometry.Convert.ExpandRectangle(garden, front, back);
                PlotPlanning.ObjectModel.SingleFamily rowHouse = new PlotPlanning.ObjectModel.SingleFamily(type + "M", carport, garden.ToPolyline(), houseGeometry, garden.Corner(1), 3, maxAmount, offset);
                rowHouse.Front = front;
                rowHouse.Back = back; 
                houses.Add(rowHouse); 
            }       

            //Set data
            DA.SetDataList(0, houses);
        }

        #endregion
    }
}
