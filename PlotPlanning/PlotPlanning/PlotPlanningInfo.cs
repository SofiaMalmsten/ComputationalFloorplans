using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace PlotPlanning
{
    public class PlotPlanningInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "PlotPlanning";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("fd9cb042-f7b8-4db6-8103-ac6f725fe724");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
