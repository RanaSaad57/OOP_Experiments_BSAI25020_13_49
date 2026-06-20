using System;

namespace OOPLab.Modules.DataRecordingNotebook
{
    [Serializable]
    public class NotebookRecord
    {
        public string ExperimentTitle { get; private set; }
        public string Observation { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public NotebookRecord(string experimentTitle, string observation)
        {
            ExperimentTitle = experimentTitle;
            Observation = observation;
            CreatedAt = DateTime.Now;
        }

        public string ToDisplayText(int recordNumber)
        {
            return $"{recordNumber}. {ExperimentTitle}\nObservation: {Observation}\nTime: {CreatedAt:hh:mm tt}";
        }
    }
}
