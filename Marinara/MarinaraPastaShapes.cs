using Grasshopper.Kernel;
using Marinara.Properties;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
                           - (0.7 * (Math.Pow((Math.Sin((Math.PI * ((3 * v) / 8) + 1)) / 2), 3 + 1)));

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
                    if (u <= 30)
                    {
                        x = (multiplier * Math.Cos(Math.PI * v / 30)) + (multiplier * Math.Cos(Math.PI * ((2 * u + v + 16) / 40)));
                    }
                    else
                    {
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

    public class MLancette : MarinaraComponent
    {
        public MLancette()
         : base("MLancette",
                "MLancette",
                "Hands of a clock",
               "Marinara",
               "Pasta")
        {
        }

        private double radius = 4;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddNumberParameter("radius", "radius", "Radius of the pasta", GH_ParamAccess.item, radius);
        }

        protected override Interval DefaultUDomain()
        {
            return new Interval(0, 50);
        }

        protected override Interval DefaultVDomain()
        {
            return new Interval(0, 90);
        }

        public override List<Point3d> SolveMarinara(IGH_DataAccess DA)
        {
            Debug.WriteLine($"Lancette");
            List<Point3d> points = new List<Point3d>();
            if (!DA.GetData(3, ref this.radius)) return points;

            foreach (double u in this.u_vals)
            {
                foreach (double v in this.v_vals)
                {
                    double x, y, z;

                    x = this.radius * Math.Cos(Math.PI * 3 * this.HelperZeta(u, v) / 50);
                    y = this.radius * Math.Sin(Math.PI * 3 * this.HelperZeta(u, v) / 50) *
                        Math.Pow((0.3 + (1 - Math.Sin(Math.PI * v / 90))), 0.6);
                    z = v / 3;
                    Point3d p1 = new Point3d(x, y, z);

                    points.Add(p1);
                }
            }
            return points;
        }

        private double HelperAlpha(double v)
        {
            return this.radius / 10 * Math.Sin(Math.PI * 5 * v / 9);
        }

        private double HelperBeta(double v)
        {
            return Math.Sin(Math.PI * (v + 45) / 90);
        }

        private double HelperGamma(double u, double v)
        {
            double gamma = ((3 * u - 75) / 5) * Math.Sin(Math.PI * v / 90) -
                Math.Pow(1 - Math.Sin(Math.PI * u / 50), 25) *
                HelperAlpha(v) *
                HelperBeta(v);
            return gamma;
        }

        private double HelperZeta(double u, double v)
        {
            double zeta;
            if (u < 25)
            {
                zeta = HelperGamma(u, v);
            }
            else
            {
                zeta = 30 * ((u - 25) / 50) * Math.Sin(Math.PI * v / 90) +
                    Math.Sin(Math.PI * ((u - 25) / 50)) *
                    HelperAlpha(v) * HelperBeta(v);
            }
            return zeta;
        }

        public override Guid ComponentGuid => new Guid("FE653446-179D-49D8-AD90-7E3241DD38E7");
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Resource1.MLancette;
    }

    public class MScialatielli : MarinaraComponent
    {
        public MScialatielli()
         : base("MScialatielli",
                "MScialatielli",
                "Rustic Amalfi pasta",
               "Marinara",
               "Pasta")
        {
        }

        private double pinch = 3;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddNumberParameter("pinch", "pinch", "How pinched is the profile", GH_ParamAccess.item, pinch);
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
            Debug.WriteLine($"Lancette");
            List<Point3d> points = new List<Point3d>();
            if (!DA.GetData(3, ref this.pinch)) return points;

            foreach (double u in this.u_vals)
            {
                foreach (double v in this.v_vals)
                {
                    double x, y, z;

                    x = 0.1 * Math.Cos(Math.PI * u / (uInterval.T1 / 2)) +
                        0.1 * Math.Pow(Math.Cos(Math.PI * (u + uInterval.T1 / 20) / (uInterval.T1 / 2)), this.pinch) +
                        0.1 * Math.Sin(Math.PI * v / vInterval.T1);
                    y = 0.1 * Math.Cos(Math.PI * u / (uInterval.T1 / 2)) +
                        0.2 * Math.Pow(Math.Sin(Math.PI * u / (uInterval.T1 / 2)), this.pinch) +
                        0.1 * Math.Sin(Math.PI * v / vInterval.T1);
                    z = 3 * v / vInterval.T1;
                    Point3d p1 = new Point3d(x, y, z);

                    points.Add(p1);
                }
            }
            return points;
        }

        public override Guid ComponentGuid => new Guid("BB721D66-1FC6-48AC-8D40-EA140F99A024");
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Resource1.MScialatielli;
    }

    public class MAgnolotti : MarinaraComponent
    {
        public MAgnolotti()
         : base("MAgnolotti",
                "MAgnolotti",
                "Shells from Piedmont.",
               "Marinara",
               "Pasta")
        {
        }

        private double plumpness = 3;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            pManager.AddNumberParameter("plumpness", "plumpness", "How plump is the pasta", GH_ParamAccess.item, plumpness);
        }

        protected override Interval DefaultUDomain()
        {
            return new Interval(0, 60);
        }

        protected override Interval DefaultVDomain()
        {
            return new Interval(0, 100);
        }

        public override List<Point3d> SolveMarinara(IGH_DataAccess DA)
        {
            Debug.WriteLine(this.GetType().Name);
            List<Point3d> points = new List<Point3d>();
            if (!DA.GetData(3, ref this.plumpness)) return points;

            foreach (double u in this.u_vals)
            {
                foreach (double v in this.v_vals)
                {
                    double x, y, z;

                    x = (10 * Math.Pow(Math.Sin(Math.PI * (u / 120)), 0.5) +
                        ((u / 400) * Math.Sin((3 * v / 10) * Math.PI)))
                        *
                        Math.Cos(((19 * v * Math.PI) / 2000) + (0.03 * Math.PI));

                    y = ((10 * Math.Sin(Math.PI * (u / 120))) +
                          ((u / 400) * Math.Cos((3 * v / 10) * Math.PI))) *
                        Math.Sin((19 * v * Math.PI / 2000) + 0.03 * Math.PI);

                    z = (5 * (Math.Pow(Math.Cos(Math.PI * u / 120), 5)) *
                        Math.Sin(Math.PI * v / 100)) -
                        (5 * (Math.Sin(Math.PI * v / 100)) *
                        Math.Pow(Math.Cos(Math.PI * u / 120), 200));

                    Point3d p1 = new Point3d(x, y, z);

                    points.Add(p1);
                }
            }
            return points;
        }

        public override Guid ComponentGuid => new Guid("B89724A4-7477-4E96-88EC-DE663607F3BC");
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Resource1.MAgnolotti;
    }

    public class MCinqueSapori : MarinaraComponent
    {
        public MCinqueSapori()
         : base("MCinqueSapori",
                "MCinqueSapori",
                "Curls in five flavors.",
               "Marinara",
               "Pasta")
        {
        }

        protected override Interval DefaultUDomain()
        {
            return new Interval(0, 20);
        }

        protected override Interval DefaultVDomain()
        {
            return new Interval(0, 100);
        }

        public override List<Point3d> SolveMarinara(IGH_DataAccess DA)
        {
            Debug.WriteLine(this.GetType().Name);
            List<Point3d> points = new List<Point3d>();

            foreach (double u in this.u_vals)
            {
                foreach (double v in this.v_vals)
                {
                    double x, y, z;
                    double pi = Math.PI;

                    x = 1.5 * Math.Cos(pi * u / 20) *
                        (1 + 0.5 * Math.Sin(pi * v / 25) * Math.Sin(pi * u / 40)) +
                        0.43 * Math.Sin(pi * ((v + 18.75) / 25)) * Math.Cos(pi * u / 40) +
                        2 * Math.Cos(pi * v / 50);

                    y = 1.5 * Math.Pow(Math.Sin(pi * u / 20), 3) +
                        Math.Cos(pi * v / 25);
                    z = Math.Sin(pi * v / 100) + 20 * Math.Pow(Math.Cos(pi * v / 200), 2);

                    Point3d p1 = new Point3d(x, y, z);

                    points.Add(p1);
                }
            }
            return points;
        }

        public override Guid ComponentGuid => new Guid("F48CB779-D7A8-4E11-BF07-4C9975D5CD78");
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Resource1.MCinqueSapori;
    }
}