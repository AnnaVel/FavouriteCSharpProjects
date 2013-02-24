using System;


namespace SomeGeometry
{
    public class MyGeometricMethods
    {
        public static double CalculateShapeArea(double AX, double AY, double BX, double BY, double CX, double CY, double DX, double DY)
        {
            double result = CalculateTriangleArea(AX, AY, BX, BY, CX, CY);
            double dummyIntersect1;
            double dummyIntersect2;
            if (DoTwoSectionsIntersect(AX, AY, DX, DY, BX, BY, CX, CY, out dummyIntersect1, out dummyIntersect2))
            {
                result += CalculateTriangleArea(BX, BY, CX, CY, DX, DY);
            }
            else if (DoTwoSectionsIntersect(BX, BY, DX, DY, AX, AY, CX, CY, out dummyIntersect1, out dummyIntersect2))
            {
                result += CalculateTriangleArea(AX, AY, CX, CY, DX, DY);
            }
            else if (DoTwoSectionsIntersect(CX, CY, DX, DY, BX, BY, AX, AY, out dummyIntersect1, out dummyIntersect2))
            {
                result += CalculateTriangleArea(BX, BY, AX, AY, DX, DY);
            }
            return result;
        }

        public static double CalculateTriangleArea(double AX, double AY, double BX, double BY, double CX, double CY)
        {
            double sideA = FindLengthBetweenPoints(BX, BY, CX, CY);
            double sideB = FindLengthBetweenPoints(AX, AY, CX, CY);
            double sideC = FindLengthBetweenPoints(BX, BY, AX, AY);
            return CalculateTriangleAreaThroughThreeSides(sideA, sideB, sideC);
        }

        public static double CalculateTriangleAreaThroughThreeSides(double a, double b, double c)
        {
            double semiPeriphery = (a + b + c) / 2;
            double result = System.Math.Sqrt(semiPeriphery * (semiPeriphery - a) * (semiPeriphery - b) * (semiPeriphery - c));
            return result;
        }

        public static double FindLengthBetweenPoints(double x1, double y1, double x2, double y2)
        {
            double result = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            return result;
        }

        /// <summary>
        /// Determines whether two line sections intersect.
        /// </summary>
        /// <param name="x1">X coordinate of the first point of the first section</param>
        /// <param name="y1">Y coordinate of the first point of the first section</param>
        /// <param name="x2">X coordinate of the second point of the first section</param>
        /// <param name="y2">Y coordinate of the second point of the first section</param>
        /// <param name="x3">X coordinate of the first point of the second section</param>
        /// <param name="y3">Y coordinate of the first point of the second section</param>
        /// <param name="x4">X coordinate of the second point of the second section</param>
        /// <param name="y4">Y coordinate of the second point of the second section</param>
        /// <param name="interceptionX">If the sections intersect, this will return the X coordinate of the point where this happens. Otherwise it returns infinity.</param>
        /// <param name="interceptionY">If the sections intersect, this will return the Y coordinate of the point where this happens. Otherwise it returns infinity.</param>
        /// <returns>Returns true if the sections intersect, returns false otherwise. Note: if they are on the same line and have more than one common points, it will also return false!</returns>
        public static bool DoTwoSectionsIntersect(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, out double interceptionX, out double interceptionY)
        {
            double slope1;
            double intercept1;
            double slope2;
            double intercept2;

            if (x1 == x2 && x3 == x4) // the lines are vertical and the slope is Infinity
            {
                interceptionX = double.PositiveInfinity;
                interceptionY = double.PositiveInfinity;
                return false;
            }

            else if (x1 == x2) //one of the lines is vertical
            {
                slope1 = double.PositiveInfinity;
                intercept1 = 0;
                slope2 = (y3 - y4) / (x3 - x4);
                intercept2 = y3 - x3 * slope2;

                interceptionX = x1;
                interceptionY = slope2 * x1 + intercept2;
            }
            else if (x3 == x4) // one of the lines is vertical
            {
                slope1 = (y1 - y2) / (x1 - x2);
                intercept1 = y1 - x1 * slope1;
                slope2 = double.PositiveInfinity;
                intercept2 = 0;

                interceptionX = x3;
                interceptionY = slope1 * x3 + intercept1;
            }
            else
            {
                slope1 = (y1 - y2) / (x1 - x2);
                intercept1 = y1 - x1 * slope1;
                slope2 = (y3 - y4) / (x3 - x4);
                intercept2 = y3 - x3 * slope2;

                if (slope1 == slope2) //the lines are paralel
                {
                    interceptionX = double.PositiveInfinity;
                    interceptionY = double.PositiveInfinity;
                    return false;
                }
                else
                {
                    interceptionX = (intercept2 - intercept1) / (slope1 - slope2);
                    interceptionY = slope1 * interceptionX + intercept1;
                }
            }

            double smallerX1 = Math.Min(x1, x2);
            double biggerX1 = Math.Max(x1, x2);
            double smallerY1 = Math.Min(y1, y2);
            double biggerY1 = Math.Max(y1, y2);
            double smallerX2 = Math.Min(x3, x4);
            double biggerX2 = Math.Max(x3, x4);
            double smallerY2 = Math.Min(y3, y4);
            double biggerY2 = Math.Max(y3, y4);

            if (!(interceptionX >= smallerX1 && interceptionX <= biggerX1) ||
                !(interceptionX >= smallerX2 && interceptionX <= biggerX2) ||
                !(interceptionY >= smallerY1 && interceptionY <= biggerY1) ||
                !(interceptionY >= smallerY2 && interceptionY <= biggerY2))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
