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
    public class CreateAccesspoints : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CreateAccesspoints()
          : base("PlotPlanning", "CreateAccesspoints",
              "Creates accesspoints on a line",
              "SitePlanningTool", "Generate")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("line", "line", "line to place accesspoints on", GH_ParamAccess.item);
            pManager.AddNumberParameter("minAmount", "minAmount", "min amount of houses in a row", GH_ParamAccess.item);
            pManager.AddNumberParameter("maxAmount", "maxAmount", "max amount of houses in a row", GH_ParamAccess.item);
            pManager.AddRectangleParameter("rectangle", "rectangle", "rectangle to place", GH_ParamAccess.item);
            pManager.AddNumberParameter("spcaceDist", "spaceDist", "distance between two house segments", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("accessPts", "accessPts", "placed rectangles", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Create class instances
            Line line = new Line();
            double minAmount = 1;
            double maxAmount = 1;
            Rectangle3d rectangle = new Rectangle3d();
            double spaceDist = 0;

            if (!DA.GetData(0, ref line))
            return;

            if (!DA.GetData(1, ref minAmount))
                return;

            if (!DA.GetData(2, ref maxAmount))
                return;

            if (!DA.GetData(3, ref rectangle))
                return;
            if (!DA.GetData(4, ref spaceDist))
                return;

            //Calculate

            //========================================================
            //Declaration - fixed values
            //========================================================
            double lineLength = line.Length;
                Point3d startPt = line.From;
                Vector3d vec = (line.Direction) / lineLength;

                double segmentWidth = rectangle.Width;
                double segmentHeight = rectangle.Height;

                double segmentLength = Math.Min(segmentWidth, segmentHeight);

                Vector3d husVec = vec * segmentLength;
                Vector3d spaceVec = vec * (spaceDist + segmentLength);

                //========================================================
                //Declaration - lists and new objects
                //========================================================
                Line currLine = new Line();
                Point3d currPt = new Point3d();
                List<Point3d> pointPos = new List<Point3d>();
                List<Vector3d> vectList = new List<Vector3d>();
                List<Line> lineCombination = new List<Line>();

                currPt = startPt;
                double currLength = segmentLength;
                int i = 0;
                while (currLength < lineLength)
                {
                    currLine = new Line(currPt, husVec);
                    currPt = currLine.To;
                    pointPos.Add(currPt);
                    lineCombination.Add(currLine);
                    currLength = currLength + segmentLength;
                    i = i + 1;

                    //if i = max amunt: add spaceDist and reset counter.
                    // Todo: add a new branch
                    if (i == maxAmount)
                    {
                        currLine = new Line(currPt, spaceVec);
                        currPt = currLine.To;
                        currLength = currLength + spaceDist + segmentLength;
                        i = 0;
                    }
                }

                //========================================================
                // Remove lines that are too short. minAmount * segmentLength
                //========================================================
                if (i < minAmount)
                {
                    lineCombination.RemoveRange(lineCombination.Count - i, i);
                }

            //Set data for the outputs
            DA.SetDataList(0, pointPos);
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
                return Properties.Resources.Plot2D;
                //return null;
            }
        }



        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2b088e34-ec05-4547-abc5-f7772f9f3ff3"); }
        }
    }


}
