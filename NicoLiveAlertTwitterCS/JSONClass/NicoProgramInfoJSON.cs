using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoLiveAlertTwitterCS.JSONClass
{
    public class NicoMeta
    {
        public int status { get; set; }
        public string errorCode { get; set; }
    }

    public class SocialGroup
    {
        public string type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string ownerName { get; set; }
    }

    public class Broadcaster
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class NicoLiveProgramData
    {
        public string title { get; set; }
        public string description { get; set; }
        public bool isMemberOnly { get; set; }
        public int vposBaseAt { get; set; }
        public int beginAt { get; set; }
        public int endAt { get; set; }
        public string status { get; set; }
        public List<string> categories { get; set; }
        public List<object> rooms { get; set; }
        public bool isUserNiconicoAdsEnabled { get; set; }
        public SocialGroup socialGroup { get; set; }
        public Broadcaster broadcaster { get; set; }
    }

    public class NicoProgramInfoRootObject
    {
        public NicoMeta meta { get; set; }
        public NicoLiveProgramData data { get; set; }
    }
}
