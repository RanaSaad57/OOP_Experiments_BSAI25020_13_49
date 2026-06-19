using System;
using UnityEngine;
using UnityEngine.UI;

namespace OOPLab.Modules.FreeFallSetup
{
    public class FreeFallController : MonoBehaviour
    {
        [Header("Apparatus")]
        [SerializeField] private Rigidbody fallingBall;
        [SerializeField] private Transform releaseAssembly;
        [SerializeField] private Transform releasePoint;
        [SerializeField] private float landingHeight = 0.18f;

        [Header("Popup")]
        [SerializeField] private GameObject controlPanel;
        [SerializeField] private Button closeButton;
        [SerializeField] private Slider heightSlider;
        [SerializeField] private InputField heightInput;
        [SerializeField] private Button heightMinusButton;
        [SerializeField] private Button heightPlusButton;
        [SerializeField] private Button releaseButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button sampleButton;
        [SerializeField] private Text heightText;
        [SerializeField] private Text timeText;
        [SerializeField] private Text gravityText;
        [SerializeField] private Text statusText;

        private float heightMeters = 2f;
        private float elapsedTime;
        private float previousElapsedTime;
        private float previousBallY;
        private bool isRunning;
        private bool uiSync;
        private int trialNumber;
        private Quaternion ballStartRotation;

        public event Action<FreeFallTrial> TrialCompleted;

        public float HeightMeters => heightMeters;
        public float ElapsedTime => elapsedTime;
        public bool IsRunning => isRunning;

        private void Awake()
        {
            if (fallingBall != null)
            {
                ballStartRotation = fallingBall.rotation;
            }

            WireControls();
            SetHeight(2f);
            ResetTrial();
            HideMenu();
        }

        private void Update()
        {
            if (!isRunning || fallingBall == null)
            {
                return;
            }

            previousElapsedTime = elapsedTime;
            previousBallY = fallingBall.position.y;
            elapsedTime += Time.deltaTime;
            UpdateReadouts();

            float ballRadius = fallingBall.transform.lossyScale.y * 0.5f;
            float impactY = landingHeight + ballRadius;
            if (fallingBall.position.y <= impactY)
            {
                float frameTravel = previousBallY - fallingBall.position.y;
                if (frameTravel > 0.00001f)
                {
                    float impactFraction = Mathf.Clamp01((previousBallY - impactY) / frameTravel);
                    elapsedTime = Mathf.Lerp(previousElapsedTime, elapsedTime, impactFraction);
                }

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

        public void SetHeight(float value)
        {
            heightMeters = Mathf.Clamp(value, 0.5f, 4f);
            if (releaseAssembly != null)
            {
                Vector3 position = releaseAssembly.position;
                float ballRadius = fallingBall != null ? fallingBall.transform.lossyScale.y * 0.5f : 0f;
                float releaseOffset = releasePoint != null ? releasePoint.localPosition.y : 0f;
                position.y = landingHeight + ballRadius + heightMeters - releaseOffset;
                releaseAssembly.position = position;
            }

            ResetBallPosition();
            SyncUi();
            UpdateReadouts();
        }

        public void ReleaseBall()
        {
            if (fallingBall == null)
            {
                SetStatus("Steel ball is missing.");
                return;
            }

            ResetBallPosition();
            elapsedTime = 0f;
            previousElapsedTime = 0f;
            previousBallY = releasePoint != null ? releasePoint.position.y : 0f;
            isRunning = true;
            fallingBall.isKinematic = false;
            fallingBall.linearVelocity = Vector3.zero;
            fallingBall.angularVelocity = Vector3.zero;
            SetStatus("Ball released. Timer running...");
        }

        public void ResetTrial()
        {
            isRunning = false;
            elapsedTime = 0f;
            previousElapsedTime = 0f;
            ResetBallPosition();
            SetStatus("Ready. Select height and release the ball.");
            UpdateReadouts();
        }

        public void LoadSample()
        {
            SetHeight(2.5f);
            SetStatus("Sample height set to 2.5 m.");
        }

        private void CompleteTrial()
        {
            isRunning = false;
            fallingBall.isKinematic = true;
            trialNumber++;
            float safeTime = Mathf.Max(elapsedTime, 0.001f);
            FreeFallTrial trial = new FreeFallTrial
            {
                TrialNumber = trialNumber,
                HeightMeters = heightMeters,
                TimeSeconds = elapsedTime,
                TwoHeight = 2f * heightMeters,
                TimeSquared = elapsedTime * elapsedTime,
                CalculatedGravity = 2f * heightMeters / (safeTime * safeTime)
            };

            UpdateReadouts();
            SetStatus($"Trial {trialNumber} complete and recorded.");
            TrialCompleted?.Invoke(trial);
        }

        private void ResetBallPosition()
        {
            if (fallingBall == null || releasePoint == null)
            {
                return;
            }

            if (!fallingBall.isKinematic)
            {
                fallingBall.linearVelocity = Vector3.zero;
                fallingBall.angularVelocity = Vector3.zero;
            }

            fallingBall.isKinematic = true;
            fallingBall.position = releasePoint.position;
            fallingBall.rotation = ballStartRotation;
        }

        private void WireControls()
        {
            closeButton?.onClick.AddListener(HideMenu);
            releaseButton?.onClick.AddListener(ReleaseBall);
            resetButton?.onClick.AddListener(ResetTrial);
            sampleButton?.onClick.AddListener(LoadSample);
            heightMinusButton?.onClick.AddListener(() => SetHeight(heightMeters - 0.25f));
            heightPlusButton?.onClick.AddListener(() => SetHeight(heightMeters + 0.25f));

            if (heightSlider != null)
            {
                heightSlider.minValue = 0.5f;
                heightSlider.maxValue = 4f;
                heightSlider.onValueChanged.AddListener(value =>
                {
                    if (!uiSync)
                    {
                        SetHeight(value);
                    }
                });
            }

            if (heightInput != null)
            {
                heightInput.onEndEdit.AddListener(value =>
                {
                    if (float.TryParse(value, out float parsed))
                    {
                        SetHeight(parsed);
                    }
                    else
                    {
                        SyncUi();
                    }
                });
            }
        }

        private void SyncUi()
        {
            uiSync = true;
            if (heightSlider != null)
            {
                heightSlider.value = heightMeters;
            }

            if (heightInput != null)
            {
                heightInput.text = heightMeters.ToString("0.00");
            }
            uiSync = false;
        }

        private void UpdateReadouts()
        {
            if (heightText != null)
            {
                heightText.text = $"Drop height: {heightMeters:0.00} m";
            }

            if (timeText != null)
            {
                timeText.text = $"Time: {elapsedTime:0.000} s";
            }

            if (gravityText != null)
            {
                gravityText.text = elapsedTime > 0f
                    ? $"g = {2f * heightMeters / (elapsedTime * elapsedTime):0.00} m/s^2"
                    : "g = -- m/s^2";
            }
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }
    }
}
