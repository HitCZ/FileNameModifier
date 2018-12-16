using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileNameModifier.Logic.FileModification
{
    public static class FileNameCutter
    {
        #region Public Methods

        /// <summary>
        /// Cuts the first occurrence of given text in the name of the file.
        /// </summary>
        public static void CutSpecificOccurrence(FileInfo fileInfo, string textToCut, int numberOfOccurrence)
        {
            var indexOfOccurrence = numberOfOccurrence - 1;
            var indexes = GetIndexesOfSubstring(fileInfo.FullName, textToCut);

            if (!indexes.Any() || numberOfOccurrence > indexes.Count || numberOfOccurrence < 1)
                return;

            var startIndex = indexes[indexOfOccurrence];
            File.Move(fileInfo.FullName, fileInfo.FullName.Remove(startIndex, textToCut.Length));
        }

        /// <summary>
        /// Cuts all occurrences from the given text in the name of the file.
        /// </summary>
        public static void CutAllOccurrences(FileInfo fileInfo, string textToCut)
        {
            var startIndex = GetStartIndex(fileInfo, textToCut);
            var newFileName = fileInfo.FullName;

            while (startIndex > -1)
            {
                newFileName = newFileName.Remove(startIndex, textToCut.Length);
                startIndex = GetStartIndex(newFileName, textToCut);
            }
            File.Move(fileInfo.FullName, fileInfo.FullName.Replace(fileInfo.FullName, newFileName));
        }

        #endregion Public Methods

        #region Private Methods

        private static int GetStartIndex(FileInfo fileInfo, string textToCut)
        {
            return fileInfo.FullName.IndexOf(textToCut, StringComparison.OrdinalIgnoreCase);
        }

        private static int GetStartIndex(string name, string textToCut)
        {
            return name.IndexOf(textToCut, StringComparison.OrdinalIgnoreCase);
        }

        private static List<int> GetIndexesOfSubstring(string fullText, string substring)
        {
            var list = new List<int>();
            var strBuilder = new StringBuilder(fullText);

            while (strBuilder.ToString().Contains(substring))
            {
                var index = strBuilder.ToString().IndexOf(substring, StringComparison.OrdinalIgnoreCase);
                list.Add(index);
                strBuilder.Remove(index, substring.Length);
            }

            return list;
        }
        #endregion Private Methods
    }
}
