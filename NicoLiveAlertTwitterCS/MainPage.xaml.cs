using NicoLiveAlertTwitterCS.AutoAdmission;
using NicoLiveAlertTwitterCS.niconico;
using NicoLiveAlertTwitterCS.Twitter;
using NicoLiveAlertTwitterCS.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace NicoLiveAlertTwitterCS
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Windows.Storage.ApplicationDataContainer setting = Windows.Storage.ApplicationData.Current.RoamingSettings;

        List<Grid> PanelList = new List<Grid>();

        //今開いてるやつ
        String nowParge = "home";

        //Twitterログイン
        TwitterLogin twitterLogin = new TwitterLogin();
        //Twitterアカウント登録・削除
        TwitterAccountRegister twitterAccountRegister = new TwitterAccountRegister();
        //FilterStream
        FilterStream filterStream = new FilterStream();

        //niconicoログイン
        NicoLogin nicoLogin = new NicoLogin();
        //ニコ生フォロー中番組
        NicoLiveFavList nicoLiveFavList = new NicoLiveFavList();
        //ニコレポ
        NicoRepoList nicoRepoList = new NicoRepoList();

        //予約枠自動入場
        NicoLiveAlertTwitterCS.AutoAdmission.AutoAdmissionList autoAdmissionList = new NicoLiveAlertTwitterCS.AutoAdmission.AutoAdmissionList();
        //自動入場タイマー
        NicoLiveAlertTwitterCS.AutoAdmission.AutoAdmission autoAdmission = new NicoLiveAlertTwitterCS.AutoAdmission.AutoAdmission();
        //予約枠自動登録自動入場
        AutoAddAutoAdmission addAutoAdmission = new AutoAddAutoAdmission();

        public MainPage()
        {
            this.InitializeComponent();

            //タイトルばーを半透明に
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            //タイトル
            ApplicationView appView = ApplicationView.GetForCurrentView();
            appView.Title = "ニコ生アラート（青鳥CS）";

            //配列に入れる
            PanelList.Add(login_stackpanel);
            PanelList.Add(home_stackpanel); ;
            PanelList.Add(nicolivefav_stackpanel);
            PanelList.Add(nicorepo_stackpanel);
            PanelList.Add(twitterregister_stackpanel);
            PanelList.Add(autoadmission_stackpanel);

            //はじめはホーム画面出す
            PanelList[1].Visibility = Visibility.Visible;
            //ホームを選択しておく
            navigation_view.SelectedItem = navigation_view.MenuItems[0];

            //Twitterアカウント設定
            twitterAccountRegister.initTwitter(false);
            twitterregister_listview.ItemsSource = twitterAccountRegister.list;

            //ニコ生フォロー中
            nicoLiveFavList.loadNicoFavList(false);
            nico_fav_listview.ItemsSource = nicoLiveFavList.list;

            //ニコレポ
            nicoRepoList.loadNicoRepo(false);
            nico_repo_listview.ItemsSource = nicoRepoList.list;

            //予約枠自動入場
            autoAdmissionList.loadList(false);
            auto_admission_listview.ItemsSource = autoAdmissionList.list;

            //予約枠自動入場有効。
            autoAdmission.startAutoAdmission();


            //設定
            loadSettings();


        }

        private void loadSettings()
        {
            //起動したらすぐにFilterStreamに接続する
            if (loadSettingBoolean("setting_lunch_filterstream"))
            {
                filterStream.connectFilterStream(this);
                home_twitter_stream_switch.IsOn = true;
                home_twitter_stream_textblock.Text = "ニコ生アラート状態：接続中です。";
            }
            if (loadSettingBoolean("setting_lunch_autoadd"))
            {
                filterStream.connectFilterStream(this);
                home_auto_admission_switch.IsOn = true;
                home_auto_admission_textblock.Text = "予約枠自動登録自動入場状態：定期巡回中です。";
            }
        }

        private Boolean loadSettingBoolean(string key)
        {
            //設定をかえすやーつ
            if (setting.Values[key] != null)
            {
                return bool.Parse(setting.Values[key].ToString());
            }
            return false;
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                //設定選択時
                showSetting();
            }
        }

        private void Navigation_view_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //NavItem押したときのやつ
            var tag = (sender as NavigationViewItem).Tag;
            nowParge = tag.ToString();
            showStackPanel(tag.ToString());
        }


        public void changeLayout(String tag)
        {
            /*            switch (tag)
                        {
                            case "home":
                                //ホーム
                                navigation_view.Header = "ホーム";
                                ContentFrame.Navigate(typeof(HomePanel));
                                break;
                            case "register_account":
                                //登録済みアカウント
                                navigation_view.Header = "登録したアカウント";
                                ContentFrame.Navigate(typeof(RegisterTwitterAccountPanel));
                                break;
                            case "alert_history":
                                //アラート履歴
                                navigation_view.Header = "アラート履歴";
                                ContentFrame.Navigate(typeof(HomePanel));
                                break;
                            case "alert_list":
                                //予約リスト
                                navigation_view.Header = "予約リスト";
                                ContentFrame.Navigate(typeof(AutoAdmissionList));
                                break;
                            case "follow_program":
                                //フォロー中
                                navigation_view.Header = "フォロー中の番組";
                                ContentFrame.Navigate(typeof(NicoLiveFavouritePage));
                                break;
                            case "nicorepo":
                                //ニコレポ
                                navigation_view.Header = "ニコレポ";
                                ContentFrame.Navigate(typeof(NicoRepoListPage));
                                break;
                            case "account_setting":
                                //アカウント
                                navigation_view.Header = "アカウント設定";
                                ContentFrame.Navigate(typeof(LoginPage));
                                break;
                        }
            */
        }


        private void showStackPanel(string panelName)
        {
            foreach (var panel in PanelList)
            {
                panel.Visibility = Visibility.Collapsed;
            }

            //アカウント追加系も消す
            account_textblock.Visibility = Visibility.Collapsed;
            account_add_button.Visibility = Visibility.Collapsed;
            AddAdmissionButotn.Visibility = Visibility.Collapsed;

            switch (panelName)
            {
                case "home":
                    NavHeaderTitle.Text = "ホーム";
                    autoAdmission.startAutoAdmission();
                    UpdateButton.Visibility = Visibility.Collapsed;
                    PanelList[1].Visibility = Visibility.Visible;
                    break;
                case "register_account":
                    NavHeaderTitle.Text = "生主アカウント一覧";
                    account_textblock.Visibility = Visibility.Visible;
                    account_add_button.Visibility = Visibility.Visible;
                    UpdateButton.Visibility = Visibility.Collapsed;
                    PanelList[4].Visibility = Visibility.Visible;
                    break;
                case "alert_list":
                    NavHeaderTitle.Text = "予約枠自動入場リスト";
                    autoAdmissionList.loadList(false);
                    autoAdmission.startAutoAdmission();
                    auto_admission_listview.ItemsSource = autoAdmissionList.list;
                    UpdateButton.Visibility = Visibility.Collapsed;
                    PanelList[5].Visibility = Visibility.Visible;
                    AddAdmissionButotn.Visibility = Visibility.Visible;
                    break;
                case "follow_program":
                    NavHeaderTitle.Text = "フォロー中の番組";
                    nicoLiveFavList.loadNicoFavList(false);
                    nico_fav_listview.ItemsSource = nicoLiveFavList.list;
                    UpdateButton.Visibility = Visibility.Visible;
                    PanelList[2].Visibility = Visibility.Visible;
                    break;
                case "nicorepo":
                    NavHeaderTitle.Text = "ニコレポ";
                    //ニコレポ
                    nicoRepoList.loadNicoRepo(false);
                    nico_repo_listview.ItemsSource = nicoRepoList.list;
                    UpdateButton.Visibility = Visibility.Visible;
                    PanelList[3].Visibility = Visibility.Visible;
                    break;
                case "account_setting":
                    //ログイン
                    NavHeaderTitle.Text = "Twitter/niconicoログイン";
                    UpdateButton.Visibility = Visibility.Collapsed;
                    PanelList[0].Visibility = Visibility.Visible;
                    break;
            }
        }

        public async void showSetting()
        {
            //設定ウィンドウ表示
            // https://blog.okazuki.jp/entry/2015/10/23/214946
            var currentViewId = ApplicationView.GetForCurrentView().Id;
            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                Window.Current.Content = new Frame();
                ((Frame)Window.Current.Content).Navigate(typeof(SettingsPage));
                Window.Current.Activate();
                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                    ApplicationView.GetApplicationViewIdForWindow(Window.Current.CoreWindow),
                    ViewSizePreference.Default,
                    currentViewId,
                    ViewSizePreference.Default);
            });
        }

        private async void TwitterLoginButtonClick(object sender, RoutedEventArgs e)
        {
            //Twitterログイン画面出す。
            twitterLogin.showTwitterParge();
            //ダイアログ出す
            await pin_dialog.ShowAsync();
        }

        private async void NicoLoginButtonClick(object sender, RoutedEventArgs e)
        {
            //メアド、パスワード入力ダイアログだす
            await nico_login_dialog.ShowAsync();
        }

        private void NicoFavAddAutoAdmissionButtonClick(object sender, RoutedEventArgs e)
        {
            //ニコ生フォロー中から予約枠自動入場追加した
            nicoLiveFavList.addAdmissionProgram(int.Parse((sender as Button).Tag.ToString()));
        }

        private void FilterStreamToggleSwitch(object sender, RoutedEventArgs e)
        {
            //FilterStream接続
            var toggleSwitch = (sender as ToggleSwitch);
            if (toggleSwitch.IsOn == true)
            {
                filterStream.connectFilterStream(this);
                home_twitter_stream_textblock.Text = "ニコ生アラート状態：接続中です。";
            }
            else
            {
                filterStream.tokenSource.Cancel();
                home_twitter_stream_textblock.Text = "ニコ生アラート状態：未接続です。";
            }
        }
        private void AutoAdmisionToggleSwitch(object sender, RoutedEventArgs e)
        {
            //予約枠自動登録自動入場有効スイッチ
            var toggleSwitch = (sender as ToggleSwitch);
            if (toggleSwitch.IsOn == true)
            {
                //開始。
                addAutoAdmission.timerFollowAutoAddAdmission();
                addAutoAdmission.timerNicorepoAutoAddAdmission();
                addAutoAdmission.nicorepoTimer.Start();
                addAutoAdmission.followTimer.Start();
                home_auto_admission_textblock.Text = "予約枠自動登録自動入場状態：定期巡回中です。";
            }
            else
            {
                //止める。
                addAutoAdmission.nicorepoTimer.Stop();
                addAutoAdmission.followTimer.Stop();
                home_auto_admission_textblock.Text = "予約枠自動登録自動入場状態：停止中です。";
            }
        }
        private void NicoRepoAddAdmissionButtonClick(object sender, RoutedEventArgs e)
        {
            //ニコレポから予約枠自動入場追加した
            nicoRepoList.addAdmissionProgram(int.Parse((sender as Button).Tag.ToString()));
        }

        private void TwitterAccountAddButtonClick(object sender, RoutedEventArgs e)
        {
            //生主アカウント追加ボタン
            twitterAccountRegister.addAccount(account_textblock.Text);
            twitterregister_listview.ItemsSource = twitterAccountRegister.list;
        }

        private void TwitterAccountDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            //生主アカウント削除ボタン
            twitterAccountRegister.deleteAccount(int.Parse((sender as Button).Tag.ToString()));
            twitterregister_listview.ItemsSource = twitterAccountRegister.list;
        }

        private void TwitterLoginDialogButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //TwitterのPINコードダイアログの認証ボタン
            pin_dialog.Hide();
            twitterLogin.getAccessToken(pin_dialog_textbox.Text);
        }
        private void NicoLoginDialogButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //ニコニコログインダイアログのログインボタンを押したとき
            nico_login_dialog.Hide();
            nicoLogin.niconicoLogin(nico_login_dialog_mail_textbox.Text, nico_login_dialog_pass_textbox.Password);
        }

        private void NicoLiveFavUpdateClick(object sender, RoutedEventArgs e)
        {
            //ニコ生フォロー中更新
            nicoLiveFavList.loadNicoFavList(true);
        }


        private void AutoAdmissionDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            //予約枠自動入場削除
            autoAdmissionList.deleteAutoAdmission(int.Parse((sender as Button).Tag.ToString()));
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            //更新ボタン
            switch (nowParge)
            {
                case "follow_program":
                    nicoLiveFavList.loadNicoFavList(true);
                    break;
                case "nicorepo":
                    nicoRepoList.loadNicoRepo(true);
                    break;
            }
        }

        private async void AddAdmissionButotn_Click(object sender, RoutedEventArgs e)
        {
            //予約枠自動入場リストの手動追加ボタン
            await AddAdmissionDialog.ShowAsync();
        }

        private void AddAdmissionDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //予約枠手動登録ダイアログの登録ボタン
            var title = AddAdmissionDialogProgramTitle.Text;
            var id = AddAdmissionDialogProgramID.Text;
            var datePicker = AddAdmissionDialogDatePicker.Date;
            var timePicker = AddAdmissionDialogTimePicker.Time;
            if (title != null && id != null && datePicker != null && timePicker != null)
            {
                //Dateに時間追加すりゅ
                var date = DateTime.Parse($"{datePicker.Year}/{datePicker.Month}/{datePicker.Day} {timePicker.Hours}:{timePicker.Minutes}:00");
                //UnixTimeへ
                var unix = new DateTimeOffset(date.Ticks, new TimeSpan(+09, 00, 00));
                //追加する
                autoAdmissionList.addAdmission(title, id, unix.ToUnixTimeSeconds());
            }
        }
    }
}
