using System.Collections.Generic;
using System.Text;
using OOPLab.Modules.InclinedPlaneSetup;
using UnityEngine;
using UnityEngine.UI;

namespace OOPLab.Experiments
{
    public class Experiment4ExperienceController : MonoBehaviour
    {
        [Header("Experiment")]
        [SerializeField] private GameObject workspaceRoot;
        [SerializeField] private InclinedPlaneController inclinedPlane;

        [Header("Navigation Screens")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject experimentIntro;
        [SerializeField] private GameObject modeSelection;
        [SerializeField] private GameObject instructionBar;
        [SerializeField] private GameObject sideToolbar;
        [SerializeField] private Text globalMessage;
        [SerializeField] private Text instructionText;

        [Header("Navigation Buttons")]
        [SerializeField] private Button experiment4Button;
        [SerializeField] private Button experiment5Button;
        [SerializeField] private Button introBackButton;
        [SerializeField] private Button introStartButton;
        [SerializeField] private Button modeBackButton;
        [SerializeField] private Button guidedButton;
        [SerializeField] private Button independentButton;
        [SerializeField] private Button instructionToggleButton;
        [SerializeField] private Text instructionToggleLabel;
        [SerializeField] private Button homeButton;
        [SerializeField] private Experiment5ExperienceController experiment5Experience;

        [Header("Tool Panels")]
        [SerializeField] private GameObject notebookPanel;
        [SerializeField] private GameObject graphPanel;
        [SerializeField] private GameObject calculatorPanel;
        [SerializeField] private GameObject quickInfoPanel;
        [SerializeField] private Button notebookButton;
        [SerializeField] private Button graphButton;
        [SerializeField] private Button calculatorButton;
        [SerializeField] private Button quickInfoButton;
        [SerializeField] private Button notebookCloseButton;
        [SerializeField] private Button graphCloseButton;
        [SerializeField] private Button calculatorCloseButton;
        [SerializeField] private Button quickInfoCloseButton;

        [Header("Notebook")]
        [SerializeField] private Text notebookText;
        [SerializeField] private Text trialCountText;
        [SerializeField] private Button clearRecordsButton;

        [Header("Graph")]
        [SerializeField] private Experiment4GraphGraphic graphGraphic;
        [SerializeField] private Text graphSummaryText;
        [SerializeField] private InputField graphXInput;
        [SerializeField] private InputField graphYInput;
        [SerializeField] private Button graphAddPointButton;
        [SerializeField] private Button graphClearButton;

        [Header("Calculator")]
        [SerializeField] private InputField distanceInput;
        [SerializeField] private InputField timeInput;
        [SerializeField] private Text calculatorResult;
        [SerializeField] private Button calculateButton;
        [SerializeField] private Button useLatestTrialButton;
        [SerializeField] private InputField manualFirstInput;
        [SerializeField] private InputField manualSecondInput;
        [SerializeField] private Text manualCalculatorResult;
        [SerializeField] private Button addButton;
        [SerializeField] private Button subtractButton;
        [SerializeField] private Button multiplyButton;
        [SerializeField] private Button divideButton;
        [SerializeField] private Button powerButton;
        [SerializeField] private Button squareButton;
        [SerializeField] private Button squareRootButton;
        [SerializeField] private Button sinButton;
        [SerializeField] private Button cosButton;
        [SerializeField] private Button tanButton;
        [SerializeField] private Button logButton;
        [SerializeField] private Button calculatorClearButton;

        private readonly List<InclinedPlaneTrial> trials = new List<InclinedPlaneTrial>();
        private readonly List<Vector2> graphPoints = new List<Vector2>();
        private bool guidedMode;
        private bool instructionsVisible = true;

        private void Awake()
        {
            WireButtons();

            if (inclinedPlane != null)
            {
                inclinedPlane.TrialCompleted += RecordTrial;
            }

            ShowMainMenu();
            RefreshRecords();
        }

        private void OnDestroy()
        {
            if (inclinedPlane != null)
            {
                inclinedPlane.TrialCompleted -= RecordTrial;
            }
        }

        public void ShowMainMenu()
        {
            CloseAllTools();
            SetActive(mainMenu, true);
            SetActive(experimentIntro, false);
            SetActive(modeSelection, false);
            SetActive(workspaceRoot, false);
            SetActive(instructionBar, false);
            SetActive(sideToolbar, false);
            SetMessage("Choose an experiment to begin.");
        }

        private void ShowExperiment4Intro()
        {
            SetActive(mainMenu, false);
            SetActive(experimentIntro, true);
            SetActive(modeSelection, false);
            SetMessage(string.Empty);
        }

        private void ShowModeSelection()
        {
            SetActive(experimentIntro, false);
            SetActive(modeSelection, true);
        }

        private void LaunchExperiment(bool useGuidance)
        {
            guidedMode = useGuidance;
            instructionsVisible = useGuidance;
            SetActive(mainMenu, false);
            SetActive(experimentIntro, false);
            SetActive(modeSelection, false);
            SetActive(workspaceRoot, true);
            SetActive(sideToolbar, true);
            SetActive(instructionBar, instructionsVisible);
            UpdateInstructionToggleLabel();
            CloseAllTools();

            if (inclinedPlane != null)
            {
                inclinedPlane.ResetExperiment();
            }

            FrameInclinedPlane();
            UpdateGuidance();
            SetMessage(useGuidance
                ? "Guided Practice started. Follow the instruction at the top."
                : "Independent Mode started. Instructions are available when needed.");
        }

        private void ToggleInstructions()
        {
            instructionsVisible = !instructionsVisible;
            SetActive(instructionBar, instructionsVisible);
            UpdateInstructionToggleLabel();
        }

        private void RecordTrial(InclinedPlaneTrial trial)
        {
            trials.Add(trial);
            graphPoints.Add(new Vector2(trial.TimeSquared, trial.TwoDistance));
            RefreshRecords();
            UpdateGuidance();
            SetMessage($"Trial {trial.TrialNumber} recorded: t = {trial.TimeSeconds:0.000} s, 2s = {trial.TwoDistance:0.00} m.");
        }

        private void RefreshRecords()
        {
            if (trialCountText != null)
            {
                trialCountText.text = $"Recorded trials: {trials.Count}";
            }

            if (notebookText != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("Trial   Angle   Friction   s (m)   t (s)   2s (m)   t^2 (s^2)   a (m/s^2)");
                builder.AppendLine("--------------------------------------------------------------------------------");
                for (int i = 0; i < trials.Count; i++)
                {
                    InclinedPlaneTrial item = trials[i];
                    builder.AppendLine(
                        $"{item.TrialNumber,3}     {item.AngleDegrees,5:0.0}     {item.FrictionCoefficient,5:0.00}     " +
                        $"{item.DistanceMeters,5:0.00}   {item.TimeSeconds,5:0.000}    {item.TwoDistance,5:0.00}      " +
                        $"{item.TimeSquared,6:0.000}       {item.CalculatedAcceleration,6:0.00}");
                }

                if (trials.Count == 0)
                {
                    builder.AppendLine("No observations recorded yet.");
                }

                notebookText.text = builder.ToString();
            }

            if (graphGraphic != null)
            {
                graphGraphic.SetPoints(graphPoints);
            }

            if (graphSummaryText != null)
            {
                float slope = CalculateSlopeThroughOrigin();
                graphSummaryText.text = trials.Count < 2
                    ? "Record at least two trials to calculate the graph slope."
                    : $"Graph: 2s against t^2\nSlope = acceleration = {slope:0.00} m/s^2";
            }
        }

        private float CalculateSlopeThroughOrigin()
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
            RefreshRecords();
            UpdateGuidance();
            SetMessage("All Experiment 4 records cleared.");
        }

        private void AddManualGraphPoint()
        {
            if (!float.TryParse(graphXInput.text, out float x) ||
                !float.TryParse(graphYInput.text, out float y))
            {
                graphSummaryText.text = "Enter valid X and Y values.";
                return;
            }

            graphPoints.Add(new Vector2(x, y));
            graphXInput.text = string.Empty;
            graphYInput.text = string.Empty;
            RefreshRecords();
            SetMessage($"Manual graph point ({x:0.###}, {y:0.###}) added.");
        }

        private void ClearGraph()
        {
            graphPoints.Clear();
            RefreshRecords();
            SetMessage("Graph points cleared. Notebook records were kept.");
        }

        private void UseLatestTrial()
        {
            if (trials.Count == 0)
            {
                if (calculatorResult != null)
                {
                    calculatorResult.text = "Complete a trial first.";
                }
                return;
            }

            InclinedPlaneTrial latest = trials[trials.Count - 1];
            distanceInput.text = latest.DistanceMeters.ToString("0.000");
            timeInput.text = latest.TimeSeconds.ToString("0.000");
            CalculateAcceleration();
        }

        private void CalculateAcceleration()
        {
            if (!float.TryParse(distanceInput.text, out float distance) ||
                !float.TryParse(timeInput.text, out float time) ||
                time <= 0f)
            {
                calculatorResult.text = "Enter a valid distance and a time greater than zero.";
                return;
            }

            float acceleration = 2f * distance / (time * time);
            calculatorResult.text = $"a = 2s / t^2\n= (2 x {distance:0.000}) / ({time:0.000})^2\n= {acceleration:0.00} m/s^2";
        }

        private void RunBinaryCalculator(System.Func<double, double, double> operation)
        {
            if (!TryReadManualNumber(manualFirstInput, "Enter the first number.", out double first) ||
                !TryReadManualNumber(manualSecondInput, "Enter the second number.", out double second))
            {
                return;
            }

            ShowManualResult(operation, first, second);
        }

        private void RunSingleCalculator(System.Func<double, double> operation)
        {
            if (!TryReadManualNumber(manualFirstInput, "Enter a number in the first box.", out double value))
            {
                return;
            }

            try
            {
                double result = operation(value);
                SetManualResult(result);
            }
            catch (System.Exception exception)
            {
                manualCalculatorResult.text = exception.Message;
            }
        }

        private bool TryReadManualNumber(InputField input, string message, out double value)
        {
            if (input == null || !double.TryParse(input.text, out value))
            {
                manualCalculatorResult.text = message;
                value = 0d;
                return false;
            }

            return true;
        }

        private void ShowManualResult(System.Func<double, double, double> operation, double first, double second)
        {
            try
            {
                SetManualResult(operation(first, second));
            }
            catch (System.Exception exception)
            {
                manualCalculatorResult.text = exception.Message;
            }
        }

        private void SetManualResult(double result)
        {
            if (double.IsNaN(result) || double.IsInfinity(result))
            {
                manualCalculatorResult.text = "The result is undefined.";
                return;
            }

            string formatted = result.ToString("0.####");
            manualCalculatorResult.text = $"Result: {formatted}";
            manualFirstInput.text = formatted;
        }

        private void ClearManualCalculator()
        {
            manualFirstInput.text = string.Empty;
            manualSecondInput.text = string.Empty;
            manualCalculatorResult.text = "Result: 0";
        }

        private void UpdateGuidance()
        {
            if (instructionText == null)
            {
                return;
            }

            if (!guidedMode)
            {
                instructionText.text = "Independent Mode: adjust the plane, run trials, then inspect the notebook and graph.";
                return;
            }

            if (trials.Count == 0)
            {
                instructionText.text = "Step 1 of 4: Click the plane. Keep angle/friction fixed, choose a target distance, then press Start Roll.";
            }
            else if (trials.Count < 3)
            {
                instructionText.text = $"Step 2 of 4: Trial {trials.Count} is saved. Change only the target distance and record {3 - trials.Count} more trial(s).";
            }
            else
            {
                instructionText.text = "Step 3 of 4: Open the Notebook to inspect values, then open Graph to find acceleration from the slope.";
            }
        }

        private void OpenTool(GameObject panel)
        {
            CloseAllTools();
            SetActive(panel, true);
        }

        private void CloseAllTools()
        {
            SetActive(notebookPanel, false);
            SetActive(graphPanel, false);
            SetActive(calculatorPanel, false);
            SetActive(quickInfoPanel, false);
        }

        private void WireButtons()
        {
            Add(experiment4Button, ShowExperiment4Intro);
            Add(experiment5Button, () =>
            {
                if (experiment5Experience != null)
                {
                    experiment5Experience.ShowIntroduction();
                }
                else
                {
                    SetMessage("Experiment 5 is not connected.");
                }
            });
            Add(introBackButton, ShowMainMenu);
            Add(introStartButton, ShowModeSelection);
            Add(modeBackButton, ShowExperiment4Intro);
            Add(guidedButton, () => LaunchExperiment(true));
            Add(independentButton, () => LaunchExperiment(false));
            Add(instructionToggleButton, ToggleInstructions);
            Add(homeButton, ShowMainMenu);
            Add(notebookButton, () => OpenTool(notebookPanel));
            Add(graphButton, () => OpenTool(graphPanel));
            Add(calculatorButton, () => OpenTool(calculatorPanel));
            Add(quickInfoButton, () => OpenTool(quickInfoPanel));
            Add(notebookCloseButton, CloseAllTools);
            Add(graphCloseButton, CloseAllTools);
            Add(calculatorCloseButton, CloseAllTools);
            Add(quickInfoCloseButton, CloseAllTools);
            Add(clearRecordsButton, ClearRecords);
            Add(graphAddPointButton, AddManualGraphPoint);
            Add(graphClearButton, ClearGraph);
            Add(calculateButton, CalculateAcceleration);
            Add(useLatestTrialButton, UseLatestTrial);
            Add(addButton, () => RunBinaryCalculator((a, b) => a + b));
            Add(subtractButton, () => RunBinaryCalculator((a, b) => a - b));
            Add(multiplyButton, () => RunBinaryCalculator((a, b) => a * b));
            Add(divideButton, () => RunBinaryCalculator((a, b) =>
            {
                if (System.Math.Abs(b) < 0.0000001d)
                {
                    throw new System.DivideByZeroException("Cannot divide by zero.");
                }
                return a / b;
            }));
            Add(powerButton, () => RunBinaryCalculator(System.Math.Pow));
            Add(squareButton, () => RunSingleCalculator(a => a * a));
            Add(squareRootButton, () => RunSingleCalculator(a =>
            {
                if (a < 0d)
                {
                    throw new System.ArgumentException("Square root requires a non-negative number.");
                }
                return System.Math.Sqrt(a);
            }));
            Add(sinButton, () => RunSingleCalculator(a => System.Math.Sin(a * System.Math.PI / 180d)));
            Add(cosButton, () => RunSingleCalculator(a => System.Math.Cos(a * System.Math.PI / 180d)));
            Add(tanButton, () => RunSingleCalculator(a => System.Math.Tan(a * System.Math.PI / 180d)));
            Add(logButton, () => RunSingleCalculator(a =>
            {
                if (a <= 0d)
                {
                    throw new System.ArgumentException("Log requires a positive number.");
                }
                return System.Math.Log10(a);
            }));
            Add(calculatorClearButton, ClearManualCalculator);
        }

        private void UpdateInstructionToggleLabel()
        {
            if (instructionToggleLabel != null)
            {
                instructionToggleLabel.text = instructionsVisible ? "Hide Instructions" : "Show Instructions";
            }
        }

        private void SetMessage(string message)
        {
            if (globalMessage != null)
            {
                globalMessage.text = message;
            }
        }

        private static void FrameInclinedPlane()
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                camera.transform.position = new Vector3(1.6f, 3.3f, -6.2f);
                camera.transform.rotation = Quaternion.Euler(29f, -11f, 0f);
                camera.fieldOfView = 50f;
            }
        }

        private static void SetActive(GameObject target, bool active)
        {
            if (target != null)
            {
                target.SetActive(active);
            }
        }

        private static void Add(Button button, UnityEngine.Events.UnityAction action)
        {
            if (button != null)
            {
                button.onClick.AddListener(action);
            }
        }
    }
}
