using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OOPLab.Modules.GraphPlotting
{
    public class GraphPlotArea : Graphic
    {
        private readonly List<GraphPoint> points = new List<GraphPoint>();

        public void SetPoints(IReadOnlyList<GraphPoint> newPoints)
        {
            points.Clear();
            points.AddRange(newPoints);
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();
            DrawGrid(vertexHelper);

            if (points.Count == 0)
            {
                return;
            }

            List<Vector2> plottedPoints = GetPlottedPoints();

            for (int i = 0; i < plottedPoints.Count; i++)
            {
                AddCircle(vertexHelper, plottedPoints[i], 6f, new Color32(102, 255, 140, 255));
            }

            for (int i = 0; i < plottedPoints.Count - 1; i++)
            {
                AddLine(vertexHelper, plottedPoints[i], plottedPoints[i + 1], 4f, new Color32(96, 190, 255, 255));
            }
        }

        private void DrawGrid(VertexHelper vertexHelper)
        {
            Rect rect = rectTransform.rect;
            Color32 axisColor = new Color32(190, 210, 230, 255);
            Color32 gridColor = new Color32(45, 58, 72, 255);

            for (int i = 0; i <= 10; i++)
            {
                float x = Mathf.Lerp(rect.xMin, rect.xMax, i / 10f);
                float y = Mathf.Lerp(rect.yMin, rect.yMax, i / 10f);
                AddLine(vertexHelper, new Vector2(x, rect.yMin), new Vector2(x, rect.yMax), i == 0 ? 3f : 1.2f, i == 0 ? axisColor : gridColor);
                AddLine(vertexHelper, new Vector2(rect.xMin, y), new Vector2(rect.xMax, y), i == 0 ? 3f : 1.2f, i == 0 ? axisColor : gridColor);
            }
        }

        private List<Vector2> GetPlottedPoints()
        {
            Rect rect = rectTransform.rect;
            float minX = points[0].X;
            float maxX = points[0].X;
            float minY = points[0].Y;
            float maxY = points[0].Y;

            foreach (GraphPoint point in points)
            {
                minX = Mathf.Min(minX, point.X);
                maxX = Mathf.Max(maxX, point.X);
                minY = Mathf.Min(minY, point.Y);
                maxY = Mathf.Max(maxY, point.Y);
            }

            if (Mathf.Approximately(minX, maxX))
            {
                maxX = minX + 1;
            }

            if (Mathf.Approximately(minY, maxY))
            {
                maxY = minY + 1;
            }

            List<Vector2> plotted = new List<Vector2>();

            foreach (GraphPoint point in points)
            {
                float normalizedX = Mathf.InverseLerp(minX, maxX, point.X);
                float normalizedY = Mathf.InverseLerp(minY, maxY, point.Y);
                float x = Mathf.Lerp(rect.xMin + 28, rect.xMax - 18, normalizedX);
                float y = Mathf.Lerp(rect.yMin + 22, rect.yMax - 18, normalizedY);
                plotted.Add(new Vector2(x, y));
            }

            return plotted;
        }

        private void AddLine(VertexHelper vertexHelper, Vector2 start, Vector2 end, float thickness, Color32 lineColor)
        {
            Vector2 direction = (end - start).normalized;
            Vector2 normal = new Vector2(-direction.y, direction.x) * thickness * 0.5f;

            int index = vertexHelper.currentVertCount;
            vertexHelper.AddVert(start - normal, lineColor, Vector2.zero);
            vertexHelper.AddVert(start + normal, lineColor, Vector2.zero);
            vertexHelper.AddVert(end + normal, lineColor, Vector2.zero);
            vertexHelper.AddVert(end - normal, lineColor, Vector2.zero);
            vertexHelper.AddTriangle(index, index + 1, index + 2);
            vertexHelper.AddTriangle(index, index + 2, index + 3);
        }

        private void AddCircle(VertexHelper vertexHelper, Vector2 center, float radius, Color32 circleColor)
        {
            int segments = 18;
            int centerIndex = vertexHelper.currentVertCount;
            vertexHelper.AddVert(center, circleColor, Vector2.zero);

            for (int i = 0; i <= segments; i++)
            {
                float angle = i / (float)segments * Mathf.PI * 2f;
                Vector2 point = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                vertexHelper.AddVert(point, circleColor, Vector2.zero);

                if (i > 0)
                {
                    vertexHelper.AddTriangle(centerIndex, centerIndex + i, centerIndex + i + 1);
                }
            }
        }
    }
}
