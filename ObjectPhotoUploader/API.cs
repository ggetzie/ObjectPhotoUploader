﻿using Flurl;
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
            string contextListURL = _baseurl.AppendPathSegment("api/context") + "/";

            IList<SpatialContext> c = await contextListURL.WithHeader("Authorization", token).GetJsonAsync<IList<SpatialContext>>();

            return c;
        }

        public async Task<IList<ObjectFind>> GetObjectFindsAsync(SpatialContext sc)
        {
            string token = string.Format("Token {0}", getCurrentToken());
            string url = _baseurl
                .AppendPathSegment("api/find")
                .AppendPathSegments(
                sc.utm_hemisphere,
                sc.utm_zone,
                sc.area_utm_easting_meters,
                sc.area_utm_northing_meters,
                sc.context_number) + "/";
            
            ObjectFindRoot root = await url.WithHeader("Authorization", token).GetJsonAsync<ObjectFindRoot>();
            System.Diagnostics.Debug.WriteLine(root.results);
            IList<ObjectFind> finds = root.results;
            int i = 0;
            while (finds.Count() < root.count && i < 20)
            {
                string next_url = root.next;
                root = await next_url.WithHeader("Authorization", token).GetJsonAsync<ObjectFindRoot>();
                finds = finds.Concat(root.results).ToList();
                i++;
            }
            finds = finds.OrderByDescending(x => x.find_number).ToList<ObjectFind>();
            return finds;
        }
    }

    class AuthToken
    {
        public string Token { get; set; }

    }

}
