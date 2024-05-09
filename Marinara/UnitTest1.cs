using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Rhino.Geometry;
using System.Collections.Generic;

namespace MarinaraUnitTestSuite
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBasic()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestLinq()
        {
            List<Point3d> points = new List<Point3d>();
            Assert.AreEqual(1, 1);
        }
    }
}