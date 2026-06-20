using System;

namespace OOPLab.Modules.MeasurementReading
{
    public class MeasurementReading
    {
        public MeasurementInstrumentType InstrumentType { get; private set; }
        public double Value { get; private set; }
        public string Unit { get; private set; }
        public int DecimalPlaces { get; private set; }
        public double StepSize { get; private set; }

        public MeasurementReading()
        {
            SetInstrument(MeasurementInstrumentType.Thermometer);
        }

        public void SetInstrument(MeasurementInstrumentType instrumentType)
        {
            InstrumentType = instrumentType;

            switch (instrumentType)
            {
                case MeasurementInstrumentType.Thermometer:
                    Unit = "C";
                    DecimalPlaces = 1;
                    StepSize = 0.5;
                    Value = 25.0;
                    break;
                case MeasurementInstrumentType.Voltmeter:
                    Unit = "V";
                    DecimalPlaces = 2;
                    StepSize = 0.1;
                    Value = 1.50;
                    break;
                case MeasurementInstrumentType.Ammeter:
                    Unit = "A";
                    DecimalPlaces = 3;
                    StepSize = 0.01;
                    Value = 0.120;
                    break;
                case MeasurementInstrumentType.VernierCaliper:
                    Unit = "mm";
                    DecimalPlaces = 2;
                    StepSize = 0.05;
                    Value = 12.50;
                    break;
                case MeasurementInstrumentType.ScrewGauge:
                    Unit = "mm";
                    DecimalPlaces = 3;
                    StepSize = 0.005;
                    Value = 1.250;
                    break;
            }
        }

        public void Increase()
        {
            Value += StepSize;
        }

        public void Decrease()
        {
            Value -= StepSize;
            if (Value < 0)
            {
                Value = 0;
            }
        }

        public void SimulateRandomReading()
        {
            Random random = new Random();

            switch (InstrumentType)
            {
                case MeasurementInstrumentType.Thermometer:
                    Value = 20 + random.NextDouble() * 80;
                    break;
                case MeasurementInstrumentType.Voltmeter:
                    Value = random.NextDouble() * 12;
                    break;
                case MeasurementInstrumentType.Ammeter:
                    Value = random.NextDouble() * 2;
                    break;
                case MeasurementInstrumentType.VernierCaliper:
                    Value = random.NextDouble() * 150;
                    break;
                case MeasurementInstrumentType.ScrewGauge:
                    Value = random.NextDouble() * 25;
                    break;
            }
        }

        public string GetInstrumentName()
        {
            switch (InstrumentType)
            {
                case MeasurementInstrumentType.Thermometer:
                    return "Thermometer";
                case MeasurementInstrumentType.Voltmeter:
                    return "Voltmeter";
                case MeasurementInstrumentType.Ammeter:
                    return "Ammeter";
                case MeasurementInstrumentType.VernierCaliper:
                    return "Vernier Caliper";
                case MeasurementInstrumentType.ScrewGauge:
                    return "Screw Gauge";
                default:
                    return "Instrument";
            }
        }

        public string GetFormattedValue()
        {
            return Value.ToString($"F{DecimalPlaces}");
        }

        public string GetModuleDetails()
        {
            switch (InstrumentType)
            {
                case MeasurementInstrumentType.Thermometer:
                    return "Used to measure temperature during heating, cooling, calorimetry, and reaction experiments.";
                case MeasurementInstrumentType.Voltmeter:
                    return "Used to measure potential difference across circuit components. It is connected in parallel.";
                case MeasurementInstrumentType.Ammeter:
                    return "Used to measure electric current flowing through a circuit. It is connected in series.";
                case MeasurementInstrumentType.VernierCaliper:
                    return "Used to measure length, external diameter, internal diameter, and depth with decimal precision.";
                case MeasurementInstrumentType.ScrewGauge:
                    return "Used to measure very small thicknesses and diameters using pitch and circular scale readings.";
                default:
                    return "Displays a measurement reading with unit and decimal precision.";
            }
        }
    }
}
