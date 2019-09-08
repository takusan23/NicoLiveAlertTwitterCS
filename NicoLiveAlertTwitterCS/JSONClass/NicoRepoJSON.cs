using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoLiveAlertTwitterCS.JSONClass
{
    public class Meta
    {
        public int status { get; set; }
        public string maxId { get; set; }
        public string minId { get; set; }
        public string impressionId { get; set; }
        public string clientAppGroup { get; set; }
        public int _limit { get; set; }
    }

    public class Sender
    {
        public string idType { get; set; }
        public string id { get; set; }
        public string type { get; set; }
    }

    public class MuteContext
    {
        public string task { get; set; }
        public Sender sender { get; set; }
        public string trigger { get; set; }
    }

    public class Urls
    {
        public string s50x50 { get; set; }
        public string s150x150 { get; set; }
    }

    public class DefaultValue
    {
        public Urls urls { get; set; }
    }

    public class Tags
    {
        public DefaultValue defaultValue { get; set; }
    }

    public class Icons
    {
        public Tags tags { get; set; }
    }

    public class SenderNiconicoUser
    {
        public string nickname { get; set; }
        public int id { get; set; }
        public Icons icons { get; set; }
    }

    public class ThumbnailUrl
    {
        public string small { get; set; }
    }

    public class Community
    {
        public string name { get; set; }
        public string id { get; set; }
        public ThumbnailUrl thumbnailUrl { get; set; }
    }

    public class Program
    {
        public string id { get; set; }
        public string beginAt { get; set; }
        public bool isPayProgram { get; set; }
        public string thumbnailUrl { get; set; }
        public string title { get; set; }
    }

    public class ThumbnailUrl2
    {
        public string normal { get; set; }
    }

    public class Video
    {
        public string id { get; set; }
        public string status { get; set; }
        public ThumbnailUrl2 thumbnailUrl { get; set; }
        public string title { get; set; }
        public string videoWatchPageId { get; set; }
    }

    public class ThumbnailUrl3
    {
        public string small { get; set; }
    }

    public class CommunityForFollower
    {
        public string name { get; set; }
        public string id { get; set; }
        public ThumbnailUrl3 thumbnailUrl { get; set; }
    }

    public class CommunityNews
    {
        public string title { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string topic { get; set; }
        public string createdAt { get; set; }
        public bool isVisible { get; set; }
        public bool isMuted { get; set; }
        public MuteContext muteContext { get; set; }
        public SenderNiconicoUser senderNiconicoUser { get; set; }
        public object actionLog { get; set; }
        public Community community { get; set; }
        public Program program { get; set; }
        public Video video { get; set; }
        public CommunityForFollower communityForFollower { get; set; }
        public CommunityNews communityNews { get; set; }
    }

    public class Error
    {
        public string id { get; set; }
        public string errorCode { get; set; }
        public List<string> errorReasonCodes { get; set; }
    }

    public class NicoRepoRootObject
    {
        public Meta meta { get; set; }
        public List<Datum> data { get; set; }
        public List<Error> errors { get; set; }
        public string status { get; set; }
    }
}
