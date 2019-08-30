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
    public class GreenArea : GH_Component
    {

        #region Register Node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GreenArea()
          : base("GreenArea", "GrnAr",
              "Calculates the green area",
              "PlotPlanningTool", "Evaluate")
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
            get { return new Guid("011bc95f-757d-4f46-a362-3ef50ca9e089"); }
        }
        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("GreenArea", "G", "Polygon representing noundary of the green area to evaluate", GH_ParamAccess.item);
        }
        

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Area", "A", "Area of each green area", GH_ParamAccess.item);
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
            Curve greenArea = new PolylineCurve();

            //Get Data
            if (!DA.GetData(0, ref greenArea))
                return;

            //Calculate
            double area = AreaMassProperties.Compute(greenArea).Area;
           
            //Set data
            DA.SetData(0, area);
        }
        #endregion

    }
}
