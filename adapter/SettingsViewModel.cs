using System.ComponentModel;

namespace Icf.ProPresenter.KidsCall
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string? proPresenterUrl;

        private string? webUrl;

        private string? webPassword;

        public string? ProPresenterUrl
        {
            get => proPresenterUrl;
            set
            {
                if (proPresenterUrl != value)
                {
                    proPresenterUrl = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProPresenterUrl)));
                }
            }
        }

        public string? WebUrl
        {
            get => webUrl;
            set
            {
                if (webUrl != value)
                {
                    webUrl = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WebUrl)));
                }
            }
        }

        public string? WebPassword
        {
            get => webPassword;
            set
            {
                if (webPassword != value)
                {
                    webPassword = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WebPassword)));
                }
            }
        }
    }
}
