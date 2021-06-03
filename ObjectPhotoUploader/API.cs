using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPhotoUploader
{
    class API
    {
        private string _baseurl = "https://j20200007.kotsf.com";

        public async Task<string> login(string username, string password)
        {
            

            var url = _baseurl.AppendPathSegment("auth-token") + "/";
            System.Diagnostics.Debug.WriteLine(url);
            AuthToken token = await url.PostJsonAsync(new
            {
                username = username,
                password = password
            }).ReceiveJson<AuthToken>();
            return token.Token;
        }
    }

    class AuthToken
    {
        public string Token { get; set; }

    }

}
