using AngleSharp.Html.Parser;
using Newtonsoft.Json;
using NicoLiveAlertTwitterCS.ListViewClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace NicoLiveAlertTwitterCS.View
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class NicoLiveFavouritePage : Page
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;

        //ListViewにいれるやつ
        private ObservableCollection<ProgramListViewData> list = new ObservableCollection<ProgramListViewData>();

        public NicoLiveFavouritePage()
        {
            this.InitializeComponent();
            loadNicoFavList();
        }

        private async void loadNicoFavList()
        {
            list.Clear();
            if (setting.Values["user_session"] != null)
            {
                // タイトルを取得したいサイトのURL
                var urlstring = "https://sp.live.nicovideo.jp/favorites";
                // 指定したサイトのHTMLをストリームで取得する
                var client = new HttpClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "NicoLiveAlert_Twitter;@takusan_23");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", "user_session=" + setting.Values["user_session"]);
                using (var stream = await client.GetStreamAsync(new Uri(urlstring)))
                {
                    //var html = await client.GetStringAsync(urlstring);


                    var parser = new HtmlParser();
                    var doc = await parser.ParseDocumentAsync(stream);

                    // HtmlParserクラスをインスタンス化
                    //var parser = new HtmlParser();
                    // HtmlParserクラスのParserメソッドを使用してパースする。
                    // Parserメソッドの戻り値の型はIHtmlDocument
                    //var htmlDocument = parser.ParseDocument(html);

                    //フォロー中の番組のJSON
                    var json = doc.Head.GetElementsByTagName("script")[3].TextContent;
                    json = HttpUtility.UrlDecode(json);

                    json = json.Replace("window.__initial_state__ = \"", "");
                    json = json.Replace("locationBeforeTransitions\":null}}\";", "locationBeforeTransitions\":null}}");
                    json = json.Replace("window.__public_path__ = \"https://nicolive.cdn.nimg.jp/relive/sp/\";", "");


                    var nicoJSON = JsonConvert.DeserializeObject<RootObject>(json);

                    //forEachで取り出す
                    foreach (var program in nicoJSON.pageContents.favorites.favoritePrograms.programs)
                    {
                        //予約枠だけ取得
                        if (program.liveCycle == "BeforeOpen")
                        {
                            var pos = nicoJSON.pageContents.favorites.favoritePrograms.programs.IndexOf(program);
                            var item = new ProgramListViewData { Name = program.title + " | " + program.socialGroupName + " | " + program.id, Pos = pos, beginAt = program.beginAt, ID = program.id };
                            list.Add(item);
                            nico_fav_listview.ItemsSource = list;
                        }
                    }
                }
            }
            else
            {
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loadNicoFavList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //登録ボタン押した

            var pos = (sender as Button).Tag.ToString();
            var item = list[int.Parse(pos)];

            //追加
            if (setting.Values["auto_admission_list"] != null)
            {
                //追加
                var account_list = setting.Values["auto_admission_list"].ToString();
                var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>(account_list);
                accountJSONArray.Add(new NicoFavListJSON { Name = item.Name, ID = item.ID });
                //JSON配列に変換
                setting.Values["auto_admission_list"] = JsonConvert.SerializeObject(accountJSONArray);
            }
            else
            {
                //初めて
                var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>("[]");
                accountJSONArray.Add(new NicoFavListJSON { Name = item.Name, ID = item.ID });
                //JSON配列に変換
                setting.Values["auto_admission_list"] = JsonConvert.SerializeObject(accountJSONArray);
            }

        }
    }
}
