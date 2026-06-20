using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OOPLab.Modules.GraphPlotting
{
    public class GraphPlotRenderer : MonoBehaviour
    {
        [SerializeField] private RectTransform plotArea;

        private readonly List<GameObject> drawnObjects = new List<GameObject>();

        private void Awake()
        {
            if (plotArea == null)
            {
                plotArea = GetComponent<RectTransform>();
            }
        }

        public void SetPoints(IReadOnlyList<GraphPoint> points)
        {
            ClearDrawing();
            DrawGrid();

            if (points == null || points.Count == 0)
            {
                return;
            }

            List<Vector2> plottedPoints = GetPlottedPoints(points);

            for (int i = 0; i < plottedPoints.Count - 1; i++)
            {
                DrawLine(plottedPoints[i], plottedPoints[i + 1], 5f, new Color(0.25f, 0.68f, 1f, 1f));
            }

            for (int i = 0; i < plottedPoints.Count; i++)
            {
                DrawPoint(plottedPoints[i], new Color(0.4f, 1f, 0.55f, 1f));
            }
        }

        private void DrawGrid()
        {
            Rect rect = plotArea.rect;
            Color smallGridColor = new Color(0.08f, 0.13f, 0.17f, 1f);
            Color bigGridColor = new Color(0.18f, 0.27f, 0.36f, 1f);
            Color axisColor = new Color(0.75f, 0.85f, 1f, 1f);

            for (int i = 0; i <= 50; i++)
            {
                float x = Mathf.Lerp(rect.xMin, rect.xMax, i / 50f);
                float y = Mathf.Lerp(rect.yMin, rect.yMax, i / 50f);
                bool isAxis = i == 0;
                bool isBigBox = i % 5 == 0;
                float thickness = isAxis ? 4f : isBigBox ? 2f : 0.8f;
                Color color = isAxis ? axisColor : isBigBox ? bigGridColor : smallGridColor;

                DrawLine(new Vector2(x, rect.yMin), new Vector2(x, rect.yMax), thickness, color);
                DrawLine(new Vector2(rect.xMin, y), new Vector2(rect.xMax, y), thickness, color);
            }
        }

        private List<Vector2> GetPlottedPoints(IReadOnlyList<GraphPoint> points)
        {
            Rect rect = plotArea.rect;
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

            if (minX > 0)
            {
                minX = 0;
            }

            if (minY > 0)
            {
                minY = 0;
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
                float y = Mathf.Lerp(rect.yMin + 24, rect.yMax - 18, normalizedY);
                plotted.Add(new Vector2(x, y));
            }

            return plotted;
        }

        private void DrawPoint(Vector2 position, Color color)
        {
            GameObject pointObject = CreateImageObject("Point", color);
            RectTransform rectTransform = pointObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;
            rectTransform.sizeDelta = new Vector2(14, 14);
        }

        private void DrawLine(Vector2 start, Vector2 end, float thickness, Color color)
        {
            GameObject lineObject = CreateImageObject("Line", color);
            RectTransform rectTransform = lineObject.GetComponent<RectTransform>();
            Vector2 direction = end - start;
            rectTransform.anchoredPosition = start + direction * 0.5f;
            rectTransform.sizeDelta = new Vector2(direction.magnitude, thickness);
            rectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }

        private GameObject CreateImageObject(string objectName, Color color)
        {
            GameObject imageObject = new GameObject(objectName, typeof(RectTransform), typeof(RawImage));
            imageObject.transform.SetParent(plotArea, false);

            RawImage image = imageObject.GetComponent<RawImage>();
            image.color = color;
            image.texture = Texture2D.whiteTexture;
            image.raycastTarget = false;

            drawnObjects.Add(imageObject);
            return imageObject;
        }

        private void ClearDrawing()
        {
            for (int i = drawnObjects.Count - 1; i >= 0; i--)
            {
                if (drawnObjects[i] != null)
                {
                    Destroy(drawnObjects[i]);
                }
            }

            drawnObjects.Clear();
        }
    }
}
