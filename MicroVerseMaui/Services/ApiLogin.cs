using MicroVerseMaui.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroVerseMaui.Services
{
    public class ApiLogin : IApiLogin



    {
        // Log in using API

        public async Task<LoginResponse> Authenticate(LoginInfo loginRequest)
        {

            HttpResponseMessage response;
            string loginRequestStr = JsonConvert.SerializeObject(loginRequest);

            // If android, use helper for https request
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7028);
                var http = devSslHelper.HttpClient;
                response = await http.PostAsync(devSslHelper.DevServerRootUrl + "/api/User/Login/",
                    new StringContent(loginRequestStr, Encoding.UTF8, "application/json"));
            }
            else // Windows
            {
                var httpClient = new HttpClient();
                var url = "https://localhost:7028/api/User/Login";
                 response = await httpClient.PostAsync(url,
                    new StringContent(loginRequestStr, Encoding.UTF8, "application/json"));
            }

            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);
                }
                else
                {
                    return null;
                }
            
        }

    }
}
