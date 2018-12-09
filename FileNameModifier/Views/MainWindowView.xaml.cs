using System.Windows;
using System.Windows.Forms;

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
            }
        }

        private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is System.Windows.Controls.Button))
                return;

            var textToCut = TextBoxCut?.Text;

            if (!(textToCut is null) && (textToCut != string.Empty))
                ViewModel.CutText(textToCut);
        }

        #endregion Event Handlers
    }
}
