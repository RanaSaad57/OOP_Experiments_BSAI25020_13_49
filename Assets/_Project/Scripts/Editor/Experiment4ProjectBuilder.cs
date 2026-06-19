using System.IO;
using OOPLab.Experiments;
using OOPLab.Modules.InclinedPlaneSetup;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace OOPLab.EditorTools
{
    public static class Experiment4ProjectBuilder
    {
        private const string ScenePath = "Assets/Scenes/ExperimentLab.unity";
        private static readonly Color Ink = new Color(0.055f, 0.075f, 0.085f, 0.98f);
        private static readonly Color PanelColor = new Color(0.075f, 0.095f, 0.105f, 0.98f);
        private static readonly Color Blue = new Color(0.04f, 0.48f, 0.72f);
        private static readonly Color Green = new Color(0.12f, 0.48f, 0.34f);
        private static readonly Color Red = new Color(0.65f, 0.15f, 0.20f);
        private static readonly Color TextColor = new Color(0.88f, 0.93f, 0.95f);
        private static Font font;

        [MenuItem("OOP Lab/Build Final Project - Experiment 4")]
        public static void Build()
        {
            if (!EditorUtility.DisplayDialog(
                    "Build Experiment 4",
                    "This rebuilds the current scene as the Experiment 4 experience. Continue?",
                    "Build",
                    "Cancel"))
            {
                return;
            }

            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf")
                   ?? Resources.GetBuiltinResource<Font>("Arial.ttf");

            ClearScene();
            CreateCameraAndLight();
            CreateEnvironment();
            InclinedPlaneSetupModuleBuilder.BuildModule();

            GameObject module = GameObject.Find("InclinedPlaneSetupModule");
            InclinedPlaneController inclinedPlane = module.GetComponent<InclinedPlaneController>();
            GameObject workspaceRoot = new GameObject("Experiment4Workspace");
            module.transform.SetParent(workspaceRoot.transform, true);

            Canvas canvas = CreateCanvas();
            UiRefs ui = CreateExperienceUi(canvas.transform);
            SetInitialVisibility(ui, workspaceRoot);

            GameObject system = new GameObject("Experiment4ExperienceSystem");
            Experiment4ExperienceController controller = system.AddComponent<Experiment4ExperienceController>();
            SerializedObject serialized = new SerializedObject(controller);
            Set(serialized, "workspaceRoot", workspaceRoot);
            Set(serialized, "inclinedPlane", inclinedPlane);
            Set(serialized, "mainMenu", ui.MainMenu);
            Set(serialized, "experimentIntro", ui.Intro);
            Set(serialized, "modeSelection", ui.Mode);
            Set(serialized, "instructionBar", ui.InstructionBar);
            Set(serialized, "sideToolbar", ui.SideToolbar);
            Set(serialized, "globalMessage", ui.GlobalMessage);
            Set(serialized, "instructionText", ui.InstructionText);
            Set(serialized, "experiment4Button", ui.Experiment4Button);
            Set(serialized, "experiment5Button", ui.Experiment5Button);
            Set(serialized, "introBackButton", ui.IntroBack);
            Set(serialized, "introStartButton", ui.IntroStart);
            Set(serialized, "modeBackButton", ui.ModeBack);
            Set(serialized, "guidedButton", ui.Guided);
            Set(serialized, "independentButton", ui.Independent);
            Set(serialized, "instructionToggleButton", ui.InstructionToggle);
            Set(serialized, "instructionToggleLabel", ui.InstructionToggleLabel);
            Set(serialized, "homeButton", ui.Home);
            Set(serialized, "notebookPanel", ui.NotebookPanel);
            Set(serialized, "graphPanel", ui.GraphPanel);
            Set(serialized, "calculatorPanel", ui.CalculatorPanel);
            Set(serialized, "quickInfoPanel", ui.QuickInfoPanel);
            Set(serialized, "notebookButton", ui.NotebookButton);
            Set(serialized, "graphButton", ui.GraphButton);
            Set(serialized, "calculatorButton", ui.CalculatorButton);
            Set(serialized, "quickInfoButton", ui.QuickInfoButton);
            Set(serialized, "notebookCloseButton", ui.NotebookClose);
            Set(serialized, "graphCloseButton", ui.GraphClose);
            Set(serialized, "calculatorCloseButton", ui.CalculatorClose);
            Set(serialized, "quickInfoCloseButton", ui.QuickInfoClose);
            Set(serialized, "notebookText", ui.NotebookText);
            Set(serialized, "trialCountText", ui.TrialCount);
            Set(serialized, "clearRecordsButton", ui.ClearRecords);
            Set(serialized, "graphGraphic", ui.GraphGraphic);
            Set(serialized, "graphSummaryText", ui.GraphSummary);
            Set(serialized, "graphXInput", ui.GraphXInput);
            Set(serialized, "graphYInput", ui.GraphYInput);
            Set(serialized, "graphAddPointButton", ui.GraphAddPoint);
            Set(serialized, "graphClearButton", ui.GraphClear);
            Set(serialized, "distanceInput", ui.DistanceInput);
            Set(serialized, "timeInput", ui.TimeInput);
            Set(serialized, "calculatorResult", ui.CalculatorResult);
            Set(serialized, "calculateButton", ui.Calculate);
            Set(serialized, "useLatestTrialButton", ui.UseLatest);
            Set(serialized, "manualFirstInput", ui.ManualFirstInput);
            Set(serialized, "manualSecondInput", ui.ManualSecondInput);
            Set(serialized, "manualCalculatorResult", ui.ManualCalculatorResult);
            Set(serialized, "addButton", ui.Add);
            Set(serialized, "subtractButton", ui.Subtract);
            Set(serialized, "multiplyButton", ui.Multiply);
            Set(serialized, "divideButton", ui.Divide);
            Set(serialized, "powerButton", ui.Power);
            Set(serialized, "squareButton", ui.Square);
            Set(serialized, "squareRootButton", ui.SquareRoot);
            Set(serialized, "sinButton", ui.Sin);
            Set(serialized, "cosButton", ui.Cos);
            Set(serialized, "tanButton", ui.Tan);
            Set(serialized, "logButton", ui.Log);
            Set(serialized, "calculatorClearButton", ui.CalculatorClear);
            Experiment5ExperienceController experiment5 =
                Experiment5AddonBuilder.Build(canvas, ui.MainMenu, ui.GlobalMessage, controller);
            Set(serialized, "experiment5Experience", experiment5);
            serialized.ApplyModifiedProperties();

            Directory.CreateDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            Selection.activeGameObject = system;
            Debug.Log("Experiment 4 built. Press Play and choose Experiment 4.");
        }

        private static void ClearScene()
        {
            GameObject[] roots = EditorSceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                Object.DestroyImmediate(roots[i]);
            }
        }

        private static void CreateCameraAndLight()
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            Camera camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            camera.transform.position = new Vector3(1.6f, 3.3f, -6.2f);
            camera.transform.rotation = Quaternion.Euler(29f, -11f, 0f);
            camera.fieldOfView = 50f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.13f, 0.17f, 0.18f);

            GameObject lightObject = new GameObject("Directional Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.25f;
            light.transform.rotation = Quaternion.Euler(48f, -32f, 0f);

            GameObject fillObject = new GameObject("Fill Light");
            Light fill = fillObject.AddComponent<Light>();
            fill.type = LightType.Point;
            fill.range = 14f;
            fill.intensity = 2.1f;
            fill.color = new Color(0.48f, 0.72f, 0.90f);
            fill.transform.position = new Vector3(-3f, 5f, -3f);
        }

        private static void CreateEnvironment()
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "LaboratoryFloor";
            floor.transform.position = new Vector3(0f, -0.25f, 0f);
            floor.transform.localScale = new Vector3(14f, 0.25f, 10f);
            floor.GetComponent<Renderer>().sharedMaterial = CreateMaterial(
                "ExperimentFloor",
                new Color(0.16f, 0.19f, 0.20f));

            GameObject rear = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rear.name = "LaboratoryBackdrop";
            rear.transform.position = new Vector3(0f, 3.1f, 3.6f);
            rear.transform.localScale = new Vector3(14f, 6.5f, 0.18f);
            rear.GetComponent<Renderer>().sharedMaterial = CreateMaterial(
                "ExperimentBackdrop",
                new Color(0.10f, 0.15f, 0.17f));
        }

        private static Canvas CreateCanvas()
        {
            GameObject root = new GameObject("ExperimentScreenCanvas");
            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 20;
            CanvasScaler scaler = root.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            root.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static UiRefs CreateExperienceUi(Transform canvas)
        {
            UiRefs ui = new UiRefs();
            ui.GlobalMessage = Text(canvas, "GlobalMessage", "", new Vector2(0f, -505f), new Vector2(1300f, 34f), 18, TextAnchor.MiddleCenter);
            ui.GlobalMessage.color = new Color(0.62f, 0.95f, 0.72f);

            ui.MainMenu = FullScreen(canvas, "MainMenu", new Color(0.025f, 0.04f, 0.045f, 0.98f));
            Text(ui.MainMenu.transform, "Title", "INTERACTIVE PHYSICS LAB", new Vector2(0f, 250f), new Vector2(1000f, 80f), 52, TextAnchor.MiddleCenter).fontStyle = FontStyle.Bold;
            Text(ui.MainMenu.transform, "Subtitle", "Select an experiment", new Vector2(0f, 180f), new Vector2(700f, 40f), 22, TextAnchor.MiddleCenter).color = new Color(0.55f, 0.72f, 0.80f);
            ui.Experiment4Button = Button(ui.MainMenu.transform, "Experiment4", "EXPERIMENT 4\nAcceleration on an Inclined Plane", new Vector2(-260f, -15f), new Vector2(430f, 190f), Blue, 25);
            ui.Experiment5Button = Button(ui.MainMenu.transform, "Experiment5", "EXPERIMENT 5\nFree-Fall Method", new Vector2(260f, -15f), new Vector2(430f, 190f), new Color(0.22f, 0.30f, 0.35f), 25);
            Text(ui.MainMenu.transform, "Footer", "Experiments 4 and 5 | Physics Practical Laboratory", new Vector2(0f, -275f), new Vector2(900f, 32f), 18, TextAnchor.MiddleCenter).color = new Color(0.48f, 0.58f, 0.62f);

            ui.Intro = FullScreen(canvas, "Experiment4Introduction", new Color(0.025f, 0.04f, 0.045f, 0.98f));
            GameObject introCard = Card(ui.Intro.transform, "IntroCard", Vector2.zero, new Vector2(1180f, 760f));
            Heading(introCard.transform, "Experiment 4: Acceleration of a Rolling Ball", 305f);
            Text(introCard.transform, "Objective", "OBJECTIVE\nFind the acceleration of a steel ball rolling down an inclined angle iron by plotting a graph between 2s and t^2.", new Vector2(-300f, 180f), new Vector2(500f, 140f), 21, TextAnchor.UpperLeft);
            Text(introCard.transform, "Apparatus", "APPARATUS\nInclined plane, steel ball, distance scale, stopwatch, notebook, calculator and graph plotting system.", new Vector2(300f, 180f), new Vector2(500f, 140f), 21, TextAnchor.UpperLeft);
            Text(introCard.transform, "Theory", "THEORY\nFor motion from rest: s = 1/2 at^2. Therefore 2s = at^2. On a graph of 2s against t^2, the slope gives acceleration.", new Vector2(-300f, -5f), new Vector2(500f, 150f), 21, TextAnchor.UpperLeft);
            Text(introCard.transform, "Steps", "PROCEDURE\n1. Set the plane angle and friction.\n2. Release the ball and record distance and time.\n3. Repeat for multiple trials.\n4. Plot 2s against t^2 and calculate the slope.", new Vector2(300f, -20f), new Vector2(500f, 190f), 21, TextAnchor.UpperLeft);
            ui.IntroBack = Button(introCard.transform, "IntroBack", "Back", new Vector2(-180f, -295f), new Vector2(220f, 56f), new Color(0.20f, 0.27f, 0.30f), 20);
            ui.IntroStart = Button(introCard.transform, "IntroStart", "Choose Mode", new Vector2(180f, -295f), new Vector2(260f, 56f), Blue, 20);

            ui.Mode = FullScreen(canvas, "ModeSelection", new Color(0.025f, 0.04f, 0.045f, 0.98f));
            Heading(ui.Mode.transform, "Choose Learning Mode", 270f);
            ui.Guided = Button(ui.Mode.transform, "Guided", "GUIDED PRACTICE\nStep-by-step instructions, hints and automatic recording", new Vector2(-270f, 10f), new Vector2(470f, 230f), Blue, 23);
            ui.Independent = Button(ui.Mode.transform, "Independent", "INDEPENDENT MODE\nPerform freely with optional instructions and tools", new Vector2(270f, 10f), new Vector2(470f, 230f), Green, 23);
            ui.ModeBack = Button(ui.Mode.transform, "ModeBack", "Back", new Vector2(0f, -230f), new Vector2(210f, 54f), new Color(0.20f, 0.27f, 0.30f), 20);

            GameObject workspaceChrome = new GameObject("WorkspaceChrome", typeof(RectTransform));
            workspaceChrome.transform.SetParent(canvas, false);
            ui.SideToolbar = workspaceChrome;

            ui.InstructionBar = Panel(workspaceChrome.transform, "InstructionBar", new Vector2(0f, 488f), new Vector2(1500f, 74f), Ink);
            ui.InstructionText = Text(ui.InstructionBar.transform, "InstructionText", "", new Vector2(-80f, 0f), new Vector2(1260f, 58f), 20, TextAnchor.MiddleLeft);
            ui.InstructionToggle = Button(workspaceChrome.transform, "InstructionToggle", "Hide Instructions", new Vector2(790f, 488f), new Vector2(250f, 52f), new Color(0.16f, 0.25f, 0.29f), 17);
            ui.InstructionToggleLabel = ui.InstructionToggle.GetComponentInChildren<Text>();
            ui.Home = Button(workspaceChrome.transform, "HomeButton", "Menu", new Vector2(-860f, 488f), new Vector2(130f, 52f), new Color(0.16f, 0.25f, 0.29f), 17);

            GameObject toolRail = Panel(workspaceChrome.transform, "SideToolbar", new Vector2(-870f, 0f), new Vector2(150f, 430f), new Color(0.035f, 0.05f, 0.055f, 0.94f));
            ui.QuickInfoButton = Button(toolRail.transform, "Info", "Info", new Vector2(0f, 145f), new Vector2(118f, 58f), new Color(0.24f, 0.36f, 0.40f), 18);
            ui.NotebookButton = Button(toolRail.transform, "Notebook", "Notebook", new Vector2(0f, 72f), new Vector2(118f, 58f), Blue, 17);
            ui.GraphButton = Button(toolRail.transform, "Graph", "Graph", new Vector2(0f, -1f), new Vector2(118f, 58f), Green, 18);
            ui.CalculatorButton = Button(toolRail.transform, "Calculator", "Calculator", new Vector2(0f, -74f), new Vector2(118f, 58f), new Color(0.34f, 0.27f, 0.50f), 16);

            CreateNotebook(canvas, ref ui);
            CreateGraph(canvas, ref ui);
            CreateCalculator(canvas, ref ui);
            CreateQuickInfo(canvas, ref ui);
            ui.GlobalMessage.transform.SetAsLastSibling();
            return ui;
        }

        private static void CreateNotebook(Transform canvas, ref UiRefs ui)
        {
            ui.NotebookPanel = Card(canvas, "NotebookPanel", new Vector2(180f, 0f), new Vector2(1250f, 760f));
            Heading(ui.NotebookPanel.transform, "Data Recording Notebook", 320f);
            ui.NotebookClose = Close(ui.NotebookPanel.transform);
            ui.TrialCount = Text(ui.NotebookPanel.transform, "TrialCount", "Recorded trials: 0", new Vector2(-420f, 260f), new Vector2(300f, 32f), 18, TextAnchor.MiddleLeft);
            GameObject paper = Panel(ui.NotebookPanel.transform, "Paper", new Vector2(0f, -10f), new Vector2(1120f, 470f), new Color(0.91f, 0.94f, 0.90f));
            ui.NotebookText = Text(paper.transform, "NotebookText", "", Vector2.zero, new Vector2(1080f, 440f), 17, TextAnchor.UpperLeft);
            ui.NotebookText.color = new Color(0.08f, 0.12f, 0.13f);
            ui.NotebookText.font = font;
            ui.ClearRecords = Button(ui.NotebookPanel.transform, "ClearRecords", "Clear Records", new Vector2(0f, -325f), new Vector2(220f, 50f), Red, 18);
        }

        private static void CreateGraph(Transform canvas, ref UiRefs ui)
        {
            ui.GraphPanel = Card(canvas, "GraphPanel", new Vector2(120f, 0f), new Vector2(1320f, 820f));
            Heading(ui.GraphPanel.transform, "Graph Plotting System", 340f);
            ui.GraphClose = Close(ui.GraphPanel.transform);
            GameObject plot = new GameObject("PlotArea", typeof(RectTransform), typeof(CanvasRenderer));
            plot.transform.SetParent(ui.GraphPanel.transform, false);
            RectTransform plotRect = plot.GetComponent<RectTransform>();
            plotRect.anchorMin = new Vector2(0.5f, 0.5f);
            plotRect.anchorMax = new Vector2(0.5f, 0.5f);
            plotRect.pivot = new Vector2(0.5f, 0.5f);
            plotRect.anchoredPosition = new Vector2(-120f, -15f);
            plotRect.sizeDelta = new Vector2(760f, 570f);
            ui.GraphGraphic = plot.AddComponent<Experiment4GraphGraphic>();
            ui.GraphGraphic.color = new Color(0.82f, 0.90f, 0.93f);
            ui.GraphGraphic.raycastTarget = false;
            Text(plot.transform, "YAxis", "2s (m)", new Vector2(-330f, 245f), new Vector2(100f, 28f), 17, TextAnchor.MiddleLeft);
            Text(plot.transform, "XAxis", "t^2 (s^2)", new Vector2(285f, -255f), new Vector2(140f, 28f), 17, TextAnchor.MiddleRight);
            Text[] xLabels = new Text[6];
            Text[] yLabels = new Text[6];
            const float graphLeft = -338f;
            const float graphRight = 362f;
            const float graphBottom = -251f;
            const float graphTop = 267f;
            for (int i = 0; i <= 5; i++)
            {
                float fraction = i / 5f;
                xLabels[i] = Text(
                    plot.transform,
                    "XMajorLabel_" + i,
                    "0",
                    new Vector2(Mathf.Lerp(graphLeft, graphRight, fraction), graphBottom - 24f),
                    new Vector2(90f, 24f),
                    14,
                    TextAnchor.UpperCenter);
                yLabels[i] = Text(
                    plot.transform,
                    "YMajorLabel_" + i,
                    "0",
                    new Vector2(graphLeft - 48f, Mathf.Lerp(graphBottom, graphTop, fraction)),
                    new Vector2(82f, 24f),
                    14,
                    TextAnchor.MiddleRight);
            }
            Text scaleDetails = Text(
                ui.GraphPanel.transform,
                "ScaleDetails",
                "",
                new Vector2(-120f, -350f),
                new Vector2(760f, 52f),
                15,
                TextAnchor.MiddleCenter);
            scaleDetails.color = new Color(0.62f, 0.82f, 0.88f);
            ui.GraphGraphic.ConfigureLabels(xLabels, yLabels, scaleDetails);
            Text(ui.GraphPanel.transform, "ManualPointTitle", "Add Data Point", new Vector2(480f, 210f), new Vector2(260f, 34f), 21, TextAnchor.MiddleCenter).fontStyle = FontStyle.Bold;
            ui.GraphXInput = Input(ui.GraphPanel.transform, "GraphXInput", "", new Vector2(480f, 150f), new Vector2(250f, 46f), "X: t^2 value");
            ui.GraphYInput = Input(ui.GraphPanel.transform, "GraphYInput", "", new Vector2(480f, 92f), new Vector2(250f, 46f), "Y: 2s value");
            ui.GraphAddPoint = Button(ui.GraphPanel.transform, "GraphAddPoint", "Add Point", new Vector2(415f, 28f), new Vector2(150f, 48f), Blue, 17);
            ui.GraphClear = Button(ui.GraphPanel.transform, "GraphClear", "Clear Graph", new Vector2(565f, 28f), new Vector2(130f, 48f), Red, 16);
            ui.GraphSummary = Text(ui.GraphPanel.transform, "GraphSummary", "", new Vector2(480f, -105f), new Vector2(300f, 180f), 18, TextAnchor.MiddleCenter);
        }

        private static void CreateCalculator(Transform canvas, ref UiRefs ui)
        {
            ui.CalculatorPanel = Card(canvas, "CalculatorPanel", new Vector2(180f, 0f), new Vector2(1040f, 820f));
            Heading(ui.CalculatorPanel.transform, "Scientific Calculator", 350f);
            ui.CalculatorClose = Close(ui.CalculatorPanel.transform);
            Text(ui.CalculatorPanel.transform, "ExperimentSection", "EXPERIMENT CALCULATION", new Vector2(-270f, 278f), new Vector2(430f, 34f), 20, TextAnchor.MiddleCenter).fontStyle = FontStyle.Bold;
            Text(ui.CalculatorPanel.transform, "DistanceLabel", "Distance, s (m)", new Vector2(-390f, 215f), new Vector2(230f, 34f), 18, TextAnchor.MiddleLeft);
            ui.DistanceInput = Input(ui.CalculatorPanel.transform, "DistanceInput", "4.50", new Vector2(-155f, 215f), new Vector2(200f, 46f));
            Text(ui.CalculatorPanel.transform, "TimeLabel", "Time, t (s)", new Vector2(-390f, 155f), new Vector2(230f, 34f), 18, TextAnchor.MiddleLeft);
            ui.TimeInput = Input(ui.CalculatorPanel.transform, "TimeInput", "1.00", new Vector2(-155f, 155f), new Vector2(200f, 46f));
            ui.UseLatest = Button(ui.CalculatorPanel.transform, "UseLatest", "Use Latest Trial", new Vector2(-370f, 85f), new Vector2(220f, 50f), new Color(0.24f, 0.34f, 0.43f), 17);
            ui.Calculate = Button(ui.CalculatorPanel.transform, "Calculate", "Calculate a", new Vector2(-135f, 85f), new Vector2(200f, 50f), Blue, 17);
            GameObject resultBox = Panel(ui.CalculatorPanel.transform, "ResultBox", new Vector2(-265f, -25f), new Vector2(440f, 150f), new Color(0.035f, 0.05f, 0.055f));
            ui.CalculatorResult = Text(resultBox.transform, "Result", "a = 2s / t^2", Vector2.zero, new Vector2(410f, 120f), 19, TextAnchor.MiddleCenter);
            ui.CalculatorResult.color = new Color(0.55f, 0.96f, 0.65f);

            Text(ui.CalculatorPanel.transform, "ManualSection", "MANUAL CALCULATOR", new Vector2(270f, 278f), new Vector2(430f, 34f), 20, TextAnchor.MiddleCenter).fontStyle = FontStyle.Bold;
            ui.ManualFirstInput = Input(ui.CalculatorPanel.transform, "ManualFirstInput", "", new Vector2(160f, 215f), new Vector2(210f, 46f), "First number");
            ui.ManualSecondInput = Input(ui.CalculatorPanel.transform, "ManualSecondInput", "", new Vector2(385f, 215f), new Vector2(210f, 46f), "Second number");
            ui.Add = Button(ui.CalculatorPanel.transform, "Add", "+", new Vector2(110f, 145f), new Vector2(82f, 48f), Blue, 22);
            ui.Subtract = Button(ui.CalculatorPanel.transform, "Subtract", "-", new Vector2(200f, 145f), new Vector2(82f, 48f), Blue, 22);
            ui.Multiply = Button(ui.CalculatorPanel.transform, "Multiply", "x", new Vector2(290f, 145f), new Vector2(82f, 48f), Blue, 20);
            ui.Divide = Button(ui.CalculatorPanel.transform, "Divide", "/", new Vector2(380f, 145f), new Vector2(82f, 48f), Blue, 22);
            ui.Power = Button(ui.CalculatorPanel.transform, "Power", "Power", new Vector2(470f, 145f), new Vector2(90f, 48f), new Color(0.18f, 0.34f, 0.55f), 15);
            ui.Square = Button(ui.CalculatorPanel.transform, "Square", "Square", new Vector2(110f, 85f), new Vector2(100f, 46f), new Color(0.18f, 0.34f, 0.55f), 15);
            ui.SquareRoot = Button(ui.CalculatorPanel.transform, "SquareRoot", "Sqrt", new Vector2(220f, 85f), new Vector2(100f, 46f), new Color(0.18f, 0.34f, 0.55f), 16);
            ui.Sin = Button(ui.CalculatorPanel.transform, "Sin", "Sin", new Vector2(330f, 85f), new Vector2(70f, 46f), new Color(0.22f, 0.29f, 0.40f), 16);
            ui.Cos = Button(ui.CalculatorPanel.transform, "Cos", "Cos", new Vector2(410f, 85f), new Vector2(70f, 46f), new Color(0.22f, 0.29f, 0.40f), 16);
            ui.Tan = Button(ui.CalculatorPanel.transform, "Tan", "Tan", new Vector2(490f, 85f), new Vector2(70f, 46f), new Color(0.22f, 0.29f, 0.40f), 16);
            ui.Log = Button(ui.CalculatorPanel.transform, "Log", "Log", new Vector2(175f, 25f), new Vector2(90f, 46f), new Color(0.22f, 0.29f, 0.40f), 16);
            ui.CalculatorClear = Button(ui.CalculatorPanel.transform, "CalculatorClear", "Clear", new Vector2(280f, 25f), new Vector2(100f, 46f), Red, 16);
            GameObject manualResultBox = Panel(ui.CalculatorPanel.transform, "ManualResultBox", new Vector2(290f, -80f), new Vector2(440f, 110f), new Color(0.035f, 0.05f, 0.055f));
            ui.ManualCalculatorResult = Text(manualResultBox.transform, "ManualResult", "Result: 0", Vector2.zero, new Vector2(410f, 80f), 21, TextAnchor.MiddleCenter);
            ui.ManualCalculatorResult.color = new Color(0.55f, 0.96f, 0.65f);
        }

        private static void CreateQuickInfo(Transform canvas, ref UiRefs ui)
        {
            ui.QuickInfoPanel = Card(canvas, "QuickInfoPanel", new Vector2(320f, 0f), new Vector2(800f, 720f));
            Heading(ui.QuickInfoPanel.transform, "Experiment Guide", 300f);
            ui.QuickInfoClose = Close(ui.QuickInfoPanel.transform);
            Text(ui.QuickInfoPanel.transform, "GuideText",
                "FORMULA\ns = 1/2 at^2\n2s = at^2\nAcceleration = slope of 2s against t^2\n\n" +
                "GOOD PRACTICE\n- Keep the ball at the same starting point.\n- Record at least three trials.\n- Use consistent units.\n- Change one setup value at a time.\n\n" +
                "CONTROLS\nClick the 3D inclined plane to open its angle, friction and Start Roll controls.",
                new Vector2(0f, -30f), new Vector2(660f, 500f), 21, TextAnchor.UpperLeft);
        }

        private static GameObject FullScreen(Transform parent, string name, Color color)
        {
            GameObject panel = Panel(parent, name, Vector2.zero, new Vector2(1920f, 1080f), color);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return panel;
        }

        private static GameObject Card(Transform parent, string name, Vector2 position, Vector2 size)
        {
            GameObject card = Panel(parent, name, position, size, PanelColor);
            Outline outline = card.AddComponent<Outline>();
            outline.effectColor = new Color(0.20f, 0.42f, 0.50f, 0.75f);
            outline.effectDistance = new Vector2(2f, -2f);
            return card;
        }

        private static void Heading(Transform parent, string value, float y)
        {
            Text heading = Text(parent, "Heading", value, new Vector2(0f, y), new Vector2(900f, 60f), 34, TextAnchor.MiddleCenter);
            heading.fontStyle = FontStyle.Bold;
        }

        private static Button Close(Transform parent)
        {
            return Button(parent, "Close", "X", Vector2.zero, new Vector2(46f, 42f), Red, 20, Vector2.one, Vector2.one, new Vector2(-34f, -32f));
        }

        private static Button Button(Transform parent, string name, string label, Vector2 position, Vector2 size, Color color, int fontSize)
        {
            return Button(parent, name, label, position, size, color, fontSize, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), position);
        }

        private static Button Button(Transform parent, string name, string label, Vector2 position, Vector2 size, Color color, int fontSize, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition)
        {
            GameObject root = Panel(parent, name, position, size, color);
            RectTransform rect = root.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPosition;
            Button button = root.AddComponent<Button>();
            button.targetGraphic = root.GetComponent<Image>();
            ColorBlock colors = button.colors;
            colors.highlightedColor = Color.Lerp(color, Color.white, 0.15f);
            colors.pressedColor = Color.Lerp(color, Color.black, 0.20f);
            button.colors = colors;
            Text text = Text(root.transform, "Label", label, Vector2.zero, size - new Vector2(18f, 10f), fontSize, TextAnchor.MiddleCenter);
            text.fontStyle = FontStyle.Bold;
            return button;
        }

        private static InputField Input(Transform parent, string name, string value, Vector2 position, Vector2 size, string placeholderValue = "")
        {
            GameObject root = Panel(parent, name, position, size, new Color(0.90f, 0.93f, 0.94f));
            InputField input = root.AddComponent<InputField>();
            input.targetGraphic = root.GetComponent<Image>();
            Text text = Text(root.transform, "Text", value, Vector2.zero, size - new Vector2(20f, 8f), 20, TextAnchor.MiddleCenter);
            text.color = new Color(0.06f, 0.09f, 0.10f);
            Text placeholder = Text(root.transform, "Placeholder", string.IsNullOrEmpty(placeholderValue) ? value : placeholderValue, Vector2.zero, size - new Vector2(20f, 8f), 18, TextAnchor.MiddleCenter);
            placeholder.color = new Color(0.42f, 0.48f, 0.50f);
            input.textComponent = text;
            input.placeholder = placeholder;
            input.contentType = InputField.ContentType.DecimalNumber;
            input.text = value;
            return input;
        }

        private static GameObject Panel(Transform parent, string name, Vector2 position, Vector2 size, Color color)
        {
            GameObject root = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            root.transform.SetParent(parent, false);
            RectTransform rect = root.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            Image image = root.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = true;
            return root;
        }

        private static Text Text(Transform parent, string name, string value, Vector2 position, Vector2 size, int fontSize, TextAnchor alignment)
        {
            GameObject root = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            root.transform.SetParent(parent, false);
            RectTransform rect = root.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            Text text = root.GetComponent<Text>();
            text.font = font;
            text.text = value;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = TextColor;
            text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        private static void SetInitialVisibility(UiRefs ui, GameObject workspaceRoot)
        {
            workspaceRoot.SetActive(false);
            ui.MainMenu.SetActive(true);
            ui.Intro.SetActive(false);
            ui.Mode.SetActive(false);
            ui.SideToolbar.SetActive(false);
            ui.InstructionBar.SetActive(false);
            ui.NotebookPanel.SetActive(false);
            ui.GraphPanel.SetActive(false);
            ui.CalculatorPanel.SetActive(false);
            ui.QuickInfoPanel.SetActive(false);
            ui.MainMenu.transform.SetAsLastSibling();
        }

        private static Material CreateMaterial(string name, Color color)
        {
            string folder = "Assets/_Project/Materials/Experiment4";
            Directory.CreateDirectory(folder);
            string path = folder + "/" + name + ".mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
                material.color = color;
                AssetDatabase.CreateAsset(material, path);
            }
            return material;
        }

        private static void Set(SerializedObject serialized, string propertyName, Object value)
        {
            SerializedProperty property = serialized.FindProperty(propertyName);
            if (property == null)
            {
                Debug.LogError("Missing serialized property: " + propertyName);
                return;
            }
            property.objectReferenceValue = value;
        }

        private struct UiRefs
        {
            public GameObject MainMenu, Intro, Mode, InstructionBar, SideToolbar;
            public GameObject NotebookPanel, GraphPanel, CalculatorPanel, QuickInfoPanel;
            public Text GlobalMessage, InstructionText, InstructionToggleLabel;
            public Button Experiment4Button, Experiment5Button, IntroBack, IntroStart, ModeBack, Guided, Independent;
            public Button InstructionToggle, Home, NotebookButton, GraphButton, CalculatorButton, QuickInfoButton;
            public Button NotebookClose, GraphClose, CalculatorClose, QuickInfoClose, ClearRecords;
            public Text NotebookText, TrialCount, GraphSummary, CalculatorResult;
            public Experiment4GraphGraphic GraphGraphic;
            public InputField GraphXInput, GraphYInput, DistanceInput, TimeInput, ManualFirstInput, ManualSecondInput;
            public Button GraphAddPoint, GraphClear, Calculate, UseLatest;
            public Text ManualCalculatorResult;
            public Button Add, Subtract, Multiply, Divide, Power, Square, SquareRoot, Sin, Cos, Tan, Log, CalculatorClear;
        }
    }
}
