using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using static CoreTweet.OAuth;

namespace NicoLiveAlertTwitterCS.Twitter
{
    class TwitterLogin
    {
        OAuthSession session;
        //コンシューマーキー取得
        String consumer_key = TwitterKey.consumer_key;
        String consumer_secret = TwitterKey.consumer_secret;

        //認証画面出す
        public async void showTwitterParge()
        {
            //認証画面をブラウザで開く。
            session = Authorize(consumer_key, consumer_secret);
            //非同期らしいー
            await Windows.System.Launcher.LaunchUriAsync(new Uri(session.AuthorizeUri.AbsoluteUri));
        }

        //PINコードからアクセストークン発行
        public async void getAccessToken(string pin)
        {
            //ダイアログの認証押したとき

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
    }
}
