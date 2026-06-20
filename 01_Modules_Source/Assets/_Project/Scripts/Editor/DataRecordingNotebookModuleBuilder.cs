using OOPLab.Modules.DataRecordingNotebook;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

public static class DataRecordingNotebookModuleBuilder
{
    private const string PrefabPath = "Assets/_Project/Prefabs/Modules/DataRecordingNotebookModule.prefab";

    [MenuItem("OOP Lab/02 Build Data Recording Notebook Module")]
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

        GameObject oldModule = GameObject.Find("DataRecordingNotebookModule");
        if (oldModule != null)
        {
            Object.DestroyImmediate(oldModule);
        }

        GameObject calculatorModule = GameObject.Find("ScientificCalculatorModule");
        if (calculatorModule != null)
        {
            Object.DestroyImmediate(calculatorModule);
        }

        GameObject module = CreateUiObject("DataRecordingNotebookModule", canvas.transform);
        RectTransform moduleRect = module.GetComponent<RectTransform>();
        SetRect(moduleRect, 0, 0, 820, 760);

        Image panel = module.AddComponent<Image>();
        panel.color = new Color(0.1f, 0.12f, 0.14f, 0.98f);

        DataRecordingNotebookController controller = module.AddComponent<DataRecordingNotebookController>();

        TMP_Text title = CreateText("TitleText", module.transform, "Data Recording Notebook", 34, FontStyles.Bold, TextAlignmentOptions.Center);
        title.color = new Color(0.9f, 0.96f, 1f, 1f);
        SetRect(title.rectTransform, 0, 330, 720, 54);

        TMP_Text subtitle = CreateText("SubtitleText", module.transform, "Record experiment observations during lab work", 18, FontStyles.Normal, TextAlignmentOptions.Center);
        subtitle.color = new Color(0.63f, 0.72f, 0.82f, 1f);
        SetRect(subtitle.rectTransform, 0, 292, 720, 32);

        TMP_Text titleLabel = CreateText("ExperimentTitleLabel", module.transform, "Experiment Title", 18, FontStyles.Bold, TextAlignmentOptions.Left);
        titleLabel.color = new Color(0.77f, 0.86f, 0.94f, 1f);
        SetRect(titleLabel.rectTransform, 0, 242, 650, 28);

        TMP_InputField experimentTitleInput = CreateInput("ExperimentTitleInput", module.transform, "Example: Hooke's Law", 0, 202, 650, 58, false);

        TMP_Text observationLabel = CreateText("ObservationLabel", module.transform, "Observation", 18, FontStyles.Bold, TextAlignmentOptions.Left);
        observationLabel.color = new Color(0.77f, 0.86f, 0.94f, 1f);
        SetRect(observationLabel.rectTransform, 0, 145, 650, 28);

        TMP_InputField observationInput = CreateInput("ObservationInput", module.transform, "Write the reading or observation here", 0, 76, 650, 118, true);

        Button addButton = CreateButton("AddRecordButton", module.transform, "Add Record", -118, -22, new Color(0.05f, 0.42f, 0.68f, 1f));
        Button clearButton = CreateButton("ClearRecordsButton", module.transform, "Clear", 118, -22, new Color(0.58f, 0.16f, 0.19f, 1f));

        UnityEventTools.AddPersistentListener(addButton.onClick, controller.AddRecord);
        UnityEventTools.AddPersistentListener(clearButton.onClick, controller.ClearRecords);

        TMP_Text statusText = CreateText("StatusText", module.transform, "Ready to record observations.", 19, FontStyles.Bold, TextAlignmentOptions.Center);
        statusText.color = new Color(0.72f, 1f, 0.76f, 1f);
        SetRect(statusText.rectTransform, 0, -82, 650, 34);

        GameObject recordsBox = CreateUiObject("RecordsBox", module.transform);
        SetRect(recordsBox.GetComponent<RectTransform>(), 0, -235, 650, 250);
        Image recordsBackground = recordsBox.AddComponent<Image>();
        recordsBackground.color = new Color(0.93f, 0.95f, 0.91f, 1f);

        TMP_Text recordsDisplayText = CreateText("RecordsDisplayText", recordsBox.transform, "No records yet.", 21, FontStyles.Normal, TextAlignmentOptions.TopLeft);
        recordsDisplayText.color = new Color(0.08f, 0.09f, 0.1f, 1f);
        recordsDisplayText.textWrappingMode = TextWrappingModes.Normal;
        recordsDisplayText.overflowMode = TextOverflowModes.Ellipsis;
        SetRect(recordsDisplayText.rectTransform, 0, 0, 610, 218);

        Button previousButton = CreateButton("PreviousPageButton", module.transform, "Previous", -205, -370, new Color(0.18f, 0.25f, 0.34f, 1f));
        Button nextButton = CreateButton("NextPageButton", module.transform, "Next", 205, -370, new Color(0.18f, 0.25f, 0.34f, 1f));

        UnityEventTools.AddPersistentListener(previousButton.onClick, controller.PreviousPage);
        UnityEventTools.AddPersistentListener(nextButton.onClick, controller.NextPage);

        TMP_Text pageText = CreateText("PageText", module.transform, "Page 1 / 1", 18, FontStyles.Bold, TextAlignmentOptions.Center);
        pageText.color = new Color(0.9f, 0.96f, 1f, 1f);
        SetRect(pageText.rectTransform, 0, -370, 220, 42);

        SerializedObject serializedController = new SerializedObject(controller);
        serializedController.FindProperty("experimentTitleInput").objectReferenceValue = experimentTitleInput;
        serializedController.FindProperty("observationInput").objectReferenceValue = observationInput;
        serializedController.FindProperty("recordsDisplayText").objectReferenceValue = recordsDisplayText;
        serializedController.FindProperty("statusText").objectReferenceValue = statusText;
        serializedController.FindProperty("pageText").objectReferenceValue = pageText;
        serializedController.ApplyModifiedPropertiesWithoutUndo();

        PrefabUtility.SaveAsPrefabAsset(module, PrefabPath);
        Selection.activeObject = module;
        EditorUtility.SetDirty(module);
        Debug.Log($"Data Recording Notebook module built and saved as prefab: {PrefabPath}");
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

    private static TMP_InputField CreateInput(string name, Transform parent, string placeholder, float x, float y, float width, float height, bool multiline)
    {
        GameObject inputObject = CreateUiObject(name, parent);
        SetRect(inputObject.GetComponent<RectTransform>(), x, y, width, height);

        Image background = inputObject.AddComponent<Image>();
        background.color = Color.white;

        TMP_InputField input = inputObject.AddComponent<TMP_InputField>();
        input.lineType = multiline ? TMP_InputField.LineType.MultiLineNewline : TMP_InputField.LineType.SingleLine;

        TMP_Text text = CreateText("Text", inputObject.transform, string.Empty, 22, FontStyles.Normal, multiline ? TextAlignmentOptions.TopLeft : TextAlignmentOptions.MidlineLeft);
        text.color = new Color(0.08f, 0.09f, 0.1f, 1f);
        text.textWrappingMode = multiline ? TextWrappingModes.Normal : TextWrappingModes.NoWrap;
        text.overflowMode = TextOverflowModes.ScrollRect;
        SetRect(text.rectTransform, 18, 0, width - 36, height - 16);

        TMP_Text placeholderText = CreateText("Placeholder", inputObject.transform, placeholder, 21, FontStyles.Italic, multiline ? TextAlignmentOptions.TopLeft : TextAlignmentOptions.MidlineLeft);
        placeholderText.color = new Color(0.42f, 0.45f, 0.5f, 1f);
        SetRect(placeholderText.rectTransform, 18, 0, width - 36, height - 16);

        input.textComponent = text;
        input.placeholder = placeholderText;
        input.textViewport = inputObject.GetComponent<RectTransform>();
        input.scrollSensitivity = 1f;
        return input;
    }

    private static Button CreateButton(string name, Transform parent, string label, float x, float y, Color color)
    {
        GameObject buttonObject = CreateUiObject(name, parent);
        SetRect(buttonObject.GetComponent<RectTransform>(), x, y, 200, 54);

        Image image = buttonObject.AddComponent<Image>();
        image.color = color;

        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;

        TMP_Text text = CreateText("Label", buttonObject.transform, label, 22, FontStyles.Bold, TextAlignmentOptions.Center);
        text.color = Color.white;
        SetRect(text.rectTransform, 0, 0, 180, 44);
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
