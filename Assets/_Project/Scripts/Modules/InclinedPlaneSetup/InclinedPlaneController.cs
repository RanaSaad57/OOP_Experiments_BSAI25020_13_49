using System;
using UnityEngine;
using UnityEngine.UI;

namespace OOPLab.Modules.InclinedPlaneSetup
{
    public class InclinedPlaneController : MonoBehaviour
    {
        [Header("Scene Parts")]
        [SerializeField] private Transform rampPivot;
        [SerializeField] private Transform heightSupport;
        [SerializeField] private Transform ballSpawnPoint;
        [SerializeField] private Rigidbody rollingBall;
        [SerializeField] private PhysicsMaterial rampPhysicsMaterial;

        [Header("Menu")]
        [SerializeField] private GameObject controlPanel;
        [SerializeField] private Button closeButton;

        [Header("Angle Controls")]
        [SerializeField] private Slider angleSlider;
        [SerializeField] private InputField angleInput;
        [SerializeField] private Button angleMinusButton;
        [SerializeField] private Button anglePlusButton;

        [Header("Friction Controls")]
        [SerializeField] private Slider frictionSlider;
        [SerializeField] private InputField frictionInput;
        [SerializeField] private Button frictionMinusButton;
        [SerializeField] private Button frictionPlusButton;

        [Header("Distance Controls")]
        [SerializeField] private Slider distanceSlider;
        [SerializeField] private InputField distanceInput;
        [SerializeField] private Button distanceMinusButton;
        [SerializeField] private Button distancePlusButton;
        [SerializeField] private Text targetDistanceText;

        [Header("Experiment Controls")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button sampleButton;

        [Header("Readouts")]
        [SerializeField] private Text angleText;
        [SerializeField] private Text frictionText;
        [SerializeField] private Text accelerationText;
        [SerializeField] private Text timeText;
        [SerializeField] private Text distanceText;
        [SerializeField] private Text statusText;

        private readonly InclinedPlaneModel model = new InclinedPlaneModel();
        private Quaternion startRotation;
        private bool isRunning;
        private bool isSyncingUi;
        private float elapsedTime;
        private float distanceTravelled;
        private Vector3 runStartPosition;
        private int trialNumber;
        private bool trialReported;
        private const float RampLength = 4.5f;
        private const float LowerEndHeight = 0.18f;

        public event Action<InclinedPlaneTrial> TrialCompleted;

        public float AngleDegrees => model.AngleDegrees;
        public float FrictionCoefficient => model.FrictionCoefficient;
        public float ElapsedTime => elapsedTime;
        public float DistanceTravelled => distanceTravelled;
        public float TargetDistance => model.TravelDistanceMeters;
        public bool IsRunning => isRunning;

        private void Awake()
        {
            if (rollingBall != null)
            {
                startRotation = rollingBall.rotation;
                rollingBall.isKinematic = true;
            }

            SetupControls();
            SetAngle(25f);
            SetFriction(0.12f);
            SetTravelDistance(1.5f);
            ResetExperiment();
            HideMenu();
        }

        private void Update()
        {
            if (!isRunning || rollingBall == null)
            {
                return;
            }

            elapsedTime += Time.deltaTime;
            distanceTravelled = Vector3.Distance(runStartPosition, rollingBall.position);
            UpdateReadouts();

            if (rollingBall.position.y < -1.2f || distanceTravelled >= model.TravelDistanceMeters)
            {
                CompleteTrial();
            }
        }

        public void ShowMenu()
        {
            if (controlPanel != null)
            {
                controlPanel.SetActive(true);
            }
        }

        public void HideMenu()
        {
            if (controlPanel != null)
            {
                controlPanel.SetActive(false);
            }
        }

        public void SetAngle(float angleDegrees)
        {
            model.SetAngle(angleDegrees);

            if (rampPivot != null)
            {
                Vector3 pivotPosition = rampPivot.position;
                pivotPosition.y = LowerEndHeight + Mathf.Sin(model.AngleDegrees * Mathf.Deg2Rad) * RampLength;
                rampPivot.position = pivotPosition;
                rampPivot.localRotation = Quaternion.Euler(0f, 0f, -model.AngleDegrees);
            }

            if (heightSupport != null && rampPivot != null)
            {
                Vector3 supportPosition = heightSupport.position;
                supportPosition.y = rampPivot.position.y * 0.5f - 0.05f;
                heightSupport.position = supportPosition;

                Vector3 supportScale = heightSupport.localScale;
                supportScale.y = Mathf.Max(0.2f, rampPivot.position.y);
                heightSupport.localScale = supportScale;
            }

            PositionBallAtStart();
            SyncUiValues();
            UpdateReadouts();
        }

        public void SetFriction(float frictionCoefficient)
        {
            model.SetFriction(frictionCoefficient);

            if (rampPhysicsMaterial != null)
            {
                rampPhysicsMaterial.dynamicFriction = model.FrictionCoefficient;
                rampPhysicsMaterial.staticFriction = model.FrictionCoefficient;
            }

            SyncUiValues();
            UpdateReadouts();
        }

        public void SetTravelDistance(float distanceMeters)
        {
            model.SetTravelDistance(distanceMeters);
            SyncUiValues();
            UpdateReadouts();
        }

        public void StartRoll()
        {
            if (rollingBall == null)
            {
                SetStatus("Ball is missing.");
                return;
            }

            PositionBallAtStart();
            elapsedTime = 0f;
            distanceTravelled = 0f;
            runStartPosition = rollingBall.position;
            isRunning = true;
            trialReported = false;
            rollingBall.isKinematic = false;
            rollingBall.linearVelocity = Vector3.zero;
            rollingBall.angularVelocity = Vector3.zero;
            SetStatus("Rolling...");
        }

        public void ResetExperiment()
        {
            isRunning = false;
            elapsedTime = 0f;
            distanceTravelled = 0f;
            trialReported = false;
            PositionBallAtStart();
            SetStatus("Ready. Click Start Roll.");
            UpdateReadouts();
        }

        public void LoadSampleExperiment()
        {
            SetAngle(30f);
            SetFriction(0.08f);
            SetTravelDistance(2.5f);
            SetStatus("Sample loaded: 2.5 m low-friction rolling trial.");
        }

        private void SetupControls()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(HideMenu);
            }

            if (angleSlider != null)
            {
                angleSlider.minValue = 0f;
                angleSlider.maxValue = 45f;
                angleSlider.wholeNumbers = false;
                angleSlider.onValueChanged.AddListener(value =>
                {
                    if (!isSyncingUi)
                    {
                        SetAngle(value);
                    }
                });
            }

            if (frictionSlider != null)
            {
                frictionSlider.minValue = 0f;
                frictionSlider.maxValue = 0.8f;
                frictionSlider.wholeNumbers = false;
                frictionSlider.onValueChanged.AddListener(value =>
                {
                    if (!isSyncingUi)
                    {
                        SetFriction(value);
                    }
                });
            }

            if (distanceSlider != null)
            {
                distanceSlider.minValue = 0.5f;
                distanceSlider.maxValue = 4.5f;
                distanceSlider.wholeNumbers = false;
                distanceSlider.onValueChanged.AddListener(value =>
                {
                    if (!isSyncingUi)
                    {
                        SetTravelDistance(value);
                    }
                });
            }

            if (angleInput != null)
            {
                angleInput.onEndEdit.AddListener(value =>
                {
                    if (float.TryParse(value, out float parsed))
                    {
                        SetAngle(parsed);
                    }
                    else
                    {
                        SyncUiValues();
                    }
                });
            }

            if (frictionInput != null)
            {
                frictionInput.onEndEdit.AddListener(value =>
                {
                    if (float.TryParse(value, out float parsed))
                    {
                        SetFriction(parsed);
                    }
                    else
                    {
                        SyncUiValues();
                    }
                });
            }

            if (distanceInput != null)
            {
                distanceInput.onEndEdit.AddListener(value =>
                {
                    if (float.TryParse(value, out float parsed))
                    {
                        SetTravelDistance(parsed);
                    }
                    else
                    {
                        SyncUiValues();
                    }
                });
            }

            if (angleMinusButton != null)
            {
                angleMinusButton.onClick.AddListener(() => SetAngle(model.AngleDegrees - 1f));
            }

            if (anglePlusButton != null)
            {
                anglePlusButton.onClick.AddListener(() => SetAngle(model.AngleDegrees + 1f));
            }

            if (frictionMinusButton != null)
            {
                frictionMinusButton.onClick.AddListener(() => SetFriction(model.FrictionCoefficient - 0.02f));
            }

            if (frictionPlusButton != null)
            {
                frictionPlusButton.onClick.AddListener(() => SetFriction(model.FrictionCoefficient + 0.02f));
            }

            if (distanceMinusButton != null)
            {
                distanceMinusButton.onClick.AddListener(() => SetTravelDistance(model.TravelDistanceMeters - 0.5f));
            }

            if (distancePlusButton != null)
            {
                distancePlusButton.onClick.AddListener(() => SetTravelDistance(model.TravelDistanceMeters + 0.5f));
            }

            if (startButton != null)
            {
                startButton.onClick.AddListener(StartRoll);
            }

            if (resetButton != null)
            {
                resetButton.onClick.AddListener(ResetExperiment);
            }

            if (sampleButton != null)
            {
                sampleButton.onClick.AddListener(LoadSampleExperiment);
            }
        }

        private void SyncUiValues()
        {
            isSyncingUi = true;

            if (angleSlider != null)
            {
                angleSlider.value = model.AngleDegrees;
            }

            if (angleInput != null)
            {
                angleInput.text = model.AngleDegrees.ToString("0.0");
            }

            if (frictionSlider != null)
            {
                frictionSlider.value = model.FrictionCoefficient;
            }

            if (frictionInput != null)
            {
                frictionInput.text = model.FrictionCoefficient.ToString("0.00");
            }

            if (distanceSlider != null)
            {
                distanceSlider.value = model.TravelDistanceMeters;
            }

            if (distanceInput != null)
            {
                distanceInput.text = model.TravelDistanceMeters.ToString("0.0");
            }

            isSyncingUi = false;
        }

        private void PositionBallAtStart()
        {
            if (rollingBall == null || ballSpawnPoint == null)
            {
                return;
            }

            if (!rollingBall.isKinematic)
            {
                rollingBall.linearVelocity = Vector3.zero;
                rollingBall.angularVelocity = Vector3.zero;
            }

            rollingBall.isKinematic = true;
            rollingBall.position = ballSpawnPoint.position;
            rollingBall.rotation = startRotation;
        }

        private void UpdateReadouts()
        {
            if (angleText != null)
            {
                angleText.text = $"Angle: {model.AngleDegrees:0.0} deg";
            }

            if (frictionText != null)
            {
                frictionText.text = $"Friction: {model.FrictionCoefficient:0.00}";
            }

            if (accelerationText != null)
            {
                accelerationText.text = $"Acceleration: {model.CalculateAcceleration():0.00} m/s^2";
            }

            if (timeText != null)
            {
                timeText.text = $"Time: {elapsedTime:0.00} s";
            }

            if (distanceText != null)
            {
                distanceText.text = $"Travelled: {distanceTravelled:0.00} m";
            }

            if (targetDistanceText != null)
            {
                targetDistanceText.text = $"Target distance: {model.TravelDistanceMeters:0.0} m";
            }
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }

        private void CompleteTrial()
        {
            if (trialReported)
            {
                return;
            }

            trialReported = true;
            isRunning = false;
            rollingBall.isKinematic = true;
            trialNumber++;

            float measuredDistance = model.TravelDistanceMeters;
            float safeTime = Mathf.Max(elapsedTime, 0.001f);
            InclinedPlaneTrial trial = new InclinedPlaneTrial
            {
                TrialNumber = trialNumber,
                AngleDegrees = model.AngleDegrees,
                FrictionCoefficient = model.FrictionCoefficient,
                DistanceMeters = measuredDistance,
                TimeSeconds = elapsedTime,
                TwoDistance = 2f * measuredDistance,
                TimeSquared = elapsedTime * elapsedTime,
                CalculatedAcceleration = 2f * measuredDistance / (safeTime * safeTime)
            };

            SetStatus($"Trial {trialNumber} complete. Result recorded.");
            UpdateReadouts();
            TrialCompleted?.Invoke(trial);
        }
    }
}
