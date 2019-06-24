using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class SegmentBounds : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SegmentBounds()
          : base("SegmentBounds", "bounds",
              "Creates lines to place houses on",
              "PlotPlanningTool", "Generate")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("bounds", "bounds", "siteBoundaries", GH_ParamAccess.item);
            pManager.AddRectangleParameter("rectangle", "rec", "rectanlge to place on the site", GH_ParamAccess.item);
            pManager.AddIntegerParameter("seed", "seed", "change seed in order to change plot layout", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("allLines", "allLines", "all possible lines", GH_ParamAccess.list);
            pManager.AddLineParameter("currentLine", "currLine", "current line to place houses on", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {



            //define instances
            Curve pline = new PolylineCurve();
           
            Rectangle3d rectangle = new Rectangle3d();
            int seed = 0;


            //Get data
            if (!DA.GetData(0, ref pline))
                return;

            if (!DA.GetData(1, ref rectangle))
                return;

            if (!DA.GetData(2, ref seed))
                return;

            //Calculate

            PolylineCurve siteBound2 = pline as PolylineCurve;
            Polyline siteBound = siteBound2.ToPolyline();

            List<Line> segments = PlotPlanning.Methods.Generate.SegmentBounds(siteBound, rectangle, seed);

            if (segments.Count != 0)
            {
                DA.SetDataList(0, segments);
                DA.SetData(1, segments[0]);
            }
            

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
                return Properties.Resources.pLineBound;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2b088e34-ec05-4547-abc5-f7772f9f3ff1"); }
        }
    }


}
