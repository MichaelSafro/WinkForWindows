using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinkApiWrapper
{
    public class Security
    {
        public static Security Instance { get; private set; }

        private Security() { }

        static Security() { Instance = new Security(); }

            public string access_token;
            public string refresh_token;
            public string token_type;


        public async Task<bool> Authenticate(string userName, string passWord)
        {
            try
            {
                var rawAuth = await CallAuthApi(userName, passWord);
                //this.TextBox1.Text = rawAuth;
                JObject results = JObject.Parse(rawAuth);
                this.access_token = (string)results["data"]["access_token"];
                this.refresh_token = (string)results["data"]["refresh_token"];
                this.token_type = (string)results["data"]["token_type"];

                return true;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.Write(e.ToString());
                return false;
            }
        }


        public async Task<string> CallAuthApi(string userName, string passWord)
        {
            string returnString = "None";

            using (var httpClient = new HttpClient { BaseAddress = Settings.Instance.baseAddress })
            {

                returnString = await AuthCall(returnString, httpClient,userName, passWord);

            }

            return returnString;

        }

        private async Task<string> AuthCall(string returnString, HttpClient httpClient, string userName, string passWord)
        {
            var client_id = "quirky_wink_android_app";

            var client_secret = "e749124ad386a5a35c0ab554a4f2c045";

            using (var content = new StringContent(string.Format("{{\n    \"client_id\": \"{2}\",\n    \"client_secret\": \"{3}\",\n    \"username\": \"{0}\",\n    \"password\": \"{1}\",\n    \"grant_type\": \"password\"\n}}",userName, passWord,client_id,client_secret), System.Text.Encoding.UTF8, "application/json"))
            {
                using (var response = await httpClient.PostAsync("oauth2/token", content))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    returnString = responseData;
                }
            }
            return returnString;
        }
    }


}
