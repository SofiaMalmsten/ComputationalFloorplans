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
    public class ShapeFactor : GH_Component
    {
        #region Register node
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ShapeFactor()
          : base("ShapeFactor", "ShFac",
              "Calculates the shape factor for a multi family house",
              "PlotPlanningTool", "Evaluate")
        {
        }/// <summary>
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
            get { return new Guid("ed105268-6c8d-4997-82f2-ad29f09d0616"); }
        }


        #endregion

        #region Input/Output
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("MFH", "M", "MFH", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("ShapeFactor", "S", "ShapeFactor", GH_ParamAccess.item);
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
            ObjectModel.MultiFamily MFH = new ObjectModel.MultiFamily();

            //Get Data
            if (!DA.GetData(0, ref MFH))
                return;

            //Calculate (shapeFactor = envelopArea/Atemp). Only works for breps with the same floor geometry on every floor.
            double envelopArea = AreaMassProperties.Compute(MFH.HouseGeom).Area;
            Rhino.Geometry.Collections.BrepFaceList faces = MFH.HouseGeom.Faces;
            double floorArea = 0;
            foreach (var face in faces)
            {
                if (face.NormalAt(0.5, 0.5) == new Vector3d(0, 0, -1))
                {
                    floorArea = AreaMassProperties.Compute(face).Area;
                };
            }

            double shapefactor = envelopArea / (floorArea * MFH.MaxFloors); //Should be the actual number of floors. 


            //Set data
            DA.SetData(0, shapefactor);
        }

        #endregion
    }
}
