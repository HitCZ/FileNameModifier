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

            try
            {
                var counter = 0;
                var dialogConfirmed = false;
                var dialogShown = false;
                var fileInfos = TryGetDirectoryFiles();

                foreach (var info in fileInfos)
                {
                    var numberOfOccurrences = Regex.Matches(info.FullName, textToCut).Count;
                    currentFile = info;

                    if (fileInfos.Length == 1 && numberOfOccurrences == 0)
                    {
                        ShowNoOccurrencesDialog(info, textToCut);
                        return;
                    }
                    if (!info.FullName.ToLower().Contains(textToCut.ToLower()))
                        continue;
                    if (numberOfOccurrences > 1)
                    {
                        dialogConfirmed = ShowDeletionOptionDialog();
                        dialogShown = true;
                    }

                    if (!dialogShown)
                        FileNameCutter.CutFirstOccurrence(info, textToCut);
                    counter++;
                }

                if ((!dialogShown || !dialogConfirmed) && dialogShown)
                    return;

                var unchangedFilesCount = fileInfos.Length - counter;
                ShowSuccessDialog(counter, unchangedFilesCount);
            }
            catch (Exception e)
            {
                ShowFailDialog(e.Message);
            }
        }

        private FileInfo[] TryGetDirectoryFiles()
        {
            var directoryInfo = new DirectoryInfo(selectedPath);
            if (!directoryInfo.Exists)
            {
                ShowFileNotFoundDialog();
                return null;
            }

            var fileInfos = directoryInfo.GetFiles();

            return fileInfos;
        }

        #endregion Public Methods

        #region Dialogs

        private bool ShowDeletionOptionDialog()
        {
            var deletionDialog = new DeleteOptionDialog();

            deletionDialog.BeforeClosing += DeletionDialog_BeforeClosing;
            deletionDialog.ShowDialog();

            return deletionDialog.IsConfirmed;
        }

        private void ShowSuccessDialog(int changedCount, int unchangedCount)
        {
            MessageBox.Show($"Operation was successful.\nNumber of changed files: {changedCount}." +
                            $"\nNumber of unchanged files: {unchangedCount}.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void ShowFailDialog(string errorMessage)
        {
            MessageBox.Show($"Operation wasn't successful.\n\nError message: {errorMessage}", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowFileNotFoundDialog()
        {
            MessageBox.Show($"The path \"{selectedPath}\" could not be found.", "File not found", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowNoOccurrencesDialog(FileInfo fileInfo, string textToCut)
        {
            MessageBox.Show(
                $"There were no occurrences of \"{textToCut}\" found in the filename \"{fileInfo.FullName}\".", 
                "No occurrence", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        #endregion Dialogs

        #region Event Handlers

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

        #endregion Event Handlers

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
