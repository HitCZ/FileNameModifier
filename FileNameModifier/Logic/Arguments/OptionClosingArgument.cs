using FileNameModifier.Logic.Enumerations;
using System;

namespace FileNameModifier.Logic.Arguments
{
    public class OptionClosingArgument : EventArgs
    {
        public bool IsConfirmed { get; set; }
        public DeletionOption SelectedOption { get; set; }
        public int SelectedOccurrence { get; set; }
        public bool IsAllFilesChecked { get; set; }
    }
}
