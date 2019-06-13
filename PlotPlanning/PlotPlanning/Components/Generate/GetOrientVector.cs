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
    public class GetOrientVector : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GetOrientVector()
          : base("OrientVector", "GetOrientVectors",
              "Creates accesspoints on a line",
              "PlotPlanningTool", "Generate")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("point", "pt", "points to evaluate", GH_ParamAccess.list);
            pManager.AddLineParameter("line", "ln", "line points are on", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("tanVec", "tanVec", "tanVector", GH_ParamAccess.list);
            pManager.AddVectorParameter("normVec", "normVec", "normVector", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            List<Point3d> Points = new List<Point3d>();
            Line line = new Line();

            if (!DA.GetDataList(0, Points))
                return;

            if (!DA.GetData(1, ref line))
                return;


            //Calculate

            Vector3d unitZ = new Vector3d(0, 0, 1);
            List<Vector3d> tanList = new List<Vector3d>();
            List<Vector3d> normList = new List<Vector3d>();

            //======================================================
            //Curve closest point
            //======================================================
            for (int i = 0; i < Points.Count; i++)
            {
                Vector3d tan = line.UnitTangent;
                tanList.Add(tan);

                Vector3d norm = Vector3d.CrossProduct(tan, unitZ);
                normList.Add(norm);
            }

            //Set data for the outputs
            DA.SetDataList(0, tanList);
            DA.SetDataList(1, normList);
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
                return Properties.Resources.Generate;
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
            get { return new Guid("2b088e34-ec05-4547-abc5-f7772f9f3ff4"); }
        }
    }


}
