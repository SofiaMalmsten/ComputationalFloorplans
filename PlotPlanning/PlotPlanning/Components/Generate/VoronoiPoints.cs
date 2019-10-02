using Grasshopper.Kernel;
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
    public class VoronoiPoints : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public VoronoiPoints()
          : base("VoronoiPoints", "VPts",
              "Generates a number of points on a site that can be used to create a street network with the GenerateStreetNetwork component.",
              "PlotPlanningTool", "Generate")
        {
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Empty;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e9f418d8-a484-4cc6-b8f4-30d869c3b2fc"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("SiteBoundary", "C", "The boundary of the site for which you want to generate Voronoi points. Has to be closed and planar", GH_ParamAccess.item);
            pManager.AddPointParameter("AccessPoints", "P", "The points from where you want the street network to be accessible from the outside. " +
                "These points should ideally be on the site boundary.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("density", "d", "The density of the points, i.e. how many points are generated. " +
                "A more dense point distribution will result in a denser street network as well.", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("Offset", "o", "The offet from the boundary where the points will be created and later where the street network will be formed." +
                "A higher offset will result in more streets in the center region of the plot.", GH_ParamAccess.item, 5);
            pManager.AddIntegerParameter("Seed", "s", "A seed for the random generation. Equal seeds will generate equal results.", GH_ParamAccess.item, 1);

            pManager[1].Optional = true; //making accessPoints optional
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("VoronoiPoints", "VPts", "The points generated. Use these as an input to GenerareStreetNetwork component.", GH_ParamAccess.list);
            pManager.AddCurveParameter("Offsetde boundary curve", "C", "The offsed boundary curve. Use these as an input to GenerareStreetNetwork component.", GH_ParamAccess.item);
        }

        #endregion

        #region Solution
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve SiteBoundary = new PolylineCurve();
            List<Point3d> accessPoints = new List<Point3d>();
            int density = 10;
            double offset = 5; 
            int seed = 1;

            //Get Data
            if (!DA.GetData(0, ref SiteBoundary))
                return;
            if (!DA.GetDataList(1, accessPoints))
               
            if (!DA.GetData(2, ref density))
                return;
            if (!DA.GetData(3, ref offset))
                return;
            if (!DA.GetData(4, ref seed))
                return;

            (List<Point3d>, Curve) objTuple = PlotPlanning.Methods.Generate.VoronoiPoints(SiteBoundary, accessPoints, offset, density, seed); 

            //Set data for the outputs
            DA.SetDataList(0, objTuple.Item1);
            DA.SetData(1, objTuple.Item2);
        }

        #endregion
    }
}
