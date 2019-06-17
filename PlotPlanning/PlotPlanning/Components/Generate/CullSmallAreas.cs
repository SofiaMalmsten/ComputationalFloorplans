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
    public class CullSmallAreas : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// 
        /// </summary>
        public CullSmallAreas()
          : base("CullSmallAreas", "CullSmallAreas",
              "Description",
              "PlotPlanningTool", "Test")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("A", "A", "rectangle that should be places on lines", GH_ParamAccess.list);
            pManager.AddCurveParameter("B", "B", "base positipon for the rectangles", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("R", "region", "placed rectangles", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            List<Curve> A = new List<Curve>();
            Curve B = new PolylineCurve();

            if (!DA.GetDataList(0, A))
            return;

            if (!DA.GetData(1, ref B))
                return;


            //Calculate
            List<Curve> R = PlotPlanning.Methods.Generate.CullSmallAreas(A,B);

            //Set data for the outputs
            DA.SetDataList(0, R);
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
                //return Properties.Resources.Houses;
                return null;
            }
        }



        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2b088e34-ec05-4547-abc5-f7772f9f3ff4"); }
        }
    }


}
