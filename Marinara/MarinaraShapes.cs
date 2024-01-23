using Ed.Eto;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Marinara.Properties;
using Rhino.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace Marinara
{
    public class MFiocchiRigati : MarinaraComponent
    {
        public MFiocchiRigati()
         : base("MFiocchiRigati",
                "MFiocchiRigati",
                "Grooved flakes.",
               "Marinara",
               "Pasta")
        {
        }

        private double DEFAULT_PLUMPNESS = 3;
        private double plumpness;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddNumberParameter("plumpness", "Plumpness", "How plump is the pasta", GH_ParamAccess.item, DEFAULT_PLUMPNESS);

        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            base.RetrieveAndInitUV(DA);
            if (!DA.GetData(3, ref this.plumpness)) return;

            List<GH_Point> points = SolveMarinara();
            // Assign the points to the output
            DA.SetDataList(0, points);
        }

        public List<GH_Point> SolveMarinara()
        {
            Debug.WriteLine($"Fiocchi");
            List<GH_Point> points = new List<GH_Point>();
            foreach (double u in this.u_vals)
            {
                foreach (double v in this.v_vals)
                {
                    // calculate X
                    double x = ((30 * u) / 80);
                    double x_plus;
                    if (u >= 20 && u <= 60)
                    {
                        x_plus = 7 * 
                            (Math.Pow(Math.Sin((Math.PI * (u + 40)) / 40), 3) * 
                            Math.Pow(Math.Sin(((Math.PI * (v + 110)) / 100)), 9));

                    }
                    else
                    {
                        x_plus = Alpha(u, v);
                    }
                    x = x + x_plus;

                    // calculate Y
                    double y = Beta(u, v);
                    y -= (4 * Math.Sin((Math.PI * u) / 80) * Math.Sin(Math.PI * ((70 - v) / 120)));

                    // calculate Z
                    double z = (this.plumpness * Math.Sin(Math.PI * ((u + 10) / 20)) * 
                        Math.Pow(Math.Sin(((Math.PI * v) / 80)), 1.5))
                           - (0.7 * (Math.Pow((Math.Sin((Math.PI * ((3 * v) / 8) + 1)) / 2), 3+1)));

                    Point3d p1 = new Point3d(x, y, z);
                    Debug.WriteLine($"Generating point ({x},{y},{z})");
                    points.Add(new GH_Point(p1));

                }
            }
            return points;
        }

        private double Alpha(double u, double v)
        {
            return 10 * Math.Cos(Math.PI * ((u + 80) / 80)) * 
                    Math.Pow(Math.Sin(Math.PI * ((v + 110) / 100)), 9);
        }

        private double Beta(double u, double v)
        {
            return ((35 * v) / 80) + (4 * Math.Sin((Math.PI * u) / 80) * Math.Sin(Math.PI * ((v - 10) / 120)));
        }


        public override Guid ComponentGuid => new Guid("9A37514B-4DEC-4843-B0A6-8CFE61555E5C");
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Resource1.MFiocchiRigati;
    }
}