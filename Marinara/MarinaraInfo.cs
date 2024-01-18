using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Marinara3
{
    public class MarinaraInfo : GH_AssemblyInfo
    {
        public override string Name => "Marinara3";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("818b80e4-76a4-4ffe-a7e0-ced41dfdb3c6");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}