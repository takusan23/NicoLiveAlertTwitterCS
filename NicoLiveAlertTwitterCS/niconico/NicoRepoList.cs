using Newtonsoft.Json;
using NicoLiveAlertTwitterCS.JSONClass;
using NicoLiveAlertTwitterCS.ListViewClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using Windows.UI.Xaml.Controls;

namespace NicoLiveAlertTwitterCS.niconico
{
    class NicoRepoList
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;
        //ListViewのでーた
        public ObservableCollection<ProgramListViewData> list = new ObservableCollection<ProgramListViewData>();

        public async void loadNicoRepo(Boolean dialogShow)
        {
            list.Clear();
            //今のUnixTime
            long nowUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (setting.Values["user_session"] != null)
            {
                var urlString = "https://www.nicovideo.jp/api/nicorepo/timeline/my/all?client_app=pc_myrepo";
                var client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "NicoLiveAlert_Twitter;@takusan_23");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "user_session=" + setting.Values["user_session"]);
                using (var stream = await client.GetAsync(new Uri(urlString)))
                {
                    if (stream.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = await stream.Content.ReadAsStringAsync();
                        var jsonObject = JsonConvert.DeserializeObject<NicoRepoRootObject>(jsonString);

                        //forEach
                        var pos = 0;
                        foreach (var json in jsonObject.data)
                        {
                            //ニコレポには生放送以外の内容も流れてくるので生放送だけふるう
                            if (json.program != null)
                            {
                                //予約枠の投稿だけひろう
                                if (json.topic == "live.user.program.reserve")
                                {
                                    //番組開始時刻
                                    var date = DateTime.Parse(json.program.beginAt);
                                    //DateTime→UnixTime
                                    var unix = new DateTimeOffset(date.Ticks, new TimeSpan(+09, 00, 00));
                                    //すでに終わってる予約枠は拾わない
                                    if (nowUnixTime <= unix.ToUnixTimeSeconds())
                                    {
                                        var name = $"{json.program.title} | {json.community.name} | {json.program.id}";
                                        var item = new ProgramListViewData { Name = name, beginAt = unix.ToUnixTimeSeconds(), ID = json.program.id, Pos = pos };
                                        list.Add(item);
                                        pos += 1;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //user_session再取得
                        var nicologin = new NicoLogin();
                        nicologin.ReNiconicoLogin();
                    }

                }
            }
            else
            {
                if (dialogShow)
                {
                    showLoginMessage();
                }
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

        private async void showAPIErrorDialog()
        {
            //ダイアログ出す
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "niconicoへ再ログインします。",
                PrimaryButtonText = "閉じる"
            };

            await deleteFileDialog.ShowAsync();
        }

        public async void addAdmissionProgram(int pos)
        {
            //登録ボタン押した

            var item = list[pos];

            //登録ボタン押した。ダイアログだす
            var dateTime = FromUnixTime(item.beginAt);
            //ダイアログ出す
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "この番組は開場時間になったら自動で入場します。",
                Content = $"{item.Name}\n入場 : {dateTime.ToString()}",
                PrimaryButtonText = "登録",
                CloseButtonText = "キャンセル"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();
            //押したとき
            if (result == ContentDialogResult.Primary)
            {
                //追加
                if (setting.Values["auto_admission_list"] != null)
                {
                    //追加
                    var account_list = setting.Values["auto_admission_list"].ToString();
                    var accountJSONArray = JsonConvert.DeserializeObject<List<AutoAdmissionJSON>>(account_list);
                    accountJSONArray.Add(new AutoAdmissionJSON { Name = item.Name, ID = item.ID, UnixTime = item.beginAt });
                    //JSON配列に変換
                    setting.Values["auto_admission_list"] = JsonConvert.SerializeObject(accountJSONArray);
                }
                else
                {
                    //初めて
                    var accountJSONArray = JsonConvert.DeserializeObject<List<AutoAdmissionJSON>>("[]");
                    accountJSONArray.Add(new AutoAdmissionJSON { Name = item.Name, ID = item.ID, UnixTime = item.beginAt });
                    //JSON配列に変換
                    setting.Values["auto_admission_list"] = JsonConvert.SerializeObject(accountJSONArray);
                }
            }
        }

        public DateTime FromUnixTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
        }


    }
}