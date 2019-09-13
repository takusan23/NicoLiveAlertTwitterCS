using AdaptiveCards;
using CoreTweet;
using CoreTweet.Streaming;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserActivities;
using Windows.UI.Notifications;
using Windows.UI.Shell;
using Windows.UI.Xaml.Controls;

namespace NicoLiveAlertTwitterCS.Twitter
{
    class FilterStream
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;

        public CancellationToken cancelToken;
        public CancellationTokenSource tokenSource;
        //Timeline
        UserActivitySession _currentActivity;
        AdaptiveCard card;

        public void connectFilterStream(Page page)
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
                    try
                    {
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
                                    /*
                                                                    Debug.WriteLine("-------------------");
                                                                    Debug.WriteLine(tw.User.Name);
                                                                    Debug.WriteLine(tw.Text);
                                                                    Debug.WriteLine("-------------------");

                                    */
                                    await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                  {
                                      //UIスレッドで動く
                                      if (!string.IsNullOrEmpty(findProgramId(tw.Text)))
                                      {
                                          lunchBrowser(findProgramId(tw.Text));
                                          showNotification(tw);
                                          setMicrosoftTimeline(tw);
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

                    }
                    catch (TwitterException e)
                    {
                        await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            //エラー？
                            ContentDialog errorDiaog = new ContentDialog
                            {
                                Title = "リアルタイム更新を有効にできませんでした。",
                                Content = "Twitter APIの制限にかかった可能性があります。その場合はしばらく待ってから再度接続してみて下さい。",
                                CloseButtonText = "閉じる"
                            };
                            await errorDiaog.ShowAsync();
                        });

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

        private async void setMicrosoftTimeline(Status tw)
        {
            CreateAdaptiveCardForTimeline(tw);
            var programId = findProgramId(tw.Text);
            //タイムラインに追加する
            var userChannel = UserActivityChannel.GetDefault();
            var userActivity = await userChannel.GetOrCreateUserActivityAsync($"NicoLiveAlert_TwitterCS_{programId}");

            //設定
            userActivity.VisualElements.DisplayText = $"番組が開始しました。\n{tw.Text}";
            userActivity.VisualElements.Content = AdaptiveCardBuilder.CreateAdaptiveCardFromJson(card.ToJson());
            userActivity.ActivationUri = new Uri("https://live2.nicovideo.jp/watch/" + programId);

            //保存
            await userActivity.SaveAsync();

            _currentActivity?.Dispose();
            _currentActivity = userActivity.CreateSession();
        }

        private void CreateAdaptiveCardForTimeline(Status tw)
        {
            // Create an adaptive card specifically to reference this app in Windows 10 Timeline.
            card = new AdaptiveCard("1.0")
            {
                // Select a good background image.
                BackgroundImage = new Uri(tw.User.ProfileImageUrlHttps)
            };

            // Add a heading to the card, which allows the heading to wrap to the next line if necessary.
            var apodHeading = new AdaptiveTextBlock
            {
                Text = tw.Text,
                Size = AdaptiveTextSize.Large,
                Weight = AdaptiveTextWeight.Bolder,
                Wrap = true,
                MaxLines = 2
            };
            card.Body.Add(apodHeading);

            // Add a description to the card, and note that it can wrap for several lines.
            var apodDesc = new AdaptiveTextBlock
            {
                Text = tw.User.Name,
                Size = AdaptiveTextSize.Default,
                Weight = AdaptiveTextWeight.Lighter,
                Wrap = true,
                MaxLines = 3,
                Separator = true
            };
            card.Body.Add(apodDesc);
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


        //ブラウザ起動
        private async void lunchBrowser(string programId)
        {
            if (!string.IsNullOrEmpty(programId))
            {
                var uri = new Uri("https://live2.nicovideo.jp/watch/" + programId);
                var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            }

        }

        //通知
        private void showNotification(Status tw)
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
                                Text = tw.Text
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = tw.User.ProfileImageUrlHttps
                        }
                    }
                }
            };
            //通知を作成
            var notification = new ToastNotification(content.GetXml());
            //通知を送信
            ToastNotificationManager.CreateToastNotifier().Show(notification);
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

        //私のフォロワー
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



    }
}
