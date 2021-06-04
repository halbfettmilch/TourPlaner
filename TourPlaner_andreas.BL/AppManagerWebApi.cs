using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TourPlaner_andreas.BL
{
    public class AppManagerWebApi
    {
        public void getApiConnection()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.mapquestapi.com/directions/v2/");
                //HTTP GET
                var responseTask = client.GetAsync("route?key=VrUZ28kkdvGNfMj8GSE1jEKBGpJGfhzV&from=wien&to=linz");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                   
                   Debug.WriteLine("hey" + result);
                }
            }

        }
    }
}

