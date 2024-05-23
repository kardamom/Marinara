using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using MarinaraTestHelpers;
using System.Diagnostics.Contracts;

namespace MarinaraUnitTestSuite
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestNormalize()
        {
            Assert.AreEqual(1, 1);

            List<TestPoint3d> pts = new List<TestPoint3d>();
            pts.Add(new TestPoint3d(-1, 2, 1));
            pts.Add(new TestPoint3d(1, 4, -1));
            pts.Add(new TestPoint3d(3, 5, 1));

            float x_min, x_max, y_min, y_max, z_min, z_max, x_range, y_range, z_range;
            x_min = y_min = z_min = float.MaxValue;
            x_max = y_max = z_max = float.MinValue;
            x_range = y_range = z_range = 0;

            Assert.AreEqual(3, pts.Count);
            x_max = pts.Max(x => x.x);
            x_min = pts.Min(x => x.x);
            Assert.AreEqual(x_max, 3);
            Assert.AreEqual(x_min, -1);

            y_max = pts.Max(x => x.y);
            Assert.AreEqual(y_max, 5);

            z_max = pts.Max(x => x.z);
            Assert.AreEqual(z_max, 1);

            foreach (TestPoint3d pt in pts)
            {
                x_max = Math.Max(x_max, pt.x);
                y_max = Math.Max(y_max, pt.y);
                z_max = Math.Max(z_max, pt.z);

                x_min = Math.Min(x_min, pt.x);
                y_min = Math.Min(y_min, pt.y);
                z_min = Math.Min(z_min, pt.z);
            }

            Assert.AreEqual(3, x_max);
            Assert.AreEqual(-1, x_min);

            Assert.AreEqual(5, y_max);
            Assert.AreEqual(2, y_min);
            Assert.AreEqual(-1, z_min);
            Assert.AreEqual(1, z_max);

            x_range = x_max - x_min;
            y_range = y_max - y_min;
            z_range = z_max - z_min;

            Assert.AreEqual(4, x_range);
            Assert.AreEqual(3, y_range);
            Assert.AreEqual(2, z_range);

            float normalized_len = 10;
            HelperMethods hm = new HelperMethods();
            List<TestPoint3d> new_pts = hm.NormalizePoints(pts, normalized_len);

            Assert.AreEqual(new_pts[0].x, -2.5);
            Assert.AreEqual(new_pts[2].x, 7.5);
            // Assert.AreEqual(
        }
    }
}