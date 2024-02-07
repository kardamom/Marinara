using Ed.Eto;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Marinara.Properties;
using Rhino.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

      
        private double plumpness = 3;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddNumberParameter("plumpness", "Plumpness", "How plump is the pasta", GH_ParamAccess.item, plumpness);

        }
        protected override Interval DefaultUDomain()
        {
            return new Interval(0, 80);
        }
        protected override Interval DefaultVDomain()
        {
            return new Interval(0, 80);
        }


        public override List<Point3d> SolveMarinara(IGH_DataAccess DA)
        {
            Debug.WriteLine($"Fiocchi");
            List<Point3d> points = new List<Point3d>();
            if (!DA.GetData(3, ref this.plumpness)) return points;

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
                    points.Add(p1);

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
    public class MRadiatori : MarinaraComponent
    {
        public MRadiatori()
         : base("MRadiatori",
                "MRadiatori",
                "Ruffled radiators.",
               "Marinara",
               "Pasta")
        {
        }


        private int tightness = 50;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddIntegerParameter("tightness", "Tightness", "How tight is the shaped coiled", GH_ParamAccess.item, tightness);

        }
        protected override Interval DefaultUDomain()
        {
            return new Interval(0, 70);
        }
        protected override Interval DefaultVDomain()
        {
            return new Interval(0, 1000);
        }


        public override List<Point3d> SolveMarinara(IGH_DataAccess DA)
        {
            Debug.WriteLine($"Radiatori");
            List<Point3d> points = new List<Point3d>();
            if (!DA.GetData(3, ref this.tightness)) return points;

            foreach (double u in this.u_vals)
            {
                foreach (double v in this.v_vals)
                {
                    // calculate X

                    double multiplier = _GetMultiplier(u, v);
                    double x = multiplier * Math.Cos((4 * Math.PI * u) / 175);
                    double y = multiplier * Math.Sin((4 * Math.PI * u) / 175);

                    double z = (v / 50) + (Math.Cos((3 * Math.PI * u) / 14) * Math.Sin((Math.PI * v) / 1000));


                    Point3d p1 = new Point3d(x, y, z);
                    Debug.WriteLine($"Generating point ({x},{y},{z})");
                    points.Add(p1);

                }
            }
            return points;
        }

        private double _GetMultiplier(double u, double v)
        {
            double offset = 1.5;
            int exponent = this.tightness;
            double val = offset + (3 * Math.Pow(u / 70, 5)) + (4 * Math.Pow(Math.Sin(Math.PI * (v / 200)), exponent));
            //double val = offset + (3 * Math.Pow(u / 70, this.tightness)) + (4 * Math.Sin(Math.Pow((Math.PI * v), exponent)));
            return val;


        }

        public override Guid ComponentGuid => new Guid("0006C2DE-8506-4B0E-8DD0-B1FA92A23F6C");
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Resource1.MRadiatori;
    }
    public class MFusilliCapri : MarinaraComponent
    {
        public MFusilliCapri()
         : base("MFusilliCapri",
                "MFusilliCapri",
                "Longer fusilli",
               "Marinara",
               "Pasta")
        {
        }


        private int radius = 6;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddIntegerParameter("radius", "radius", "Radius of the pasta", GH_ParamAccess.item, radius);

        }
        protected override Interval DefaultUDomain()
        {
            return new Interval(0, 150);
        }
        protected override Interval DefaultVDomain()
        {
            return new Interval(0, 50);
        }


        public override List<Point3d> SolveMarinara(IGH_DataAccess DA)
        {
            Debug.WriteLine($"Radiatori");
            List<Point3d> points = new List<Point3d>();
            if (!DA.GetData(3, ref this.radius)) return points;

            foreach (double u in this.u_vals)
            {
                foreach (double v in this.v_vals)
                {
                    // calculate X

                    double multiplier = this.radius;
                    double x = multiplier * Math.Cos((Math.PI * v) / 50) * Math.Cos(Math.PI * ((u + 2.5) / 25));
                    double y = multiplier * Math.Cos((Math.PI * v) / 50) * Math.Sin(Math.PI * ((u + 2.5) / 25));

                    double z = ((2 * u) / 3) + 14 * (Math.Cos(Math.PI * ((v + 25) / 50))); 


                    Point3d p1 = new Point3d(x, y, z);
                    Debug.WriteLine($"Generating point ({x},{y},{z})");
                    points.Add(p1);

                }
            }
            return points;
        }


        public override Guid ComponentGuid => new Guid("9E7580C9-25E2-4923-841E-9AED34AF8B66");
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Resource1.MFusilliCapri;
    }

    public class MCasarecce : MarinaraComponent
    {
        public MCasarecce()
         : base("MCasarecce",
                "MCasarecce",
                "S-shaped",
               "Marinara",
               "Pasta")
        {
        }


        private double radius = 0.5;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddNumberParameter("radius", "radius", "Radius of the pasta", GH_ParamAccess.item, radius);

        }
        protected override Interval DefaultUDomain()
        {
            return new Interval(0, 60);
        }
        protected override Interval DefaultVDomain()
        {
            return new Interval(0, 60);
        }


        public override List<Point3d> SolveMarinara(IGH_DataAccess DA)
        {
            Debug.WriteLine($"Casarecce");
            List<Point3d> points = new List<Point3d>();
            if (!DA.GetData(3, ref this.radius)) return points;

            foreach (double u in this.u_vals)
            {
                foreach (double v in this.v_vals)
                {
                    // calculate X

                    double multiplier = this.radius;
                    double x, y;
                    // change to half max u
                    if ( u <= 30 )
                    {
                        x = (multiplier * Math.Cos(Math.PI * v / 30)) + (multiplier * Math.Cos(Math.PI * ((2*u + v + 16)/40))); 

                    }
                    else { 
                        x = Math.Cos(Math.PI * v / 40) + 
                            (multiplier * Math.Cos(Math.PI * v / 30)) + 
                            (multiplier * Math.Sin(Math.PI * ((2 * u - v) / 40)));
                    }
                    if (u <= 30)
                    {
                        y = (multiplier * Math.Sin(Math.PI * v / 30)) + (multiplier * Math.Sin(Math.PI * ((2 * u + v + 16) / 40)));

                    }
                    else
                    {
                        y = Math.Sin(Math.PI * v / 40) +
                            (multiplier * Math.Sin(Math.PI * v / 30)) +
                            (multiplier * Math.Cos(Math.PI * ((2 * u - v) / 40)));
                    }


                    double z = v / 4;


                    Point3d p1 = new Point3d(x, y, z);
                    Debug.WriteLine($"Generating point ({x},{y},{z})");
                    points.Add(p1);

                }
            }
            return points;
        }


        public override Guid ComponentGuid => new Guid("DE23DC34-15AA-4417-9CD4-C9EE176CE0E4");
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Resource1.MCasarecce;
    }
}