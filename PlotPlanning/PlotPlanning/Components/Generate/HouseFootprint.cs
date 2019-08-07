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
    public class HouseFootprint : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// 
        /// </summary>
        public HouseFootprint()
          : base("HOuseFootprint", "CreateRectangles",
              "Description",
              "PlotPlanningTool", "Generate")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddRectangleParameter("baseRectangle", "baseRec", "rectangle that should be places on lines", GH_ParamAccess.item);
            pManager.AddPointParameter("position", "pos", "base positipon for the rectangles", GH_ParamAccess.item);
            pManager.AddVectorParameter("tanVector", "tan", "tangent vector for the line", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddRectangleParameter("rectangles", "rec", "placed rectangles", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            Rectangle3d baseRectangle = new Rectangle3d();
            Point3d Points = new Point3d();
            Vector3d tan = new Vector3d();

            if (!DA.GetData(0, ref baseRectangle))
            return;

            if (!DA.GetData(1, ref Points))
                return;

            if (!DA.GetData(2, ref tan))
                return;

            //Calculate
            Polyline pLines = PlotPlanning.Methods.Calculate.Translate(baseRectangle, Points, tan);

            //Set data for the outputs
            DA.SetData(0, pLines);
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
            get { return new Guid("012a11d3-3b6d-45bd-938f-f694161bd61d"); }
        }
    }


}
