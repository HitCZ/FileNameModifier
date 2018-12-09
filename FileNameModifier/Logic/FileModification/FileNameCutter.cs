using System;
using System.IO;

namespace FileNameModifier.Logic.FileModification
{
    public static class FileNameCutter
    {
        public static void CutSubstringFromFileName(FileInfo fileInfo, string textToCut)
        {
            var startIndex = fileInfo.FullName.IndexOf(textToCut, StringComparison.OrdinalIgnoreCase);

            if (startIndex > -1)
                File.Move(fileInfo.FullName, fileInfo.FullName.Remove(startIndex, textToCut.Length));
        }
    }
}
