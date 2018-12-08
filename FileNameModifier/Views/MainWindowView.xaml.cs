using System.IO;
using System.Windows;
using System.Windows.Forms;
using FileNameModifier.ViewModels;

namespace FileNameModifier.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void ButtonBrowseFile_OnClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var dialogResult = dialog.ShowDialog();
                var selectedPath = dialog.SelectedPath;

                if (dialogResult != System.Windows.Forms.DialogResult.OK ||
                    string.IsNullOrWhiteSpace(selectedPath)) return;

                ViewModel.SelectedPath = selectedPath;
                var files = Directory.GetFiles(selectedPath);
            }

        }

        private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {


        }

        #endregion Event Handlers
    }
}
