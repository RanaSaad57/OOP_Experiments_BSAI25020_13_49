using OOPLab.Modules.GraphPlotting;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

public static class GraphPlottingModuleBuilder
{
    private const string PrefabPath = "Assets/_Project/Prefabs/Modules/GraphPlottingModule.prefab";

    [MenuItem("OOP Lab/04 Build Graph Plotting Module")]
    public static void Build()
    {
        EnsureFolder("Assets/_Project");
        EnsureFolder("Assets/_Project/Prefabs");
        EnsureFolder("Assets/_Project/Prefabs/Modules");

        Canvas canvas = Object.FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("ModuleTestCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
        }

        canvas.name = "ModuleTestCanvas";
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        DestroyIfExists("GraphPlottingModule");

        GameObject module = CreateUiObject("GraphPlottingModule", canvas.transform);
        SetRect(module.GetComponent<RectTransform>(), 0, 0, 980, 680);

        Image body = module.AddComponent<Image>();
        body.color = new Color(0.06f, 0.075f, 0.09f, 0.98f);

        GraphPlottingController controller = module.AddComponent<GraphPlottingController>();

        TMP_Text title = CreateText("TitleText", module.transform, "Graph Plotting System", 36, FontStyles.Bold, TextAlignmentOptions.Center);
        title.color = new Color(0.88f, 0.96f, 1f, 1f);
        SetRect(title.rectTransform, 0, 292, 900, 54);

        TMP_Text subtitle = CreateText("SubtitleText", module.transform, "Plot experiment data points and calculate slope", 18, FontStyles.Normal, TextAlignmentOptions.Center);
        subtitle.color = new Color(0.62f, 0.72f, 0.82f, 1f);
        SetRect(subtitle.rectTransform, 0, 254, 900, 30);

        GameObject graphBox = CreateUiObject("GraphBox", module.transform);
        SetRect(graphBox.GetComponent<RectTransform>(), -185, 20, 560, 420);
        Image graphBackground = graphBox.AddComponent<Image>();
        graphBackground.color = new Color(0.01f, 0.018f, 0.025f, 1f);

        GameObject plotLayer = CreateUiObject("PlotLayer", graphBox.transform);
        SetRect(plotLayer.GetComponent<RectTransform>(), 0, 0, 560, 420);
        GraphPlotRenderer plotRenderer = plotLayer.AddComponent<GraphPlotRenderer>();

        TMP_Text yAxis = CreateText("YAxisLabel", graphBox.transform, "Y", 20, FontStyles.Bold, TextAlignmentOptions.Center);
        yAxis.color = new Color(0.78f, 0.88f, 1f, 1f);
        SetRect(yAxis.rectTransform, -250, 186, 40, 30);

        TMP_Text xAxis = CreateText("XAxisLabel", graphBox.transform, "X", 20, FontStyles.Bold, TextAlignmentOptions.Center);
        xAxis.color = new Color(0.78f, 0.88f, 1f, 1f);
        SetRect(xAxis.rectTransform, 250, -186, 40, 30);

        TMP_Text inputHeader = CreateText("InputHeader", module.transform, "Add Data Point", 23, FontStyles.Bold, TextAlignmentOptions.Center);
        inputHeader.color = new Color(0.86f, 0.94f, 1f, 1f);
        SetRect(inputHeader.rectTransform, 292, 196, 300, 36);

        TMP_InputField xInput = CreateInput("XValueInput", module.transform, "X value", 292, 134, 280, 52);
        TMP_InputField yInput = CreateInput("YValueInput", module.transform, "Y value", 292, 72, 280, 52);

        Button addButton = CreateButton("AddPointButton", module.transform, "Add Point", 210, 6, new Color(0.04f, 0.45f, 0.68f, 1f), 130);
        Button sampleButton = CreateButton("SampleDataButton", module.transform, "Sample", 356, 6, new Color(0.16f, 0.30f, 0.46f, 1f), 130);
        Button clearButton = CreateButton("ClearGraphButton", module.transform, "Clear", 292, -58, new Color(0.58f, 0.16f, 0.19f, 1f), 180);

        UnityEventTools.AddPersistentListener(addButton.onClick, controller.AddPoint);
        UnityEventTools.AddPersistentListener(sampleButton.onClick, controller.AddSampleData);
        UnityEventTools.AddPersistentListener(clearButton.onClick, controller.ClearGraph);

        TMP_Text slopeText = CreateText("SlopeText", module.transform, "Slope: add at least 2 valid points", 20, FontStyles.Bold, TextAlignmentOptions.Center);
        slopeText.color = new Color(0.7f, 1f, 0.75f, 1f);
        SetRect(slopeText.rectTransform, 292, -112, 350, 36);

        TMP_Text scaleText = CreateText("ScaleText", module.transform, "Scale: add points to calculate X and Y axis scale.", 16, FontStyles.Bold, TextAlignmentOptions.Center);
        scaleText.color = new Color(0.82f, 0.9f, 1f, 1f);
        scaleText.textWrappingMode = TextWrappingModes.Normal;
        SetRect(scaleText.rectTransform, 292, -158, 360, 48);

        GameObject listBox = CreateUiObject("PointListBox", module.transform);
        SetRect(listBox.GetComponent<RectTransform>(), 292, -252, 350, 126);
        Image listBackground = listBox.AddComponent<Image>();
        listBackground.color = new Color(0.92f, 0.95f, 0.92f, 1f);

        TMP_Text pointsDisplay = CreateText("PointsDisplayText", listBox.transform, "No points added yet.", 18, FontStyles.Normal, TextAlignmentOptions.TopLeft);
        pointsDisplay.color = new Color(0.07f, 0.08f, 0.09f, 1f);
        pointsDisplay.textWrappingMode = TextWrappingModes.Normal;
        pointsDisplay.overflowMode = TextOverflowModes.Truncate;
        SetRect(pointsDisplay.rectTransform, 0, 0, 315, 98);

        TMP_Text statusText = CreateText("StatusText", module.transform, "Ready to plot points.", 19, FontStyles.Bold, TextAlignmentOptions.Center);
        statusText.color = new Color(0.72f, 1f, 0.76f, 1f);
        SetRect(statusText.rectTransform, 0, -306, 850, 34);

        SerializedObject serializedController = new SerializedObject(controller);
        serializedController.FindProperty("xValueInput").objectReferenceValue = xInput;
        serializedController.FindProperty("yValueInput").objectReferenceValue = yInput;
        serializedController.FindProperty("pointsDisplayText").objectReferenceValue = pointsDisplay;
        serializedController.FindProperty("slopeText").objectReferenceValue = slopeText;
        serializedController.FindProperty("scaleText").objectReferenceValue = scaleText;
        serializedController.FindProperty("statusText").objectReferenceValue = statusText;
        serializedController.FindProperty("graphPlotRenderer").objectReferenceValue = plotRenderer;
        serializedController.ApplyModifiedPropertiesWithoutUndo();

        PrefabUtility.SaveAsPrefabAsset(module, PrefabPath);
        Selection.activeObject = module;
        EditorUtility.SetDirty(module);
        Debug.Log($"Graph Plotting module built and saved as prefab: {PrefabPath}");
    }

    private static void DestroyIfExists(string objectName)
    {
        GameObject existing = GameObject.Find(objectName);
        if (existing != null)
        {
            Object.DestroyImmediate(existing);
        }
    }

    private static GameObject CreateUiObject(string name, Transform parent)
    {
        GameObject gameObject = new GameObject(name, typeof(RectTransform));
        gameObject.transform.SetParent(parent, false);
        return gameObject;
    }

    private static TMP_Text CreateText(string name, Transform parent, string text, float size, FontStyles style, TextAlignmentOptions alignment)
    {
        GameObject gameObject = CreateUiObject(name, parent);
        TMP_Text label = gameObject.AddComponent<TextMeshProUGUI>();
        label.text = text;
        label.fontSize = size;
        label.fontStyle = style;
        label.alignment = alignment;
        return label;
    }

    private static TMP_InputField CreateInput(string name, Transform parent, string placeholder, float x, float y, float width, float height)
    {
        GameObject inputObject = CreateUiObject(name, parent);
        SetRect(inputObject.GetComponent<RectTransform>(), x, y, width, height);

        Image background = inputObject.AddComponent<Image>();
        background.color = Color.white;

        TMP_InputField input = inputObject.AddComponent<TMP_InputField>();
        input.contentType = TMP_InputField.ContentType.DecimalNumber;

        TMP_Text text = CreateText("Text", inputObject.transform, string.Empty, 22, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
        text.color = new Color(0.08f, 0.09f, 0.1f, 1f);
        SetRect(text.rectTransform, 16, 0, width - 32, height - 12);

        TMP_Text placeholderText = CreateText("Placeholder", inputObject.transform, placeholder, 21, FontStyles.Italic, TextAlignmentOptions.MidlineLeft);
        placeholderText.color = new Color(0.42f, 0.45f, 0.5f, 1f);
        SetRect(placeholderText.rectTransform, 16, 0, width - 32, height - 12);

        input.textComponent = text;
        input.placeholder = placeholderText;
        return input;
    }

    private static Button CreateButton(string name, Transform parent, string label, float x, float y, Color color, float width)
    {
        GameObject buttonObject = CreateUiObject(name, parent);
        SetRect(buttonObject.GetComponent<RectTransform>(), x, y, width, 52);

        Image image = buttonObject.AddComponent<Image>();
        image.color = color;

        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;

        TMP_Text text = CreateText("Label", buttonObject.transform, label, 20, FontStyles.Bold, TextAlignmentOptions.Center);
        text.color = Color.white;
        SetRect(text.rectTransform, 0, 0, width - 16, 42);
        return button;
    }

    private static void SetRect(RectTransform rectTransform, float x, float y, float width, float height)
    {
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(x, y);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
        {
            return;
        }

        string parent = System.IO.Path.GetDirectoryName(path)?.Replace("\\", "/");
        string folder = System.IO.Path.GetFileName(path);

        if (!string.IsNullOrEmpty(parent))
        {
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, folder);
        }
    }
}
