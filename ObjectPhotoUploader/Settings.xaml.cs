using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ObjectPhotoUploader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public string allowedExts;
        public Settings()
        {
            this.InitializeComponent();
            if (localSettings.Values.ContainsKey("ALLOWED_EXTS"))
            {
                allowedExts = (string)localSettings.Values["ALLOWED_EXTS"];
            } else
            {
                allowedExts = App.DEFAULT_EXTS;
            }
        }

        private void saveSettings_Click(object sender, RoutedEventArgs e)
        {
            // allowedExts = fileExts.Text;
            localSettings.Values["ALLOWED_EXTS"] = allowedExts;
            Status.Text = "Settings saved.";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }
    }
}
