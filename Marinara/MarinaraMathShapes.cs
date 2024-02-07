using Grasshopper.Kernel;
using Marinara.Properties;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Marinara
{
    public class MSphere : MarinaraComponent
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear,
        /// Subcategory the panel. If you use non-existing tab or panel names,
        /// new tabs/panels will automatically be created.
        /// </summary>

        public MSphere()
            : base("MSphere", "MSphere",
         "Generate a point sphere",
         "Marinara", "Chomas")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
        }

        protected override Interval DefaultUDomain()
        {
            return new Interval(0, Math.PI);
        }

        protected override Interval DefaultVDomain()
        {
            return new Interval(0, 2 * Math.PI);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and
        /// to store data in output parameters.</param>
        public override List<Point3d> SolveMarinara(IGH_DataAccess DA)
        {
            Debug.WriteLine($"Sphere");
            List<Point3d> points = new List<Point3d>();
            foreach (double u in u_vals)
            {
                foreach (double v in v_vals)
                {
                    double x = Math.Sin(v) * Math.Cos(u);
                    double y = Math.Sin(v) * Math.Sin(u);
                    double z = Math.Cos(v);

                    points.Add(new Point3d(x, y, z));
                }
            }
            return points;
        }

        /// <summary>
        /// The Exposure property controls where in the panel a component icon
        /// will appear. There are seven possible locations (primary to septenary),
        /// each of which can be combined with the GH_Exposure.obscure flag, which
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resource1.MSphere;

        /// <summary>
        /// Each component must have a unique Guid to identify it.
        /// It is vital this Guid doesn't change otherwise old ghx files B
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("b06904b0-9f9d-49d4-83d8-35f876e7b8bb");
    }

    public class MPinch : MarinaraComponent
    {
        public MPinch()
          : base("MPinch", "MPinch", "Pinch the geometry using exponents.",
                "Marinara",
                "Chomas")
        {
        }

        private int pinch_degree = 3;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddIntegerParameter("pinchness", "pinchness", "How deep to pinch", GH_ParamAccess.item, pinch_degree);
        }

        protected override Interval DefaultUDomain()
        {
            return new Interval(0, 2 * Math.PI);
        }

        protected override Interval DefaultVDomain()
        {
            return new Interval(0, Math.PI);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and
        /// to store data in output parameters.</param>
        public override List<Point3d> SolveMarinara(IGH_DataAccess DA)
        {
            List<Point3d> points = new List<Point3d>();

            if (!DA.GetData(3, ref this.pinch_degree)) return points;

            Debug.WriteLine($"Pinching");
            int M = this.pinch_degree;
            foreach (double u in this.u_vals)
            {
                foreach (double v in this.v_vals)
                {
                    double x = Math.Sin(v) * Math.Pow(Math.Cos(u), M);
                    double y = Math.Sin(v) * Math.Pow(Math.Sin(u), M);

                    double z = Math.Cos(v);

                    Point3d p1 = new Point3d(x, y, z);

                    points.Add(p1);
                }
            }
            return points;
        }

        /// <summary>
        /// The Exposure property controls where in the panel a component icon
        /// will appear. There are seven possible locations (primary to septenary),
        /// each of which can be combined with the GH_Exposure.obscure flag, which
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resource1.MPinch;

        /// <summary>
        /// Each component must have a unique Guid to identify it.
        /// It is vital this Guid doesn't change otherwise old ghx files B
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("41C36840-F273-4DEA-B2CA-AABC819F6740");
    }
}