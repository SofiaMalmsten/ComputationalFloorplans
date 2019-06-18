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
    public class Generate2DLayot : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Generate2DLayot()
          : base("Generate2DLyaout", "Generate2DLayout",
              "Creates accesspoints on a line",
              "PlotPlanningTool", "Generate")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddRectangleParameter("baseRectangle", "baseRec", "rectangle that should be places on lines", GH_ParamAccess.item);
            pManager.AddCurveParameter("bound", "bound", "base positipon for the rectangles", GH_ParamAccess.item);
            pManager.AddNumberParameter("minAmount", "minAmount", "tangent vector for the line", GH_ParamAccess.item);
            pManager.AddNumberParameter("maxAmount", "maxAmount", "base positipon for the rectangles", GH_ParamAccess.item);
            pManager.AddNumberParameter("spaceDist", "spaceDist", "base positipon for the rectangles", GH_ParamAccess.item);
            pManager.AddIntegerParameter("seed", "seed", "seed", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("R", "region", "placed rectangles", GH_ParamAccess.list);
            pManager.AddVectorParameter("tan", "tan", "tan vectors", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Rectangle3d baseRectangle = new Rectangle3d();
            Curve bound = new PolyCurve();
            double minAmount = 1;
            double maxAmount = 1;
            double spaceDist = 0;
            int seed = 1;

            //Get Data
            if (!DA.GetData(0, ref baseRectangle))
                return;
            if (!DA.GetData(1, ref bound))
                return;
            if (!DA.GetData(2, ref minAmount))
                return;
            if (!DA.GetData(3, ref maxAmount))
                return;
            if (!DA.GetData(4, ref spaceDist))
                return;
            if (!DA.GetData(5, ref seed))
                return;

            //Calculate
            List<Line> line = PlotPlanning.Methods.Generate.SegmentBounds(Methods.Calculate.ConvertToPolyline(bound as PolylineCurve), baseRectangle, seed);
            List<Point3d> pos = PlotPlanning.Methods.Generate.AccessPoints(line[0], minAmount, maxAmount, baseRectangle, spaceDist);
            List<Vector3d> tan = PlotPlanning.Methods.Generate.GetTanVect(pos, line[0]);

            List<Curve> rectangles = new List<Curve>();
            for (int i = 0; i < pos.Count; i++)
            {
                Polyline pLines = PlotPlanning.Methods.Generate.HouseFootprint(baseRectangle, pos[i], tan[i]);
                Curve rec = Curve.CreateControlPointCurve(pLines.ToList(), 1);
                rectangles.AddRange(PlotPlanning.Methods.Generate.CullSmallAreas(rec, bound));
            }

            //Set data for the outputs
            DA.SetDataList(0, rectangles);
            DA.SetDataList(1, tan);

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
            get { return new Guid("2b088e34-ec05-4547-abc5-f7772f9f3ff6"); }
        }
    }


}
