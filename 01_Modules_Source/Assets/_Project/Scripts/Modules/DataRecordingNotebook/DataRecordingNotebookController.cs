using TMPro;
using UnityEngine;

namespace OOPLab.Modules.DataRecordingNotebook
{
    public class DataRecordingNotebookController : MonoBehaviour
    {
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField experimentTitleInput;
        [SerializeField] private TMP_InputField observationInput;

        [Header("Output")]
        [SerializeField] private TMP_Text recordsDisplayText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_Text pageText;

        private readonly DataNotebook notebook = new DataNotebook();
        private const int MaxCharactersPerPage = 520;
        private int currentPageIndex;

        private void Start()
        {
            RefreshDisplay();
            ShowStatus("Ready to record observations.");
        }

        public void AddRecord()
        {
            string experimentTitle = experimentTitleInput != null ? experimentTitleInput.text.Trim() : string.Empty;
            string observation = observationInput != null ? observationInput.text.Trim() : string.Empty;

            if (string.IsNullOrWhiteSpace(experimentTitle))
            {
                ShowStatus("Enter an experiment title.");
                return;
            }

            if (string.IsNullOrWhiteSpace(observation))
            {
                ShowStatus("Write an observation.");
                return;
            }

            notebook.AddRecord(experimentTitle, observation);
            currentPageIndex = notebook.GetPageCount(MaxCharactersPerPage) - 1;

            if (experimentTitleInput != null)
            {
                experimentTitleInput.text = string.Empty;
            }

            if (observationInput != null)
            {
                observationInput.text = string.Empty;
            }

            RefreshDisplay();
            ShowStatus($"Record saved. Total records: {notebook.RecordCount}");
        }

        public void ClearRecords()
        {
            notebook.Clear();
            currentPageIndex = 0;
            RefreshDisplay();
            ShowStatus("All records cleared.");
        }

        public void NextPage()
        {
            int pageCount = notebook.GetPageCount(MaxCharactersPerPage);
            if (currentPageIndex < pageCount - 1)
            {
                currentPageIndex++;
                RefreshDisplay();
                ShowStatus("Showing next page.");
            }
        }

        public void PreviousPage()
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                RefreshDisplay();
                ShowStatus("Showing previous page.");
            }
        }

        private void RefreshDisplay()
        {
            int pageCount = notebook.GetPageCount(MaxCharactersPerPage);
            currentPageIndex = Mathf.Clamp(currentPageIndex, 0, pageCount - 1);

            if (recordsDisplayText != null)
            {
                recordsDisplayText.text = notebook.GetPageDisplayText(currentPageIndex, MaxCharactersPerPage);
            }

            if (pageText != null)
            {
                pageText.text = $"Page {currentPageIndex + 1} / {pageCount}";
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
