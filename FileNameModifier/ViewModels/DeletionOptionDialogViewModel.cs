using System.Collections.Generic;
using FileNameModifier.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FileNameModifier.Logic.Enumerations;

namespace FileNameModifier.ViewModels
{
    public class DeletionOptionDialogViewModel : INotifyPropertyChanged
    {
        #region Fields

        private bool isConfirmed;
        private bool isAllFilesChecked;
        private int selectedOccurrence;
        private DeletionOption selectedOption;
        private List<int> occurrenceCounts;

        #endregion Fields

        #region Properties

        public bool IsConfirmed
        {
            get => isConfirmed;
            set
            {
                isConfirmed = value;
                OnPropertyChanged(nameof(IsConfirmed));
            }
        }

        public DeletionOption SelectedOption
        {
            get => selectedOption;
            set
            {
                selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
            }
        }

        public int SelectedOccurrence
        {
            get => selectedOccurrence;
            set
            {
                selectedOccurrence = value;
                OnPropertyChanged(nameof(SelectedOccurrence));
            }
        }

        public List<int> OccurrenceCounts
        {
            get => occurrenceCounts;
            set
            {
                occurrenceCounts = value;
                OnPropertyChanged(nameof(OccurrenceCounts));
            }
        }

        public bool IsAllFilesChecked
        {
            get => isAllFilesChecked;
            set
            {
                isAllFilesChecked = value;
                OnPropertyChanged(nameof(IsAllFilesChecked));
            }
        }

        #endregion Properties

        #region Constructor

        public DeletionOptionDialogViewModel(List<int> occurrenceCounts)
        {
            OccurrenceCounts = occurrenceCounts;
            SelectedOption = DeletionOption.RemoveFirst;
            IsAllFilesChecked = false;

            if (occurrenceCounts.Count > 1)
                selectedOccurrence = 2;
            else if (occurrenceCounts.Count == 0)
                selectedOccurrence = 1;
            else
                selectedOccurrence = 0;
        }

        #endregion Constructor

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
