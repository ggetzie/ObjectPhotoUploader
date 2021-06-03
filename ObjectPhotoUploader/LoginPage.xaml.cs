﻿using System;
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
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void LoginButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            API api = new API();
            try
            {
                SetLoading(true);
                string token = await api.login(Username.Text, Password.Password);
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["AuthToken"] = token;
                System.Diagnostics.Debug.WriteLine(token);
                this.Frame.Navigate(typeof(HomePage));
            } catch (FlurlHttpException ex)
            {
                LoginError err = await ex.GetResponseJsonAsync<LoginError>();
                Status.Text = String.Join("\n", err.non_field_errors);
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
    }

    class LoginError
    {
        public List<string> non_field_errors { get; set; }
    }
}