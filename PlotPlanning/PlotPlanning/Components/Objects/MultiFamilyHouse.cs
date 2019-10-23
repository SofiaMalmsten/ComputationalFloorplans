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
    public class MFHComponent : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public MFHComponent()
          : base("MultiFamilyHouse", "MFH",
              "MultiFamilyHouse",
              "PlotPlanningTool", "1.Objects")
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
                return Properties.Resources.MFH;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d921a823-6059-4f89-8326-7341ff81a8c1"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("CentreCrv", "C", "centreCrv", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Floors", "Floors", "Amount of floors", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Thickness", "T", "Thickness", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("levelHeight", "Lh", "Height between levels", GH_ParamAccess.item, 3);
            pManager.AddRectangleParameter("garden", "G", "garden", GH_ParamAccess.item);
            pManager.AddPointParameter("accessPoint", "P", "accessPoint", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("MultiFamilyHouse", "M", "MFH", GH_ParamAccess.item);
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

            //Create class instances
            Curve centreCrv = new PolylineCurve();
            int floors = 1;
            double thickness = 1;
            double levelHeight = 1;
            Rectangle3d garden = new Rectangle3d();
            Point3d accessPt = new Point3d();

            //Get Data
            if (!DA.GetData(0, ref centreCrv))
                return;
            if (!DA.GetData(1, ref floors))
                return;
            if (!DA.GetData(2, ref thickness))
                return;
            if (!DA.GetData(3, ref levelHeight))
                return;
            if (!DA.GetData(4, ref garden))
                return;
            if (!DA.GetData(5, ref accessPt))
                return;


            //Set properties
            PlotPlanning.ObjectModel.MultiFamily house = new ObjectModel.MultiFamily();
            house.Floors = floors;
            house.Thickness = thickness;
            house.LevelHeight = levelHeight;
            house.GardenBound = garden.ToPolyline();
            house.AccessPoint = accessPt;

            //Create geometry. 
            Brep[] b = Engine.Geometry.Compute.Sweep(centreCrv, thickness, 2);

            //Get and join all brep edges
            Curve[] En = b[0].DuplicateNakedEdgeCurves(true, true);
            Curve[] bound = Curve.JoinCurves(En);

            // TODO: make sure all the bounding curves are clockwise. This is to ensure extrution in the correct diection
           // For now I used a neg sign before extrution height.... 
            Extrusion ex = Extrusion.Create(bound[0], -levelHeight * floors, true);

            if (centreCrv.IsClosed)
            {
                //Sort profiles by length. The longer curve will be the outer profile. The shorter the inner.
                Curve innerCrv = bound.ToList().OrderBy(x => x.GetLength()).First();
                Extrusion exInner = Extrusion.Create(innerCrv, levelHeight * floors, true);
                Brep[] diff = Brep.CreateBooleanDifference(ex.ToBrep(), exInner.ToBrep(), ObjectModel.Tolerance.Distance);
                house.HouseGeom = diff[0];
            }
            else
                house.HouseGeom = ex.ToBrep();


            //Set data
            DA.SetData(0, house);
        }

        #endregion
    }
}
