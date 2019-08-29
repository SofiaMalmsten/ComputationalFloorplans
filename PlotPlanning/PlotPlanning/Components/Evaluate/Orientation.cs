using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using PlotPlanning.Engine.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace PlotPlanning.Components
{
    public class Orientation : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Orientation()
          : base("Orientation", "Orient",
              "Calculates the orentation for each house on the plot",
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
            get { return new Guid("b02e53b7-ec4b-479d-bc48-7e43e1799c0f"); }
        }

        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        /// 
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("SFH", "S", "SFH", GH_ParamAccess.list);
            pManager.AddVectorParameter("ReferenceVector", "V", "RefVec", GH_ParamAccess.item);
            pManager.AddVectorParameter("NorthVector", "N", "NorthVec", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Mean", "M", "-1 represents 180 deg from ref vector, 1 represents the ref vec", GH_ParamAccess.item); //mean value of dot product
            pManager.AddNumberParameter("Variance", "V", "Variance", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Distubution", "D", "The distribution of houses starting with North, going counter clockwise. (N,NW,W,SW,S,SE,E,NE)", GH_ParamAccess.list);
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
            List<ObjectModel.SingleFamily> SFH = new List<ObjectModel.SingleFamily>();
            Vector3d nortVec = new Vector3d();
            Vector3d refVec = new Vector3d();

            //Get Data
            if (!DA.GetDataList(0, SFH))
                return;
            if (!DA.GetData(1, ref refVec))
                return;
            if (!DA.GetData(2, ref nortVec))
                return;

            //Calculate
            int[] distrList = new int[8];
            List<double> dotProdList = new List<double>();

            List<double> domain = new List<double>();
            for (int i = 1; i <= 8; i++)
            {
                domain.Add(360 / 8 * i);
            }

            foreach (var s in SFH)
            {
                Vector3d houseVec = s.Orientation;
                double dotProd = Compute.DotProduct(houseVec / houseVec.Length, refVec / refVec.Length);
                dotProdList.Add(dotProd);

                //angle between house vecotrs and north vector in order to create intervals for orientations
                double angle = Vector3d.VectorAngle(houseVec, nortVec, Plane.WorldXY) * 360 / (2 * Math.PI);

                //Group the angle list per weather direction. 8 groups.
                for (int i = 0; i < 8; i++)
                {
                    if (angle <= domain[i])
                    {
                        distrList[i] = distrList[i] + 1;
                        break;
                    }
                }
            }

            double average = dotProdList.Average();
            double variance = distrList.Average(v => Math.Pow(v - distrList.Average(), 2));

            //Set data
            DA.SetData(0, average);
            DA.SetData(1, variance);
            DA.SetDataList(2, distrList.ToList());
        }

        #endregion

    }
}
