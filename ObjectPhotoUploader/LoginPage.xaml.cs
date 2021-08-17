using System;
using Flurl;
using Flurl.Http;
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
using Windows.Web.Http;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPhotoUploader
{
    /// <summary>
    /// A page showing a login form that allows the user to send their credentials
    /// to the server. Upon successful login, the user will be redirected to the home page
    /// </summary>
    public sealed partial class LoginPage : Page
    {

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public string BaseUrl;
        public LoginPage()
        {
            this.InitializeComponent();
            if (localSettings.Values.ContainsKey("BASE_URL"))
            {
                BaseUrl = (string)localSettings.Values["BASE_URL"];
            } else
            {
                BaseUrl = "http://aslcv2";
            }
        }

        private async void LoginButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            localSettings.Values["BASE_URL"] = URL_tb.Text;
            API api = new API();
            try
            {
                SetLoading(true);
                string token = await api.login(Username.Text, Password.Password);
            
                localSettings.Values["AuthToken"] = token;
                localSettings.Values["username"] = Username.Text;
                this.Frame.Navigate(typeof(HomePage));
            } catch (FlurlHttpException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                string err = await ex.GetResponseStringAsync();
                if (err != null)
                {
                    Status.Text = err;
                }

            } finally
            {
                SetLoading(false);
            }
        }

        public void SetLoading(bool isLoading)
        {
            ProgBar.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            LoginButton.IsEnabled = !isLoading;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (localSettings.Values.ContainsKey("AuthToken"))
            {
                this.Frame.Navigate(typeof(HomePage));
            }

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }
    }

    class LoginError
    {
        public List<string> non_field_errors { get; set; }
    }
}

