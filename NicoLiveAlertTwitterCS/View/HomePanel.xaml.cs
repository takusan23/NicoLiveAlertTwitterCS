using CoreTweet;
using CoreTweet.Streaming;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace NicoLiveAlertTwitterCS
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class HomePanel : Page
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;

        CancellationToken cancelToken;
        CancellationTokenSource tokenSource;


        public HomePanel(Page page)
        {
            this.InitializeComponent();

            //lunchBrowser(findProgramId("【生放送】暗黒放送  怒りの枠！旅部をぶっ潰したニコニコの運営に居座る無能野郎は許さない放送 を開始しました。 nico.ms/lv321820618?ni… #lv321820618"));
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    //接続
                    connectFilterStream();
                }
                else
                {
                    //終了
                    tokenSource.Cancel();
                }
            }
        }

        //引数は利用するかしないか

        private void connectFilterStream()
        {
            //ログイン情報取得
            if (setting.Values["consumer_key"] != null)
            {
                //スレッド止める時に使うらしい。
                tokenSource = new CancellationTokenSource();
                cancelToken = tokenSource.Token;

                //認証取得
                var consumer_key = setting.Values["consumer_key"].ToString();
                var consumer_secret = setting.Values["consumer_secret"].ToString();
                var access_token = setting.Values["access_token"].ToString();
                var access_token_secret = setting.Values["access_token_secret"].ToString();
                var twitter = CoreTweet.Tokens.Create(consumer_key, consumer_secret, access_token, access_token_secret);

                //ID取得
                var account_list = setting.Values["account_list"].ToString();
                var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>(account_list);
                List<long> ids = new List<long>();
                foreach (var id in accountJSONArray)
                {
                    ids.Add(long.Parse(id.ID));
                }

                //FilterStreamの検証用。
                //ids = getTakusan23Followers();

                //FilterStream
                Task task = new Task(async () =>
                {
                    Debug.WriteLine("接続します");
                    var stream = twitter.Streaming.Filter(follow: ids).OfType<StatusMessage>().Select(x => x.Status);
                    foreach (var tw in stream)
                    {
                        //キャンセルされてるか
                        if (!cancelToken.IsCancellationRequested)
                        {
                            //キャンセルされていないときは続ける
                            //本人以外（RTなんかも拾ってしまう）のツイートには反応しない
                            if (ids.Contains(tw.User.Id ?? 0))
                            {
                                /*                                Debug.WriteLine(tw.User.Name);
                                                                Debug.WriteLine(tw.Text);
                                                                Debug.WriteLine("-------------------");
                                */
                                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                              {
                                  if (string.IsNullOrEmpty(findProgramId(tw.Text)))
                                  {
                                      lunchBrowser(findProgramId(tw.Text));
                                      showNotification(tw.Text);
                                  }
                              });

                            }
                        }
                        else
                        {
                            //キャンセルされたので終了
                            return;
                        }
                    }
                }, cancelToken);
                task.Start();

            }
            else
            {
                //ログインしてね
                showLoginMessage();
            }


        }

        private async void showLoginMessage()
        {
            //ダイアログ出す
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "ログインして下さい",
                PrimaryButtonText = "閉じる"
            };

            await deleteFileDialog.ShowAsync();
        }

        private List<long> getTakusan23Followers()
        {
            var consumer_key = setting.Values["consumer_key"].ToString();
            var consumer_secret = setting.Values["consumer_secret"].ToString();
            var access_token = setting.Values["access_token"].ToString();
            var access_token_secret = setting.Values["access_token_secret"].ToString();
            var twitter = CoreTweet.Tokens.Create(consumer_key, consumer_secret, access_token, access_token_secret);
            List<long> list = new List<long>();
            var follows = twitter.Followers.Ids(screen_name: "takusan__23");
            foreach (var id in follows)
            {
                list.Add(id);
            }
            return list;
        }

        //ブラウザ起動
        private async void lunchBrowser(string programId)
        {
            if (!string.IsNullOrEmpty(programId))
            {
                var uri = new Uri("https://live2.nicovideo.jp/watch/" + programId);
                var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            }

        }

        //ツイート内容から番組IDを取得する（正規表現）
        private String findProgramId(string text)
        {
            Match matche = Regex.Match(text, "(lv)([0-9]+)");
            if (Regex.IsMatch(text, "(lv)([0-9]+)"))
            {
                //一致した。
                return matche.Value;

            }
            else
            {
                return "";
            }
        }

        private void showNotification(string value)
        {

            var content = new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Children = {
                            new AdaptiveText
                            {
                                Text = "番組が開始しました"
                            },
                            new AdaptiveText
                            {
                                Text = value
                            }
                        }
                    }
                }
            };
            //通知を作成
            var notification = new ToastNotification(content.GetXml());
            //通知を送信
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }

        

    }

}
