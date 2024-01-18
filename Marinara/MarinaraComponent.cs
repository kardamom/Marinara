using Ed.Eto;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace Marinara
{


    public class MarinaraComponent : GH_Component
    {


        public int DEFAULT_STEPS = 10;
        protected List<double> u_vals = new List<double>();
        protected List<double> v_vals = new List<double>();

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>

        public MarinaraComponent(String c_base, String cmd, String longName)
         : base(c_base, cmd,
           longName,
           "Marinara", "Primitive")
        {
        }

        public MarinaraComponent(String c_base, String cmd, String longName, String Category, String SubCategory)
         : base(c_base, cmd,
           longName,
           Category, SubCategory)
        {
        }
        public List<double> InitUVValues(GH_Interval interval, int steps)
        {
            List<double> vals = new List<double>();

            double start = interval.Value.T0;
            double end = interval.Value.T1;

            double step = (end - start) / (steps - 1);

            double next = start;
            for (int i = 0; i < steps; i++)
            {
                next = start + (i * step);
                // Debug.WriteLine($"Start {start} End {end} step {step} next {next}");
                vals.Add(next);
            }
            return vals;
        }

        public Boolean InitUV(GH_Interval u_int, GH_Interval v_int, int steps)
        {
            u_vals = InitUVValues(u_int, steps);
            v_vals = InitUVValues(v_int, steps);

            return true;
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // Use the pManager object to register your input parameters.
            // You can often supply default values when creating parameters.
            // All parameters must have the correct access type. If you want 
            // to import lists or trees of values, modify the ParamAccess flag.
           

            // If you want to change properties of certain parameters, 
            // you can use the pManager instance to access them by index:
            //pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
           

            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
          
        }
        public override Guid ComponentGuid => new Guid("96AA1085-52B3-45CB-AF3E-F1B2259EC521");
    }
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
         "Marinara", "Primitive")
        {
           

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // Use the pManager object to register your input parameters.
            // You can often supply default values when creating parameters.
            // All parameters must have the correct access type. If you want 
            // to import lists or trees of values, modify the ParamAccess flag.
      

            pManager.AddIntervalParameter("u domain", "u", "Domain of u", GH_ParamAccess.item);
            pManager.AddIntervalParameter("v domain", "v", "Domain of v", GH_ParamAccess.item);
            pManager.AddIntegerParameter("steps", "steps", "Number of points to create per u and v", GH_ParamAccess.item, DEFAULT_STEPS);

            // If you want to change properties of certain parameters, 
            // you can use the pManager instance to access them by index:
            //pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
            pManager.AddPointParameter("Points", "pts", "Output points", GH_ParamAccess.list);

            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // First, we need to retrieve all data from the input parameters.
            // We'll start by declaring variables and assigning them starting values.
            GH_Interval u_int = new GH_Interval();
            GH_Interval v_int = new GH_Interval();
            int steps = 0;

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref u_int)) return;
            if (!DA.GetData(1, ref v_int)) return;
            if (!DA.GetData(2, ref steps)) return;

            // We should now validate the data and warn the user if invalid data is supplied.
            /*
            if (radius0 < 0.0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Inner radius must be bigger than or equal to zero");
                return;
            }
            if (radius1 <= radius0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Outer radius must be bigger than the inner radius");
                return;
            }
            if (turns <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Spiral turn count must be bigger than or equal to one");
                return;
            }
            */

            InitUV(u_int, v_int, steps);

            List<GH_Point> points = RunForSphere();
            // Finally assign the spiral to the output parameter.
            DA.SetDataList(0, points);
        }


        public List<GH_Point> RunForSphere()
        {
            Debug.WriteLine($"Sphere");
            List<GH_Point> points = new List<GH_Point>();
            foreach (double u in u_vals)
            {
                foreach (double v in v_vals)
                {
                    double x = Math.Sin(v) * Math.Cos(u);
                    double y = Math.Sin(v) * Math.Sin(u);
                    double z = Math.Cos(v);

                    Point3d p1 = new Point3d(x, y, z);
                    //Debug.WriteLine($"Generating point ({x},{y},{z})");
                    points.Add(new GH_Point(p1));

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
        protected override System.Drawing.Bitmap Icon => null;

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
          : base("MPinch", "MPinch", "Pinch the geometry using exponents.", "Marinara", "Primitive")
        {
           
        }

        private int pinch_degree = 0;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // Use the pManager object to register your input parameters.
            // You can often supply default values when creating parameters.
            // All parameters must have the correct access type. If you want 
            // to import lists or trees of values, modify the ParamAccess flag.
            pManager.AddIntervalParameter("u domain", "u", "Domain of u", GH_ParamAccess.item);
            pManager.AddIntervalParameter("v domain", "v", "Domain of v", GH_ParamAccess.item);
            pManager.AddIntegerParameter("steps", "steps", "Number of points to create per u and v", GH_ParamAccess.item, DEFAULT_STEPS);
            pManager.AddIntegerParameter("exponent", "exponent", "The pinch degree", GH_ParamAccess.item, DEFAULT_STEPS);

            // If you want to change properties of certain parameters, 
            // you can use the pManager instance to access them by index:
            //pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
            pManager.AddPointParameter("Points", "pts", "Output points", GH_ParamAccess.list);

            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // First, we need to retrieve all data from the input parameters.
            // We'll start by declaring variables and assigning them starting values.
            GH_Interval u_int = new GH_Interval();
            GH_Interval v_int = new GH_Interval();
            int steps = 0;

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref u_int)) return;
            if (!DA.GetData(1, ref v_int)) return;
            if (!DA.GetData(2, ref steps)) return;
            if (!DA.GetData(3, ref pinch_degree)) return;
            // We should now validate the data and warn the user if invalid data is supplied.
            /*
            if (radius0 < 0.0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Inner radius must be bigger than or equal to zero");
                return;
            }
            if (radius1 <= radius0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Outer radius must be bigger than the inner radius");
                return;
            }
            if (turns <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Spiral turn count must be bigger than or equal to one");
                return;
            }
            */

            InitUV(u_int, v_int, steps);

            List<GH_Point> points = RunForPinch();
            // Finally assign the spiral to the output parameter.
            DA.SetDataList(0, points);

        }
        public List<GH_Point> RunForPinch()
        {
            List<GH_Point> points = new List<GH_Point>();
            Debug.WriteLine($"Pinching");
            int M = pinch_degree;
            foreach (double u in u_vals)
            {
                foreach (double v in v_vals)
                {
                    double x = Math.Sin(v) * Math.Pow(Math.Cos(u), M);
                    double y = Math.Sin(v) * Math.Pow(Math.Sin(u), M);

                    double z = Math.Cos(v);

                    Point3d p1 = new Point3d(x, y, z);
                    
                    points.Add(new GH_Point(p1));

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
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files B
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("41C36840-F273-4DEA-B2CA-AABC819F6740");
    }
    
}

