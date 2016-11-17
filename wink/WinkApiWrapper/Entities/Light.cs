using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WinkApiWrapper.Entities
{   [DataContract]
    public class Light : BaseDevice
    {

        [DataMember]
        public bool Switched { get; set; }
        [DataMember]
        public float Brightness { get; set; }

        [DataMember]
        public int ConvertedBrightness
        {
            get {
                  return (int)(Brightness * 100);
                
                  
            }
        }
    }
}
