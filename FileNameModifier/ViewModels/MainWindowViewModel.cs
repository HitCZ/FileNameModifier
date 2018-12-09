using System;
using FileNameModifier.Annotations;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using FileNameModifier.Logic.Arguments;
using FileNameModifier.Logic.FileModification;
using FileNameModifier.Views;

namespace FileNameModifier.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string selectedPath = string.Empty;
        private FileInfo currentFile;
        private string currentTextToCut = string.Empty;

        #endregion Fields

        #region Properties

        public string SelectedPath
        {
            get => selectedPath;
            set
            {
                selectedPath = value;
                OnPropertyChanged(nameof(SelectedPath));
            }
        }

        #endregion Properties

        #region Public Methods

        public void CutText(string textToCut)
        {
            if (selectedPath == string.Empty)
                return;

            currentTextToCut = textToCut;

            var directoryInfo = new DirectoryInfo(selectedPath);

            if (!directoryInfo.Exists)
            {
                MessageBox.Show($"The path \"{selectedPath}\" could not be found.");
                return;
            }

            var fileInfo = directoryInfo.GetFiles();

            try
            {
                var counter = 0;
                var dialogConfirmed = false;
                var dialogShown = false;

                foreach (var info in fileInfo)
                {
                    if (!info.FullName.ToLower().Contains(textToCut.ToLower()))
                        continue;
                    var numberOfOccurences = Regex.Matches(info.FullName, textToCut).Count;
                    this.currentFile = info;

                    if (numberOfOccurences > 1)
                    {
                        var deletionDialog = new DeleteOptionDialog();

                        deletionDialog.BeforeClosing += DeletionDialog_BeforeClosing;
                        var result = deletionDialog.ShowDialog();
                        dialogShown = true;

                        if (!(result is null))
                            dialogConfirmed = !(bool)result;
                    }

                    if (!dialogShown)
                        FileNameCutter.CutFirstOccurrence(info, textToCut);
                    counter++;
                }

                if ((!dialogShown || !dialogConfirmed) && dialogShown)
                    return;
                var unchangedFilesCount = fileInfo.Length - counter;

                MessageBox.Show($"Operation was successful.\nNumber of changed files: {counter}." +
                                $"\nNumber of unchanged files: {unchangedFilesCount}.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Operation wasn't successful.\n{e.Message}");
            }
        }

        private void DeletionDialog_BeforeClosing(object sender, OptionClosingArgument e)
        {
            if (!e.IsConfirmed)
                return;
            switch (e.SelectedOption)
            {
                case Logic.Enumerations.DeletionOption.RemoveFirst:
                    FileNameCutter.CutFirstOccurrence(currentFile, currentTextToCut);
                    break;
                case Logic.Enumerations.DeletionOption.RemoveAll:
                    FileNameCutter.CutAllOccurrences(currentFile, currentTextToCut);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion Public Methods

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

    }
}
