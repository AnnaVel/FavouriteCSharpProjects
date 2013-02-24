using System;

namespace SomeGeometry
{
    public class TestSomeGeometry
    {
        public static void Main(string[] args)
        {
            double x1 = Double.Parse(Console.ReadLine());
            double y1 = Double.Parse(Console.ReadLine());
            double x2 = Double.Parse(Console.ReadLine());
            double y2 = Double.Parse(Console.ReadLine());
            double x3 = Double.Parse(Console.ReadLine());
            double y3 = Double.Parse(Console.ReadLine());
            double x4 = Double.Parse(Console.ReadLine());
            double y4 = Double.Parse(Console.ReadLine());
            double interceptX;
            double interceptY;
            MyGeometricMethods.DoTwoSectionsIntersect(x1, y1, x2, y2, x3, y3, x4, y4, out interceptX, out interceptY);

            Console.WriteLine(interceptX + " " + interceptY);
        }
    }

}
