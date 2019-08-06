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
    public class Accesspoints : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Accesspoints()
          : base("AccessPoints", "CreateAccesspoints",
              "Creates accesspoints on a line",
              "PlotPlanningTool", "Generate")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("line", "line", "line to place accesspoints on", GH_ParamAccess.item);
            pManager.AddNumberParameter("minAmount", "minAmount", "min amount of houses in a row", GH_ParamAccess.item);
            pManager.AddNumberParameter("maxAmount", "maxAmount", "max amount of houses in a row", GH_ParamAccess.item);
            pManager.AddRectangleParameter("rectangle", "rectangle", "rectangle to place", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("accessPts", "accessPts", "placed rectangles", GH_ParamAccess.list);
            pManager.AddVectorParameter("tanVect", "tanVec", "tnagentVector", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            Line line = new Line();
            Rectangle3d rectangle = new Rectangle3d();
            double minAmount = 1;
            double maxAmount = 1;

            //Get Data
            if (!DA.GetData(0, ref line))
            return;
            if (!DA.GetData(1, ref minAmount))
                return;
            if (!DA.GetData(2, ref maxAmount))
                return;
            if (!DA.GetData(3, ref rectangle))
                return;

            //Calculate
            List<Point3d> pointPos = PlotPlanning.Methods.Generate.AccessPoints(line, minAmount, maxAmount, rectangle);
            List<Vector3d> tanList = PlotPlanning.Methods.Generate.GetTanVect(pointPos, line);
           
            //Set data
            DA.SetDataList(0, pointPos);
            DA.SetDataList(1, tanList);
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
            get { return new Guid("839be6df-802c-4fec-befe-6df5c1645dbd"); }
        }
    }


}
