using Grasshopper.Kernel;
using pp = PlotPlanning.Methods;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using PlotPlanning.ObjectModel;

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
            pManager.AddGenericParameter("houses", "houses", "rectangles that should be places on lines", GH_ParamAccess.list);
            pManager.AddCurveParameter("bound", "bound", "base positipon for the rectangles", GH_ParamAccess.item);
            pManager.AddNumberParameter("minAmounts", "minAmount", "tangent vector for the line", GH_ParamAccess.list);
            pManager.AddNumberParameter("maxAmount", "maxAmount", "base positipon for the rectangles", GH_ParamAccess.item);
            pManager.AddNumberParameter("offset", "offset", "offset around houses", GH_ParamAccess.item);
            pManager.AddIntegerParameter("itts", "itts", "itts", GH_ParamAccess.item);
            pManager.AddIntegerParameter("seed", "seed", "seed", GH_ParamAccess.item);
            pManager.AddTextParameter("method", "method", "random, shortest or longest", GH_ParamAccess.item);
            pManager.AddCurveParameter("roads", "roads", "roads", GH_ParamAccess.list);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("house", "houses", "placed house footprints", GH_ParamAccess.list);
            pManager.AddCurveParameter("cell", "cell", "region that's left after placing houses", GH_ParamAccess.list);
            pManager.AddPointParameter("midPt", "midPt", "center points of all the houses", GH_ParamAccess.list);
            pManager.AddRectangleParameter("garden", "garden", "placed house footprints", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //List<Rectangle3d> baseRectangles = new List<Rectangle3d>();
            List<ObjectModel.House> houses = new List<ObjectModel.House>();
            Curve bound = new PolylineCurve();
            List<double> minAmounts = new List<double>();
            double maxAmount = 1;
            int itts = 1;
            int seed = 1;
            double offset = 0;
            string method = "";
            List<Curve> roads = new List<Curve>();


            //Get Data
            if (!DA.GetDataList(0, houses))
                return;
            if (!DA.GetData(1, ref bound))
                return;
            if (!DA.GetDataList(2, minAmounts))
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
            if (!DA.GetDataList(8, roads))
                return;


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

            List<Polyline> rectangles = new List<Polyline>();
            List<ObjectModel.House> houseList = new List<ObjectModel.House>();
            List<Point3d> middlePts = new List<Point3d>();
            Random random = new Random(seed);
            Curve originalBound = bound;

            List<Curve> BoundList = new List<Curve>() { bound };






            for (int i = 0; i < itts; i++)
            {
                int idx = random.Next(BoundList.Count);
                Curve c = BoundList[idx]; 
                BoundList.RemoveAt(idx);

                int index = random.Next(houses.Count);
                Rectangle3d baseRectangle = houses[index].gardenBound;
                double minAmount = minAmounts[index];

                pp.Generate.PlaceHouseRow(baseRectangle, c, originalBound, roads, minAmount, maxAmount, offset, random,
                    method, out List<Polyline> outRecs, out List<Vector3d> tan, out List<PolylineCurve> newBound, out List<Point3d> midPts);

                int j = 0;
                foreach (var rec in outRecs)
                {
                    ObjectModel.House outHouse = new ObjectModel.House();
                    outHouse.gardenBound = pp.Calculate.BoundingRect(rec);
                    outHouse.Type = houses[index].Type;
                    outHouse.orientation = tan[j];
                    //ObjectModel.House houseClone = new ObjectModel.House() { houseGeom = houses[index].houseGeom };
                    outHouse.houseGeom = houses[index].houseGeom.DuplicateBrep();
                    outHouse.houseGeom.Translate(Methods.Calculate.createVector(midPts[j], baseRectangle.Center));
                    houseList.Add(outHouse);

                    j++;
                }

                rectangles.AddRange(outRecs);
                middlePts.AddRange(midPts);
                BoundList.AddRange(newBound);
                if (BoundList.Count == 0) break;
            }

            List<Curve> newRegions = BoundList;


            //Set data for the outputs
            DA.SetDataList(0, houseList);
            DA.SetDataList(1, newRegions);
            DA.SetDataList(2, middlePts);
            DA.SetDataList(3, rectangles);
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
            get { return new Guid("874a8451-aa5e-4c5d-90fb-5e2158862b5d"); }
        }
    }


}
