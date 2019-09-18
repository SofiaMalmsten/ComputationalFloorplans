using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using PlotPlanning.Methods; 

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class StreetInclination : GH_Component
    {

        #region Register Node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public StreetInclination()
          : base("StreetInclination", "SIncl",
              "Calculates the average inclination of a street as a discrete dirivative (rise/run)",
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
            get { return new Guid("bf0b6133-968f-4fee-bb69-004404f8a2b4"); }
        }
        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Street", "S", "Center curve of the street", GH_ParamAccess.item);
            pManager.AddNumberParameter("Resolution", "R", "The distance between the points where the curve is evaluated. Smaller values give more precise results but might" +
                "slow down the calculation", GH_ParamAccess.item, 1);
        }
        

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Average street inclination", "I", "The average absolute value for the derivative of the curve in z direction. " +
                "A meassure of how flat the street is.", GH_ParamAccess.item);
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
            Curve street = new PolylineCurve();
            double dist = 1; 

            //Get Data
            if (!DA.GetData(0, ref street))
                return;
            if (!DA.GetData(1, ref dist))
                return;


            //Calculate
            double streetIncl = Evaluate.StreetInclination(street, dist); 
           
            //Set data
            DA.SetData(0, streetIncl);
        }
        #endregion

    }
}
