using System.Collections.Generic;
using System.Text;

namespace OOPLab.Modules.GraphPlotting
{
    public class GraphData
    {
        private readonly List<GraphPoint> points = new List<GraphPoint>();

        public IReadOnlyList<GraphPoint> Points => points;
        public int Count => points.Count;

        public void AddPoint(float x, float y)
        {
            points.Add(new GraphPoint(x, y));
        }

        public void Clear()
        {
            points.Clear();
        }

        public bool TryGetSlope(out float slope)
        {
            slope = 0;

            if (points.Count < 2)
            {
                return false;
            }

            GraphPoint first = points[0];
            GraphPoint last = points[points.Count - 1];
            float deltaX = last.X - first.X;

            if (deltaX == 0)
            {
                return false;
            }

            slope = (last.Y - first.Y) / deltaX;
            return true;
        }

        public string GetPointListText()
        {
            if (points.Count == 0)
            {
                return "No points added yet.";
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < points.Count; i++)
            {
                builder.AppendLine($"{i + 1}. ({points[i].X:0.##}, {points[i].Y:0.##})");
            }

            return builder.ToString();
        }

        public string GetScaleText()
        {
            if (points.Count == 0)
            {
                return "Scale: add points to calculate X and Y axis scale.";
            }

            GetAxisBounds(out float minX, out float maxX, out float minY, out float maxY);

            float xBigBox = (maxX - minX) / 10f;
            float yBigBox = (maxY - minY) / 10f;
            float xSmallBox = xBigBox / 5f;
            float ySmallBox = yBigBox / 5f;

            return $"X: {minX:0.##} to {maxX:0.##} | 1 big = {xBigBox:0.##}, 1 small = {xSmallBox:0.##}\n" +
                   $"Y: {minY:0.##} to {maxY:0.##} | 1 big = {yBigBox:0.##}, 1 small = {ySmallBox:0.##}";
        }

        private void GetAxisBounds(out float minX, out float maxX, out float minY, out float maxY)
        {
            minX = points[0].X;
            maxX = points[0].X;
            minY = points[0].Y;
            maxY = points[0].Y;

            foreach (GraphPoint point in points)
            {
                minX = System.Math.Min(minX, point.X);
                maxX = System.Math.Max(maxX, point.X);
                minY = System.Math.Min(minY, point.Y);
                maxY = System.Math.Max(maxY, point.Y);
            }

            if (minX > 0)
            {
                minX = 0;
            }

            if (minY > 0)
            {
                minY = 0;
            }

            if (System.Math.Abs(maxX - minX) < 0.001f)
            {
                maxX = minX + 1;
            }

            if (System.Math.Abs(maxY - minY) < 0.001f)
            {
                maxY = minY + 1;
            }
        }
    }
}
