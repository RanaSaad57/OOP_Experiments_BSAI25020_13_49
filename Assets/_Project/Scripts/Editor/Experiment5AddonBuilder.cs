using System.IO;
using OOPLab.Experiments;
using OOPLab.Modules.FreeFallSetup;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace OOPLab.EditorTools
{
    public static class Experiment5AddonBuilder
    {
        private static readonly Color Background = new Color(0.025f, 0.04f, 0.045f, 0.98f);
        private static readonly Color PanelColor = new Color(0.075f, 0.095f, 0.105f, 0.98f);
        private static readonly Color Blue = new Color(0.04f, 0.48f, 0.72f);
        private static readonly Color Green = new Color(0.12f, 0.48f, 0.34f);
        private static readonly Color Red = new Color(0.65f, 0.15f, 0.20f);
        private static readonly Color TextColor = new Color(0.88f, 0.93f, 0.95f);
        private static Font font;

        public static Experiment5ExperienceController Build(
            Canvas canvas,
            GameObject mainMenu,
            Text globalMessage,
            Experiment4ExperienceController mainNavigation)
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf")
                   ?? Resources.GetBuiltinResource<Font>("Arial.ttf");

            FreeFallController freeFall = CreateFreeFallModule();
            GameObject workspace = new GameObject("Experiment5Workspace");
            freeFall.transform.SetParent(workspace.transform, true);

            UiRefs ui = CreateUi(canvas.transform);
            Experiment5ExperienceController controller =
                new GameObject("Experiment5ExperienceSystem").AddComponent<Experiment5ExperienceController>();

            SerializedObject serialized = new SerializedObject(controller);
            Set(serialized, "mainNavigation", mainNavigation);
            Set(serialized, "mainMenu", mainMenu);
            Set(serialized, "introductionScreen", ui.Intro);
            Set(serialized, "modeScreen", ui.Mode);
            Set(serialized, "workspaceRoot", workspace);
            Set(serialized, "workspaceChrome", ui.Chrome);
            Set(serialized, "sceneCamera", Camera.main);
            Set(serialized, "instructionBar", ui.InstructionBar);
            Set(serialized, "instructionText", ui.InstructionText);
            Set(serialized, "instructionToggleText", ui.InstructionToggleText);
            Set(serialized, "globalMessage", globalMessage);
            Set(serialized, "introBackButton", ui.IntroBack);
            Set(serialized, "introStartButton", ui.IntroStart);
            Set(serialized, "modeBackButton", ui.ModeBack);
            Set(serialized, "guidedButton", ui.Guided);
            Set(serialized, "independentButton", ui.Independent);
            Set(serialized, "instructionToggleButton", ui.InstructionToggle);
            Set(serialized, "homeButton", ui.Home);
            Set(serialized, "freeFallController", freeFall);
            Set(serialized, "notebookPanel", ui.NotebookPanel);
            Set(serialized, "graphPanel", ui.GraphPanel);
            Set(serialized, "calculatorPanel", ui.CalculatorPanel);
            Set(serialized, "infoPanel", ui.InfoPanel);
            Set(serialized, "notebookButton", ui.NotebookButton);
            Set(serialized, "graphButton", ui.GraphButton);
            Set(serialized, "calculatorButton", ui.CalculatorButton);
            Set(serialized, "infoButton", ui.InfoButton);
            Set(serialized, "notebookCloseButton", ui.NotebookClose);
            Set(serialized, "graphCloseButton", ui.GraphClose);
            Set(serialized, "calculatorCloseButton", ui.CalculatorClose);
            Set(serialized, "infoCloseButton", ui.InfoClose);
            Set(serialized, "notebookText", ui.NotebookText);
            Set(serialized, "trialCountText", ui.TrialCount);
            Set(serialized, "clearRecordsButton", ui.ClearRecords);
            Set(serialized, "graphGraphic", ui.GraphGraphic);
            Set(serialized, "graphSummaryText", ui.GraphSummary);
            Set(serialized, "graphXInput", ui.GraphXInput);
            Set(serialized, "graphYInput", ui.GraphYInput);
            Set(serialized, "graphAddButton", ui.GraphAdd);
            Set(serialized, "graphClearButton", ui.GraphClear);
            Set(serialized, "heightInput", ui.HeightInput);
            Set(serialized, "timeInput", ui.TimeInput);
            Set(serialized, "gravityResultText", ui.GravityResult);
            Set(serialized, "useLatestButton", ui.UseLatest);
            Set(serialized, "calculateGravityButton", ui.CalculateGravity);
            serialized.ApplyModifiedProperties();

            workspace.SetActive(false);
            ui.Intro.SetActive(false);
            ui.Mode.SetActive(false);
            ui.Chrome.SetActive(false);
            ui.NotebookPanel.SetActive(false);
            ui.GraphPanel.SetActive(false);
            ui.CalculatorPanel.SetActive(false);
            ui.InfoPanel.SetActive(false);
            return controller;
        }

        private static FreeFallController CreateFreeFallModule()
        {
            GameObject root = new GameObject("FreeFallSetupModule");
            Material metal = Material("FreeFallMetal", new Color(0.55f, 0.62f, 0.66f));
            Material dark = Material("FreeFallBase", new Color(0.09f, 0.12f, 0.13f));
            Material accent = Material("FreeFallAccent", new Color(0.07f, 0.43f, 0.66f));
            Material ballMaterial = Material("FreeFallBall", new Color(0.80f, 0.84f, 0.87f));

            Cube(root.transform, "Base", new Vector3(0f, 0.04f, 0f), new Vector3(3.8f, 0.16f, 2.2f), dark);
            Cube(root.transform, "LeftPost", new Vector3(-1.35f, 2.25f, 0.55f), new Vector3(0.14f, 4.5f, 0.14f), metal);
            Cube(root.transform, "RightPost", new Vector3(1.35f, 2.25f, 0.55f), new Vector3(0.14f, 4.5f, 0.14f), metal);
            Cube(root.transform, "TopBar", new Vector3(0f, 4.45f, 0.55f), new Vector3(2.85f, 0.14f, 0.14f), metal);
            GameObject landingPlate = Cube(root.transform, "LandingPlate", new Vector3(0f, 0.15f, 0f), new Vector3(1.5f, 0.18f, 1.25f), accent);
            landingPlate.AddComponent<FreeFallClickTarget>();

            for (int i = 0; i <= 8; i++)
            {
                float y = 0.35f + i * 0.5f;
                Cube(root.transform, "ScaleTick_" + i, new Vector3(-1.22f, y, 0.43f), new Vector3(0.28f, 0.025f, 0.04f), metal);
            }

            GameObject releaseAssembly = Cube(root.transform, "ReleaseAssembly", new Vector3(0f, 2.18f, 0.25f), new Vector3(1.1f, 0.22f, 0.55f), accent);
            releaseAssembly.AddComponent<FreeFallClickTarget>();
            GameObject releasePoint = new GameObject("ReleasePoint");
            releasePoint.transform.SetParent(releaseAssembly.transform, false);
            releasePoint.transform.localPosition = new Vector3(0f, -0.34f, -0.38f);

            GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball.name = "FreeFallSteelBall";
            ball.transform.SetParent(root.transform, false);
            ball.transform.position = releasePoint.transform.position;
            ball.transform.localScale = Vector3.one * 0.46f;
            ball.GetComponent<Renderer>().sharedMaterial = ballMaterial;
            Rigidbody rigidbody = ball.AddComponent<Rigidbody>();
            rigidbody.mass = 0.25f;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.isKinematic = true;

            Canvas popupCanvas = PopupCanvas(root.transform);
            PopupRefs popup = CreatePopup(popupCanvas.transform);

            FreeFallController controller = root.AddComponent<FreeFallController>();
            SerializedObject serialized = new SerializedObject(controller);
            Set(serialized, "fallingBall", rigidbody);
            Set(serialized, "releaseAssembly", releaseAssembly.transform);
            Set(serialized, "releasePoint", releasePoint.transform);
            serialized.FindProperty("landingHeight").floatValue = 0.24f;
            Set(serialized, "controlPanel", popup.Panel);
            Set(serialized, "closeButton", popup.Close);
            Set(serialized, "heightSlider", popup.HeightSlider);
            Set(serialized, "heightInput", popup.HeightInput);
            Set(serialized, "heightMinusButton", popup.HeightMinus);
            Set(serialized, "heightPlusButton", popup.HeightPlus);
            Set(serialized, "releaseButton", popup.Release);
            Set(serialized, "resetButton", popup.Reset);
            Set(serialized, "sampleButton", popup.Sample);
            Set(serialized, "heightText", popup.HeightText);
            Set(serialized, "timeText", popup.TimeText);
            Set(serialized, "gravityText", popup.GravityText);
            Set(serialized, "statusText", popup.Status);
            serialized.ApplyModifiedProperties();

            releaseAssembly.GetComponent<FreeFallClickTarget>().SetController(controller);
            landingPlate.GetComponent<FreeFallClickTarget>().SetController(controller);
            root.transform.position = new Vector3(0f, 0f, 0.35f);
            return controller;
        }

        private static PopupRefs CreatePopup(Transform parent)
        {
            PopupRefs popup = new PopupRefs();
            popup.Panel = Card(parent, "FreeFallPopup", new Vector2(550f, 0f), new Vector2(540f, 610f));
            Heading(popup.Panel.transform, "Free-Fall Setup", 250f);
            popup.Close = Close(popup.Panel.transform);
            Text(popup.Panel.transform, "Hint", "Click the release clamp to open these controls.", new Vector2(0f, 210f), new Vector2(450f, 28f), 15, TextAnchor.MiddleCenter);
            popup.HeightText = Text(popup.Panel.transform, "HeightText", "Drop height: 2.00 m", new Vector2(0f, 145f), new Vector2(420f, 32f), 20, TextAnchor.MiddleCenter);
            popup.HeightSlider = Slider(popup.Panel.transform, "HeightSlider", new Vector2(0f, 100f), new Vector2(340f, 30f));
            popup.HeightInput = Input(popup.Panel.transform, "HeightInput", "2.00", new Vector2(0f, 48f), new Vector2(150f, 44f), "Height (m)");
            popup.HeightMinus = Button(popup.Panel.transform, "HeightMinus", "-", new Vector2(-130f, 48f), new Vector2(52f, 44f), new Color(0.13f, 0.28f, 0.43f), 24);
            popup.HeightPlus = Button(popup.Panel.transform, "HeightPlus", "+", new Vector2(130f, 48f), new Vector2(52f, 44f), Green, 24);
            popup.TimeText = Text(popup.Panel.transform, "TimeText", "Time: 0.000 s", new Vector2(0f, -20f), new Vector2(420f, 30f), 19, TextAnchor.MiddleCenter);
            popup.GravityText = Text(popup.Panel.transform, "GravityText", "g = -- m/s^2", new Vector2(0f, -60f), new Vector2(420f, 30f), 21, TextAnchor.MiddleCenter);
            popup.GravityText.color = new Color(0.55f, 0.96f, 0.65f);
            popup.Release = Button(popup.Panel.transform, "Release", "Release Ball", new Vector2(-140f, -130f), new Vector2(160f, 50f), Blue, 17);
            popup.Reset = Button(popup.Panel.transform, "Reset", "Reset", new Vector2(25f, -130f), new Vector2(120f, 50f), Red, 17);
            popup.Sample = Button(popup.Panel.transform, "Sample", "Sample", new Vector2(160f, -130f), new Vector2(120f, 50f), new Color(0.18f, 0.34f, 0.55f), 17);
            popup.Status = Text(popup.Panel.transform, "Status", "Ready.", new Vector2(0f, -205f), new Vector2(450f, 52f), 16, TextAnchor.MiddleCenter);
            popup.Status.color = new Color(0.66f, 0.96f, 0.70f);
            return popup;
        }

        private static UiRefs CreateUi(Transform canvas)
        {
            UiRefs ui = new UiRefs();
            ui.Intro = FullScreen(canvas, "Experiment5Introduction", Background);
            GameObject introCard = Card(ui.Intro.transform, "IntroCard", Vector2.zero, new Vector2(1180f, 760f));
            Heading(introCard.transform, "Experiment 5: Determine g by Free Fall", 305f);
            Text(introCard.transform, "Objective", "OBJECTIVE\nDetermine gravitational acceleration g by measuring the time taken by a steel ball to fall through a known height.", new Vector2(-300f, 180f), new Vector2(500f, 145f), 21, TextAnchor.UpperLeft);
            Text(introCard.transform, "Apparatus", "APPARATUS\nFree-fall stand, steel ball, release mechanism, metre scale, landing detector, stopwatch, notebook, graph and calculator.", new Vector2(300f, 180f), new Vector2(500f, 145f), 21, TextAnchor.UpperLeft);
            Text(introCard.transform, "Theory", "THEORY\nFor an object released from rest: h = 1/2 gt^2. Therefore g = 2h/t^2. The slope of 2h against t^2 also gives g.", new Vector2(-300f, -10f), new Vector2(500f, 160f), 21, TextAnchor.UpperLeft);
            Text(introCard.transform, "Steps", "PROCEDURE\n1. Set a measured drop height.\n2. Release the ball and record time automatically.\n3. Repeat at different heights.\n4. Calculate g and plot 2h against t^2.", new Vector2(300f, -20f), new Vector2(500f, 180f), 21, TextAnchor.UpperLeft);
            ui.IntroBack = Button(introCard.transform, "IntroBack", "Back", new Vector2(-180f, -295f), new Vector2(220f, 56f), new Color(0.20f, 0.27f, 0.30f), 20);
            ui.IntroStart = Button(introCard.transform, "IntroStart", "Choose Mode", new Vector2(180f, -295f), new Vector2(260f, 56f), Blue, 20);

            ui.Mode = FullScreen(canvas, "Experiment5Mode", Background);
            Heading(ui.Mode.transform, "Choose Learning Mode", 270f);
            ui.Guided = Button(ui.Mode.transform, "Guided", "GUIDED PRACTICE\nInstructions, hints and automatic recording", new Vector2(-270f, 10f), new Vector2(470f, 230f), Blue, 23);
            ui.Independent = Button(ui.Mode.transform, "Independent", "INDEPENDENT MODE\nOptional instructions and scientific tools", new Vector2(270f, 10f), new Vector2(470f, 230f), Green, 23);
            ui.ModeBack = Button(ui.Mode.transform, "ModeBack", "Back", new Vector2(0f, -230f), new Vector2(210f, 54f), new Color(0.20f, 0.27f, 0.30f), 20);

            ui.Chrome = new GameObject("Experiment5Chrome", typeof(RectTransform));
            ui.Chrome.transform.SetParent(canvas, false);
            ui.InstructionBar = Panel(ui.Chrome.transform, "InstructionBar", new Vector2(0f, 488f), new Vector2(1500f, 74f), new Color(0.055f, 0.075f, 0.085f, 0.98f));
            ui.InstructionText = Text(ui.InstructionBar.transform, "InstructionText", "", new Vector2(-80f, 0f), new Vector2(1260f, 58f), 20, TextAnchor.MiddleLeft);
            ui.InstructionToggle = Button(ui.Chrome.transform, "InstructionToggle", "Hide Instructions", new Vector2(790f, 488f), new Vector2(250f, 52f), new Color(0.16f, 0.25f, 0.29f), 17);
            ui.InstructionToggleText = ui.InstructionToggle.GetComponentInChildren<Text>();
            ui.Home = Button(ui.Chrome.transform, "Home", "Menu", new Vector2(-860f, 488f), new Vector2(130f, 52f), new Color(0.16f, 0.25f, 0.29f), 17);
            GameObject rail = Panel(ui.Chrome.transform, "ToolRail", new Vector2(-870f, 0f), new Vector2(150f, 430f), new Color(0.035f, 0.05f, 0.055f, 0.94f));
            ui.InfoButton = Button(rail.transform, "Info", "Info", new Vector2(0f, 145f), new Vector2(118f, 58f), new Color(0.24f, 0.36f, 0.40f), 18);
            ui.NotebookButton = Button(rail.transform, "Notebook", "Notebook", new Vector2(0f, 72f), new Vector2(118f, 58f), Blue, 17);
            ui.GraphButton = Button(rail.transform, "Graph", "Graph", new Vector2(0f, -1f), new Vector2(118f, 58f), Green, 18);
            ui.CalculatorButton = Button(rail.transform, "Calculator", "Calculator", new Vector2(0f, -74f), new Vector2(118f, 58f), new Color(0.34f, 0.27f, 0.50f), 16);

            CreateNotebook(canvas, ref ui);
            CreateGraph(canvas, ref ui);
            CreateCalculator(canvas, ref ui);
            CreateInfo(canvas, ref ui);
            return ui;
        }

        private static void CreateNotebook(Transform canvas, ref UiRefs ui)
        {
            ui.NotebookPanel = Card(canvas, "Experiment5Notebook", new Vector2(180f, 0f), new Vector2(1120f, 720f));
            Heading(ui.NotebookPanel.transform, "Free-Fall Data Notebook", 295f);
            ui.NotebookClose = Close(ui.NotebookPanel.transform);
            ui.TrialCount = Text(ui.NotebookPanel.transform, "TrialCount", "Recorded trials: 0", new Vector2(-370f, 235f), new Vector2(300f, 32f), 18, TextAnchor.MiddleLeft);
            GameObject paper = Panel(ui.NotebookPanel.transform, "Paper", new Vector2(0f, -10f), new Vector2(980f, 430f), new Color(0.91f, 0.94f, 0.90f));
            ui.NotebookText = Text(paper.transform, "NotebookText", "", Vector2.zero, new Vector2(940f, 400f), 18, TextAnchor.UpperLeft);
            ui.NotebookText.color = new Color(0.08f, 0.12f, 0.13f);
            ui.ClearRecords = Button(ui.NotebookPanel.transform, "ClearRecords", "Clear Records", new Vector2(0f, -300f), new Vector2(220f, 50f), Red, 18);
        }

        private static void CreateGraph(Transform canvas, ref UiRefs ui)
        {
            ui.GraphPanel = Card(canvas, "Experiment5Graph", new Vector2(120f, 0f), new Vector2(1320f, 820f));
            Heading(ui.GraphPanel.transform, "Free-Fall Graph: 2h against t^2", 340f);
            ui.GraphClose = Close(ui.GraphPanel.transform);
            GameObject plot = new GameObject("PlotArea", typeof(RectTransform), typeof(CanvasRenderer));
            plot.transform.SetParent(ui.GraphPanel.transform, false);
            RectTransform rect = plot.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(-120f, -15f);
            rect.sizeDelta = new Vector2(760f, 570f);
            ui.GraphGraphic = plot.AddComponent<Experiment4GraphGraphic>();
            ui.GraphGraphic.color = TextColor;
            ui.GraphGraphic.raycastTarget = false;
            Text(plot.transform, "YAxis", "2h (m)", new Vector2(-330f, 245f), new Vector2(100f, 28f), 17, TextAnchor.MiddleLeft);
            Text(plot.transform, "XAxis", "t^2 (s^2)", new Vector2(285f, -255f), new Vector2(140f, 28f), 17, TextAnchor.MiddleRight);
            Text[] xLabels = new Text[6];
            Text[] yLabels = new Text[6];
            for (int i = 0; i <= 5; i++)
            {
                float fraction = i / 5f;
                xLabels[i] = Text(plot.transform, "XLabel_" + i, "0", new Vector2(Mathf.Lerp(-338f, 362f, fraction), -275f), new Vector2(90f, 24f), 14, TextAnchor.UpperCenter);
                yLabels[i] = Text(plot.transform, "YLabel_" + i, "0", new Vector2(-386f, Mathf.Lerp(-251f, 267f, fraction)), new Vector2(82f, 24f), 14, TextAnchor.MiddleRight);
            }
            Text scale = Text(ui.GraphPanel.transform, "Scale", "", new Vector2(-120f, -350f), new Vector2(760f, 52f), 15, TextAnchor.MiddleCenter);
            ui.GraphGraphic.ConfigureLabels(xLabels, yLabels, scale, "t^2", "s^2", "2h", "m");
            Text(ui.GraphPanel.transform, "AddTitle", "Add Data Point", new Vector2(480f, 210f), new Vector2(260f, 34f), 21, TextAnchor.MiddleCenter).fontStyle = FontStyle.Bold;
            ui.GraphXInput = Input(ui.GraphPanel.transform, "GraphXInput", "", new Vector2(480f, 150f), new Vector2(250f, 46f), "X: t^2 value");
            ui.GraphYInput = Input(ui.GraphPanel.transform, "GraphYInput", "", new Vector2(480f, 92f), new Vector2(250f, 46f), "Y: 2h value");
            ui.GraphAdd = Button(ui.GraphPanel.transform, "GraphAdd", "Add Point", new Vector2(415f, 28f), new Vector2(150f, 48f), Blue, 17);
            ui.GraphClear = Button(ui.GraphPanel.transform, "GraphClear", "Clear Graph", new Vector2(565f, 28f), new Vector2(130f, 48f), Red, 16);
            ui.GraphSummary = Text(ui.GraphPanel.transform, "GraphSummary", "", new Vector2(480f, -105f), new Vector2(300f, 180f), 18, TextAnchor.MiddleCenter);
        }

        private static void CreateCalculator(Transform canvas, ref UiRefs ui)
        {
            ui.CalculatorPanel = Card(canvas, "Experiment5Calculator", new Vector2(250f, 0f), new Vector2(760f, 650f));
            Heading(ui.CalculatorPanel.transform, "Calculate Gravitational Acceleration", 270f);
            ui.CalculatorClose = Close(ui.CalculatorPanel.transform);
            Text(ui.CalculatorPanel.transform, "HeightLabel", "Drop height, h (m)", new Vector2(-180f, 150f), new Vector2(260f, 34f), 19, TextAnchor.MiddleLeft);
            ui.HeightInput = Input(ui.CalculatorPanel.transform, "HeightInput", "2.00", new Vector2(165f, 150f), new Vector2(220f, 48f), "Height");
            Text(ui.CalculatorPanel.transform, "TimeLabel", "Fall time, t (s)", new Vector2(-180f, 80f), new Vector2(260f, 34f), 19, TextAnchor.MiddleLeft);
            ui.TimeInput = Input(ui.CalculatorPanel.transform, "TimeInput", "0.64", new Vector2(165f, 80f), new Vector2(220f, 48f), "Time");
            ui.UseLatest = Button(ui.CalculatorPanel.transform, "UseLatest", "Use Latest Trial", new Vector2(-135f, -5f), new Vector2(240f, 52f), new Color(0.24f, 0.34f, 0.43f), 18);
            ui.CalculateGravity = Button(ui.CalculatorPanel.transform, "CalculateGravity", "Calculate g", new Vector2(145f, -5f), new Vector2(220f, 52f), Blue, 18);
            GameObject result = Panel(ui.CalculatorPanel.transform, "Result", new Vector2(0f, -145f), new Vector2(620f, 180f), new Color(0.035f, 0.05f, 0.055f));
            ui.GravityResult = Text(result.transform, "GravityResult", "g = 2h / t^2", Vector2.zero, new Vector2(580f, 150f), 22, TextAnchor.MiddleCenter);
            ui.GravityResult.color = new Color(0.55f, 0.96f, 0.65f);
        }

        private static void CreateInfo(Transform canvas, ref UiRefs ui)
        {
            ui.InfoPanel = Card(canvas, "Experiment5Info", new Vector2(320f, 0f), new Vector2(800f, 690f));
            Heading(ui.InfoPanel.transform, "Free-Fall Guide", 285f);
            ui.InfoClose = Close(ui.InfoPanel.transform);
            Text(ui.InfoPanel.transform, "Text",
                "FORMULA\nh = 1/2 gt^2\ng = 2h/t^2\n\n" +
                "METHOD\nUse different measured heights. Release the ball from rest and allow the automatic landing detector to stop the timer.\n\n" +
                "GRAPH\nPlot t^2 on X and 2h on Y. The slope is g.\n\n" +
                "GOOD PRACTICE\n- Do not push the ball.\n- Keep the landing plate fixed.\n- Record at least three heights.\n- Use metres and seconds.",
                new Vector2(0f, -35f), new Vector2(660f, 500f), 21, TextAnchor.UpperLeft);
        }

        private static Canvas PopupCanvas(Transform parent)
        {
            GameObject root = new GameObject("FreeFallPopupCanvas");
            root.transform.SetParent(parent, false);
            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 30;
            CanvasScaler scaler = root.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            root.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static GameObject Cube(Transform parent, string name, Vector3 position, Vector3 scale, Material material)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.SetParent(parent, false);
            cube.transform.position = position;
            cube.transform.localScale = scale;
            cube.GetComponent<Renderer>().sharedMaterial = material;
            return cube;
        }

        private static Slider Slider(Transform parent, string name, Vector2 position, Vector2 size)
        {
            GameObject root = new GameObject(name, typeof(RectTransform));
            root.transform.SetParent(parent, false);
            RectTransform rect = root.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            GameObject background = Panel(root.transform, "Background", Vector2.zero, new Vector2(size.x, 10f), new Color(0.15f, 0.18f, 0.20f));
            GameObject fillArea = new GameObject("Fill Area", typeof(RectTransform));
            fillArea.transform.SetParent(root.transform, false);
            RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.offsetMin = new Vector2(8f, 0f);
            fillAreaRect.offsetMax = new Vector2(-8f, 0f);
            GameObject fill = Panel(fillArea.transform, "Fill", Vector2.zero, new Vector2(size.x, 10f), new Color(0.04f, 0.58f, 0.78f));
            RectTransform fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = fillRect.offsetMax = Vector2.zero;
            GameObject handleArea = new GameObject("Handle Slide Area", typeof(RectTransform));
            handleArea.transform.SetParent(root.transform, false);
            RectTransform handleAreaRect = handleArea.GetComponent<RectTransform>();
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            handleAreaRect.offsetMin = new Vector2(8f, 0f);
            handleAreaRect.offsetMax = new Vector2(-8f, 0f);
            GameObject handle = Panel(handleArea.transform, "Handle", Vector2.zero, new Vector2(28f, 34f), new Color(0.93f, 0.97f, 0.98f));
            Slider slider = root.AddComponent<Slider>();
            slider.targetGraphic = handle.GetComponent<Image>();
            slider.fillRect = fillRect;
            slider.handleRect = handle.GetComponent<RectTransform>();
            background.transform.SetSiblingIndex(0);
            return slider;
        }

        private static GameObject FullScreen(Transform parent, string name, Color color)
        {
            GameObject panel = Panel(parent, name, Vector2.zero, new Vector2(1920f, 1080f), color);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
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

        private static void Heading(Transform parent, string text, float y)
        {
            Text heading = Text(parent, "Heading", text, new Vector2(0f, y), new Vector2(900f, 60f), 34, TextAnchor.MiddleCenter);
            heading.fontStyle = FontStyle.Bold;
        }

        private static Button Close(Transform parent)
        {
            return Button(parent, "Close", "X", Vector2.zero, new Vector2(46f, 42f), Red, 20, Vector2.one, new Vector2(-34f, -32f));
        }

        private static Button Button(Transform parent, string name, string label, Vector2 position, Vector2 size, Color color, int fontSize)
        {
            return Button(parent, name, label, position, size, color, fontSize, new Vector2(0.5f, 0.5f), position);
        }

        private static Button Button(Transform parent, string name, string label, Vector2 position, Vector2 size, Color color, int fontSize, Vector2 anchor, Vector2 anchoredPosition)
        {
            GameObject root = Panel(parent, name, position, size, color);
            RectTransform rect = root.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = anchor;
            rect.anchoredPosition = anchoredPosition;
            Button button = root.AddComponent<Button>();
            button.targetGraphic = root.GetComponent<Image>();
            Text(root.transform, "Label", label, Vector2.zero, size - new Vector2(16f, 8f), fontSize, TextAnchor.MiddleCenter).fontStyle = FontStyle.Bold;
            return button;
        }

        private static InputField Input(Transform parent, string name, string value, Vector2 position, Vector2 size, string placeholder)
        {
            GameObject root = Panel(parent, name, position, size, new Color(0.90f, 0.93f, 0.94f));
            InputField input = root.AddComponent<InputField>();
            input.targetGraphic = root.GetComponent<Image>();
            Text text = Text(root.transform, "Text", value, Vector2.zero, size - new Vector2(20f, 8f), 19, TextAnchor.MiddleCenter);
            text.color = new Color(0.06f, 0.09f, 0.10f);
            Text placeholderText = Text(root.transform, "Placeholder", placeholder, Vector2.zero, size - new Vector2(20f, 8f), 17, TextAnchor.MiddleCenter);
            placeholderText.color = new Color(0.42f, 0.48f, 0.50f);
            input.textComponent = text;
            input.placeholder = placeholderText;
            input.contentType = InputField.ContentType.DecimalNumber;
            input.text = value;
            return input;
        }

        private static GameObject Panel(Transform parent, string name, Vector2 position, Vector2 size, Color color)
        {
            GameObject root = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            root.transform.SetParent(parent, false);
            RectTransform rect = root.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            root.GetComponent<Image>().color = color;
            return root;
        }

        private static Text Text(Transform parent, string name, string value, Vector2 position, Vector2 size, int fontSize, TextAnchor alignment)
        {
            GameObject root = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            root.transform.SetParent(parent, false);
            RectTransform rect = root.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
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

        private static Material Material(string name, Color color)
        {
            string folder = "Assets/_Project/Materials/Experiment5";
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
            if (property != null)
            {
                property.objectReferenceValue = value;
            }
        }

        private struct PopupRefs
        {
            public GameObject Panel;
            public Button Close, HeightMinus, HeightPlus, Release, Reset, Sample;
            public Slider HeightSlider;
            public InputField HeightInput;
            public Text HeightText, TimeText, GravityText, Status;
        }

        private struct UiRefs
        {
            public GameObject Intro, Mode, Chrome, InstructionBar;
            public Text InstructionText, InstructionToggleText;
            public Button IntroBack, IntroStart, ModeBack, Guided, Independent, InstructionToggle, Home;
            public Button NotebookButton, GraphButton, CalculatorButton, InfoButton;
            public GameObject NotebookPanel, GraphPanel, CalculatorPanel, InfoPanel;
            public Button NotebookClose, GraphClose, CalculatorClose, InfoClose;
            public Text NotebookText, TrialCount;
            public Button ClearRecords;
            public Experiment4GraphGraphic GraphGraphic;
            public Text GraphSummary;
            public InputField GraphXInput, GraphYInput;
            public Button GraphAdd, GraphClear;
            public InputField HeightInput, TimeInput;
            public Text GravityResult;
            public Button UseLatest, CalculateGravity;
        }
    }
}
