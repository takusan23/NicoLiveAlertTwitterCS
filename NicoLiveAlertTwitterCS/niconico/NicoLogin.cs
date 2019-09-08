using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace NicoLiveAlertTwitterCS.niconico
{
    class NicoLogin
    {
        public async void niconicoLogin(string mail, string pass)
        {
            //niconicoログイン
            // 
            var cookie_container = new CookieContainer();
            using (var h = new HttpClientHandler() { CookieContainer = cookie_container })
            using (var c = new HttpClient(h) { BaseAddress = new Uri("https://secure.nicovideo.jp/secure/login?site=niconico") })
            {
                var content = new FormUrlEncodedContent(
                    new Dictionary<string, string> {
            { "next_url", "" },
            { "mail", mail },
            { "password", pass }
                    }
                );
                c.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "NicoLiveAlert_Twitter;@takusan_23");
                var login = c.PostAsync("", content).Result;
            }

            foreach (Cookie cookie in cookie_container.GetCookies(new Uri("https://secure.nicovideo.jp/secure/login?site=niconico")))
            {
                if (cookie.Name == "user_session")
                {
                    //user_session保存
                    var setting = Windows.Storage.ApplicationData.Current.RoamingSettings;
                    setting.Values["user_session"] = cookie.Value;
                    //一応メアドも保存
                    setting.Values["mail"] = mail;
                    setting.Values["pass"] = pass;

                    //ダイアログ出す
                    ContentDialog deleteFileDialog = new ContentDialog
                    {
                        Title = "ログインに成功しました。",
                        PrimaryButtonText = "閉じる"
                    };

                    await deleteFileDialog.ShowAsync();
                }
            }
        }
    }
}
