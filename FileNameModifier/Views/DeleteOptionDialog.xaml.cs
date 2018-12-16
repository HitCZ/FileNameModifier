using FileNameModifier.Logic.Arguments;
using FileNameModifier.Logic.Enumerations;
using FileNameModifier.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace FileNameModifier.Views
{
    /// <summary>
    /// Interaction logic for DeleteOptionDialog.xaml
    /// </summary>
    public partial class DeleteOptionDialog : Window
    {
        #region Delegates

        public delegate void BeforeClosingEventHandler(object sender, OptionClosingArgument e);

        #endregion Delegates

        #region Events

        public event BeforeClosingEventHandler BeforeClosing;

        #endregion Events

        #region Properties

        public bool IsConfirmed { get; set; }
        public bool IsAllFilesChecked { get; set; }

        public DeletionOptionDialogViewModel ViewModel { get; }

        #endregion Properties

        #region Constructors

        public DeleteOptionDialog()
        {
            InitializeComponent();
        }

        public DeleteOptionDialog(List<int> occurenceCounts)
        {
            ViewModel = new DeletionOptionDialogViewModel(occurenceCounts);
            DataContext = ViewModel;
            InitializeComponent();
        }

        #endregion Constructors

        #region Event Handlers

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
                SelectedOption = ViewModel.SelectedOption,
                SelectedOccurrence = ViewModel.SelectedOccurrence,
                IsAllFilesChecked = ViewModel.IsAllFilesChecked
            };

            BeforeClosing?.Invoke(sender, optionClosingArgs);
        }

        private void RadioButtonRemoveAll_OnChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedOption = DeletionOption.RemoveAll;
        }

        private void RadioButtonRemoveFirst_OnChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedOption = DeletionOption.RemoveFirst;
        }
        private void RadioButtonRemoveSpecificOccurrence_OnChecked(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedOption = DeletionOption.RemoveSpecific;
        }

        private void CheckBoxAllFiles_OnChecked(object sender, RoutedEventArgs e)
        {
            IsAllFilesChecked = true;
            ViewModel.IsAllFilesChecked = true;
        }

        private void CheckBoxAllFiles_OnUnchecked(object sender, RoutedEventArgs e)
        {
            IsAllFilesChecked = false;
            ViewModel.IsAllFilesChecked = false;
        }

        #endregion Event Handlers
    }
}
