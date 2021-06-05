using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace TourPlaner_andreas.BL
{
    public class AppManagerWebApi
    {
       
        public async Task<string> getApiRoute()
        {
          
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://www.mapquestapi.com/directions/v2/");

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("route?key=VrUZ28kkdvGNfMj8GSE1jEKBGpJGfhzV&from=wien&to=linz").Result;  
                if (response.IsSuccessStatusCode)
                {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                //JObject json = JObject.Parse(responseBody);
               // Debug.WriteLine(responseBody);
              
                return responseBody;
                }
                else
                {
                    Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
               

            client.Dispose();
            return null;

        }
        public async Task getApiImage(string sessionId,JObject boundingBox, int id)
        {
           
            JObject lr = boundingBox["lr"].Value<JObject>();
            string lr_lng = lr["lng"].Value<string>();
            string lr_lat = lr["lat"].Value<string>();
            JObject ul = boundingBox["ul"].Value<JObject>();
            string ul_lng = ul["lng"].Value<string>();
            string ul_lat = ul["lat"].Value<string>();
            string paramss = "&size=700,300";
                paramss += "&defaultMarker=none";
                paramss += "&zoom=11";
                paramss += "&rand=737758036";
                paramss += "&session=" + sessionId;
            string box = lr_lat + "," + lr_lng + "," + ul_lat + "," + ul_lng;
                paramss += "&boundingBox=" + box;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://www.mapquestapi.com/staticmap/v5/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            
            byte[] response = await client.GetByteArrayAsync("map?key=VrUZ28kkdvGNfMj8GSE1jEKBGpJGfhzV" + paramss);
            MemoryStream ms = new MemoryStream(response);
            Image img = Image.FromStream(ms);
           

           img.Save( "C:\\Users\\Andre\\source\\repos\\TourPlaner_andreas\\TourPlaner_andreas\\Pics\\"+id+".jpg", ImageFormat.Jpeg);
           

           client.Dispose();

        }
    }
}

