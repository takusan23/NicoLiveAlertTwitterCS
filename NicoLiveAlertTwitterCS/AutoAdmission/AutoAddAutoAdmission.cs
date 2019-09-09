using AngleSharp.Html.Parser;
using Newtonsoft.Json;
using NicoLiveAlertTwitterCS.JSONClass;
using NicoLiveAlertTwitterCS.ListViewClass;
using NicoLiveAlertTwitterCS.niconico;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NicoLiveAlertTwitterCS.AutoAdmission
{
    class AutoAddAutoAdmission
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;
        //ListViewデータ
        public ObservableCollection<NicoLiveAlertTwitterCS.ListViewClass.AutoAddAutoAdmissionListViewData> list = new ObservableCollection<ListViewClass.AutoAddAutoAdmissionListViewData>();


        //定期的にニコレポを見る
        public DispatcherTimer nicorepoTimer;

        //定期的にフォロー中番組を見る
        public DispatcherTimer followTimer;

        public void loadAutoAddAutoAdmissionList()
        {
            list.Clear();
            //コミュニティリスト読み込み
            if (setting.Values["autoadd_community"] != null)
            {
                //存在チェック通過
                var account_list = setting.Values["autoadd_community"].ToString();
                var jsonArray = JsonConvert.DeserializeObject<List<AutoAddAutoAdmissionListViewData>>(account_list);
                var pos = 0;
                foreach (var community in jsonArray)
                {
                    var item = new AutoAddAutoAdmissionListViewData { ID = community.ID, Pos = pos };
                    list.Add(item);
                    pos += 1;
                }
            }
        }

        public void addAutoAddAutoAdmissionList(string communityID)
        {
            ///予約枠自動登録自動入場リスト追加
            if (setting.Values["autoadd_community"] != null)
            {
                //追加
                var account_list = setting.Values["autoadd_community"].ToString();
                var accountJSONArray = JsonConvert.DeserializeObject<List<AutoAddAutoAdmissionListViewData>>(account_list);
                accountJSONArray.Add(new AutoAddAutoAdmissionListViewData { ID = communityID });
                //JSON配列に変換
                setting.Values["autoadd_community"] = JsonConvert.SerializeObject(accountJSONArray);
            }
            else
            {
                //初めて
                var accountJSONArray = JsonConvert.DeserializeObject<List<AutoAddAutoAdmissionListViewData>>("[]");
                accountJSONArray.Add(new AutoAddAutoAdmissionListViewData { ID = communityID });
                //JSON配列に変換
                setting.Values["autoadd_community"] = JsonConvert.SerializeObject(accountJSONArray);
            }
            loadAutoAddAutoAdmissionList();
        }

        public async void deleteAutoAddAutoAdmissionList(int pos)
        {
            //削除
            //ダイアログ出す
            var account_list = setting.Values["autoadd_community"].ToString();
            var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>(account_list);
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "削除しますか？",
                Content = accountJSONArray[pos].ID,
                PrimaryButtonText = "削除",
                CloseButtonText = "キャンセル"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();
            //押したとき
            if (result == ContentDialogResult.Primary)
            {
                //削除決行
                accountJSONArray.RemoveAt(pos);
                //accountJSONArray.RemoveAt(delete_pos);
                //保存
                setting.Values["autoadd_community"] = JsonConvert.SerializeObject(accountJSONArray);
                //ListView更新
                loadAutoAddAutoAdmissionList();
            }
        }

        public void timerFollowAutoAddAdmission()
        {
            //間隔
            var space = "10";
            if (setting.Values["autoadd_time"] != null)
            {
                space = setting.Values["autoadd_time"].ToString();
            }
            //フォロー中定期実行
            followTimer = new DispatcherTimer();
            followTimer.Interval = TimeSpan.FromMinutes(int.Parse(space)); //間隔
            followTimer.Tick += FollowTimer;
            followTimer.Start();
        }


        public void timerNicorepoAutoAddAdmission()
        {
            //間隔
            var space = "10";
            if (setting.Values["autoadd_time"] != null)
            {
                space = setting.Values["autoadd_time"].ToString();
            }
            //ニコレポ定期実行
            nicorepoTimer = new DispatcherTimer();
            nicorepoTimer.Interval = TimeSpan.FromMinutes(int.Parse(space)); //間隔
            nicorepoTimer.Tick += NicorepoTimerAsync;
            nicorepoTimer.Start();
        }

        private async void NicorepoTimerAsync(object sender, object e)
        {
            //ニコレポ巡回
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
                                        //予約枠自動入場
                                        //追加                           
                                        addAdmissionList(json.program.title, json.program.id, unix.ToUnixTimeSeconds(), json.communityForFollower.id);
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
        }

        private async void FollowTimer(object sender, object e)
        {
            //フォロー中巡回
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
                                    //予約枠自動入場
                                    //フォロー中の場合はコミュニティIDが取れない問題。
                                    //めんどくさいけどコミュのサムネURLの一部にコミュのID入ってるので正規表現で取り出す。
                                    var communityId = regexCommunityID(program.socialGroupThumbnailUrl);
                                    addAdmissionList(program.title, program.id, beginTime, communityId);
                                }
                            }
                        }
                    }
                }
            }
        }

        //正規表現でコミュのIDを取り出します。
        public string regexCommunityID(string text)
        {
            Match matche = Regex.Match(text, "(co|ch)([0-9]+)");
            if (Regex.IsMatch(text, "(co|ch)([0-9]+)"))
            {
                //一致した。
                return matche.Value;
            }
            else
            {
                return "";
            }
        }

        public void addAdmissionList(string name, string id, long unix, string communityId)
        {
            //予約枠自動登録するコミュニティリスト
            //設定読み込み
            var communityList = new List<string>();
            var communityString = setting.Values["autoadd_community"].ToString();
            var communityJsonArray = JsonConvert.DeserializeObject<List<AutoAddAutoAdmissionListViewData>>(communityString);
            foreach (var item in communityJsonArray)
            {
                communityList.Add(item.ID);
            }

            //今の予約枠自動登録リスト
            //設定読み込み
            var autoAddList = new List<string>();
            var addAdmissionString = setting.Values["auto_admission_list"].ToString();
            var addAdmissionJsonArray = JsonConvert.DeserializeObject<List<AutoAdmissionJSON>>(addAdmissionString);
            foreach (var item in addAdmissionJsonArray)
            {
                autoAddList.Add(item.ID);
            }


            //追加済みコミュニティだった！
            if (communityList.Contains(communityId))
            {
                //すでに追加済みの可能性
                if (!autoAddList.Contains(id))
                {
                    if (setting.Values["auto_admission_list"] != null)
                    {
                        //追加
                        var account_list = setting.Values["auto_admission_list"].ToString();
                        var accountJSONArray = JsonConvert.DeserializeObject<List<AutoAdmissionJSON>>(account_list);
                        accountJSONArray.Add(new AutoAdmissionJSON { Name = name, ID = id, UnixTime = unix });
                        //JSON配列に変換
                        setting.Values["auto_admission_list"] = JsonConvert.SerializeObject(accountJSONArray);
                    }
                    else
                    {
                        //初めて
                        var accountJSONArray = JsonConvert.DeserializeObject<List<AutoAdmissionJSON>>("[]");
                        accountJSONArray.Add(new AutoAdmissionJSON { Name = name, ID = id, UnixTime = unix });
                        //JSON配列に変換
                        setting.Values["auto_admission_list"] = JsonConvert.SerializeObject(accountJSONArray);
                    }
                }
            }


        }

    }
}
