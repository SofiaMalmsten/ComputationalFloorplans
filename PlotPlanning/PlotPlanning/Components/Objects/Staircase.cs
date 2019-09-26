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
    public class Staircase : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Staircase()
          : base("Staircase", "StrC",
              "Staircase",
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
                return Properties.Resources.Stair;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("044c36b0-c2a9-446b-b800-7f34e0890cc3"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("StairAlong", "sA", "Stair dimention along building backbone", GH_ParamAccess.item);
            pManager.AddNumberParameter("StairPerp", "sP", "Stair dimention perpendiculat to the building backbone", GH_ParamAccess.item);
            pManager.AddNumberParameter("LandingAlong", "lA", "Landing dimention along building backbone", GH_ParamAccess.item);
            pManager.AddNumberParameter("LandingPerp", "lP", "Landing dimention perpendicular to the building backbone", GH_ParamAccess.item);
            pManager.AddPlaneParameter("plane", "P", "Plane", GH_ParamAccess.item);
            pManager.AddBooleanParameter("flip", "f", "Flip direction", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Staircase", "S", "Staircase", GH_ParamAccess.item);
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
            double landingPerp = 0;
            double landingAlong = 0;
            double stairAlong = 0;
            double stairPerp = 0;
            Plane pl = new Plane();
            bool flip = false;

            //Get Data
            if (!DA.GetData(0, ref stairAlong))
                return;
            if (!DA.GetData(1, ref stairPerp))
                return;
            if (!DA.GetData(2, ref landingAlong))
                return;
            if (!DA.GetData(3, ref landingPerp))
                return;
            if (!DA.GetData(4, ref pl))
                return;
            if(!DA.GetData(5, ref flip))
                return;


            //Set properties
            PlotPlanning.ObjectModel.Staircase staircase = new ObjectModel.Staircase();
            staircase.StairCasePerimeter = Engine.Geometry.Create.Rectangle(pl, stairAlong, stairPerp).ToNurbsCurve();

            double factor = (stairPerp + landingPerp)/2;
            if (flip)
                factor *= -1;

            Plane p = pl.Clone();
            p.Translate(pl.YAxis * factor);
            
            staircase.LandingPerimeter = Engine.Geometry.Create.Rectangle(p, landingAlong, landingPerp).ToNurbsCurve();

            //Set data
            DA.SetData(0, staircase);
        }

        #endregion
    }
}