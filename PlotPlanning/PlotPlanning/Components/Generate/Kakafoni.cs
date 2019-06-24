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
    public class Kakafoni : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Kakafoni()
          : base("Kakafoni", "GenerateKakafoni",
              "Creates accesspoints on a line",
              "PlotPlanningTool", "Houses")
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
            pManager.AddNumberParameter("offset", "offset", "offset around houses", GH_ParamAccess.item);
            pManager.AddIntegerParameter("itts", "itts", "itts", GH_ParamAccess.item);
            pManager.AddIntegerParameter("seed", "seed", "seed", GH_ParamAccess.item);

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

            Rectangle3d baseRectangle = new Rectangle3d();
            Curve bound = new PolylineCurve();
            double minAmount = 1;
            double maxAmount = 1;
            double spaceDist = 0;
            int itts = 1; 
            int seed = 1;
            double offset = 0;
           

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
            if (!DA.GetData(5, ref offset))
                return;
            if (!DA.GetData(6, ref itts))
                return;
            if (!DA.GetData(7, ref seed))
                return;


            //Calculate


            // Curve bound = new PolylineCurve();
            List<Vector3d> tans = new List<Vector3d>();
            List<Polyline> rectangles = new List<Polyline>();

            for (int i = 0; i < itts; i++)
            {
                pp.Generate.PlaceHouseRow(baseRectangle, bound, minAmount, maxAmount, spaceDist, offset, seed, out List<Polyline> outRecs, out List<Vector3d> tan, out PolylineCurve newBound);
                rectangles.AddRange(outRecs);
                tans.AddRange(tan);
                bound = newBound; 
            }

            Curve newRegion = bound; 




                /*
                List<Line> line = PlotPlanning.Methods.Generate.SegmentBounds(Methods.Calculate.ConvertToPolyline(bound as PolylineCurve), baseRectangle, seed);
                List<Point3d> pos = PlotPlanning.Methods.Generate.AccessPoints(line.Last(), minAmount, maxAmount, baseRectangle, spaceDist);
                List<Vector3d> tan = PlotPlanning.Methods.Generate.GetTanVect(pos, line.Last());


                List<Polyline> rectangles = new List<Polyline>();
                for (int i = 0; i < pos.Count; i++)
                {
                    Polyline pLines = PlotPlanning.Methods.Generate.HouseFootprint(baseRectangle, pos[i], tan[i]);
                    Curve rec = Curve.CreateControlPointCurve(pLines.ToList(), 1);
                    rectangles.AddRange(PlotPlanning.Methods.Generate.CullSmallAreas(rec, bound));
                }

                Polyline cutRegion = PlotPlanning.Methods.Calculate.ConvexHull(rectangles);
                Curve cutCrv = Curve.CreateControlPointCurve(cutRegion.ToList(), 1);
                Curve offsetRegion = cutCrv.Offset(Plane.WorldXY, offset, PlotPlanning.Methods.Generate.DistanceTol(), CurveOffsetCornerStyle.Sharp)[0];
                Curve newRegion = Curve.CreateBooleanDifference(bound, offsetRegion, PlotPlanning.Methods.Generate.DistanceTol()).PickLargest();

                */

            /*if (rectangles.Count==0)
            {
                pos = PlotPlanning.Methods.Generate.AccessPoints(line[1], minAmount, maxAmount, baseRectangle, spaceDist);
                tan = PlotPlanning.Methods.Generate.GetTanVect(pos, line[1]);

                for (int i = 0; i < pos.Count; i++)
                {
                    Polyline pLines = PlotPlanning.Methods.Generate.HouseFootprint(baseRectangle, pos[i], tan[i]);
                    Curve rec = Curve.CreateControlPointCurve(pLines.ToList(), 1);
                    rectangles.AddRange(PlotPlanning.Methods.Generate.CullSmallAreas(rec, bound));
                }
            }*/

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
