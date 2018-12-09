using FileNameModifier.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FileNameModifier.Logic.Enumerations;

namespace FileNameModifier.ViewModels
{
    class DeletionOptionDialogViewModel :INotifyPropertyChanged
    {
        private bool isConfirmed;
        private DeletionOption selectedOption;

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

        public DeletionOptionDialogViewModel()
        {
            SelectedOption = DeletionOption.RemoveFirst;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
