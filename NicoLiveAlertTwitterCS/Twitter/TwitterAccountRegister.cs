using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace NicoLiveAlertTwitterCS.Twitter
{
    class TwitterAccountRegister
    {
        //Twitter API
        CoreTweet.Tokens twitter;
        //ListView
        public ObservableCollection<AccountListViewData> list = new ObservableCollection<AccountListViewData>();
        //設定
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;
        //アカウント、IDの配列作っとく？
        List<string> nameList = new List<string>();
        List<string> idList = new List<string>();

        public void initTwitter(Boolean showDialog)
        {
            //初期化。引数はログイン無いときダイアログだすか
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
                if (showDialog)
                {
                    //ログインしてね
                    showLoginMessage();
                }
            }
            //ListView読み込み
            setListViewData();
        }


        public async void showLoginMessage()
        {
            //ダイアログ出す
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "ログインして下さい",
                PrimaryButtonText = "閉じる"
            };

            await deleteFileDialog.ShowAsync();
        }



        public void addAccount(string name)
        {
            initTwitter(true);
            //アカウント登録
            //アカウント検索
            var user = twitter.Users.Show(screen_name: name);

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
            setListViewData();
        }

        public void setListViewData()
        {
            //ListViewのデータを入れる。

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
                }
            }
        }

        public async void deleteAccount(int pos)
        {
            //アカウント削除。引数は配列の位置です。

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
                var account_list = setting.Values["account_list"].ToString();
                var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>(account_list);
                accountJSONArray.RemoveAt(pos);
                //accountJSONArray.RemoveAt(delete_pos);
                //保存
                setting.Values["account_list"] = JsonConvert.SerializeObject(accountJSONArray);
                //ListView更新
                setListViewData();
            }

        }
    }
}
