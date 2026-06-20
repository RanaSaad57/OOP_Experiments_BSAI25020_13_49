using TMPro;
using UnityEngine;

namespace OOPLab.Modules.WeighingBalance
{
    public class WeighingBalanceController : MonoBehaviour
    {
        [SerializeField] private GameObject balanceUIPanel;
        [SerializeField] private TMP_InputField sampleNameInput;
        [SerializeField] private TMP_InputField massInput;
        [SerializeField] private TMP_Text sampleText;
        [SerializeField] private TMP_Text gramsText;
        [SerializeField] private TMP_Text kilogramsText;
        [SerializeField] private TMP_Text statusText;

        private readonly WeighingBalance balance = new WeighingBalance();
        private readonly string[] sampleNames = { "Salt", "Metal cube", "Stone", "Chemical sample", "Glass piece" };

        private void Start()
        {
            RefreshDisplay();
            ShowStatus("Ready.");
            HidePanel();
        }

        public void ShowPanel()
        {
            if (balanceUIPanel != null)
            {
                balanceUIPanel.SetActive(true);
            }
        }

        public void HidePanel()
        {
            if (balanceUIPanel != null)
            {
                balanceUIPanel.SetActive(false);
            }
        }

        public void SetMassFromInput()
        {
            if (massInput == null || !float.TryParse(massInput.text, out float mass))
            {
                ShowStatus("Enter a valid mass in grams.");
                return;
            }

            string sampleName = sampleNameInput != null ? sampleNameInput.text.Trim() : "Sample";
            balance.SetSample(sampleName, mass);
            RefreshDisplay();
            ShowStatus("Mass reading updated.");
        }

        public void SetObjectOnBalance(string sampleName, float massInGrams)
        {
            balance.SetSample(sampleName, massInGrams);
            RefreshDisplay();
            ShowStatus("Object placed on balance.");
        }

        public void SimulateSample()
        {
            float simulatedMass = Random.Range(10f, 500f);
            string sampleName = sampleNames[Random.Range(0, sampleNames.Length)];
            balance.SetSample(sampleName, simulatedMass);
            RefreshDisplay();
            ShowStatus("Sample placed on balance.");
        }

        public void Tare()
        {
            balance.Tare();
            RefreshDisplay();
            ShowStatus("Balance tared to zero.");
        }

        public void ResetBalance()
        {
            balance.Reset();

            if (sampleNameInput != null)
            {
                sampleNameInput.text = string.Empty;
            }

            if (massInput != null)
            {
                massInput.text = string.Empty;
            }

            RefreshDisplay();
            ShowStatus("Balance reset.");
        }

        private void RefreshDisplay()
        {
            if (sampleText != null)
            {
                sampleText.text = $"Sample: {balance.SampleName}";
            }

            if (gramsText != null)
            {
                gramsText.text = $"{balance.DisplayMassInGrams:0.00} g";
            }

            if (kilogramsText != null)
            {
                kilogramsText.text = $"{balance.DisplayMassInKilograms:0.000} kg";
            }
        }

        private void ShowStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }
    }
}
