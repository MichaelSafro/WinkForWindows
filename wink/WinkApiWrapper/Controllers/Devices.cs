using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WinkApiWrapper.Controllers
{
    public class Devices
    {
        public string DevicesList; 

        public async Task<string> GetDevicesString()
        {
            var rawListAllDevices = await CallListDevicesApi();
            //TextBox1.Text = rawListAllDevices;
            var strB = new StringBuilder();

            JObject results = JObject.Parse(rawListAllDevices);
            foreach (var data in results["data"])
            {
                if (data["light_bulb_id"]!=null)
               {
                    strB.Append((string)data["name"]);
                    strB.Append("-");
                    strB.Append((string)data["manufacturer_device_model"]);
                    strB.Append("-");
                    //  strB.Append(data.ToString());
                    strB.Append((string)data["object_type"]);
                    strB.Append("-");
                    strB.Append((string)data["object_id"]);
                    strB.Append("-");
                    strB.AppendLine();
                }
            }
            // DevicesList= strB.ToString();
            return strB.ToString();
        }


        public async Task<ObservableCollection<Entities.Light>> GetLights()
        {
            var rawListAllDevices = await CallListDevicesApi();
            //TextBox1.Text = rawListAllDevices;
            var lights = new ObservableCollection<Entities.Light>();

            JObject results = JObject.Parse(rawListAllDevices);
            foreach (var data in results["data"])
            {
                if (data["light_bulb_id"] != null)
                {
                    lights.Add(new Entities.Light { Name = (string)data["name"], Id = (int)data["light_bulb_id"], Switched =(bool)data["desired_state"]["powered"],Brightness=(float)data["desired_state"]["brightness"] });
                }
            }
            // DevicesList= strB.ToString();
            return lights;
        }

        public async Task<ObservableCollection<Entities.Lock>> GetLocks()
        {
            var rawListAllDevices = await CallListDevicesApi();
            //TextBox1.Text = rawListAllDevices;
            var locks = new ObservableCollection<Entities.Lock>();

            JObject results = JObject.Parse(rawListAllDevices);
            foreach (var data in results["data"])
            {
                if (data["lock_id"] != null)
                {
                    locks.Add(new Entities.Lock { Name = (string)data["name"], Id = (int)data["lock_id"], Locked = (bool)data["desired_state"]["locked"] });
                }
            }
            // DevicesList= strB.ToString();
            return locks;
        }

        async Task<string> CallListDevicesApi()
        {
            string returnString = "None";

            using (var httpClient = new HttpClient { BaseAddress = Settings.Instance.baseAddress })
            {

                returnString = await ListDevices(returnString, httpClient);

            }

            return returnString;

        }

        public async Task<string> GetLightString(int lightBulbId)
        {
            return await CallGetLight(lightBulbId);
        }


        public async Task<WinkApiWrapper.Entities.Light> GetLightObject(int lightBulbId)
        {
            var rawListAllDevices = await CallGetLight(lightBulbId); 
            //TextBox1.Text = rawListAllDevices;
            var strB = new StringBuilder();

            JObject results = JObject.Parse(rawListAllDevices);

            WinkApiWrapper.Entities.Light light = new Entities.Light();
            light.Name = (string)results["data"]["name"];
            light.Id = (int)results["data"]["light_bulb_id"];
            light.Switched = (bool)results["data"]["desired_state"]["powered"];
            light.Brightness = (float)results["data"]["desired_state"]["brightness"];

            return light;
        }

        public async Task SetightObject(int lightBulbId, bool state)
        {
            var rawListAllDevices = await CallSetLight(lightBulbId,state);
        }

        public async Task SetLockObject(int LockId, bool state)
        {
            var rawListAllDevices = await CallSetLock(LockId, state);
        }

        public async Task SetightObjectBrightness(int lightBulbId, float brightness)
        {
            brightness = brightness / 100;
            var rawListAllDevices = await CallSetLightBrightness(lightBulbId, brightness);
        }


        async Task<string> CallGetLight(int lightBulbId)
        {
            string returnString = "None";

            using (var httpClient = new HttpClient { BaseAddress = Settings.Instance.baseAddress })
            {

                returnString = await GetLight(httpClient,lightBulbId);

            }

            return returnString;

        }


        async Task<string> CallSetLight(int lightBulbId, bool state)
        {
            string returnString = "None";

            using (var httpClient = new HttpClient { BaseAddress = Settings.Instance.baseAddress })
            {

                returnString = await SetLight(httpClient, lightBulbId, state);

            }

            return returnString;

        }

        async Task<string> CallSetLock(int LockId, bool state)
        {
            string returnString = "None";

            using (var httpClient = new HttpClient { BaseAddress = Settings.Instance.baseAddress })
            {

                returnString = await SetLock(httpClient, LockId, state);

            }

            return returnString;

        }

        async Task<string> CallSetLightBrightness(int lightBulbId, float brightness)
        {
            string returnString = "None";

            using (var httpClient = new HttpClient { BaseAddress = Settings.Instance.baseAddress })
            {

                returnString = await SetLightBrightness(httpClient, lightBulbId, brightness);

            }

            return returnString;

        }

        private async Task<string> ListDevices(string returnString, HttpClient httpClient)
        {

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", Security.Instance.token_type + " " + Security.Instance.access_token);
            using (var response = await httpClient.GetAsync("users/me/wink_devices"))
            {
                returnString = await response.Content.ReadAsStringAsync();
            }

            return returnString;
        }

         private async Task<string> GetLight(HttpClient httpClient, int lightBulbId)
        {
            string responseData = "None";

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", Security.Instance.token_type + " " + Security.Instance.access_token);

            using (var response = await httpClient.GetAsync(string.Format("light_bulbs/{0}",lightBulbId.ToString())))
                {

                     responseData = await response.Content.ReadAsStringAsync();
                }

            return responseData;
        }


        private async Task<string> SetLight(HttpClient httpClient, int lightBulbId, bool state)
        {
            string responseData = "None";

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", Security.Instance.token_type + " " + Security.Instance.access_token);
            using (var content = new StringContent(string.Format("{{  \"desired_state\": {{    \"powered\": {0}  }}}}", state.ToString().ToLower()), System.Text.Encoding.UTF8, "application/json"))
            { 
                using (var response = await httpClient.PutAsync(string.Format("light_bulbs/{0}/desired_state", lightBulbId.ToString()),content))
                {

                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return responseData;
        }

        private async Task<string> SetLock(HttpClient httpClient, int LockId, bool state)
        {
            string responseData = "None";

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", Security.Instance.token_type + " " + Security.Instance.access_token);
            using (var content = new StringContent(string.Format("{{  \"desired_state\": {{    \"locked\": {0}  }}}}", state.ToString().ToLower()), System.Text.Encoding.UTF8, "application/json"))
            {
                using (var response = await httpClient.PutAsync(string.Format("locks/{0}/desired_state", LockId.ToString()), content))
                {

                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return responseData;
        }

        private async Task<string> SetLightBrightness(HttpClient httpClient, int lightBulbId, float brightness)
        {
            string responseData = "None";

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", Security.Instance.token_type + " " + Security.Instance.access_token);
            using (var content = new StringContent(string.Format("{{  \"desired_state\": {{    \"brightness\": {0}  }}}}", brightness.ToString().ToLower()), System.Text.Encoding.UTF8, "application/json"))
            {
                using (var response = await httpClient.PutAsync(string.Format("light_bulbs/{0}/desired_state", lightBulbId.ToString()), content))
                {

                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return responseData;
        }
    }
}
