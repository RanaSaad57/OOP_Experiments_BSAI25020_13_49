using OOPLab.Modules.MeasurementReading;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

public static class MeasurementReadingModuleBuilder
{
    private const string PrefabPath = "Assets/_Project/Prefabs/Modules/MeasurementReadingModule.prefab";

    [MenuItem("OOP Lab/03 Build Measurement Reading Module")]
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

        DestroyIfExists("MeasurementReadingModule");

        GameObject module = CreateUiObject("MeasurementReadingModule", canvas.transform);
        SetRect(module.GetComponent<RectTransform>(), 0, 0, 860, 650);

        Image body = module.AddComponent<Image>();
        body.color = new Color(0.055f, 0.065f, 0.08f, 0.98f);

        MeasurementReadingController controller = module.AddComponent<MeasurementReadingController>();

        TMP_Text title = CreateText("TitleText", module.transform, "Measurement Reading UI", 34, FontStyles.Bold, TextAlignmentOptions.Center);
        title.color = new Color(0.88f, 0.96f, 1f, 1f);
        SetRect(title.rectTransform, 0, 275, 760, 50);

        TMP_Text subtitle = CreateText("SubtitleText", module.transform, "Central display for lab instrument values", 18, FontStyles.Normal, TextAlignmentOptions.Center);
        subtitle.color = new Color(0.58f, 0.68f, 0.78f, 1f);
        SetRect(subtitle.rectTransform, 0, 239, 760, 28);

        Button thermometerButton = CreateButton("ThermometerButton", module.transform, "Thermometer", -320, 188, new Color(0.17f, 0.25f, 0.34f, 1f));
        Button voltmeterButton = CreateButton("VoltmeterButton", module.transform, "Voltmeter", -160, 188, new Color(0.17f, 0.25f, 0.34f, 1f));
        Button ammeterButton = CreateButton("AmmeterButton", module.transform, "Ammeter", 0, 188, new Color(0.17f, 0.25f, 0.34f, 1f));
        Button vernierButton = CreateButton("VernierButton", module.transform, "Vernier", 160, 188, new Color(0.17f, 0.25f, 0.34f, 1f));
        Button screwGaugeButton = CreateButton("ScrewGaugeButton", module.transform, "Screw Gauge", 320, 188, new Color(0.17f, 0.25f, 0.34f, 1f));

        UnityEventTools.AddPersistentListener(thermometerButton.onClick, controller.ShowThermometer);
        UnityEventTools.AddPersistentListener(voltmeterButton.onClick, controller.ShowVoltmeter);
        UnityEventTools.AddPersistentListener(ammeterButton.onClick, controller.ShowAmmeter);
        UnityEventTools.AddPersistentListener(vernierButton.onClick, controller.ShowVernierCaliper);
        UnityEventTools.AddPersistentListener(screwGaugeButton.onClick, controller.ShowScrewGauge);

        GameObject screen = CreateUiObject("DigitalScreen", module.transform);
        SetRect(screen.GetComponent<RectTransform>(), 0, 70, 650, 170);
        Image screenImage = screen.AddComponent<Image>();
        screenImage.color = new Color(0.015f, 0.025f, 0.018f, 1f);

        TMP_Text instrumentNameText = CreateText("InstrumentNameText", screen.transform, "Thermometer", 25, FontStyles.Bold, TextAlignmentOptions.Center);
        instrumentNameText.color = new Color(0.55f, 0.88f, 1f, 1f);
        SetRect(instrumentNameText.rectTransform, 0, 55, 560, 36);

        TMP_Text readingValueText = CreateText("ReadingValueText", screen.transform, "25.0", 60, FontStyles.Bold, TextAlignmentOptions.Center);
        readingValueText.color = new Color(0.65f, 1f, 0.7f, 1f);
        SetRect(readingValueText.rectTransform, -48, -8, 400, 82);

        TMP_Text unitText = CreateText("UnitText", screen.transform, "C", 34, FontStyles.Bold, TextAlignmentOptions.Left);
        unitText.color = new Color(0.65f, 1f, 0.7f, 1f);
        SetRect(unitText.rectTransform, 235, -6, 110, 50);

        TMP_Text precisionText = CreateText("PrecisionText", module.transform, "Precision: 1 decimal place(s)", 20, FontStyles.Bold, TextAlignmentOptions.Center);
        precisionText.color = new Color(0.82f, 0.9f, 1f, 1f);
        SetRect(precisionText.rectTransform, 0, -42, 650, 34);

        TMP_Text detailsText = CreateText("DetailsText", module.transform, "Used to measure temperature during heating, cooling, calorimetry, and reaction experiments.", 18, FontStyles.Normal, TextAlignmentOptions.Center);
        detailsText.color = new Color(0.72f, 0.82f, 0.9f, 1f);
        detailsText.textWrappingMode = TextWrappingModes.Normal;
        SetRect(detailsText.rectTransform, 0, -90, 690, 56);

        Button previousButton = CreateButton("PreviousInstrumentButton", module.transform, "Prev", -276, -158, new Color(0.17f, 0.25f, 0.34f, 1f));
        Button decreaseButton = CreateButton("DecreaseButton", module.transform, "-", -138, -158, new Color(0.13f, 0.32f, 0.52f, 1f));
        Button simulateButton = CreateButton("SimulateButton", module.transform, "Simulate", 0, -158, new Color(0.04f, 0.45f, 0.68f, 1f));
        Button increaseButton = CreateButton("IncreaseButton", module.transform, "+", 138, -158, new Color(0.13f, 0.32f, 0.52f, 1f));
        Button nextButton = CreateButton("NextInstrumentButton", module.transform, "Next", 276, -158, new Color(0.17f, 0.25f, 0.34f, 1f));
        Button resetButton = CreateButton("ResetButton", module.transform, "Reset", 0, -226, new Color(0.58f, 0.16f, 0.19f, 1f));

        UnityEventTools.AddPersistentListener(previousButton.onClick, controller.PreviousInstrument);
        UnityEventTools.AddPersistentListener(decreaseButton.onClick, controller.DecreaseReading);
        UnityEventTools.AddPersistentListener(simulateButton.onClick, controller.SimulateReading);
        UnityEventTools.AddPersistentListener(increaseButton.onClick, controller.IncreaseReading);
        UnityEventTools.AddPersistentListener(nextButton.onClick, controller.NextInstrument);
        UnityEventTools.AddPersistentListener(resetButton.onClick, controller.ResetReading);

        TMP_Text statusText = CreateText("StatusText", module.transform, "Ready.", 19, FontStyles.Bold, TextAlignmentOptions.Center);
        statusText.color = new Color(0.72f, 1f, 0.76f, 1f);
        SetRect(statusText.rectTransform, 0, -282, 620, 34);

        SerializedObject serializedController = new SerializedObject(controller);
        serializedController.FindProperty("instrumentNameText").objectReferenceValue = instrumentNameText;
        serializedController.FindProperty("readingValueText").objectReferenceValue = readingValueText;
        serializedController.FindProperty("unitText").objectReferenceValue = unitText;
        serializedController.FindProperty("precisionText").objectReferenceValue = precisionText;
        serializedController.FindProperty("detailsText").objectReferenceValue = detailsText;
        serializedController.FindProperty("statusText").objectReferenceValue = statusText;
        serializedController.ApplyModifiedPropertiesWithoutUndo();

        PrefabUtility.SaveAsPrefabAsset(module, PrefabPath);
        Selection.activeObject = module;
        EditorUtility.SetDirty(module);
        Debug.Log($"Measurement Reading module built and saved as prefab: {PrefabPath}");
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

    private static Button CreateButton(string name, Transform parent, string label, float x, float y, Color color)
    {
        GameObject buttonObject = CreateUiObject(name, parent);
        SetRect(buttonObject.GetComponent<RectTransform>(), x, y, 120, 52);

        Image image = buttonObject.AddComponent<Image>();
        image.color = color;

        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;

        TMP_Text text = CreateText("Label", buttonObject.transform, label, 21, FontStyles.Bold, TextAlignmentOptions.Center);
        text.color = Color.white;
        SetRect(text.rectTransform, 0, 0, 110, 42);
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
