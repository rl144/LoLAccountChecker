#region

using System.Windows;
using System.Windows.Threading;

#endregion

namespace LoLAccountChecker
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Utils.ExportException(e.Exception);
        }
    }
}