using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning
{
    public class CreateRectangles : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// 
        /// </summary>
        public CreateRectangles()
          : base("PlotPlanning", "CreateRectangles",
              "Description",
              "SitePlanningTool", "Generate")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddRectangleParameter("baseRectangle", "rec", "rectangle that should be places on lines", GH_ParamAccess.item);
            pManager.AddPointParameter("position", "pos", "base positipon for the rectangles", GH_ParamAccess.list);
            pManager.AddVectorParameter("tanVector", "tan", "tangent vector for the line", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddRectangleParameter("rectangles", "rec", "placed rectangles", GH_ParamAccess.list);
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
            List<Point3d> Points = new List<Point3d>();
            List<Vector3d> tan = new List<Vector3d>();

            if (!DA.GetData(0, ref baseRectangle))
            return;

            if (!DA.GetDataList(1, Points))
                return;

            if (!DA.GetDataList(2, tan))
                return;

            //Calculate
            double wDim = baseRectangle.Width;
            double hDim = baseRectangle.Height;
            Vector3d unitZ = new Vector3d(0, 0, 1);

            List<Point3d> movedPts = new List<Point3d>();
            List<Polyline> pLines = new List<Polyline>();

            //======================================================
            //Create Polyline
            //======================================================
            for (int i = 0; i < Points.Count; i++)
            {
                //create points
                Point3d pt0 = Points[i];
                Point3d pt1 = pt0 + tan[i] * hDim;
                Point3d pt2 = pt1 + Vector3d.CrossProduct(tan[i], unitZ) * (wDim);
                Point3d pt3 = pt2 - tan[i] * hDim;
                Point3d pt4 = pt0;

                //add points
                movedPts.Add(pt0);
                movedPts.Add(pt1);
                movedPts.Add(pt2);
                movedPts.Add(pt3);
                movedPts.Add(pt4);

                Polyline pLine = new Polyline(movedPts);
                pLines.Add(pLine);
            }


            //Set data for the outputs
            DA.SetDataList(0, pLines);
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
                return Properties.Resources.Plot2D;
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
            get { return new Guid("2b088e34-ec05-4547-abc5-f7772f9f3ff2"); }
        }
    }


}
