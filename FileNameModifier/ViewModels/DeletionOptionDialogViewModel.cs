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
        private DeletionOption selectedOption;
        private List<int> occurenceCounts;

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

        public List<int> OccurenceCounts
        {
            get => occurenceCounts;
            set
            {
                occurenceCounts = value;
                OnPropertyChanged(nameof(OccurenceCounts));
            }
        }

        #endregion Properties

        #region Constructor

        public DeletionOptionDialogViewModel(List<int> occurenceCounts)
        {
            OccurenceCounts = occurenceCounts;
            SelectedOption = DeletionOption.RemoveFirst;
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
