using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnipSnap.Mathematics;

[TestClass]
public class GeometryTest {
    [TestMethod]
    public void Abs() {
        0.0.Abs().AssertEquals(0);
        1.0.Abs().AssertEquals(1);
        (-1.0).Abs().AssertEquals(1);
        2.0.Abs().AssertEquals(2);
        (-2.0).Abs().AssertEquals(2);
    }
    [TestMethod]
    public void Max() {
        0.Max(0).AssertEquals(0);
        2.Max(1).AssertEquals(2);
        1.Max(2).AssertEquals(2);
    }
    [TestMethod]
    public void Min() {
        0.Min(0).AssertEquals(0);
        2.Min(1).AssertEquals(1);
        1.Min(2).AssertEquals(1);
    }
    [TestMethod]
    public void Clamp() {
        0.Clamp(1, 2).AssertEquals(1);
        0.Clamp(-1, 2).AssertEquals(0);
        0.Clamp(-3, -2).AssertEquals(-2);
    }
    [TestMethod]
    public void RangeSign() {
        0.RangeSign(1, 2).AssertEquals(-1);
        0.RangeSign(-1, 2).AssertEquals(0);
        0.RangeSign(-3, -2).AssertEquals(+1);
    }

    [TestMethod]
    public void ProperMod() {
        0.0.ProperMod(Math.PI).AssertEquals(0);
        2.0.ProperMod(Math.PI).AssertEquals(2);
        4.0.ProperMod(Math.PI).AssertEquals(4 - Math.PI, 0.00001);
        (-1.0).ProperMod(Math.PI).AssertEquals(-1 + Math.PI, 0.00001);
    }
    [TestMethod]
    public void SignedModularDifference() {
        0.0.SignedModularDifference(1, Math.PI).AssertEquals(-1, 0.00001);
        0.0.SignedModularDifference(2, Math.PI).AssertEquals(-2 + Math.PI, 0.00001);
        1.0.SignedModularDifference(0, Math.PI).AssertEquals(1, 0.00001);
    }
    [TestMethod]
    public void RangeBounceVelocity() {
        3.0.RangeBounceVelocity(-1, 0, 10).AssertEquals(+3);
        3.0.RangeBounceVelocity(0, 0, 10).AssertEquals(+3);
        3.0.RangeBounceVelocity(5, 0, 10).AssertEquals(+3);
        3.0.RangeBounceVelocity(10, 0, 10).AssertEquals(+3);
        3.0.RangeBounceVelocity(11, 0, 10).AssertEquals(-3);

        (-3.0).RangeBounceVelocity(-1, 0, 10).AssertEquals(+3);
        (-3.0).RangeBounceVelocity(0, 0, 10).AssertEquals(-3);
        (-3.0).RangeBounceVelocity(5, 0, 10).AssertEquals(-3);
        (-3.0).RangeBounceVelocity(10, 0, 10).AssertEquals(-3);
        (-3.0).RangeBounceVelocity(11, 0, 10).AssertEquals(-3);
    }
    [TestMethod]
    public void ProportionToByte() {
        0.0.ProportionToByte().AssertEquals((byte)0);
        0.25.ProportionToByte().AssertEquals((byte)64);
        0.5.ProportionToByte().AssertEquals((byte)128);
        1.0.ProportionToByte().AssertEquals((byte)255);
        
        53489523.0.ProportionToByte().AssertEquals((byte)255);
        (-4234.0).ProportionToByte().AssertEquals((byte)0);
    }
}
