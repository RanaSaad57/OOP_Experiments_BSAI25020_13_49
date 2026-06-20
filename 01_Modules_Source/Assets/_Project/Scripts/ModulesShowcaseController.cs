using UnityEngine;
using UnityEngine.UI;

public class ModulesShowcaseController : MonoBehaviour
{
    [SerializeField] private GameObject calculator;
    [SerializeField] private GameObject notebook;
    [SerializeField] private GameObject measurement;
    [SerializeField] private GameObject graph;
    [SerializeField] private GameObject weighingBalance;
    [SerializeField] private Text heading;
    [SerializeField] private Text helpText;

    private GameObject[] modules;

    private void Awake()
    {
        modules = new[]
        {
            calculator,
            notebook,
            measurement,
            graph,
            weighingBalance
        };

        ShowCalculator();
    }

    public void ShowCalculator()
    {
        Show(0, "Scientific Calculator",
            "Enter two values and choose an arithmetic or scientific operation.");
    }

    public void ShowNotebook()
    {
        Show(1, "Data Recording Notebook",
            "Enter an experiment title and observation, then add records and navigate pages.");
    }

    public void ShowMeasurement()
    {
        Show(2, "Measurement Reading UI",
            "Browse instrument types, change precision, and use Simulate to demonstrate readings.");
    }

    public void ShowGraph()
    {
        Show(3, "Graph Plotting System",
            "Enter X and Y values or load sample data to draw a graph and calculate its slope.");
    }

    public void ShowWeighingBalance()
    {
        Show(4, "Weighing Balance",
            "Click the 3D balance to open its controls. Use simulate, tare, reset, and close.");
    }

    private void Show(int index, string title, string help)
    {
        for (int i = 0; i < modules.Length; i++)
        {
            if (modules[i] != null)
            {
                modules[i].SetActive(i == index);
            }
        }

        if (heading != null)
        {
            heading.text = title;
        }

        if (helpText != null)
        {
            helpText.text = help;
        }
    }
}
