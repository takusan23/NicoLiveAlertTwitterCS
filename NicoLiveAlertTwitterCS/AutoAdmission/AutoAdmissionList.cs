using Newtonsoft.Json;
using NicoLiveAlertTwitterCS.JSONClass;
using NicoLiveAlertTwitterCS.ListViewClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace NicoLiveAlertTwitterCS.AutoAdmission
{
    class AutoAdmissionList
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;
        //ListViewのデータ
        public ObservableCollection<ProgramListViewData> list = new ObservableCollection<ProgramListViewData>();

        public void loadList(Boolean showDialog)
        {
            list.Clear();
            //追加
            if (setting.Values["auto_admission_list"] != null)
            {
                //追加
                var account_list = setting.Values["auto_admission_list"].ToString();
                var admissionArray = JsonConvert.DeserializeObject<List<AutoAdmissionJSON>>(account_list);


                //ListView
                foreach (var autoAdmission in admissionArray)
                {
                    var pos = admissionArray.IndexOf(autoAdmission);
                    var item = new ProgramListViewData { Name = autoAdmission.Name, Pos = pos };
                    list.Add(item);
                }

            }
            else
            {
                if (showDialog)
                {
                    //登録されてないよ！
                    showEmptyMessage();
                }
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


        public async void deleteAutoAdmission(int pos)
        {
            //削除ボタンおした

            var account_list = setting.Values["auto_admission_list"].ToString();
            var accountJSONArray = JsonConvert.DeserializeObject<List<NicoFavListJSON>>(account_list);

            //ダイアログ出す
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "削除しますか？",
                Content = accountJSONArray[pos].Name,
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
                setting.Values["auto_admission_list"] = JsonConvert.SerializeObject(accountJSONArray);
                //ListView更新
                loadList(false);
            }
        }

    }
}
