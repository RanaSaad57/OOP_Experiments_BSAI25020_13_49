using OOPLab.Modules.ScientificCalculator;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ScientificCalculatorModuleBuilder
{
    private const string PrefabPath = "Assets/_Project/Prefabs/Modules/ScientificCalculatorModule.prefab";

    [MenuItem("OOP Lab/01 Build Scientific Calculator Module")]
    public static void Build()
    {
        EnsureFolder("Assets/_Project");
        EnsureFolder("Assets/_Project/Prefabs");
        EnsureFolder("Assets/_Project/Prefabs/Modules");

        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
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

        GameObject oldModule = GameObject.Find("ScientificCalculatorModule");
        if (oldModule != null)
        {
            Object.DestroyImmediate(oldModule);
        }

        GameObject module = CreateUiObject("ScientificCalculatorModule", canvas.transform);
        RectTransform moduleRect = module.GetComponent<RectTransform>();
        moduleRect.anchorMin = new Vector2(0.5f, 0.5f);
        moduleRect.anchorMax = new Vector2(0.5f, 0.5f);
        moduleRect.pivot = new Vector2(0.5f, 0.5f);
        moduleRect.anchoredPosition = Vector2.zero;
        moduleRect.sizeDelta = new Vector2(620, 720);

        Image panel = module.AddComponent<Image>();
        panel.color = new Color(0.06f, 0.075f, 0.095f, 0.98f);

        ScientificCalculatorController controller = module.AddComponent<ScientificCalculatorController>();

        TMP_Text title = CreateText("Title", module.transform, "Scientific Calculator", 36, FontStyles.Bold, TextAlignmentOptions.Center);
        title.color = new Color(0.83f, 0.95f, 1f, 1f);
        SetRect(title.rectTransform, 0, 310, 560, 60);

        TMP_Text firstLabel = CreateText("FirstNumberLabel", module.transform, "First Number", 18, FontStyles.Bold, TextAlignmentOptions.Left);
        firstLabel.color = new Color(0.68f, 0.79f, 0.9f, 1f);
        SetRect(firstLabel.rectTransform, 0, 252, 500, 26);

        TMP_InputField firstInput = CreateInput("FirstNumberInput", module.transform, "Enter first value", 0, 215);

        TMP_Text secondLabel = CreateText("SecondNumberLabel", module.transform, "Second Number", 18, FontStyles.Bold, TextAlignmentOptions.Left);
        secondLabel.color = new Color(0.68f, 0.79f, 0.9f, 1f);
        SetRect(secondLabel.rectTransform, 0, 157, 500, 26);

        TMP_InputField secondInput = CreateInput("SecondNumberInput", module.transform, "Enter second value", 0, 120);

        TMP_Text resultText = CreateText("ResultText", module.transform, "Result: 0", 28, FontStyles.Bold, TextAlignmentOptions.Center);
        resultText.color = new Color(0.78f, 1f, 0.78f, 1f);
        SetRect(resultText.rectTransform, 0, 42, 560, 58);

        string[] labels =
        {
            "+", "-", "x", "/",
            "Power", "Square", "Sqrt", "Log",
            "Sin", "Cos", "Tan", "Clear"
        };

        UnityAction[] actions =
        {
            controller.Add,
            controller.Subtract,
            controller.Multiply,
            controller.Divide,
            controller.Power,
            controller.Square,
            controller.SquareRoot,
            controller.Log10,
            controller.Sin,
            controller.Cos,
            controller.Tan,
            controller.Clear
        };

        float startX = -210;
        float startY = -45;
        int index = 0;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                Button button = CreateButton(labels[index], module.transform, startX + col * 140, startY - row * 85);
                UnityEventTools.AddPersistentListener(button.onClick, actions[index]);
                index++;
            }
        }

        SerializedObject serializedController = new SerializedObject(controller);
        serializedController.FindProperty("firstNumberInput").objectReferenceValue = firstInput;
        serializedController.FindProperty("secondNumberInput").objectReferenceValue = secondInput;
        serializedController.FindProperty("resultText").objectReferenceValue = resultText;
        serializedController.ApplyModifiedPropertiesWithoutUndo();

        PrefabUtility.SaveAsPrefabAsset(module, PrefabPath);
        Selection.activeObject = module;
        EditorUtility.SetDirty(module);
        Debug.Log($"Scientific Calculator module built and saved as prefab: {PrefabPath}");
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
        label.color = Color.white;
        return label;
    }

    private static TMP_InputField CreateInput(string name, Transform parent, string placeholder, float x, float y)
    {
        GameObject inputObject = CreateUiObject(name, parent);
        SetRect(inputObject.GetComponent<RectTransform>(), x, y, 500, 58);

        Image background = inputObject.AddComponent<Image>();
        background.color = new Color(0.94f, 0.96f, 0.98f, 1f);

        TMP_InputField input = inputObject.AddComponent<TMP_InputField>();

        TMP_Text text = CreateText("Text", inputObject.transform, string.Empty, 24, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
        text.color = new Color(0.06f, 0.07f, 0.09f, 1f);
        SetRect(text.rectTransform, 18, 0, 460, 48);

        TMP_Text placeholderText = CreateText("Placeholder", inputObject.transform, placeholder, 22, FontStyles.Italic, TextAlignmentOptions.MidlineLeft);
        placeholderText.color = new Color(0.35f, 0.38f, 0.43f, 1f);
        SetRect(placeholderText.rectTransform, 18, 0, 460, 48);

        input.textComponent = text;
        input.placeholder = placeholderText;
        input.contentType = TMP_InputField.ContentType.DecimalNumber;
        return input;
    }

    private static Button CreateButton(string label, Transform parent, float x, float y)
    {
        GameObject buttonObject = CreateUiObject(label, parent);
        SetRect(buttonObject.GetComponent<RectTransform>(), x, y, 120, 58);

        Image image = buttonObject.AddComponent<Image>();
        if (label == "Clear")
        {
            image.color = new Color(0.72f, 0.15f, 0.18f, 1f);
        }
        else if (label == "+" || label == "-" || label == "x" || label == "/")
        {
            image.color = new Color(0.11f, 0.42f, 0.72f, 1f);
        }
        else
        {
            image.color = new Color(0.16f, 0.24f, 0.34f, 1f);
        }

        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;

        TMP_Text text = CreateText("Label", buttonObject.transform, label, 22, FontStyles.Bold, TextAlignmentOptions.Center);
        text.color = Color.white;
        SetRect(text.rectTransform, 0, 0, 110, 48);
        return button;
    }

    private static void SetRect(RectTransform rectTransform, float x, float y, float width, float height)
    {
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(x, y);
        rectTransform.sizeDelta = new Vector2(width, height);
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
