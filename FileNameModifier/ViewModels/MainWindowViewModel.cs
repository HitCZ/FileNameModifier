using System;
using FileNameModifier.Annotations;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using FileNameModifier.Logic.FileModification;

namespace FileNameModifier.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string selectedPath = string.Empty;

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

            var directoryInfo = new DirectoryInfo(selectedPath);
            var fileInfo = directoryInfo.GetFiles();

            try
            {
                var counter = 0;

                foreach (var info in fileInfo)
                {
                    if (!info.FullName.ToLower().Contains(textToCut.ToLower()))
                        continue;

                    FileNameCutter.CutSubstringFromFileName(info, textToCut);
                    counter++;
                }

                var unchangedFilesCount = fileInfo.Length - counter;

                MessageBox.Show($"Operation was successful.\nNumber of changed files: {counter}." +
                                $"\nNumber of unchanged files: {unchangedFilesCount}.");
            }
            catch(Exception e)
            {
                MessageBox.Show($"Operation wasn't successful.\n{e.Message}");
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
