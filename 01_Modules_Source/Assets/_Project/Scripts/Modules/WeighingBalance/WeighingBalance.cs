namespace OOPLab.Modules.WeighingBalance
{
    public class WeighingBalance
    {
        public string SampleName { get; private set; }
        public float MassInGrams { get; private set; }
        public float TareInGrams { get; private set; }

        public float DisplayMassInGrams => MassInGrams - TareInGrams;
        public float DisplayMassInKilograms => DisplayMassInGrams / 1000f;

        public WeighingBalance()
        {
            Reset();
        }

        public void SetSample(string sampleName, float massInGrams)
        {
            SampleName = string.IsNullOrWhiteSpace(sampleName) ? "Unknown Sample" : sampleName;
            MassInGrams = massInGrams < 0 ? 0 : massInGrams;
        }

        public void Tare()
        {
            TareInGrams = MassInGrams;
        }

        public void Reset()
        {
            SampleName = "No sample";
            MassInGrams = 0;
            TareInGrams = 0;
        }
    }
}
