#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class ModulesShowcaseBuilder
{
    public const string ScenePath = "Assets/Scenes/ModulesShowcase.unity";

    [MenuItem("OOP Lab/Build Modules Showcase")]
    public static void Build()
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        Camera camera = CreateCamera();
        CreateLight();
        CreateEventSystem();

        Canvas canvas = CreateCanvas();
        GameObject controllerObject = new GameObject("ModulesShowcaseController");
        ModulesShowcaseController controller =
            controllerObject.AddComponent<ModulesShowcaseController>();

        Image topBar = CreatePanel(canvas.transform, "TopBar",
            new Color32(13, 27, 33, 255));
        SetRect(topBar.rectTransform, new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, 0), new Vector2(0, 94));

        Text heading = CreateText(topBar.transform, "Heading", "Scientific Calculator",
            30, FontStyle.Bold, TextAnchor.MiddleLeft, new Color32(225, 239, 244, 255));
        SetRect(heading.rectTransform, new Vector2(0, 0), new Vector2(1, 1),
            new Vector2(270, 0), new Vector2(-30, 0));

        Image sidebar = CreatePanel(canvas.transform, "Sidebar",
            new Color32(18, 30, 36, 255));
        SetRect(sidebar.rectTransform, new Vector2(0, 0), new Vector2(0, 1),
            new Vector2(0, 0), new Vector2(245, -94));

        Image helpBar = CreatePanel(canvas.transform, "HelpBar",
            new Color32(20, 37, 43, 245));
        SetRect(helpBar.rectTransform, new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(245, 0), new Vector2(0, 58));

        Text help = CreateText(helpBar.transform, "HelpText",
            "Choose a module from the left.", 17, FontStyle.Normal,
            TextAnchor.MiddleCenter, new Color32(174, 211, 221, 255));
        Stretch(help.rectTransform, 20);

        Transform uiArea = CreateRectObject(canvas.transform, "ModuleUIArea").transform;
        SetRect((RectTransform)uiArea, new Vector2(0, 0), new Vector2(1, 1),
            new Vector2(245, 58), new Vector2(0, -94));

        GameObject calculator = InstantiateUiPrefab(
            "Assets/_Project/Prefabs/Modules/ScientificCalculatorModule.prefab", uiArea);
        GameObject notebook = InstantiateUiPrefab(
            "Assets/_Project/Prefabs/Modules/DataRecordingNotebookModule.prefab", uiArea);
        GameObject measurement = InstantiateUiPrefab(
            "Assets/_Project/Prefabs/Modules/MeasurementReadingModule.prefab", uiArea);
        GameObject graph = InstantiateUiPrefab(
            "Assets/_Project/Prefabs/Modules/GraphPlottingModule.prefab", uiArea);

        GameObject weighingBalance = InstantiateWorldPrefab(
            "Assets/_Project/Prefabs/Modules/WeighingBalanceModule.prefab");

        SerializedObject serialized = new SerializedObject(controller);
        serialized.FindProperty("calculator").objectReferenceValue = calculator;
        serialized.FindProperty("notebook").objectReferenceValue = notebook;
        serialized.FindProperty("measurement").objectReferenceValue = measurement;
        serialized.FindProperty("graph").objectReferenceValue = graph;
        serialized.FindProperty("weighingBalance").objectReferenceValue = weighingBalance;
        serialized.FindProperty("heading").objectReferenceValue = heading;
        serialized.FindProperty("helpText").objectReferenceValue = help;
        serialized.ApplyModifiedPropertiesWithoutUndo();

        CreateMenuButton(sidebar.transform, "Calculator", 0,
            new Color32(24, 132, 188, 255), controller.ShowCalculator);
        CreateMenuButton(sidebar.transform, "Notebook", 1,
            new Color32(39, 137, 97, 255), controller.ShowNotebook);
        CreateMenuButton(sidebar.transform, "Measurement", 2,
            new Color32(48, 92, 145, 255), controller.ShowMeasurement);
        CreateMenuButton(sidebar.transform, "Graph", 3,
            new Color32(32, 142, 112, 255), controller.ShowGraph);
        CreateMenuButton(sidebar.transform, "Weighing Balance", 4,
            new Color32(136, 77, 156, 255), controller.ShowWeighingBalance);

        calculator.SetActive(true);
        notebook.SetActive(false);
        measurement.SetActive(false);
        graph.SetActive(false);
        weighingBalance.SetActive(false);

        EditorSceneManager.SaveScene(scene, ScenePath);

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(ScenePath, true)
        };

        AssetDatabase.SaveAssets();
        Debug.Log("Modules showcase built: " + ScenePath);
        Selection.activeGameObject = controllerObject;
    }

    private static Camera CreateCamera()
    {
        GameObject obj = new GameObject("Main Camera");
        Camera camera = obj.AddComponent<Camera>();
        obj.tag = "MainCamera";
        obj.AddComponent<AudioListener>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color32(35, 47, 53, 255);
        camera.fieldOfView = 48;
        obj.transform.position = new Vector3(0, 3.4f, -8.5f);
        obj.transform.rotation = Quaternion.Euler(20, 0, 0);
        return camera;
    }

    private static void CreateLight()
    {
        GameObject obj = new GameObject("Directional Light");
        Light light = obj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        obj.transform.rotation = Quaternion.Euler(45, -30, 0);
    }

    private static void CreateEventSystem()
    {
        GameObject obj = new GameObject("EventSystem");
        obj.AddComponent<EventSystem>();
        obj.AddComponent<StandaloneInputModule>();
    }

    private static Canvas CreateCanvas()
    {
        GameObject obj = new GameObject("ShowcaseCanvas");
        Canvas canvas = obj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        obj.AddComponent<GraphicRaycaster>();
        CanvasScaler scaler = obj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        return canvas;
    }

    private static GameObject InstantiateUiPrefab(string path, Transform parent)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
        RectTransform rect = instance.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;
        }
        return instance;
    }

    private static GameObject InstantiateWorldPrefab(string path)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.transform.position = new Vector3(0.8f, 0, 0.5f);
        return instance;
    }

    private static void CreateMenuButton(Transform parent, string label, int index,
        Color color, UnityEngine.Events.UnityAction action)
    {
        Button button = CreateButton(parent, label, color);
        RectTransform rect = button.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0.5f, 1);
        rect.offsetMin = new Vector2(18, -104 - index * 78);
        rect.offsetMax = new Vector2(-18, -44 - index * 78);
        UnityEventTools.AddPersistentListener(button.onClick, action);
    }

    private static Button CreateButton(Transform parent, string label, Color color)
    {
        GameObject obj = new GameObject(label + "Button", typeof(RectTransform),
            typeof(CanvasRenderer), typeof(Image), typeof(Button));
        obj.transform.SetParent(parent, false);
        Image image = obj.GetComponent<Image>();
        image.color = color;
        Button button = obj.GetComponent<Button>();
        button.targetGraphic = image;

        Text text = CreateText(obj.transform, "Label", label, 18, FontStyle.Bold,
            TextAnchor.MiddleCenter, Color.white);
        Stretch(text.rectTransform, 8);
        return button;
    }

    private static Image CreatePanel(Transform parent, string name, Color color)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform),
            typeof(CanvasRenderer), typeof(Image));
        obj.transform.SetParent(parent, false);
        Image image = obj.GetComponent<Image>();
        image.color = color;
        return image;
    }

    private static Text CreateText(Transform parent, string name, string value,
        int size, FontStyle style, TextAnchor alignment, Color color)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform),
            typeof(CanvasRenderer), typeof(Text));
        obj.transform.SetParent(parent, false);
        Text text = obj.GetComponent<Text>();
        text.text = value;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = size;
        text.fontStyle = style;
        text.alignment = alignment;
        text.color = color;
        return text;
    }

    private static GameObject CreateRectObject(Transform parent, string name)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform));
        obj.transform.SetParent(parent, false);
        return obj;
    }

    private static void Stretch(RectTransform rect, float inset)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(inset, inset);
        rect.offsetMax = new Vector2(-inset, -inset);
    }

    private static void SetRect(RectTransform rect, Vector2 anchorMin,
        Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
    {
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;
    }
}
#endif
