using Newtonsoft.Json;
using NicoLiveAlertTwitterCS.JSONClass;
using NicoLiveAlertTwitterCS.ListViewClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace NicoLiveAlertTwitterCS.View
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class NicoRepoListPage : Page
    {

        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;
        //ListViewのでーた
        private ObservableCollection<ProgramListViewData> list = new ObservableCollection<ProgramListViewData>();

        public NicoRepoListPage()
        {
            this.InitializeComponent();
            loadNicoRepo();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //更新ボタン
            loadNicoRepo();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //予約枠自動入場登録ボタン
        }

        private async void loadNicoRepo()
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
                foreach (var json in jsonObject.data)
                {
                    //ニコレポには生放送以外の内容も流れてくるので生放送だけふるう
                    if (json.program != null)
                    {
                        //予約枠の投稿だけひろう
                        if (json.topic == "live.user.program.reserve")
                        {
                            var name = $"{json.program.title} | {json.community.name} | {json.program.id}";
                            var pos = jsonObject.data.IndexOf(json);
                            //時刻
                            var date = DateTime.Parse(json.program.beginAt);
                            //DateTime→UnixTime
                            var unix = new DateTimeOffset(date.Ticks, new TimeSpan(+09, 00, 00));

                            var item = new ProgramListViewData { Name = name, beginAt = unix.ToUnixTimeSeconds(), ID = json.program.id, Pos = pos };
                            list.Add(item);
                            nico_repo_listview.ItemsSource = list;
                        }
                    }
                }
            }
        }
    }
}
