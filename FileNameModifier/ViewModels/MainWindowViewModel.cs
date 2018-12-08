using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FileNameModifier.Annotations;

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
            get => selectedPath ;
            set
            {
                selectedPath = value;
                OnPropertyChanged(nameof(SelectedPath));
            }
        }

        #endregion Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
