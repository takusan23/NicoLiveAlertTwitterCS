using AngleSharp.Html.Parser;
using Newtonsoft.Json;
using NicoLiveAlertTwitterCS.JSONClass;
using NicoLiveAlertTwitterCS.ListViewClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Windows.UI.Xaml.Controls;

namespace NicoLiveAlertTwitterCS.niconico
{
    class NicoLiveFavList
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;

        //ListViewにいれるやつ
        public ObservableCollection<ProgramListViewData> list = new ObservableCollection<ProgramListViewData>();

        public async void loadNicoFavList(Boolean dialogShow)
        {
            //ニコ生フォロー中表示。引数はログイン無い時にダイアログだすかどうか
            list.Clear();
            if (setting.Values["user_session"] != null)
            {
                // タイトルを取得したいサイトのURL
                var urlstring = "https://sp.live.nicovideo.jp/favorites";
                // 指定したサイトのHTMLをストリームで取得する
                var client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "NicoLiveAlert_Twitter;@takusan_23");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "user_session=" + setting.Values["user_session"]);
                using (var stream = await client.GetAsync(new Uri(urlstring)))
                {
                    //var html = await client.GetStringAsync(urlstring);

                    if (stream.StatusCode == HttpStatusCode.OK)
                    {

                        var parser = new HtmlParser();
                        var jsonString = await stream.Content.ReadAsStringAsync();
                        var doc = await parser.ParseDocumentAsync(jsonString);


                        //フォロー中の番組のJSON
                        var json = doc.Head.GetElementsByTagName("script")[3].TextContent;
                        json = HttpUtility.UrlDecode(json);

                        json = json.Replace("window.__initial_state__ = \"", "");
                        json = json.Replace("locationBeforeTransitions\":null}}\";", "locationBeforeTransitions\":null}}");
                        json = json.Replace("window.__public_path__ = \"https://nicolive.cdn.nimg.jp/relive/sp/\";", "");


                        var nicoJSON = JsonConvert.DeserializeObject<RootObject>(json);

                        //forEachで取り出す
                        if (nicoJSON != null)
                        {
                            var pos = 0;
                            foreach (var program in nicoJSON.pageContents.favorites.favoritePrograms.programs)
                            {
                                //予約枠だけ取得
                                if (program.liveCycle == "BeforeOpen")
                                {
                                    //なんかしらんけどbeginAtがフォロー中番組だけ値が大きすぎるのでUnixTimeにする割り算
                                    var beginTime = program.beginAt / 1000L;
                                    var item = new ProgramListViewData { Name = program.title + " | " + program.socialGroupName + " | " + program.id, Pos = pos, beginAt = beginTime, ID = program.id };
                                    pos += 1;
                                    list.Add(item);
                                }
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

        
        public async void addAdmissionProgram(int pos)
        {
            //登録ボタン押した。ダイアログだす
            var item = list[pos];
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
                    var account_list = setting.Values["auto_admission_list"].ToString();
                    var accountJSONArray = JsonConvert.DeserializeObject<List<AutoAdmissionJSON>>(account_list);
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
