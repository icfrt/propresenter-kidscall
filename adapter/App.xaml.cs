using System.IO;
using System.Windows;
using Application = System.Windows.Application;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace Icf.ProPresenter.KidsCall
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Kernel? kernel;

        public App()
        {
        }

        private void ShowSettings()
        {
            var viewModel = new SettingsViewModel
            {
                ProPresenterUrl = KidsCall.Properties.Settings.Default.ProPresenterUrl,
                WebUrl = KidsCall.Properties.Settings.Default.WebUrl,
                WebPassword = KidsCall.Properties.Settings.Default.WebPassword,
            };

            var window = new SettingsWindow(viewModel);
            if (window.ShowDialog() ?? false)
            {
                KidsCall.Properties.Settings.Default.ProPresenterUrl = viewModel.ProPresenterUrl;
                KidsCall.Properties.Settings.Default.WebUrl = viewModel.WebUrl;
                KidsCall.Properties.Settings.Default.WebPassword = viewModel.WebPassword;
                KidsCall.Properties.Settings.Default.Save();
                this.kernel?.Dispose();
                this.kernel = new Kernel();
            }
        }

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            if (string.IsNullOrEmpty(KidsCall.Properties.Settings.Default.ProPresenterUrl))
            {
                KidsCall.Properties.Settings.Default.Upgrade();
            }
            
            //
            // Setup tray icon and menu

            var menu = new ContextMenuStrip();
            menu.Items.Add("&Einstellungen", null, (_, e) => 
            {
                this.ShowSettings();
            });

            menu.Items.Add("&Beenden", null, (_, e) => this.Shutdown());

            var icon = new NotifyIcon();
            icon.Text = "ProPresenter Kids Call";

            var stream = new MemoryStream(KidsCall.Properties.Resources.AppIcon);
            icon.Icon = new Icon(stream);            
            
            icon.ContextMenuStrip = menu;

            icon.Visible = true;

            //
            // Start kernel

            if (string.IsNullOrEmpty(KidsCall.Properties.Settings.Default.ProPresenterUrl))
            {
                this.ShowSettings();
            }
            else
            {
                this.kernel = new Kernel();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            this.kernel?.Dispose();

            base.OnExit(e);
        }
    }
}
