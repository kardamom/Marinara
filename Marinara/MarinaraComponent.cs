using ExtensionMethods;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Marinara
{
    public abstract class MarinaraComponent : GH_Component
    {
        protected List<double> u_vals = new List<double>();
        protected List<double> v_vals = new List<double>();
        protected Interval uInterval = new Interval();
        protected Interval vInterval = new Interval();
        protected int steps = 100;
        protected double max_dimension = -1;

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

        protected abstract Interval DefaultUDomain();

        protected abstract Interval DefaultVDomain();

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // Every Marinara class has at least these four parameters
            pManager.AddIntervalParameter("u domain", "u", "Domain of u", GH_ParamAccess.item, this.DefaultUDomain());
            pManager.AddIntervalParameter("v domain", "v", "Domain of v", GH_ParamAccess.item, this.DefaultVDomain());
            pManager.AddIntegerParameter("steps", "steps", "Number of points to create per u and v", GH_ParamAccess.item, steps);
            pManager.AddNumberParameter("max dimension", "max", "Maximum length in any axis. Set to -1 to leave actual size (mm)", GH_ParamAccess.item, max_dimension);
        }

        protected void RetrieveAndInitUV(IGH_DataAccess DA)
        {
            // First, we need to retrieve all data from the input parameters.
            // We'll start by declaring variables and assigning them starting values.
            GH_Interval u_int = new GH_Interval(this.DefaultUDomain());
            GH_Interval v_int = new GH_Interval(this.DefaultVDomain());

            Debug.WriteLine(u_int.ToString() + " " + v_int.ToString());

            // Then we need to access the input parameters individually.
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref u_int)) return;
            if (!DA.GetData(1, ref v_int)) return;
            if (!DA.GetData(2, ref steps)) return;
            if (!DA.GetData(3, ref max_dimension)) return;

            this.InitUV(u_int, v_int, steps);
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
            uInterval = new Interval(u_int.Value.T0, u_int.Value.T1);
            vInterval = new Interval(v_int.Value.T0, v_int.Value.T1);

            u_vals = InitUVValues(u_int, steps);
            v_vals = InitUVValues(v_int, steps);

            return true;
        }

        private List<Point3d> NormalizePoints(List<Point3d> pts, double max_dim)
        {
            double x_min, x_max, y_min, y_max, z_min, z_max, x_range, y_range, z_range;
            x_min = y_min = z_min = float.MaxValue;
            x_max = y_max = z_max = float.MinValue;
            x_range = y_range = z_range = 0;

            foreach (Point3d pt in pts)
            {
                x_max = Math.Max(x_max, pt.X);
                y_max = Math.Max(y_max, pt.Y);
                z_max = Math.Max(z_max, pt.Z);

                x_min = Math.Min(x_min, pt.X);
                y_min = Math.Min(y_min, pt.Y);
                z_min = Math.Min(z_min, pt.Z);
            }
            x_range = x_max - x_min;
            y_range = y_max - y_min;
            z_range = z_max - z_min;

            double max_range = Math.Max(x_range, Math.Max(y_range, z_range));
            double ratio = max_dimension / max_range;

            Console.WriteLine($"Max range <{max_range}> Ratio >{ratio}>");

            List<Point3d> new_pts = new List<Point3d>();
            for (int i = 0; i < pts.Count; i++)
            {
                Point3d new_pt = new Point3d();
                new_pt.X = pts[i].X * ratio;
                new_pt.Y = pts[i].Y * ratio;
                new_pt.Z = pts[i].Z * ratio;
                new_pts.Add(new_pt);
            }
            return new_pts;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // pManager.AddPointParameter("Points", "pts", "Output points", GH_ParamAccess.list);
            pManager.AddSurfaceParameter("Surface", "S", "Surface from points", GH_ParamAccess.list);
            pManager.AddCurveParameter("UCurves", "UC", "U curves", GH_ParamAccess.list);
            pManager.AddCurveParameter("VCurves", "VC", "V curves", GH_ParamAccess.list);
            pManager.AddPointParameter("U", "utree", "Output U points", GH_ParamAccess.tree);
            pManager.AddPointParameter("V", "vtree", "Output V points", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.RetrieveAndInitUV(DA);

            List<Point3d> points = this.SolveMarinara(DA);

            if (this.max_dimension != -1)
            {
                points = this.NormalizePoints(points, this.max_dimension);
            }

            NurbsSurface surface = NurbsSurface.CreateFromPoints(points, this.steps, this.steps, 3, 3);

            List<NurbsCurve> uCurves = new List<NurbsCurve>();
            List<NurbsCurve> vCurves = new List<NurbsCurve>();
            List<List<Point3d>> nestedPoints = new List<List<Point3d>>();

            GH_Structure<GH_Point> uPointsTree = new GH_Structure<GH_Point>();
            GH_Structure<GH_Point> vPointsTree = new GH_Structure<GH_Point>();

            for (int i = 0; i < steps; i++)
            {
                int start = i * steps;

                List<Point3d> slice = points.GetRange(start, steps);

                NurbsCurve curve = NurbsCurve.Create(false, 3, slice);
                nestedPoints.Add(slice);
                uCurves.Add(curve);

                GH_Path pth = new GH_Path(i);
                for (int k = 0; k < steps; k++)
                {
                    GH_Point ghP = new GH_Point(slice[k]);
                    uPointsTree.Append(ghP, pth);
                }
            }
            var transposedPoints = nestedPoints.Transpose<Point3d>();

            for (int j = 0; j < transposedPoints.Count(); j++)
            {
                NurbsCurve curve = NurbsCurve.Create(false, 3, transposedPoints.ElementAt(j));
                vCurves.Add(curve);

                GH_Path pth = new GH_Path(j);
                for (int k = 0; k < steps; k++)
                {
                    GH_Point ghP = new GH_Point(transposedPoints.ElementAt(j).ElementAt(k));
                    vPointsTree.Append(ghP, pth);
                }
            }

            //DA.SetDataList(0, points);
            DA.SetData(0, surface);
            DA.SetDataList(1, uCurves);
            DA.SetDataList(2, vCurves);
            DA.SetDataTree(3, uPointsTree);
            DA.SetDataTree(4, vPointsTree);
        }

        public abstract List<Point3d> SolveMarinara(IGH_DataAccess DA);

        public override Guid ComponentGuid => new Guid("96AA1085-52B3-45CB-AF3E-F1B2259EC521");
    }
}