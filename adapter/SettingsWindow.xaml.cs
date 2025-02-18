using System.Windows;

namespace Icf.ProPresenter.KidsCall
{
    /// <summary>
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(SettingsViewModel? viewModel = null)
        {
            this.ViewModel = viewModel;
            this.DataContext = this.ViewModel;

            this.InitializeComponent();
        }

        public SettingsViewModel? ViewModel { get; set; }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.WebPasswordInput.Password) && this.ViewModel != null)
            {
                this.ViewModel.WebPassword = this.WebPasswordInput.Password;
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}
