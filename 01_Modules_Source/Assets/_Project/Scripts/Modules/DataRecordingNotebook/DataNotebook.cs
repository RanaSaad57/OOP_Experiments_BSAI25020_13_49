using System.Collections.Generic;
using System.Text;

namespace OOPLab.Modules.DataRecordingNotebook
{
    public class DataNotebook
    {
        private readonly List<NotebookRecord> records = new List<NotebookRecord>();

        public int RecordCount => records.Count;

        public void AddRecord(string experimentTitle, string observation)
        {
            records.Add(new NotebookRecord(experimentTitle, observation));
        }

        public void Clear()
        {
            records.Clear();
        }

        public int GetPageCount(int maxCharactersPerPage)
        {
            return BuildPages(maxCharactersPerPage).Count;
        }

        public string GetPageDisplayText(int pageIndex, int maxCharactersPerPage)
        {
            List<string> pages = BuildPages(maxCharactersPerPage);
            int safePageIndex = System.Math.Max(0, System.Math.Min(pageIndex, pages.Count - 1));
            return pages[safePageIndex];
        }

        private List<string> BuildPages(int maxCharactersPerPage)
        {
            List<string> pages = new List<string>();

            if (records.Count == 0)
            {
                pages.Add("No records yet.");
                return pages;
            }

            StringBuilder currentPage = new StringBuilder();

            for (int i = 0; i < records.Count; i++)
            {
                string recordText = records[i].ToDisplayText(i + 1);

                if (currentPage.Length > 0 && currentPage.Length + recordText.Length > maxCharactersPerPage)
                {
                    pages.Add(currentPage.ToString());
                    currentPage.Clear();
                }

                if (recordText.Length <= maxCharactersPerPage)
                {
                    currentPage.AppendLine(recordText);
                    currentPage.AppendLine();
                    continue;
                }

                if (currentPage.Length > 0)
                {
                    pages.Add(currentPage.ToString());
                    currentPage.Clear();
                }

                foreach (string chunk in SplitLongRecord(recordText, maxCharactersPerPage))
                {
                    pages.Add(chunk);
                }
            }

            if (currentPage.Length > 0)
            {
                pages.Add(currentPage.ToString());
            }

            return pages;
        }

        private IEnumerable<string> SplitLongRecord(string recordText, int maxCharactersPerPage)
        {
            int startIndex = 0;
            int partNumber = 1;

            while (startIndex < recordText.Length)
            {
                int length = System.Math.Min(maxCharactersPerPage, recordText.Length - startIndex);
                int endIndex = startIndex + length;

                if (endIndex < recordText.Length)
                {
                    int lastSpace = recordText.LastIndexOf(' ', endIndex - 1, length);
                    if (lastSpace > startIndex + 80)
                    {
                        endIndex = lastSpace;
                    }
                }

                string heading = partNumber == 1 ? string.Empty : "(continued)\n";
                yield return heading + recordText.Substring(startIndex, endIndex - startIndex).Trim();

                startIndex = endIndex;
                partNumber++;
            }
        }
    }
}
