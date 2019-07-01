using Grasshopper.Kernel;
using pp = PlotPlanning.Methods;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class PlotLayout2D : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PlotLayout2D()
          : base("PlotLayout2D", "PlotLayout2D",
              "Creates accesspoints on a line",
              "PlotPlanningTool", "Houses")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddRectangleParameter("baseRectangles", "baseRecs", "rectangles that should be places on lines", GH_ParamAccess.list);
            pManager.AddCurveParameter("bound", "bound", "base positipon for the rectangles", GH_ParamAccess.item);
            pManager.AddNumberParameter("minAmount", "minAmount", "tangent vector for the line", GH_ParamAccess.item);
            pManager.AddNumberParameter("maxAmount", "maxAmount", "base positipon for the rectangles", GH_ParamAccess.item);
            pManager.AddNumberParameter("offset", "offset", "offset around houses", GH_ParamAccess.item);
            pManager.AddIntegerParameter("itts", "itts", "itts", GH_ParamAccess.item);
            pManager.AddIntegerParameter("seed", "seed", "seed", GH_ParamAccess.item);
            pManager.AddTextParameter("method", "method", "random, shortest or longest", GH_ParamAccess.item);
           // pManager.AddCurveParameter("roads", "roads", "roads", GH_ParamAccess.item); 

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("R", "region", "placed rectangles", GH_ParamAccess.list);
            pManager.AddVectorParameter("tan", "tan", "tan vectors", GH_ParamAccess.list);
            pManager.AddCurveParameter("cutR", "curR", "cut region", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<Rectangle3d> baseRectangles = new List<Rectangle3d>();
            Curve bound = new PolylineCurve();
            double minAmount = 1;
            double maxAmount = 1;
            int itts = 1; 
            int seed = 1;
            double offset = 0;
            string method = "";
            Curve roads = new PolyCurve(); 
           

            //Get Data
            if (!DA.GetDataList(0, baseRectangles))
                return;
            if (!DA.GetData(1, ref bound))
                return;
            if (!DA.GetData(2, ref minAmount))
                return;
            if (!DA.GetData(3, ref maxAmount))
                return;
            if (!DA.GetData(4, ref offset))
                return;
            if (!DA.GetData(5, ref itts))
                return;
            if (!DA.GetData(6, ref seed))
                return;
            if (!DA.GetData(7, ref method))
                return;
            //if (!DA.GetData(9, ref roads))
               // return;


            //Calculate
            /*
            Polyline pl_bound = new Polyline();
            bound.TryGetPolyline(out pl_bound);
            List<Line> segmests = pl_bound.GetSegments().ToList();
            Polyline pl_roads = new Polyline();
            roads.TryGetPolyline(out pl_roads);
            List<Line> road_segmests = pl_bound.GetSegments().ToList();
            List<Line> invalid_segments = segmests.Except(road_segmests,new pp.IdComparer()).ToList(); 
            */

            List <Vector3d> tans = new List<Vector3d>();
            List<Polyline> rectangles = new List<Polyline>();
            Random random = new Random(seed);
            Curve originalBound = bound; 

            //Get random baseRectangle from imput list:
            

            for (int i = 0; i < itts; i++)
            {
                Rectangle3d baseRectangle = baseRectangles[random.Next(baseRectangles.Count)];
                pp.Generate.PlaceHouseRow(baseRectangle, bound, originalBound, minAmount, maxAmount, offset, random, method, out List<Polyline> outRecs, out List<Vector3d> tan, out PolylineCurve newBound);
                rectangles.AddRange(outRecs);
                tans.AddRange(tan);
                //List<Line> validLines = newBound.ToPolyline().GetSegments().ToList().Except(invalid_segments).ToList();
                bound = newBound;//Curve.JoinCurves(validLines.Select(x => x.ToNurbsCurve()))[0]; 
            }

            Curve newRegion = bound; 

               

            //Set data for the outputs
            DA.SetDataList(0, rectangles);
            DA.SetDataList(1, tans);
            DA.SetData(2, newRegion);

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
                return Properties.Resources.RandomHouses;
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
