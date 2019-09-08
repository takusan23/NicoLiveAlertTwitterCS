using Newtonsoft.Json;
using NicoLiveAlertTwitterCS.JSONClass;
using NicoLiveAlertTwitterCS.ListViewClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            if (setting.Values["user_session"] != null)
            {
                var urlString = "https://www.nicovideo.jp/api/nicorepo/timeline/my/all?client_app=pc_myrepo";
                var client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "NicoLiveAlert_Twitter;@takusan_23");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "user_session=" + setting.Values["user_session"]);
                using (var stream = await client.GetAsync(new Uri(urlString)))
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
                                var name = $"{json.program.title} | {json.community.name} | {json.program.id}";
                                //時刻
                                var date = DateTime.Parse(json.program.beginAt);
                                //DateTime→UnixTime
                                var unix = new DateTimeOffset(date.Ticks, new TimeSpan(+09, 00, 00));

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

        public void addAdmissionProgram(int pos)
        {
            //登録ボタン押した

            var item = list[pos];

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

}