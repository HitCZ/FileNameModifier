using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FileNameModifier.Logic.Arguments;
using FileNameModifier.Logic.Enumerations;

namespace FileNameModifier.Views
{
    /// <summary>
    /// Interaction logic for DeleteOptionDialog.xaml
    /// </summary>
    public partial class DeleteOptionDialog : Window
    {
        public delegate void BeforeClosingEventHandler(object sender, OptionClosingArgument e);

        public event BeforeClosingEventHandler BeforeClosing;

        public bool IsConfirmed { get; set; }

        public DeleteOptionDialog()
        {
            InitializeComponent();
        }

        private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            ViewModel.IsConfirmed = true;
            Close();
        }


        private void DeleteOptionDialog_OnClosing(object sender, EventArgs e)
        {
            var optionClosingArgs = new OptionClosingArgument()
            {
                IsConfirmed = ViewModel.IsConfirmed,
                SelectedOption = ViewModel.SelectedOption
            };

            BeforeClosing?.Invoke(sender,optionClosingArgs);
        }

        private void RadioButtonRemoveAll_OnChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedOption = DeletionOption.RemoveAll;
        }

        private void RadioButtonRemoveFirst_OnChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedOption = DeletionOption.RemoveFirst;
        }
    }
}
