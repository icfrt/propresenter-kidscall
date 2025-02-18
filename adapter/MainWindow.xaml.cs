using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Icf.ProPresenter.KidsCall.Web;
using SocketIOClient;

namespace Icf.ProPresenter.KidsCall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private EntryModel? entry;

        private readonly ManualResetEvent? closedEvent;

        public MainWindow(ManualResetEvent? closedEvent)
        {
            if (Properties.Settings.Default.StartupLeft != 0)
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = Properties.Settings.Default.StartupLeft;
                this.Top = Properties.Settings.Default.StartupTop;
            }

            this.closedEvent = closedEvent;

            this.CancelCommand = new DelegateCommand(
                () => true, 
                () => {
                    this.Feedback?.Invoke(false);
                    this.Close();
                    this.closedEvent?.Set();
                });
            
            this.ShowCommand = new DelegateCommand(
                () => true, 
                () => {
                    this.Feedback?.Invoke(true);
                    this.Close();
                    this.closedEvent?.Set();
                });

            this.DataContext = this;            

            this.InitializeComponent();
        }

        public Action<bool>? Feedback { get; set; }

        internal EntryModel? Entry 
        { 
            get => entry;
            set
            {
                entry = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Entry)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Code)));
            }
        }

        public string Code 
        { 
            get => entry?.Code ?? string.Empty;
            set
            {
                if (entry != null)
                {
                    entry.Code = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Code)));
                }
            }
        }

        public ICommand CancelCommand { get; private set; }

        public ICommand ShowCommand { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnWindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.StartupLeft = (int)this.Left;
            Properties.Settings.Default.StartupTop = (int)this.Top;
            Properties.Settings.Default.Save();
        }
    }
}