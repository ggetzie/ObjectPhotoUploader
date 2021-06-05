using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ObjectPhotoUploader
{
    class API
    {
        private string _baseurl = "https://j20200007.kotsf.com";

        public async Task<string> login(string username, string password)
        {
            var url = _baseurl.AppendPathSegment("auth-token") + "/";
            AuthToken token = await url.PostJsonAsync(new
            {
                username = username,
                password = password
            }).ReceiveJson<AuthToken>();
            return token.Token;
        }

        private string getCurrentToken()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            return (string)localSettings.Values["AuthToken"];
        }

        public async Task<IList<SpatialContext>> GetSpatialContextsAsync()
        {
            string token = string.Format("Token {0}", getCurrentToken());
            ObservableCollection<SpatialContext> contexts = new ObservableCollection<SpatialContext>();
            string contextListURL = _baseurl.AppendPathSegment("api/context") + "/";

            IList<SpatialContext> c = await contextListURL.WithHeader("Authorization", token).GetJsonAsync<IList<SpatialContext>>();

            return c;
        }
    }

    class AuthToken
    {
        public string Token { get; set; }

    }

}
