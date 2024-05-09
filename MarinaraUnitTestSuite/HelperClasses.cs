using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MarinaraTestHelpers
{
    public struct TestPoint3d
    {
        public float x;
        public float y;
        public float z;

        public TestPoint3d(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class HelperMethods
    {
        public List<TestPoint3d> NormalizePoints(List<TestPoint3d> pts, float max_dimension)
        {
            float x_min, x_max, y_min, y_max, z_min, z_max, x_range, y_range, z_range;
            x_min = y_min = z_min = float.MaxValue;
            x_max = y_max = z_max = float.MinValue;
            x_range = y_range = z_range = 0;

            foreach (TestPoint3d pt in pts)
            {
                x_max = Math.Max(x_max, pt.x);
                y_max = Math.Max(y_max, pt.y);
                z_max = Math.Max(z_max, pt.z);

                x_min = Math.Min(x_min, pt.x);
                y_min = Math.Min(y_min, pt.y);
                z_min = Math.Min(z_min, pt.z);
            }
            x_range = x_max - x_min;
            y_range = y_max - y_min;
            z_range = z_max - z_min;

            float max_range = Math.Max(x_range, Math.Max(y_range, z_range));
            float ratio = max_dimension / max_range;

            List<TestPoint3d> new_pts = new List<TestPoint3d>();
            for (int i = 0; i < pts.Count; i++)
            {
                TestPoint3d new_pt = new TestPoint3d();
                new_pt.x = pts[i].x * ratio;
                new_pt.y = pts[i].y * ratio;
                new_pt.z = pts[i].z * ratio;
                new_pts.Add(new_pt);
            }
            return new_pts;
        }
    }
}