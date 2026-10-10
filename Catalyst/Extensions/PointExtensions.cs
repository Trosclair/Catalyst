using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Catalyst.Extensions
{
    public static class PointExtensions
    {
        public static Point XYDeltaBetweenPoints(this Point a, Point b)
        {
            return new Point(b.X - a.X, b.Y - a.Y);
        }

        /// <summary>
        /// Calculates the Euclidean distance between two points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns>The Euclidean distance between the two points.</returns>
        public static double DistanceToPoint(this Point a, Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculates the shortest distance from a point to a line segment defined by two endpoints.
        /// </summary>
        /// <param name="p">The point for which to calculate the distance.</param>
        /// <param name="a">The first endpoint of the line segment.</param>
        /// <param name="b">The second endpoint of the line segment.</param>
        /// <returns>The shortest distance from the point to the line segment.</returns>
        public static double DistanceToSegment(this Point p, Point a, Point b)
        {
            var abx = b.X - a.X;
            var aby = b.Y - a.Y;
            var lenSq = abx * abx + aby * aby;
            var t = lenSq < 1e-12 ? 0 : ((p.X - a.X) * abx + (p.Y - a.Y) * aby) / lenSq;
            t = Math.Clamp(t, 0, 1);
            var dx = p.X - (a.X + abx * t);
            var dy = p.Y - (a.Y + aby * t);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculates a point on a cubic Bezier curve defined by the start point, two control points, and the end point, based on the given progress ratio (t).
        /// </summary>
        /// <param name="startPointOfCurve">The start point of the curve.</param>
        /// <param name="firstControlPoint">The first control point (pulls the curve toward it as it leaves the start point).</param>
        /// <param name="secondControlPoint">The second control point (pulls the curve toward it as it approaches the end point).</param>
        /// <param name="endPointOfCurve">The end point of the curve.</param>
        /// <param name="progressRatio">The progress ratio (t) along the curve.</param>
        /// <returns>The point on the curve at the specified progress ratio.</returns>
        public static Point CubicAt(this Point startPointOfCurve, Point firstControlPoint, Point secondControlPoint, Point endPointOfCurve, double progressRatio)
        {
            var mt = 1D - progressRatio;
            return new Point(
                mt * mt * mt * startPointOfCurve.X + 3 * mt * mt * progressRatio * firstControlPoint.X + 3 * mt * progressRatio * progressRatio * secondControlPoint.X + progressRatio * progressRatio * progressRatio * endPointOfCurve.X,
                mt * mt * mt * startPointOfCurve.Y + 3 * mt * mt * progressRatio * firstControlPoint.Y + 3 * mt * progressRatio * progressRatio * secondControlPoint.Y + progressRatio * progressRatio * progressRatio * endPointOfCurve.Y);
        }
    }
}
