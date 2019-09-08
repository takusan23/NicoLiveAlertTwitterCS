# NicoLiveAlertTwitterCS
ニコ生アラート（青鳥CS）です。TwitterのFilterStreamと予約枠からニコ生自動入場を利用できるアプリを作ってます。  
C#で書かれてます。UWPアプリです開発中です。そのうち公開するかも。



使いたいときは  
NicoLiveAlertTwitterCS/NicoLiveAlertTwitterCS/Twitter/  
まで進んで中に```TwitterKey.cs```でファイルを作成して下さい。  
作成したら中に以下の内容を入れて保存してね。そしたら実行できるはず。
```c#
namespace NicoLiveAlertTwitterCS
{
    public class TwitterKey
    {
        public static string consumer_key = "コンシューマーキー";
        public static string consumer_secret = "コンシューマーシークレット";
    }

}
```
