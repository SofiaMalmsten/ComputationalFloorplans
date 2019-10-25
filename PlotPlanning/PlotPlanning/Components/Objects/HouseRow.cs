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
          : base("HouseRow", "HouseRow",
              "HouseRow",
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
            pManager.AddNumberParameter("front", "f", "frontyard", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("back", "b", "backyard", GH_ParamAccess.item, 0); 

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("houseRow", "R", "HouseRow", GH_ParamAccess.item);
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
            int offset = 0;
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
            ObjectModel.HouseRow row = new ObjectModel.HouseRow();
            Brep b = new Brep();
            Point3d referencePoint = new Point3d();
            Rectangle3d garden = new Rectangle3d(); 

            if (type.Contains("S"))
            {
                (b, referencePoint, garden) = Engine.Base.ReadGeometry.ReadAllHouseGeometry(type);
                garden = PlotPlanning.Engine.Geometry.Convert.ExpandRectangle(garden, front, back);
                ObjectModel.SingleFamily freestandingHouse = new ObjectModel.SingleFamily(type, carport, garden, garden.PointAt(0),referencePoint, b, Vector3d.XAxis);
                row.Houses.Add(freestandingHouse); 
            }

            else if(type.Contains("R"))
            {
                String[] types = { type.Remove(type.Length - 1) + "R", type.Remove(type.Length - 1) + "M", type.Remove(type.Length - 1) + "L" };
                foreach (string t in types)
                {
                    (b, referencePoint, garden) = Engine.Base.ReadGeometry.ReadAllHouseGeometry(t);
                    garden = PlotPlanning.Engine.Geometry.Convert.ExpandRectangle(garden, front, back);
                    ObjectModel.SingleFamily rowHouse = new ObjectModel.SingleFamily(t, carport, garden, garden.PointAt(0), referencePoint, b, Vector3d.XAxis);
                    row.Houses.Add(rowHouse); 
                }             
            }
            row.MaxAmount = maxAmount;
            row.MinAmount = minAmount;
            row.Offset = offset;  

            //Set data
            DA.SetData(0, row);
        }

        #endregion
    }
}
