using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Icf.ProPresenter.KidsCall.ProPresenter;
using Icf.ProPresenter.KidsCall.ProPresenter.Models;
using Icf.ProPresenter.KidsCall.Web;

namespace Icf.ProPresenter.KidsCall
{
    public static class Win32
    {
        public static void ShowWindowNoActive(Window window)
        {
            IntPtr hWnd = GetForegroundWindow();
            window.Show();
            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
            }
        }

        public static void ActivatePreviousWindow(Window window)
        {
            IntPtr hWnd = (HwndSource.FromVisual(window) as HwndSource)?.Handle ?? IntPtr.Zero;
            if (hWnd != IntPtr.Zero)
            {
                IntPtr hWndPrev = GetWindow(hWnd, GetWindow_Cmd.GW_HWNDNEXT);
                if (hWnd != IntPtr.Zero)
                {
                    SetForegroundWindow(hWndPrev);
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private enum ShowWindowCommands : int
        {
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

        enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }
    }

    internal class Kernel : IDisposable
    {
        private WebClient webClient;
        private ProPresenterClient proPresenter;
        private System.Threading.Timer propTimer;

        private MessageResponse? selectedMessage;

        public Kernel()
        {
            this.webClient = new WebClient(new Uri(Properties.Settings.Default.WebUrl), Properties.Settings.Default.WebPassword);
            this.proPresenter = new ProPresenterClient(new Uri(Properties.Settings.Default.ProPresenterUrl));

            this.webClient.EntryReceived += this.OnEntryReceived;

            this.webClient.ConnectAsync();


            this.propTimer = new System.Threading.Timer(this.OnPropTimer, null, 0, 5000);
        }

        private async void OnPropTimer(object? state)
        {
            try
            {
                this.selectedMessage = await this.GetSelectedMessage();
            }
            catch
            {
                // Ignore
            }
        }

        private async Task<MessageResponse?> GetSelectedMessage()
        {
            var messages = await this.proPresenter.GetMessagesAsync();
            return messages?.FirstOrDefault(m => m.Id?.Name?.Contains("kids", StringComparison.InvariantCultureIgnoreCase) ?? false);
        }

        private async void OnEntryReceived(object? sender, EntryReceivedEventArgs e)
        {
            try
            {
                EntryModel? model = e?.Model;
                if (model != null && model.State == "created")
                {
                    var manualEvent = new ManualResetEvent(false);
                    bool show = false;

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        var mainWindow = new MainWindow(manualEvent);
                        mainWindow.Feedback = b => show = b;
                        mainWindow.Entry = model;

                        Win32.ShowWindowNoActive(mainWindow);
                    });

                    manualEvent.WaitOne();
                    
                    if (show && !string.IsNullOrWhiteSpace(model?.Code))
                    {
                        MessageResponse? message = this.selectedMessage ?? await this.GetSelectedMessage();
                        if (message != null)
                        {
                            MessageToken? token = message.Tokens.FirstOrDefault(t => t.Name?.Contains("code", StringComparison.InvariantCultureIgnoreCase) ?? false);
                            if (token != null)
                            {
                                token.Text = model.Code;
                                await this.proPresenter.TriggerMessageAsync(message.Id, new List<MessageToken> { token });

                                model.State = "done";
                                await this.webClient.SetStateAsync(model);
                            }
                        }
                    }
                    else
                    {
                        if (model != null)
                        {
                            model.State = "declined";
                            await this.webClient.SetStateAsync(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        public void Dispose()
        {
            this.propTimer.Dispose();
            this.proPresenter.Dispose();
            this.webClient.Dispose();
        }
    }
}
