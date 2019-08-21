using Grasshopper.Kernel;
using pp = PlotPlanning.Methods;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using PlotPlanning.ObjectModel;
using Grasshopper.Kernel.Parameters;

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
              "PlotPlanningTool", "Generate")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("houses", "houses", "rectangles that should be places on lines", GH_ParamAccess.list);
            pManager.AddCurveParameter("bound", "bound", "base positipon for the rectangles", GH_ParamAccess.item);
            pManager.AddIntegerParameter("itts", "itts", "itts", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("seed", "seed", "seed", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("method", "method", "random, shortest or longest", GH_ParamAccess.item, 1);
            pManager.AddCurveParameter("roads", "roads", "roads", GH_ParamAccess.list);
            pManager.AddGenericParameter("carport", "carport", "carport object", GH_ParamAccess.item);

            //add dropdown list for method input
            Param_Integer param = pManager[4] as Param_Integer;
            param.AddNamedValue("random", 0);
            param.AddNamedValue("roads", 1);
            param.AddNamedValue("boundary", 2);
            param.AddNamedValue("shortest", 3);
            param.AddNamedValue("longest", 4);
            param.AddNamedValue("boundary first", 5);
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("house", "houses", "placed house footprints", GH_ParamAccess.list);
            pManager.AddCurveParameter("cell", "cell", "region that's left after placing houses", GH_ParamAccess.list);
            pManager.AddGenericParameter("carport", "carport", "carport object", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //List<SingleFamily> houses = new List<SingleFamily>();
            List<IHouse> houses = new List<IHouse>();
            Curve bound = new PolylineCurve();
            int itts = 1;
            int seed = 1;
            int methodIdx = 1;
            List<Curve> roads = new List<Curve>();
            ObjectModel.Carport carport = new ObjectModel.Carport();

            //Get Data
            if (!DA.GetDataList(0, houses))
                return;
            if (!DA.GetData(1, ref bound))
                return;
            if (!DA.GetData(2, ref itts))
                return;
            if (!DA.GetData(3, ref seed))
                return;
            if (!DA.GetData(4, ref methodIdx))
                return;
            if (!DA.GetDataList(5, roads))
                return;
            if (!DA.GetData(6, ref carport))
                return;

            //set the method to the correct string
            string method = "";
            if (methodIdx == 0) method = "random";
            else if (methodIdx == 1) method = "roads";
            else if (methodIdx == 2) method = "boundary";
            else if (methodIdx == 3) method = "shortest";
            else if (methodIdx == 4) method = "longest";
            else if (methodIdx == 5) method = "boundary first";
            else method = "random";


            List<IHouse> houseList = new List<IHouse>();
            List<ObjectModel.Carport> carports = new List<ObjectModel.Carport>();
            Random random = new Random(seed);
            Curve originalBound = bound;


            List<Curve> BoundList = new List<Curve>() { bound };


            for (int i = 0; i < itts; i++)
            {
                int idx = random.Next(BoundList.Count);
                Curve c = BoundList[idx];
                BoundList.RemoveAt(idx);

                pp.Generate.IPlaceHouseRow(houses, c, originalBound, roads, random,
                    method, carport, out List<IHouse> outHouseList, out List<PolylineCurve> newBound, out List<ObjectModel.Carport> carportList);


                BoundList.AddRange(newBound);
                houseList.AddRange(outHouseList);
                carports.AddRange(carportList);
                if (BoundList.Count == 0) break;
            }

            List<Curve> newRegions = BoundList;

            //Set data for the outputs
            DA.SetDataList(0, houseList);
            DA.SetDataList(1, newRegions);
            DA.SetDataList(2, carports);
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
