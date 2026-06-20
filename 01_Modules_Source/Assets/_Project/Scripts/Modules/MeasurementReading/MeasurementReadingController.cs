using TMPro;
using UnityEngine;

namespace OOPLab.Modules.MeasurementReading
{
    public class MeasurementReadingController : MonoBehaviour
    {
        [SerializeField] private TMP_Text instrumentNameText;
        [SerializeField] private TMP_Text readingValueText;
        [SerializeField] private TMP_Text unitText;
        [SerializeField] private TMP_Text precisionText;
        [SerializeField] private TMP_Text detailsText;
        [SerializeField] private TMP_Text statusText;

        private readonly MeasurementReading reading = new MeasurementReading();

        private void Start()
        {
            RefreshDisplay();
        }

        public void NextInstrument()
        {
            int nextIndex = ((int)reading.InstrumentType + 1) % System.Enum.GetValues(typeof(MeasurementInstrumentType)).Length;
            reading.SetInstrument((MeasurementInstrumentType)nextIndex);
            ShowStatus("Instrument changed.");
            RefreshDisplay();
        }

        public void PreviousInstrument()
        {
            int count = System.Enum.GetValues(typeof(MeasurementInstrumentType)).Length;
            int previousIndex = ((int)reading.InstrumentType - 1 + count) % count;
            reading.SetInstrument((MeasurementInstrumentType)previousIndex);
            ShowStatus("Instrument changed.");
            RefreshDisplay();
        }

        public void IncreaseReading()
        {
            reading.Increase();
            ShowStatus("Reading increased.");
            RefreshDisplay();
        }

        public void DecreaseReading()
        {
            reading.Decrease();
            ShowStatus("Reading decreased.");
            RefreshDisplay();
        }

        public void SimulateReading()
        {
            reading.SimulateRandomReading();
            ShowStatus("Simulated live reading.");
            RefreshDisplay();
        }

        public void ResetReading()
        {
            reading.SetInstrument(reading.InstrumentType);
            ShowStatus("Reading reset.");
            RefreshDisplay();
        }

        public void ShowThermometer()
        {
            SetInstrument(MeasurementInstrumentType.Thermometer);
        }

        public void ShowVoltmeter()
        {
            SetInstrument(MeasurementInstrumentType.Voltmeter);
        }

        public void ShowAmmeter()
        {
            SetInstrument(MeasurementInstrumentType.Ammeter);
        }

        public void ShowVernierCaliper()
        {
            SetInstrument(MeasurementInstrumentType.VernierCaliper);
        }

        public void ShowScrewGauge()
        {
            SetInstrument(MeasurementInstrumentType.ScrewGauge);
        }

        private void SetInstrument(MeasurementInstrumentType instrumentType)
        {
            reading.SetInstrument(instrumentType);
            ShowStatus($"{reading.GetInstrumentName()} selected.");
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (instrumentNameText != null)
            {
                instrumentNameText.text = reading.GetInstrumentName();
            }

            if (readingValueText != null)
            {
                readingValueText.text = reading.GetFormattedValue();
            }

            if (unitText != null)
            {
                unitText.text = reading.Unit;
            }

            if (precisionText != null)
            {
                precisionText.text = $"Precision: {reading.DecimalPlaces} decimal place(s)";
            }

            if (detailsText != null)
            {
                detailsText.text = reading.GetModuleDetails();
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
