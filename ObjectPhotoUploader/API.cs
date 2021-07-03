using Flurl;
using Flurl.Http;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Diagnostics;

namespace ObjectPhotoUploader
{
    class API
    {
        // private string _baseurl = "https://j20200007.kotsf.com";
        private string _baseurl = "http://aslcv2";

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
            // return current access token formatted for inclusion in header
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            return string.Format("Token {0}", (string)localSettings.Values["AuthToken"]);
        }

        public async Task<IList<SpatialContext>> GetSpatialContextsAsync()
        {
            string token = getCurrentToken();
            string contextListURL = _baseurl.AppendPathSegment("api/context") + "/";

            IList<SpatialContext> c = await contextListURL
                .WithHeader("Authorization", token)
                .GetJsonAsync<IList<SpatialContext>>();

            return c;
        }

        public async Task<IList<ObjectFind>> GetObjectFindsAsync(SpatialContext sc)
        {
            string token = getCurrentToken();
            string url = _baseurl
                .AppendPathSegment("api/find")
                .AppendPathSegments(
                sc.utm_hemisphere,
                sc.utm_zone,
                sc.area_utm_easting_meters,
                sc.area_utm_northing_meters,
                sc.context_number) + "/";
            
            ObjectFindRoot root = await url.WithHeader("Authorization", token).GetJsonAsync<ObjectFindRoot>();
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

        public async Task<ObjectFind> CreateObjectFindAsync(ObjectFindData data)
        {
            string token = getCurrentToken();
            string url = _baseurl
                .AppendPathSegment("api/find") + "/";
            ObjectFind newFind = await url.WithHeader("Authorization", token)
                .PostJsonAsync(data).ReceiveJson<ObjectFind>();
            return newFind;
        }

        public async Task<IList<MaterialCategory>> GetMaterialCategoriesAsync()
        {
            string token = getCurrentToken();
            string url = _baseurl
                .AppendPathSegment("api/find/mc") + "/";
            IList<MaterialCategory> mcs = await url
                .WithHeader("Authorization", token)
                .GetJsonAsync<IList<MaterialCategory>>();
            return mcs;

        }

        public async Task<IFlurlResponse> UploadFindPhoto(FindPhotoFile photo)
        {
            string token = getCurrentToken();
            string url = _baseurl
                .AppendPathSegment("api/find")
                .AppendPathSegment(photo.Find.id)
                .AppendPathSegment("photo") + "/";
            Stream stream = await photo.LocalFile.OpenStreamForReadAsync();
            Debug.WriteLine(stream.ToString());
            IFlurlResponse res = await url.WithHeader("Authorization", token)
                .PostMultipartAsync(mp => mp.AddFile("photo", stream, photo.LocalFile.Name));
            Debug.WriteLine("Uploading {0}", photo.LocalFile.Name);
            Debug.WriteLine(await res.GetStringAsync());
            return res;
        }
    }

    class AuthToken
    {
        public string Token { get; set; }

    }

}
