using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TourPlaner_andreas.Models;

namespace TourPlaner_andreas.BL
{
    public class AppManagerWebApi
    {
        public async Task<string> getApiRoute(TourItem item)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://www.mapquestapi.com/directions/v2/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client
                .GetAsync("route?key=VrUZ28kkdvGNfMj8GSE1jEKBGpJGfhzV&from=" + item.Fromstart + "&to=" + item.To)
                .Result;
            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                //JObject json = JObject.Parse(responseBody);
                // Debug.WriteLine(responseBody);

                return responseBody;
            }

            Debug.WriteLine("{0} ({1})", (int) response.StatusCode, response.ReasonPhrase);


            client.Dispose();
            return null;
        }

        public async Task getApiImage(string sessionId, JObject boundingBox, int id)
        {
            var lr = boundingBox["lr"].Value<JObject>();
            var lr_lng = lr["lng"].Value<string>();
            var lr_lat = lr["lat"].Value<string>();
            var ul = boundingBox["ul"].Value<JObject>();
            var ul_lng = ul["lng"].Value<string>();
            var ul_lat = ul["lat"].Value<string>();
            var paramss = "&size=700,300";
            paramss += "&defaultMarker=none";
            paramss += "&zoom=11";
            paramss += "&rand=737758036";
            paramss += "&session=" + sessionId;
            var box = lr_lat + "," + lr_lng + "," + ul_lat + "," + ul_lng;
            paramss += "&boundingBox=" + box;
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://www.mapquestapi.com/staticmap/v5/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            var response = await client.GetByteArrayAsync("map?key=VrUZ28kkdvGNfMj8GSE1jEKBGpJGfhzV" + paramss);
            var ms = new MemoryStream(response);
            var img = Image.FromStream(ms);


            img.Save("C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Pics\\" + id + ".jpg",
                ImageFormat.Jpeg);


            client.Dispose();
        }
    }
}