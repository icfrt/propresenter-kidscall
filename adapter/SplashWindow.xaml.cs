using System;
using System.Collections.Generic;
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

namespace Icf.ProPresenter.KidsCall
{
    /// <summary>
    /// Interaktionslogik für SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        private System.Threading.Timer? timer;

        public SplashWindow()
        {
            this.ShowInTaskbar = false;
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.timer = new System.Threading.Timer(_ =>
            {
                this.Dispatcher.Invoke(() => this.Hide());
            }, null, 2000, Timeout.Infinite);
        }
    }
}
