using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnipSnap.Mathematics;

[TestClass]
public class MathUtilitiesTest {
    private static readonly Point P01 = new Point(0, 1);
    private static readonly Point P10 = new Point(1, 0);

    [TestMethod]
    public void Normal() {
        new Vector(2, 0).Normal().AssertEquals(new Vector(1, 0));
        new Vector(-2, 0).Normal().AssertEquals(new Vector(-1, 0));
        new Vector(0, 2).Normal().AssertEquals(new Vector(0, 1));
        new Vector(0, 0).Normal().AssertEquals(new Vector(0, 0));
    }
    [TestMethod]
    public void Cross() {
        new Vector(0, 1).Cross(new Vector(0, 1)).AssertEquals(0);
        new Vector(1, 0).Cross(new Vector(0, 1)).AssertEquals(1);
        new Vector(0, 1).Cross(new Vector(1, 0)).AssertEquals(-1);
        new Vector(0, 1).Cross(new Vector(2, 0)).AssertEquals(-2);
    }
    [TestMethod]
    public void ScalarProjectOnto() {
        new Vector(0, 2).ScalarProjectOnto(new Vector(0, 3)).AssertEquals(2);
        new Vector(4, 2).ScalarProjectOnto(new Vector(0, 3)).AssertEquals(2);
    }

    [TestMethod]
    public void LineSegmentProperties() {
        var line = P01.To(P10);
        line.Start.AssertEquals(P01);
        line.End.AssertEquals(P10);
        line.Delta.AssertEquals(new Vector(1, -1));
    }

    [TestMethod]
    public void LineDefinedByMovingEndPointsCrossesOrigin() {
        // parallel
        GeometryUtilities.LineDefinedByMovingEndPointsCrossesOrigin(
            new Point(-1, -1).Sweep(new Vector(0, 2)),
            new Point(+1, -1).Sweep(new Vector(0, 2))
        ).AssertEquals(new GeometryUtilities.IntersectionParameters { T = 0.5, S = 0.5 });
        GeometryUtilities.LineDefinedByMovingEndPointsCrossesOrigin(
            new Point(-1, -1).Sweep(new Vector(2, 0)),
            new Point(-1, +1).Sweep(new Vector(2, 0))
        ).AssertEquals(new GeometryUtilities.IntersectionParameters { T = 0.5, S = 0.5 });
        GeometryUtilities.LineDefinedByMovingEndPointsCrossesOrigin(
            new Point(-1, -1).Sweep(new Vector(0.5, 0)),
            new Point(-1, +1).Sweep(new Vector(0.5, 0))
        ).AssertEquals(null);

        // fixed
        GeometryUtilities.LineDefinedByMovingEndPointsCrossesOrigin(
            new Point(-1, 0).Sweep(new Vector(0, 0)),
            new Point(+1, 0).Sweep(new Vector(0, 0))
        ).AssertEquals(new GeometryUtilities.IntersectionParameters { T = 0, S = 0.5 });
        GeometryUtilities.LineDefinedByMovingEndPointsCrossesOrigin(
            new Point(-1, -1).Sweep(new Vector(0, 0)),
            new Point(+1, -1).Sweep(new Vector(0, 0))
        ).AssertEquals(null);

        // orthogonal
        GeometryUtilities.LineDefinedByMovingEndPointsCrossesOrigin(
            new Point(-1, +10).Sweep(new Vector(0, -11)),
            new Point(-1, -1).Sweep(new Vector(11, 0))
        ).HasValue.AssertIsTrue();
        GeometryUtilities.LineDefinedByMovingEndPointsCrossesOrigin(
            new Point(-1, +3).Sweep(new Vector(0, -4)),
            new Point(-1, -1).Sweep(new Vector(4, 0))
        ).AssertEquals(new GeometryUtilities.IntersectionParameters { T = 0.5, S = 0.5 });
        GeometryUtilities.LineDefinedByMovingEndPointsCrossesOrigin(
            new Point(-1, +1).Sweep(new Vector(0, -2)),
            new Point(-1, -1).Sweep(new Vector(2, 0))
        ).AssertEquals(null);

        // anchored
        GeometryUtilities.LineDefinedByMovingEndPointsCrossesOrigin(
            new Point(-1, 0).Sweep(new Vector(0, 0)),
            new Point(+1, -1).To(new Point(+1, +1))
        ).AssertEquals(new GeometryUtilities.IntersectionParameters { T = 0.5, S = 0.5 });
        GeometryUtilities.LineDefinedByMovingEndPointsCrossesOrigin(
            new Point(-1, 0).Sweep(new Vector(0, 0)),
            new Point(+1, -1).To(new Point(+1, -0.5))
        ).AssertEquals(null);
    }
}
