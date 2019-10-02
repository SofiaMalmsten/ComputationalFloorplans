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
    public class GenerateStreetNetwork : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GenerateStreetNetwork()
          : base("GenerateStreetNetwork", "SN",
              "Generates a street network from points. Try generating the points with the VoronoiPoints component.",
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
            get { return new Guid("cdffdba7-0c70-4bcb-adcb-101227a338fa"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("VoronoiPoints", "P", "The points from which the street network is generated." +
                "Try plugging in the points from the VoronoiPoints component.", GH_ParamAccess.list);
            pManager.AddCurveParameter("OffsetSiteBoundary", "C", "The region in which you whish the streets to be created." +
                "Try plugging in the offset boundary from the VoronoiPoints component.", GH_ParamAccess.item); 
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("l", "l", "l", GH_ParamAccess.tree);
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
            List<Point3d> voronoiPoints = new List<Point3d>();

            //Get Data
            if (!DA.GetDataList(0, voronoiPoints))
                return;
            if (!DA.GetData(1, ref SiteBoundary))
                return;            

            List<Line> networkLines = Methods.Generate.VoronoiNetwork(voronoiPoints, SiteBoundary);
            List<List<Line>> subgraphs = Methods.Generate.FindSubgraphs(networkLines);
            Grasshopper.DataTree<Line> tree = new Grasshopper.DataTree<Line>();

            for (int i = 0; i < subgraphs.Count; i++)            
                tree.AddRange(subgraphs[i], new Grasshopper.Kernel.Data.GH_Path(i));


            //Set data for the outputs
            DA.SetDataTree(0, tree); 
        }

        #endregion
    }
}
