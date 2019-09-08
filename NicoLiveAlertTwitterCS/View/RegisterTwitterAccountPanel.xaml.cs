using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace NicoLiveAlertTwitterCS
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class RegisterTwitterAccountPanel : Page
    {

        CoreTweet.Tokens twitter;
        //ListView
        private ObservableCollection<AccountListViewData> list = new ObservableCollection<AccountListViewData>();
        //設定
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;

        //アカウント、IDの配列作っとく？
        List<string> nameList = new List<string>();
        List<string> idList = new List<string>();

        public RegisterTwitterAccountPanel()
        {
            this.InitializeComponent();

            //ログイン情報取得
            if (setting.Values["consumer_key"] != null)
            {
                var consumer_key = setting.Values["consumer_key"].ToString();
                var consumer_secret = setting.Values["consumer_secret"].ToString();
                var access_token = setting.Values["access_token"].ToString();
                var access_token_secret = setting.Values["access_token_secret"].ToString();
                twitter = CoreTweet.Tokens.Create(consumer_key, consumer_secret, access_token, access_token_secret);
            }
            else
            {
                //ログインしてね
                showLoginMessage();
            }

            //ListView表示
            setListView();
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

      

        private void Account_add_button_Click(object sender, RoutedEventArgs e)
        {
            //アカウント追加ボタンおした
            var id = account_textblock.Text;

            //アカウント検索
            var user = twitter.Users.Show(screen_name: id);

            //追加
            if (setting.Values["account_list"] != null)
            {
                //追加
                var account_list = setting.Values["account_list"].ToString();
                var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>(account_list);
                accountJSONArray.Add(new NicoFavListJSON { Name = user.Name, ID = user.Id.ToString() });
                //JSON配列に変換
                setting.Values["account_list"] = JsonConvert.SerializeObject(accountJSONArray);
            }
            else
            {
                //初めて
                var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>("[]");
                accountJSONArray.Add(new NicoFavListJSON { Name = user.Name, ID = user.Id.ToString() });
                //JSON配列に変換
                setting.Values["account_list"] = JsonConvert.SerializeObject(accountJSONArray);
            }

            //ListView更新
            setListView();
        }

        private void setListView()
        {
            //空にする
            list.Clear();
            //読み込む
            if (setting.Values["account_list"] != null)
            {
                //存在チェック通過
                var account_list = setting.Values["account_list"].ToString();
                var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>(account_list);
                foreach (NicoFavListJSON account in accountJSONArray)
                {
                    //配列に入れる
                    list.Add(new AccountListViewData { Name = account.Name, ID = account.ID, Pos = accountJSONArray.IndexOf(account) });
                    nameList.Add(account.Name);
                    idList.Add(account.ID);
                    //ListViewに設定
                    account_listview.ItemsSource = list;
                }
            }
        }

        private async void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            //削除ボタンおしたとき

            //ダイアログ出す
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "削除しますか？",
                PrimaryButtonText = "削除",
                CloseButtonText = "キャンセル"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();
            //押したとき
            if (result == ContentDialogResult.Primary)
            {
                //削除決行
                var button = (Button)sender;
                var pos = button.Tag.ToString();
                var account_list = setting.Values["account_list"].ToString();
                var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>(account_list);
                accountJSONArray.RemoveAt(int.Parse(pos));
                //accountJSONArray.RemoveAt(delete_pos);
                //保存
                setting.Values["account_list"] = JsonConvert.SerializeObject(accountJSONArray);
                //ListView更新
                setListView();
            }
        }
    }
}
