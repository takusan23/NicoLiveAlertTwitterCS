using NicoLiveAlertTwitterCS.AutoAdmission;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
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

        //リンク
        string github = "https://github.com/takusan23/NicoLiveAlertTwitterCS";
        string twitter = "https://twitter.com/takusan__23";
        string mastodon = "https://best-friends.chat/@takusan_23";

        //データ保存とか
        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;

        List<Grid> pageList = new List<Grid>();

        //予約枠自動登録自動入場
        AutoAddAutoAdmission addAutoAdmission = new AutoAddAutoAdmission();

        public SettingsPage()
        {
            this.InitializeComponent();

            //設定項目配列に入れる
            pageList.Add(setting_lunch_setting_grid);
            pageList.Add(setting_appearance_setting_grid);
            pageList.Add(setting_autoadd_autoadmission_setting_grid);
            pageList.Add(setting_info_setting_grid);

            //はじめは「起動時の設定」を表示させる
            pageList[0].Visibility = Visibility.Visible;
            settingNavTitle.Text = "起動時の設定";

            //起動時の設定を選択しておく
            settingNavigationView.SelectedItem = settingNavigationView.MenuItems[1];

            //タイトルばーを半透明に
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            //タイトル
            ApplicationView appView = ApplicationView.GetForCurrentView();
            appView.Title = "ニコ生アラート（青鳥CS） 設定";


            setSetting();

            //予約枠自動登録自動入場リスト
            addAutoAdmission.loadAutoAddAutoAdmissionList();
            autoadd_autoadmission_listview.ItemsSource = addAutoAdmission.list;
        }

        private void setSetting()
        {
            //今ある設定項目を読み込んで値をいれる
            //FilterStream接続設定
            setToggleSwitchValue(lunch_filterstream, loadSettingBoolean("setting_lunch_filterstream"));
            setToggleSwitchValue(lunch_autoadd, loadSettingBoolean("setting_lunch_autoadd"));
            //外観の設定
            loadThemeSetting();
            //予約枠自動登録
            //予約枠間隔
            setting_autoadd_autoadmission_time_textbox.Text = loadSettingString("autoadd_time", "10");
        }

        private void SettingToggleSwitch(object sender, RoutedEventArgs e)
        {
            //FilterStreamスイッチ
            var toggleSwitch = sender as ToggleSwitch;
            var key = toggleSwitch.Tag.ToString();
            setting.Values[key] = toggleSwitch.IsOn;
        }

        private Boolean loadSettingBoolean(string keyName)
        {
            //設定内容読み込み。無いときはfalse
            if (setting.Values[keyName] != null)
            {
                //ある
                return Boolean.Parse(setting.Values[keyName].ToString());
            }
            return false;
        }

        private string loadSettingString(string keyName, string defaultValue)
        {
            //設定内容読み込み。
            if (setting.Values[keyName] != null)
            {
                //ある
                return setting.Values[keyName].ToString();
            }
            return defaultValue;
        }

        private void setToggleSwitchValue(ToggleSwitch toggleSwitch, Boolean value)
        {
            //ToggleSwitchに値を設定する
            toggleSwitch.IsOn = value;
        }

        private void SettingClick(object sender, TappedRoutedEventArgs e)
        {
            //設定項目押したとき
            var item = sender as NavigationViewItem;
            chengePage(item.Name);
        }

        private void chengePage(string name)
        {
            //全ての項目を非表示に
            foreach (var grid in pageList)
            {
                grid.Visibility = Visibility.Collapsed;
            }

            //切り替える。名前はxamlのNavigationViewItemと同じで
            switch (name)
            {
                case "setting_lunch":
                    settingNavTitle.Text = "起動時の設定";
                    pageList[0].Visibility = Visibility.Visible;
                    break;
                case "setting_appearance":
                    settingNavTitle.Text = "外観の設定";
                    pageList[1].Visibility = Visibility.Visible;
                    break;
                case "setting_autoadd_autoadmission":
                    settingNavTitle.Text = "予約枠自動登録、入場の設定";
                    pageList[2].Visibility = Visibility.Visible;
                    break;
                case "setting_info":
                    settingNavTitle.Text = "このアプリについて";
                    pageList[3].Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ThemeRadioButton(object sender, TappedRoutedEventArgs e)
        {
            //ダークモード・ライトテーマ
            var radioButton = sender as RadioButton;
            setting.Values["setting_theme"] = radioButton.Name;
        }

        private void loadThemeSetting()
        {
            //ダークモード・ライトテーマを読み込む
            if (setting.Values["setting_theme"] != null)
            {
                switch (setting.Values["setting_theme"])
                {
                    case "setting_theme_dark":
                        setting_theme_dark.IsChecked = true;
                        break;
                    case "setting_theme_light":
                        setting_theme_light.IsChecked = true;
                        break;
                    case "setting_theme_auto":
                        setting_theme_auto.IsChecked = true;
                        break;
                }
            }
        }


        private void AutoAddAutoAdmissionCommunityDelete_Click(object sender, RoutedEventArgs e)
        {
            //コミュニティ削除
            var pos = sender as Button;
            addAutoAdmission.deleteAutoAddAutoAdmissionList(int.Parse(pos.Tag.ToString()));
        }

        private void AutoAddAutoAdmissionCommunityAdd_Click(object sender, RoutedEventArgs e)
        {    
            //コミュニティ追加
            addAutoAdmission.addAutoAddAutoAdmissionList(setting_autoadd_autoadmission_community_textbox.Text);
        }

        private void AutoAddAutoAdmissionTime_Click(object sender, RoutedEventArgs e)
        {
            //間隔
            setting.Values["autoadd_time"] = setting_autoadd_autoadmission_time_textbox.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Github/Twitter/Mastodon
            var button = sender as Button;
            switch (button.Content)
            {
                case "ソースコード":
                    lunchBrowser(github);
                    break;
                case "Twitter":
                    lunchBrowser(twitter);
                    break;
                case "Mastodon":
                    lunchBrowser(mastodon);
                    break;
            }
        }

        private async void lunchBrowser(string url)
        {
            var uri = new Uri(url);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }

    }
}
