using Newtonsoft.Json;
using NicoLiveAlertTwitterCS.JSONClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NicoLiveAlertTwitterCS.niconico
{
    class NicoLiveProgramInfo
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;

        //番組情報API
        public async Task<NicoProgramInfoRootObject> getProgramInfo(string liveId)
        {
            if (setting.Values["user_session"] != null)
            {
                var urlString = $"https://live2.nicovideo.jp/watch/{liveId}/programinfo";
                var client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "NicoLiveAlert_Twitter;@takusan_23");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "user_session=" + setting.Values["user_session"]);
                using (var stream = await client.GetAsync(new Uri(urlString)))
                {
                    if (stream.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = await stream.Content.ReadAsStringAsync();
                        var jsonObject = JsonConvert.DeserializeObject<NicoProgramInfoRootObject>(jsonString);
                        return jsonObject;
                    }
                }
            }
            return null;
        }
    }
}