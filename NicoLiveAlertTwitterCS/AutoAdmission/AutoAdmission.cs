using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using NicoLiveAlertTwitterCS.JSONClass;
using NicoLiveAlertTwitterCS.ListViewClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NicoLiveAlertTwitterCS.AutoAdmission
{
    class AutoAdmission
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;

        //定期実行
        public DispatcherTimer Timer;

        //リスト
        List<String> liveIdList = new List<String>();
        List<String> nameList = new List<String>();
        List<long> unixTimeList = new List<long>();

        //予約枠自動入場JSON
        List<AutoAdmissionJSON> autoAdmissionJSON;

        public void startAutoAdmission()
        {
            //クリア
            liveIdList.Clear();
            nameList.Clear();
            unixTimeList.Clear();

            //読み込み
            var account_list = setting.Values["auto_admission_list"].ToString();
            autoAdmissionJSON = JsonConvert.DeserializeObject<List<AutoAdmissionJSON>>(account_list);

            //ListView
            foreach (var autoAdmission in autoAdmissionJSON)
            {
                //配列に入れる
                nameList.Add(autoAdmission.Name);
                liveIdList.Add(autoAdmission.ID);
                unixTimeList.Add(autoAdmission.UnixTime);
            }


            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(1000); //間隔
            Timer.Tick += TickTimer;
            Timer.Start();
        }

        private void TickTimer(object sender, object e)
        {
            //クリア
            liveIdList.Clear();
            nameList.Clear();
            unixTimeList.Clear();

            //読み込み
            var account_list = setting.Values["auto_admission_list"].ToString();
            autoAdmissionJSON = JsonConvert.DeserializeObject<List<AutoAdmissionJSON>>(account_list);

            //ListView
            foreach (var autoAdmission in autoAdmissionJSON)
            {
                //配列に入れる
                nameList.Add(autoAdmission.Name);
                liveIdList.Add(autoAdmission.ID);
                unixTimeList.Add(autoAdmission.UnixTime);
            }

            //ここで定期実行される
            //今のUnixTime取得
            long nowUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (unixTimeList.Contains(nowUnixTime))
            {
                //始まった！
                var index = unixTimeList.IndexOf(nowUnixTime);
                var name = nameList[index];
                var liveId = liveIdList[index];

                //ブラウザ起動
                lunchBrowser(liveId);

                //通知出す
                showNotification(name);

                //開場したので配列から
                autoAdmissionJSON.RemoveAt(index);
                //保存
                setting.Values["auto_admission_list"] = JsonConvert.SerializeObject(autoAdmissionJSON);
            }
        }


        //通知
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

        //ブラウザ起動
        private async void lunchBrowser(string programId)
        {
            var uri = new Uri("https://live2.nicovideo.jp/watch/" + programId);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}
