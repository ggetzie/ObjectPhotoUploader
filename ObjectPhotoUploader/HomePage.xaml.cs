using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Collections.ObjectModel;
using Flurl.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ObjectPhotoUploader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class HomePage : Page
    {
        string username;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        ObservableCollection<string> hemispheres = new ObservableCollection<string>();
        IList<SpatialContext> contexts;
        API api = new API();
        public HomePage()
        {
            this.InitializeComponent();
            username = (string)localSettings.Values["username"];
            hemispheres.Add("N");
            hemispheres.Add("S");
            loadContexts();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["AuthToken"] = "";
            Frame.Navigate(typeof(LoginPage));
        }

        private async void loadContexts()
        {
            try
            {
                status.Text = "Loading Spatial Contexts...";
                progbar.Visibility = Visibility.Visible;
                contexts = await api.GetSpatialContextsAsync();
                System.Diagnostics.Debug.WriteLine(contexts);
                status.Text = "";
            } catch (FlurlHttpException ex)
            {
                status.Text = ex.Message;
            } finally
            {
                progbar.Visibility = Visibility.Collapsed;
            }


        }
    }
}
