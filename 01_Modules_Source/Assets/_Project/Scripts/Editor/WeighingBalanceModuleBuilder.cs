using OOPLab.Modules.WeighingBalance;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

public static class WeighingBalanceModuleBuilder
{
    private const string PrefabPath = "Assets/_Project/Prefabs/Modules/WeighingBalanceModule.prefab";

    [MenuItem("OOP Lab/05 Build Weighing Balance Module")]
    public static void Build()
    {
        EnsureFolder("Assets/_Project");
        EnsureFolder("Assets/_Project/Prefabs");
        EnsureFolder("Assets/_Project/Prefabs/Modules");

        GameObject module = GameObject.Find("WeighingBalanceModule");
        if (module == null)
        {
            Debug.LogError("No WeighingBalanceModule found in the scene. Rename the cleaned scale parent to WeighingBalanceModule first.");
            return;
        }

        GameObject oldPanel = module.transform.Find("BalanceUIPanel")?.gameObject;
        if (oldPanel != null)
        {
            Object.DestroyImmediate(oldPanel);
        }

        WeighingBalanceController controller = module.GetComponent<WeighingBalanceController>();
        if (controller == null)
        {
            controller = module.AddComponent<WeighingBalanceController>();
        }

        GameObject panel = new GameObject("BalanceUIPanel", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        panel.transform.SetParent(module.transform, false);
        panel.transform.localPosition = new Vector3(0.85f, 0.55f, -0.55f);
        panel.transform.localRotation = Quaternion.Euler(18f, 0, 0);
        panel.transform.localScale = Vector3.one * 0.0018f;

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(520, 360);

        Canvas canvas = panel.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        CanvasScaler scaler = panel.GetComponent<CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 12;

        Image body = panel.AddComponent<Image>();
        body.color = new Color(0.055f, 0.065f, 0.08f, 0.96f);

        Button closeButton = CreateButton("CloseButton", panel.transform, "X", 220, 150, new Color(0.58f, 0.16f, 0.19f, 1f));
        UnityEventTools.AddPersistentListener(closeButton.onClick, controller.HidePanel);

        TMP_Text title = CreateText("TitleText", panel.transform, "Weighing Balance", 30, FontStyles.Bold, TextAlignmentOptions.Center);
        title.color = new Color(0.88f, 0.96f, 1f, 1f);
        SetRect(title.rectTransform, 0, 148, 480, 42);

        TMP_Text sampleText = CreateText("SampleText", panel.transform, "Sample: No sample", 18, FontStyles.Bold, TextAlignmentOptions.Center);
        sampleText.color = new Color(0.78f, 0.88f, 1f, 1f);
        SetRect(sampleText.rectTransform, 0, 108, 460, 32);

        TMP_Text gramsText = CreateText("GramsText", panel.transform, "0.00 g", 46, FontStyles.Bold, TextAlignmentOptions.Center);
        gramsText.color = new Color(0.65f, 1f, 0.7f, 1f);
        SetRect(gramsText.rectTransform, 0, 54, 420, 62);

        TMP_Text kilogramsText = CreateText("KilogramsText", panel.transform, "0.000 kg", 22, FontStyles.Bold, TextAlignmentOptions.Center);
        kilogramsText.color = new Color(0.65f, 1f, 0.7f, 1f);
        SetRect(kilogramsText.rectTransform, 0, 10, 420, 34);

        TMP_Text helpText = CreateText("HelpText", panel.transform, "Mass is read from the object placed on the balance.", 17, FontStyles.Bold, TextAlignmentOptions.Center);
        helpText.color = new Color(0.78f, 0.88f, 1f, 1f);
        SetRect(helpText.rectTransform, 0, -44, 440, 34);

        Button simulateButton = CreateButton("SimulateButton", panel.transform, "Place Sample", -150, -100, new Color(0.04f, 0.45f, 0.68f, 1f));
        Button tareButton = CreateButton("TareButton", panel.transform, "Tare", 0, -100, new Color(0.16f, 0.34f, 0.28f, 1f));
        Button resetButton = CreateButton("ResetButton", panel.transform, "Reset", 150, -100, new Color(0.58f, 0.16f, 0.19f, 1f));

        UnityEventTools.AddPersistentListener(simulateButton.onClick, controller.SimulateSample);
        UnityEventTools.AddPersistentListener(tareButton.onClick, controller.Tare);
        UnityEventTools.AddPersistentListener(resetButton.onClick, controller.ResetBalance);

        TMP_Text statusText = CreateText("StatusText", panel.transform, "Ready.", 16, FontStyles.Bold, TextAlignmentOptions.Center);
        statusText.color = new Color(0.72f, 1f, 0.76f, 1f);
        SetRect(statusText.rectTransform, 0, -150, 460, 28);

        SerializedObject serializedController = new SerializedObject(controller);
        serializedController.FindProperty("balanceUIPanel").objectReferenceValue = panel;
        serializedController.FindProperty("sampleNameInput").objectReferenceValue = null;
        serializedController.FindProperty("massInput").objectReferenceValue = null;
        serializedController.FindProperty("sampleText").objectReferenceValue = sampleText;
        serializedController.FindProperty("gramsText").objectReferenceValue = gramsText;
        serializedController.FindProperty("kilogramsText").objectReferenceValue = kilogramsText;
        serializedController.FindProperty("statusText").objectReferenceValue = statusText;
        serializedController.ApplyModifiedPropertiesWithoutUndo();

        PrefabUtility.SaveAsPrefabAsset(module, PrefabPath);
        Selection.activeObject = module;
        EditorUtility.SetDirty(module);
        Debug.Log($"Weighing Balance module built and saved as prefab: {PrefabPath}");
    }

    private static TMP_Text CreateText(string name, Transform parent, string text, float size, FontStyles style, TextAlignmentOptions alignment)
    {
        GameObject gameObject = new GameObject(name, typeof(RectTransform));
        gameObject.transform.SetParent(parent, false);
        TMP_Text label = gameObject.AddComponent<TextMeshProUGUI>();
        label.text = text;
        label.fontSize = size;
        label.fontStyle = style;
        label.alignment = alignment;
        return label;
    }

    private static TMP_InputField CreateInput(string name, Transform parent, string placeholder, float x, float y, float width, float height, TMP_InputField.ContentType contentType)
    {
        GameObject inputObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(TMP_InputField));
        inputObject.transform.SetParent(parent, false);
        SetRect(inputObject.GetComponent<RectTransform>(), x, y, width, height);

        Image background = inputObject.GetComponent<Image>();
        background.color = Color.white;

        TMP_InputField input = inputObject.GetComponent<TMP_InputField>();
        input.contentType = contentType;

        TMP_Text text = CreateText("Text", inputObject.transform, string.Empty, 18, FontStyles.Normal, TextAlignmentOptions.MidlineLeft);
        text.color = new Color(0.08f, 0.09f, 0.1f, 1f);
        SetRect(text.rectTransform, 12, 0, width - 24, height - 8);

        TMP_Text placeholderText = CreateText("Placeholder", inputObject.transform, placeholder, 17, FontStyles.Italic, TextAlignmentOptions.MidlineLeft);
        placeholderText.color = new Color(0.42f, 0.45f, 0.5f, 1f);
        SetRect(placeholderText.rectTransform, 12, 0, width - 24, height - 8);

        input.textComponent = text;
        input.placeholder = placeholderText;
        return input;
    }

    private static Button CreateButton(string name, Transform parent, string label, float x, float y, Color color)
    {
        GameObject buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);
        SetRect(buttonObject.GetComponent<RectTransform>(), x, y, 132, 38);

        Image image = buttonObject.GetComponent<Image>();
        image.color = color;

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = image;

        TMP_Text text = CreateText("Label", buttonObject.transform, label, 18, FontStyles.Bold, TextAlignmentOptions.Center);
        text.color = Color.white;
        SetRect(text.rectTransform, 0, 0, 120, 30);
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
