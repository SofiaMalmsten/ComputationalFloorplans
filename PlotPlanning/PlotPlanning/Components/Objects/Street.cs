using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class Street : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Street()
          : base("Street", "S",
              "Street",
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
                return Properties.Resources.Street;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("40dec9b7-c9d1-493b-96c3-957a9ba96a58"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("CenterCurve", "C", "Center curve", GH_ParamAccess.item);
            pManager.AddNumberParameter("Thickness", "T", "Thickness", GH_ParamAccess.item);
            pManager.AddNumberParameter("Fillet", "F", "Corner Fillet", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Street", "S", "Streets", GH_ParamAccess.item);
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
            double thickness = 1;
            double fillet = 0;

            //Get Data
            if (!DA.GetData(0, ref centreCrv))
                return;
            if (!DA.GetData(1, ref thickness))
                return;
            if (!DA.GetData(2, ref fillet))
                return;


            //Set properties
            PlotPlanning.ObjectModel.Street street = new ObjectModel.Street();
            street.CentreCurve = centreCrv;
            street.Width = thickness;
            street.CornerFillet = fillet;

            Vector3d tan = centreCrv.TangentAtStart;
            Vector3d projTan = new Vector3d(tan.X, tan.Y, 0);
            Vector3d norm = Engine.Geometry.Compute.CrossProduct(projTan / projTan.Length, Vector3d.ZAxis);

            Line crossSection = new Line(centreCrv.PointAtStart - norm * thickness / 2, norm * thickness);

            Rhino.Geometry.SweepOneRail sweepOne = new SweepOneRail();
            Brep[] b = sweepOne.PerformSweep(centreCrv, crossSection.ToNurbsCurve());

            //TODO: in order to merge all of the streets we need to change to list item and run the method on the list....
            Brep[] bUnion = Brep.CreateBooleanUnion(b, ObjectModel.Tolerance.Distance);

            //Set data
            DA.SetData(0, bUnion[0]);
        }

        #endregion
    }
}