using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
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
    public sealed partial class SettingsPage : Page
    {
        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;
        public SettingsPage()
        {
            this.InitializeComponent();

            //タイトル
            ApplicationView appView = ApplicationView.GetForCurrentView();
            appView.Title = "ニコ生アラート（青鳥CS） 設定";

        }

        private void FilterStreamToggleSwitch(object sender, RoutedEventArgs e)
        {
            //FilterStreamスイッチ
            var toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch.IsOn == true)
            {
                setting.Values["lunch_filterstream"] = true;
            }
            else
            {
                setting.Values["lunch_filterstream"] = false;
            }
        }
    }
}
