using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinkApiWrapper
{
    public class Settings
    {

        public static Settings Instance { get; private set; }

        private Settings() { }

        static Settings() { Instance = new Settings(); }

        public Uri baseAddress { get; set; } = new Uri("https://winkapi.quirky.com/");



    }
}
