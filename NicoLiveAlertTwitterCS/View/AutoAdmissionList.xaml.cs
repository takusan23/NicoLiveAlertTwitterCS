using Newtonsoft.Json;
using NicoLiveAlertTwitterCS.ListViewClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NicoLiveAlertTwitterCS.View
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class AutoAdmissionList : Page
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;
        //ListViewのデータ
        private ObservableCollection<ProgramListViewData> list = new ObservableCollection<ProgramListViewData>();

        public AutoAdmissionList()
        {
            this.InitializeComponent();
            loadList();
        }

        private void loadList()
        {
            list.Clear();
            //追加
            if (setting.Values["auto_admission_list"] != null)
            {
                //追加
                var account_list = setting.Values["auto_admission_list"].ToString();
                var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>(account_list);

                //ListView
                foreach (var autoAdmission in accountJSONArray)
                {
                    var pos = accountJSONArray.IndexOf(autoAdmission);
                    var item = new ProgramListViewData { Name = autoAdmission.Name, Pos = pos };
                    list.Add(item);
                    auto_admission_listview.ItemsSource = list;
                }

            }
            else
            {
                //登録されてないよ！
                showEmptyMessage();
            }
        }
        private async void showEmptyMessage()
        {
            //ダイアログ出す
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "予約枠自動入場を登録してみよう！",
                PrimaryButtonText = "閉じる"
            };

            await deleteFileDialog.ShowAsync();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //削除ボタンおした

            //内容取り出す
            var button = (Button)sender;
            var pos = button.Tag.ToString();
            var account_list = setting.Values["auto_admission_list"].ToString();
            var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>(account_list);

            //ダイアログ出す
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "削除しますか？",
                Content = accountJSONArray[int.Parse(pos)].Name,
                PrimaryButtonText = "削除",
                CloseButtonText = "キャンセル"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();
            //押したとき
            if (result == ContentDialogResult.Primary)
            {
                //削除決行           
                accountJSONArray.RemoveAt(int.Parse(pos));
                //accountJSONArray.RemoveAt(delete_pos);
                //保存
                setting.Values["auto_admission_list"] = JsonConvert.SerializeObject(accountJSONArray);
                //ListView更新
                loadList();
            }
        }
    }
}
