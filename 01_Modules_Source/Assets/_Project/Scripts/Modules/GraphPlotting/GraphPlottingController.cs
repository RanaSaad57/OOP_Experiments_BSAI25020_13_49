using TMPro;
using UnityEngine;

namespace OOPLab.Modules.GraphPlotting
{
    public class GraphPlottingController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField xValueInput;
        [SerializeField] private TMP_InputField yValueInput;
        [SerializeField] private TMP_Text pointsDisplayText;
        [SerializeField] private TMP_Text slopeText;
        [SerializeField] private TMP_Text scaleText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private GraphPlotRenderer graphPlotRenderer;

        private readonly GraphData graphData = new GraphData();

        private void Start()
        {
            if (graphPlotRenderer == null)
            {
                graphPlotRenderer = GetComponentInChildren<GraphPlotRenderer>(true);
            }

            RefreshDisplay();
            ShowStatus("Ready to plot points.");
        }

        public void AddPoint()
        {
            if (!float.TryParse(xValueInput.text, out float x))
            {
                ShowStatus("Enter a valid X value.");
                return;
            }

            if (!float.TryParse(yValueInput.text, out float y))
            {
                ShowStatus("Enter a valid Y value.");
                return;
            }

            graphData.AddPoint(x, y);
            xValueInput.text = string.Empty;
            yValueInput.text = string.Empty;
            RefreshDisplay();
            ShowStatus($"Point added. Total points: {graphData.Count}");
        }

        public void AddSampleData()
        {
            graphData.Clear();
            graphData.AddPoint(1, 2);
            graphData.AddPoint(2, 4);
            graphData.AddPoint(3, 6);
            graphData.AddPoint(4, 8);
            RefreshDisplay();
            ShowStatus("Sample graph data added.");
        }

        public void ClearGraph()
        {
            graphData.Clear();
            RefreshDisplay();
            ShowStatus("Graph cleared.");
        }

        private void RefreshDisplay()
        {
            if (graphPlotRenderer == null)
            {
                graphPlotRenderer = GetComponentInChildren<GraphPlotRenderer>(true);
            }

            if (pointsDisplayText != null)
            {
                pointsDisplayText.text = graphData.GetPointListText();
            }

            if (slopeText != null)
            {
                slopeText.text = graphData.TryGetSlope(out float slope) ? $"Slope: {slope:0.###}" : "Slope: add at least 2 valid points";
            }

            if (scaleText != null)
            {
                scaleText.text = graphData.GetScaleText();
            }

            if (graphPlotRenderer != null)
            {
                graphPlotRenderer.SetPoints(graphData.Points);
            }
            else
            {
                ShowStatus("Graph renderer is missing.");
            }
        }

        private void ShowStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }
    }
}
