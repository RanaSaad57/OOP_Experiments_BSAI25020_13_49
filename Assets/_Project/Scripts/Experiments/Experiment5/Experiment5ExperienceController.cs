using System.Collections.Generic;
using System.Text;
using OOPLab.Modules.FreeFallSetup;
using UnityEngine;
using UnityEngine.UI;

namespace OOPLab.Experiments
{
    public class Experiment5ExperienceController : MonoBehaviour
    {
        [Header("Navigation")]
        [SerializeField] private Experiment4ExperienceController mainNavigation;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject introductionScreen;
        [SerializeField] private GameObject modeScreen;
        [SerializeField] private GameObject workspaceRoot;
        [SerializeField] private GameObject workspaceChrome;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private GameObject instructionBar;
        [SerializeField] private Text instructionText;
        [SerializeField] private Text instructionToggleText;
        [SerializeField] private Text globalMessage;
        [SerializeField] private Button introBackButton;
        [SerializeField] private Button introStartButton;
        [SerializeField] private Button modeBackButton;
        [SerializeField] private Button guidedButton;
        [SerializeField] private Button independentButton;
        [SerializeField] private Button instructionToggleButton;
        [SerializeField] private Button homeButton;

        [Header("Experiment")]
        [SerializeField] private FreeFallController freeFallController;

        [Header("Tools")]
        [SerializeField] private GameObject notebookPanel;
        [SerializeField] private GameObject graphPanel;
        [SerializeField] private GameObject calculatorPanel;
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private Button notebookButton;
        [SerializeField] private Button graphButton;
        [SerializeField] private Button calculatorButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button notebookCloseButton;
        [SerializeField] private Button graphCloseButton;
        [SerializeField] private Button calculatorCloseButton;
        [SerializeField] private Button infoCloseButton;

        [Header("Notebook")]
        [SerializeField] private Text notebookText;
        [SerializeField] private Text trialCountText;
        [SerializeField] private Button clearRecordsButton;

        [Header("Graph")]
        [SerializeField] private Experiment4GraphGraphic graphGraphic;
        [SerializeField] private Text graphSummaryText;
        [SerializeField] private InputField graphXInput;
        [SerializeField] private InputField graphYInput;
        [SerializeField] private Button graphAddButton;
        [SerializeField] private Button graphClearButton;

        [Header("Calculator")]
        [SerializeField] private InputField heightInput;
        [SerializeField] private InputField timeInput;
        [SerializeField] private Text gravityResultText;
        [SerializeField] private Button useLatestButton;
        [SerializeField] private Button calculateGravityButton;

        private readonly List<FreeFallTrial> trials = new List<FreeFallTrial>();
        private readonly List<Vector2> graphPoints = new List<Vector2>();
        private bool guided;
        private bool instructionsVisible;

        private void Awake()
        {
            WireButtons();
            if (freeFallController != null)
            {
                freeFallController.TrialCompleted += RecordTrial;
            }

            HideAll();
            RefreshData();
        }

        private void OnDestroy()
        {
            if (freeFallController != null)
            {
                freeFallController.TrialCompleted -= RecordTrial;
            }
        }

        public void ShowIntroduction()
        {
            mainMenu.SetActive(false);
            HideAll();
            introductionScreen.SetActive(true);
            SetMessage(string.Empty);
        }

        private void ShowModeScreen()
        {
            introductionScreen.SetActive(false);
            modeScreen.SetActive(true);
        }

        private void StartExperiment(bool guidedMode)
        {
            guided = guidedMode;
            instructionsVisible = guidedMode;
            introductionScreen.SetActive(false);
            modeScreen.SetActive(false);
            workspaceRoot.SetActive(true);
            workspaceChrome.SetActive(true);
            instructionBar.SetActive(instructionsVisible);
            UpdateToggleText();
            CloseTools();
            freeFallController?.ResetTrial();
            FrameFreeFallApparatus();
            UpdateGuidance();
            SetMessage(guidedMode
                ? "Guided Practice started. Follow the instructions at the top."
                : "Independent Mode started. Instructions can be opened when needed.");
        }

        private void ReturnToMainMenu()
        {
            HideAll();
            mainNavigation.ShowMainMenu();
        }

        private void ToggleInstructions()
        {
            instructionsVisible = !instructionsVisible;
            instructionBar.SetActive(instructionsVisible);
            UpdateToggleText();
        }

        private void RecordTrial(FreeFallTrial trial)
        {
            trials.Add(trial);
            graphPoints.Add(new Vector2(trial.TimeSquared, trial.TwoHeight));
            RefreshData();
            UpdateGuidance();
            SetMessage($"Trial {trial.TrialNumber} recorded: h = {trial.HeightMeters:0.00} m, t = {trial.TimeSeconds:0.000} s.");
        }

        private void RefreshData()
        {
            if (trialCountText != null)
            {
                trialCountText.text = $"Recorded trials: {trials.Count}";
            }

            if (notebookText != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("Trial    h (m)    t (s)    2h (m)    t^2 (s^2)    g (m/s^2)");
                builder.AppendLine("---------------------------------------------------------------------");
                for (int i = 0; i < trials.Count; i++)
                {
                    FreeFallTrial item = trials[i];
                    builder.AppendLine(
                        $"{item.TrialNumber,3}      {item.HeightMeters,5:0.00}    {item.TimeSeconds,6:0.000}     " +
                        $"{item.TwoHeight,5:0.00}       {item.TimeSquared,6:0.000}        {item.CalculatedGravity,6:0.00}");
                }

                if (trials.Count == 0)
                {
                    builder.AppendLine("No free-fall observations recorded yet.");
                }
                notebookText.text = builder.ToString();
            }

            graphGraphic?.SetPoints(graphPoints);
            if (graphSummaryText != null)
            {
                float slope = CalculateSlope();
                graphSummaryText.text = graphPoints.Count < 2
                    ? "Record at least two heights to calculate g from the slope."
                    : $"Graph: 2h against t^2\nSlope = g = {slope:0.00} m/s^2";
            }
        }

        private float CalculateSlope()
        {
            float numerator = 0f;
            float denominator = 0f;
            for (int i = 0; i < graphPoints.Count; i++)
            {
                numerator += graphPoints[i].x * graphPoints[i].y;
                denominator += graphPoints[i].x * graphPoints[i].x;
            }
            return denominator <= 0.0001f ? 0f : numerator / denominator;
        }

        private void ClearRecords()
        {
            trials.Clear();
            graphPoints.Clear();
            RefreshData();
            UpdateGuidance();
            SetMessage("Experiment 5 records cleared.");
        }

        private void AddGraphPoint()
        {
            if (!float.TryParse(graphXInput.text, out float x) ||
                !float.TryParse(graphYInput.text, out float y))
            {
                graphSummaryText.text = "Enter valid t^2 and 2h values.";
                return;
            }

            graphPoints.Add(new Vector2(x, y));
            graphXInput.text = string.Empty;
            graphYInput.text = string.Empty;
            RefreshData();
        }

        private void ClearGraph()
        {
            graphPoints.Clear();
            RefreshData();
        }

        private void UseLatestTrial()
        {
            if (trials.Count == 0)
            {
                gravityResultText.text = "Complete a free-fall trial first.";
                return;
            }

            FreeFallTrial latest = trials[trials.Count - 1];
            heightInput.text = latest.HeightMeters.ToString("0.000");
            timeInput.text = latest.TimeSeconds.ToString("0.000");
            CalculateGravity();
        }

        private void CalculateGravity()
        {
            if (!float.TryParse(heightInput.text, out float height) ||
                !float.TryParse(timeInput.text, out float time) ||
                time <= 0f)
            {
                gravityResultText.text = "Enter a valid height and time greater than zero.";
                return;
            }

            float gravity = 2f * height / (time * time);
            gravityResultText.text =
                $"g = 2h / t^2\n= (2 x {height:0.000}) / ({time:0.000})^2\n= {gravity:0.00} m/s^2";
        }

        private void UpdateGuidance()
        {
            if (!guided)
            {
                instructionText.text = "Independent Mode: choose a height, release the ball, and inspect the recorded results.";
            }
            else if (trials.Count == 0)
            {
                instructionText.text = "Step 1 of 4: Click the free-fall stand. Select a drop height and press Release Ball.";
            }
            else if (trials.Count < 3)
            {
                instructionText.text = $"Step 2 of 4: Change the drop height and record {3 - trials.Count} more trial(s).";
            }
            else
            {
                instructionText.text = "Step 3 of 4: Open Notebook and Graph. The slope of 2h against t^2 gives g.";
            }
        }

        private void OpenTool(GameObject panel)
        {
            CloseTools();
            panel.SetActive(true);
        }

        private void CloseTools()
        {
            notebookPanel.SetActive(false);
            graphPanel.SetActive(false);
            calculatorPanel.SetActive(false);
            infoPanel.SetActive(false);
        }

        private void HideAll()
        {
            introductionScreen.SetActive(false);
            modeScreen.SetActive(false);
            workspaceRoot.SetActive(false);
            workspaceChrome.SetActive(false);
            instructionBar.SetActive(false);
            CloseTools();
        }

        private void WireButtons()
        {
            Add(introBackButton, ReturnToMainMenu);
            Add(introStartButton, ShowModeScreen);
            Add(modeBackButton, ShowIntroduction);
            Add(guidedButton, () => StartExperiment(true));
            Add(independentButton, () => StartExperiment(false));
            Add(instructionToggleButton, ToggleInstructions);
            Add(homeButton, ReturnToMainMenu);
            Add(notebookButton, () => OpenTool(notebookPanel));
            Add(graphButton, () => OpenTool(graphPanel));
            Add(calculatorButton, () => OpenTool(calculatorPanel));
            Add(infoButton, () => OpenTool(infoPanel));
            Add(notebookCloseButton, CloseTools);
            Add(graphCloseButton, CloseTools);
            Add(calculatorCloseButton, CloseTools);
            Add(infoCloseButton, CloseTools);
            Add(clearRecordsButton, ClearRecords);
            Add(graphAddButton, AddGraphPoint);
            Add(graphClearButton, ClearGraph);
            Add(useLatestButton, UseLatestTrial);
            Add(calculateGravityButton, CalculateGravity);
        }

        private void UpdateToggleText()
        {
            instructionToggleText.text = instructionsVisible ? "Hide Instructions" : "Show Instructions";
        }

        private void SetMessage(string message)
        {
            if (globalMessage != null)
            {
                globalMessage.text = message;
            }
        }

        private void FrameFreeFallApparatus()
        {
            if (sceneCamera == null)
            {
                sceneCamera = Camera.main;
            }

            if (sceneCamera != null)
            {
                sceneCamera.transform.position = new Vector3(0f, 2.55f, -7.4f);
                sceneCamera.transform.rotation = Quaternion.Euler(8f, 0f, 0f);
                sceneCamera.fieldOfView = 48f;
            }
        }

        private static void Add(Button button, UnityEngine.Events.UnityAction action)
        {
            button?.onClick.AddListener(action);
        }
    }
}
