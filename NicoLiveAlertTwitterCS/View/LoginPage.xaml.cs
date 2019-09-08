using CoreTweet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static CoreTweet.OAuth;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace NicoLiveAlertTwitterCS
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class LoginPage : Page
    {

        OAuthSession session;
        //コンシューマーキー取得
        String consumer_key = TwitterKey.consumer_key;
        String consumer_secret = TwitterKey.consumer_secret;


        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            //認証画面をブラウザで開く。
            session = Authorize(consumer_key, consumer_secret);
            //非同期らしいー
            await Windows.System.Launcher.LaunchUriAsync(new Uri(session.AuthorizeUri.AbsoluteUri));

            //同時にPINコードを受け付けるダイアログを表示させる
            await pin_dialog.ShowAsync();

        }

        private async void Pin_dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //ダイアログの認証押したとき
            var pin = pin_dialog_textbox.Text;
            //ダイアログ閉じる
            pin_dialog.Hide();

            //アクセストークン取得
            var token = session.GetTokens(pin);

            //アクセストークンほぞん
            var setting = Windows.Storage.ApplicationData.Current.RoamingSettings;

            setting.Values["consumer_key"] = consumer_key;
            setting.Values["consumer_secret"] = consumer_secret;
            setting.Values["access_token"] = token.AccessToken;
            setting.Values["access_token_secret"] = token.AccessTokenSecret;

            //ログイン成功ダイアログ
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Twitterログイン成功",
                Content = token.ScreenName,
                PrimaryButtonText = "閉じる",
            };
            await deleteFileDialog.ShowAsync();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //値取り出し
            var mail = nico_login_dialog_mail_textbox.Text;
            var pass = nico_login_dialog_pass_textbox.Text;

            //niconicoログイン
            // 
            var cookie_container = new CookieContainer();
            using (var h = new HttpClientHandler() { CookieContainer = cookie_container })
            using (var c = new HttpClient(h) { BaseAddress = new Uri("https://secure.nicovideo.jp/secure/login?site=niconico") })
            {
                var content = new FormUrlEncodedContent(
                    new Dictionary<string, string> {
            { "next_url", "" },
            { "mail", "" },
            { "password", "" }
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
