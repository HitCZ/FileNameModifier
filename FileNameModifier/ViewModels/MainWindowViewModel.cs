using System;
using System.Collections.Generic;
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
        private string currentTextToCut = string.Empty;
        private FileInfo currentFile;

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

        public FileInfo CurrentFile
        {
            get => currentFile;
            set
            {
                currentFile = value;
                OnPropertyChanged(nameof(CurrentFile));
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
                var allFilesChecked = false;
                var fileInfos = TryGetDirectoryFiles();
                DeleteOptionDialog dialog;

                foreach (var info in fileInfos)
                {
                    var numberOfOccurrences = Regex.Matches(info.FullName, textToCut).Count;
                    CurrentFile = info;

                    if (fileInfos.Length == 1 && numberOfOccurrences == 0)
                    {
                        ShowNoOccurrencesDialog(info, textToCut);
                        return;
                    }
                    if (numberOfOccurrences > 1)
                    {
                        var occurenceCountList = GetOccurenceCountList(numberOfOccurrences);
                        if (!allFilesChecked)
                        {
                            dialog = ShowDeletionOptionDialog(occurenceCountList);
                            allFilesChecked = dialog.IsAllFilesChecked;
                            dialogConfirmed = dialog.IsConfirmed;
                        }
                        dialogShown = true;
                    }

                    if (!dialogShown)
                        FileNameCutter.CutSpecificOccurrence(info, textToCut, 1);
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

        private List<int> GetOccurenceCountList(int numberOfOccurrences)
        {
            var list = new List<int>(numberOfOccurrences);

            for (var i = 1; i <= numberOfOccurrences; i++)
                list.Add(i);

            return list;
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

        private DeleteOptionDialog ShowDeletionOptionDialog(List<int> occurenceCounts)
        {
            var deletionDialog = new DeleteOptionDialog(occurenceCounts);

            deletionDialog.BeforeClosing += DeletionDialog_BeforeClosing;
            deletionDialog.ShowDialog();

            return deletionDialog;
        }

        private void ShowSuccessDialog(int changedCount, int unchangedCount)
        {
            var strUnchangedCount = unchangedCount == 0 ? "None" : unchangedCount.ToString();

            MessageBox.Show($"Operation was successful.\nNumber of changed files: {changedCount}." +
                            $"\nNumber of unchanged files: {strUnchangedCount}.", "Success", MessageBoxButton.OK,
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
                    FileNameCutter.CutSpecificOccurrence(CurrentFile, currentTextToCut, 1);
                    break;
                case Logic.Enumerations.DeletionOption.RemoveAll:
                    FileNameCutter.CutAllOccurrences(CurrentFile, currentTextToCut);
                    break;
                case Logic.Enumerations.DeletionOption.RemoveSpecific:
                    FileNameCutter.CutSpecificOccurrence(CurrentFile, currentTextToCut, e.SelectedOccurrence);
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
