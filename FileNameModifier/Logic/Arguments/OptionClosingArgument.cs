using System;
using FileNameModifier.Logic.Enumerations;
using System.ComponentModel;

namespace FileNameModifier.Logic.Arguments
{
    public class OptionClosingArgument : EventArgs
    {
        public bool IsConfirmed { get; set; }
        public DeletionOption SelectedOption { get; set; }
    }
}
