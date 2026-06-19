using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OOPLab.Experiments
{
    public class Experiment4GraphGraphic : MaskableGraphic
    {
        private readonly List<Vector2> points = new List<Vector2>();
        private Text[] xLabels;
        private Text[] yLabels;
        private Text scaleDetailsText;
        private string xScaleName = "t^2";
        private string xScaleUnit = "s^2";
        private string yScaleName = "2s";
        private string yScaleUnit = "m";
        private const int MajorDivisions = 5;
        private const int MinorDivisionsPerMajor = 5;
        private float displayMaxX = 5f;
        private float displayMaxY = 10f;

        public void ConfigureLabels(
            Text[] horizontalLabels,
            Text[] verticalLabels,
            Text scaleText,
            string horizontalName = "t^2",
            string horizontalUnit = "s^2",
            string verticalName = "2s",
            string verticalUnit = "m")
        {
            xLabels = horizontalLabels;
            yLabels = verticalLabels;
            scaleDetailsText = scaleText;
            xScaleName = horizontalName;
            xScaleUnit = horizontalUnit;
            yScaleName = verticalName;
            yScaleUnit = verticalUnit;
            UpdateScaleAndLabels();
        }

        public void SetPoints(IReadOnlyList<Vector2> source)
        {
            points.Clear();
            for (int i = 0; i < source.Count; i++)
            {
                points.Add(source[i]);
            }

            UpdateScaleAndLabels();
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            Rect rect = GetPixelAdjustedRect();
            float left = rect.xMin + 42f;
            float right = rect.xMax - 18f;
            float bottom = rect.yMin + 34f;
            float top = rect.yMax - 18f;
            Color minorGridColor = new Color(0.20f, 0.28f, 0.32f, 0.55f);
            Color majorGridColor = new Color(0.34f, 0.48f, 0.54f, 0.90f);
            int totalDivisions = MajorDivisions * MinorDivisionsPerMajor;

            for (int i = 0; i <= totalDivisions; i++)
            {
                float fraction = i / (float)totalDivisions;
                float x = Mathf.Lerp(left, right, fraction);
                float y = Mathf.Lerp(bottom, top, fraction);
                bool isMajor = i % MinorDivisionsPerMajor == 0;
                float width = isMajor ? 2f : 0.7f;
                Color gridColor = isMajor ? majorGridColor : minorGridColor;
                AddLine(vh, new Vector2(x, bottom), new Vector2(x, top), width, gridColor);
                AddLine(vh, new Vector2(left, y), new Vector2(right, y), width, gridColor);
            }

            AddLine(vh, new Vector2(left, bottom), new Vector2(right, bottom), 3f, color);
            AddLine(vh, new Vector2(left, bottom), new Vector2(left, top), 3f, color);

            if (points.Count == 0)
            {
                return;
            }

            Vector2 previous = Vector2.zero;
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 plotted = new Vector2(
                    Mathf.Lerp(left, right, points[i].x / displayMaxX),
                    Mathf.Lerp(bottom, top, points[i].y / displayMaxY));

                if (i > 0)
                {
                    AddLine(vh, previous, plotted, 4f, new Color(0.10f, 0.65f, 0.96f));
                }

                AddSquare(vh, plotted, 8f, new Color(0.47f, 1f, 0.55f));
                previous = plotted;
            }
        }

        private void UpdateScaleAndLabels()
        {
            float highestX = 1f;
            float highestY = 1f;
            for (int i = 0; i < points.Count; i++)
            {
                highestX = Mathf.Max(highestX, points[i].x);
                highestY = Mathf.Max(highestY, points[i].y);
            }

            displayMaxX = NiceAxisMaximum(highestX * 1.12f);
            displayMaxY = NiceAxisMaximum(highestY * 1.12f);
            float xLargeBox = displayMaxX / MajorDivisions;
            float yLargeBox = displayMaxY / MajorDivisions;
            float xSmallBox = xLargeBox / MinorDivisionsPerMajor;
            float ySmallBox = yLargeBox / MinorDivisionsPerMajor;

            UpdateLabels(xLabels, displayMaxX);
            UpdateLabels(yLabels, displayMaxY);

            if (scaleDetailsText != null)
            {
                scaleDetailsText.text =
                    $"X axis ({xScaleName}): 1 large box = {xLargeBox:0.###} {xScaleUnit}, 1 small box = {xSmallBox:0.###} {xScaleUnit}\n" +
                    $"Y axis ({yScaleName}): 1 large box = {yLargeBox:0.###} {yScaleUnit}, 1 small box = {ySmallBox:0.###} {yScaleUnit}";
            }
        }

        private static void UpdateLabels(Text[] labels, float maximum)
        {
            if (labels == null)
            {
                return;
            }

            for (int i = 0; i < labels.Length; i++)
            {
                if (labels[i] != null)
                {
                    labels[i].text = (maximum * i / MajorDivisions).ToString("0.###");
                }
            }
        }

        private static float NiceAxisMaximum(float value)
        {
            value = Mathf.Max(value, 0.001f);
            float exponent = Mathf.Pow(10f, Mathf.Floor(Mathf.Log10(value)));
            float normalized = value / exponent;
            float niceNormalized;

            if (normalized <= 1f)
            {
                niceNormalized = 1f;
            }
            else if (normalized <= 2f)
            {
                niceNormalized = 2f;
            }
            else if (normalized <= 5f)
            {
                niceNormalized = 5f;
            }
            else
            {
                niceNormalized = 10f;
            }

            return niceNormalized * exponent;
        }

        private static void AddLine(VertexHelper vh, Vector2 a, Vector2 b, float width, Color lineColor)
        {
            Vector2 direction = (b - a).normalized;
            Vector2 normal = new Vector2(-direction.y, direction.x) * width * 0.5f;
            int start = vh.currentVertCount;
            vh.AddVert(a - normal, lineColor, Vector2.zero);
            vh.AddVert(a + normal, lineColor, Vector2.zero);
            vh.AddVert(b + normal, lineColor, Vector2.zero);
            vh.AddVert(b - normal, lineColor, Vector2.zero);
            vh.AddTriangle(start, start + 1, start + 2);
            vh.AddTriangle(start, start + 2, start + 3);
        }

        private static void AddSquare(VertexHelper vh, Vector2 center, float size, Color squareColor)
        {
            float half = size * 0.5f;
            int start = vh.currentVertCount;
            vh.AddVert(center + new Vector2(-half, -half), squareColor, Vector2.zero);
            vh.AddVert(center + new Vector2(-half, half), squareColor, Vector2.zero);
            vh.AddVert(center + new Vector2(half, half), squareColor, Vector2.zero);
            vh.AddVert(center + new Vector2(half, -half), squareColor, Vector2.zero);
            vh.AddTriangle(start, start + 1, start + 2);
            vh.AddTriangle(start, start + 2, start + 3);
        }
    }
}
