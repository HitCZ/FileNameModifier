using System;
using System.IO;
using System.Linq;

namespace FileNameModifier.Logic.FileModification
{
    public static class FileNameCutter
    {
        public static void CutFirstOccurrence(FileInfo fileInfo, string textToCut)
        {
            var startIndex = GetStartIndex(fileInfo, textToCut);

            if (startIndex > -1)
                File.Move(fileInfo.FullName, fileInfo.FullName.Remove(startIndex, textToCut.Length));
        }

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

        private static int GetStartIndex(FileInfo fileInfo, string textToCut)
        {
            return fileInfo.FullName.IndexOf(textToCut, StringComparison.OrdinalIgnoreCase);
        }

        private static int GetStartIndex(string name, string textToCut)
        {
            return name.IndexOf(textToCut, StringComparison.OrdinalIgnoreCase);
        }
    }
}
