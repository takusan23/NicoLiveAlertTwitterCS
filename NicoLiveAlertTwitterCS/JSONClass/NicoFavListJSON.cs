using System.Collections.Generic;

public class Submodules
{
    public string singleValue { get; set; }
}

public class Menu
{
    public Submodules submodules { get; set; }
}

public class Focus
{
    public Menu menu { get; set; }
}

public class OperatorComment
{
    public object operatorComment { get; set; }
    public bool visibility { get; set; }
}

public class CommentLayer
{
    public bool userDesiredVisibility { get; set; }
    public bool commentLayerEmitter { get; set; }
}

public class CommentPanel
{
    public object dataGridOperation { get; set; }
}

public class CommentMode
{
    public string mode { get; set; }
}

public class CommentLock
{
    public bool isLocked { get; set; }
}

public class CommentPostPanel
{
    public string postFormText { get; set; }
    public bool postFormEnabled { get; set; }
    public bool showCommentPostPanel { get; set; }
    public bool postEnabled { get; set; }
    public object inputOperation { get; set; }
    public bool isFocused { get; set; }
}

public class PostComment
{
}

public class Thread
{
    public bool isJoining { get; set; }
}

public class MessageServer
{
    public PostComment postComment { get; set; }
    public Thread thread { get; set; }
}

public class CommentWidget
{
    public bool isCommentPanelOpen { get; set; }
}

public class LiveCommentRenderer
{
    public object module { get; set; }
}

public class Submodules2
{
    public bool value { get; set; }
}

public class PostCommentRequesting
{
    public Submodules2 submodules { get; set; }
}

public class Comment
{
    public OperatorComment operatorComment { get; set; }
    public CommentLayer commentLayer { get; set; }
    public CommentPanel commentPanel { get; set; }
    public CommentMode commentMode { get; set; }
    public CommentLock commentLock { get; set; }
    public CommentPostPanel commentPostPanel { get; set; }
    public MessageServer messageServer { get; set; }
    public CommentWidget commentWidget { get; set; }
    public LiveCommentRenderer liveCommentRenderer { get; set; }
    public PostCommentRequesting postCommentRequesting { get; set; }
}

public class EdgeStream
{
    public object protocol { get; set; }
    public object resource { get; set; }
    public object quality { get; set; }
    public List<object> availableQuality { get; set; }
}

public class LiveVideoComponent
{
    public object module { get; set; }
}

public class Player
{
    public bool visibility { get; set; }
    public bool isPlaying { get; set; }
}

public class PlayerController
{
    public bool visibility { get; set; }
    public bool reloadButtonIsEnabled { get; set; }
    public bool muteButtonTextVisibility { get; set; }
}

public class RetryStreamPlayback
{
    public int retryIntervalMillis { get; set; }
    public bool isStalled { get; set; }
}

public class VideoPlay
{
    public EdgeStream edgeStream { get; set; }
    public LiveVideoComponent liveVideoComponent { get; set; }
    public Player player { get; set; }
    public PlayerController playerController { get; set; }
    public RetryStreamPlayback retryStreamPlayback { get; set; }
    public bool mute { get; set; }
}

public class ElapsedTime
{
}

public class Redirect
{
    public object redirectMessage { get; set; }
    public object jumpMessage { get; set; }
    public object defaultJumpMessage { get; set; }
}

public class Room
{
    public object messageServer { get; set; }
    public object name { get; set; }
    public object id { get; set; }
}

public class Vpos
{
}

public class TrialWatch
{
    public bool isTarget { get; set; }
    public bool isVideoEnabled { get; set; }
    public string commentMode { get; set; }
}

public class WatchRestriction
{
    public bool payProgram { get; set; }
    public bool trialWatchAvailable { get; set; }
    public bool isMemberFree { get; set; }
    public TrialWatch trialWatch { get; set; }
}

public class LiveProgram
{
    public Comment comment { get; set; }
    public VideoPlay videoPlay { get; set; }
    public ElapsedTime elapsedTime { get; set; }
    public Redirect redirect { get; set; }
    public Room room { get; set; }
    public Vpos vpos { get; set; }
    public WatchRestriction watchRestriction { get; set; }
}

public class Dialog
{
    public bool isOpen { get; set; }
    public int type { get; set; }
}

public class ScreenOrientation
{
    public int orientation { get; set; }
}

public class Services
{
    public object loggingService { get; set; }
}

public class InputFocus
{
    public bool isFocusingOnTextInputElement { get; set; }
}

public class BottomNotification
{
    public bool isOpen { get; set; }
    public int current { get; set; }
    public int next { get; set; }
}

public class Reservation
{
}

public class Submodules3
{
    public bool value { get; set; }
}

public class Requesting
{
    public Submodules3 submodules { get; set; }
}

public class TimeshiftReservation
{
    public Reservation reservation { get; set; }
    public Requesting requesting { get; set; }
}

public class Submodules4
{
    public string singleValue { get; set; }
}

public class PageNavigations
{
    public Submodules4 submodules { get; set; }
}

public class LiveCycleMenu
{
    public string selectedLiveCycleName { get; set; }
    public bool isVisible { get; set; }
}

public class Favorites
{
    public LiveCycleMenu liveCycleMenu { get; set; }
}

public class Watch
{
    public bool programDescriptionIsExpanded { get; set; }
}

public class SuggestTags
{
    public List<object> suggestTags { get; set; }
}

public class WatchEventLog
{
    public object instance { get; set; }
}

public class WatchEvents
{
    public bool isVideoPlaying { get; set; }
}

public class Submodules5
{
    public string singleValue { get; set; }
}

public class Menu2
{
    public Submodules5 submodules { get; set; }
}

public class Recent
{
    public Menu2 menu { get; set; }
}

public class Submodules6
{
    public string singleValue { get; set; }
}

public class Menu3
{
    public Submodules6 submodules { get; set; }
}

public class Search
{
    public Menu3 menu { get; set; }
}

public class Submodules7
{
    public string singleValue { get; set; }
}

public class Menu4
{
    public Submodules7 submodules { get; set; }
}

public class Ranking
{
    public Menu4 menu { get; set; }
}

public class SearchHistory
{
    public List<object> searchHistory { get; set; }
}

public class BrowserRuntime
{
    public Focus focus { get; set; }
    public LiveProgram liveProgram { get; set; }
    public Dialog dialog { get; set; }
    public ScreenOrientation screenOrientation { get; set; }
    public Services services { get; set; }
    public InputFocus inputFocus { get; set; }
    public BottomNotification bottomNotification { get; set; }
    public TimeshiftReservation timeshiftReservation { get; set; }
    public PageNavigations pageNavigations { get; set; }
    public Favorites favorites { get; set; }
    public Watch watch { get; set; }
    public SuggestTags suggestTags { get; set; }
    public WatchEventLog watchEventLog { get; set; }
    public WatchEvents watchEvents { get; set; }
    public Recent recent { get; set; }
    public Search search { get; set; }
    public Ranking ranking { get; set; }
    public SearchHistory searchHistory { get; set; }
}

public class Account
{
    public string id { get; set; }
    public string nickname { get; set; }
    public string area { get; set; }
    public string language { get; set; }
    public string timezone { get; set; }
    public string description { get; set; }
    public long createdAt { get; set; }
    public bool isPremium { get; set; }
    public string icon { get; set; }
    public string birthday { get; set; }
    public int gender { get; set; }
    public string prefecture { get; set; }
    public bool isProfileRegistered { get; set; }
    public bool isMailRegistered { get; set; }
    public bool isExplicitlyLoginable { get; set; }
}

public class Account2
{
    public string authBaseUrl { get; set; }
    public string siteId { get; set; }
    public string sec { get; set; }
}

public class Top
{
    public string url { get; set; }
}

public class Video
{
    public string url { get; set; }
}

public class Live
{
    public string url { get; set; }
}

public class News
{
    public string url { get; set; }
}

public class Channel
{
    public string url { get; set; }
}

public class Manga
{
    public string url { get; set; }
}

public class Atsumaru
{
    public string url { get; set; }
}

public class Niconicoq
{
    public string url { get; set; }
}

public class Point
{
    public string url { get; set; }
}

public class NiconicoService
{
    public Top top { get; set; }
    public Video video { get; set; }
    public Live live { get; set; }
    public News news { get; set; }
    public Channel channel { get; set; }
    public Manga manga { get; set; }
    public Atsumaru atsumaru { get; set; }
    public Niconicoq niconicoq { get; set; }
    public Point point { get; set; }
}

public class IOS
{
    public string downloadUrl { get; set; }
    public string playUrl { get; set; }
    public string broadcastUrl { get; set; }
}

public class Android
{
    public string downloadUrl { get; set; }
    public string playUrl { get; set; }
    public string broadcastUrl { get; set; }
}

public class Nicolive
{
    public IOS iOS { get; set; }
    public Android android { get; set; }
}

public class SpApp
{
    public Nicolive nicolive { get; set; }
}

public class FrontendServer
{
}

public class WatchEventLog2
{
    public bool blockConnection { get; set; }
}

public class PublicNicobusApi
{
    public string apiBaseUrl { get; set; }
    public WatchEventLog2 watchEventLog { get; set; }
}

public class Help
{
    public string url { get; set; }
}

public class Rule
{
    public string url { get; set; }
}

public class Tokutei
{
    public string url { get; set; }
}

public class Community
{
    public string url { get; set; }
    public string followUrl { get; set; }
}

public class Channel2
{
    public string url { get; set; }
    public string admissionUrl { get; set; }
    public string ticketPurchaseUrl { get; set; }
}

public class SocialGroup
{
    public Community community { get; set; }
    public Channel2 channel { get; set; }
}

public class StaticFiles
{
    public string assetsBasePath { get; set; }
    public string adsJs { get; set; }
}

public class Startup
{
    public int denominator { get; set; }
}

public class RuntimeError
{
    public int denominator { get; set; }
}

public class Types
{
    public Startup startup { get; set; }
    public RuntimeError runtimeError { get; set; }
}

public class Logging
{
    public string endpoint { get; set; }
    public int timeout { get; set; }
    public Types types { get; set; }
}

public class PublicApiServer
{
    public string apiBaseUrl { get; set; }
    public int timeout { get; set; }
}

public class SuggestSearch
{
    public string apiBaseUrl { get; set; }
    public int timeout { get; set; }
}

public class AssetsConfig
{
    public List<string> app { get; set; }
    public string comment { get; set; }
    public string nico { get; set; }
    public string vendor { get; set; }
    public string video { get; set; }
    public List<string> css { get; set; }
}

public class ClientConfig
{
    public Account2 account { get; set; }
    public NiconicoService niconicoService { get; set; }
    public string niconicoServiceListPageUrl { get; set; }
    public string live1PcBaseUrl { get; set; }
    public SpApp spApp { get; set; }
    public FrontendServer frontendServer { get; set; }
    public PublicNicobusApi publicNicobusApi { get; set; }
    public Help help { get; set; }
    public Rule rule { get; set; }
    public Tokutei tokutei { get; set; }
    public SocialGroup socialGroup { get; set; }
    public StaticFiles staticFiles { get; set; }
    public Logging logging { get; set; }
    public PublicApiServer publicApiServer { get; set; }
    public SuggestSearch suggestSearch { get; set; }
    public AssetsConfig assetsConfig { get; set; }
    public bool enableTrialMode { get; set; }
    public string applicationBaseUrl { get; set; }
}

public class ClientEnvironment
{
    public int osType { get; set; }
    public bool isVideoInlinePlaySupported { get; set; }
}

public class Url
{
    public string protocol { get; set; }
    public object slashes { get; set; }
    public object auth { get; set; }
    public string host { get; set; }
    public string hostname { get; set; }
    public object hash { get; set; }
    public object search { get; set; }
    public object query { get; set; }
    public string pathname { get; set; }
    public string path { get; set; }
    public string href { get; set; }
}

public class RequestInfo
{
    public string ipAddress { get; set; }
    public Url url { get; set; }
}

public class Maintenance
{
    public object id { get; set; }
    public object beginAt { get; set; }
    public object endAt { get; set; }
}

public class Constants
{
    public Account account { get; set; }
    public ClientConfig clientConfig { get; set; }
    public ClientEnvironment clientEnvironment { get; set; }
    public string frontendId { get; set; }
    public RequestInfo requestInfo { get; set; }
    public Maintenance maintenance { get; set; }
}

public class __invalid_type__5
{
    public int width { get; set; }
    public int height { get; set; }
    public string zoneId { get; set; }
}

public class __invalid_type__6
{
    public int width { get; set; }
    public int height { get; set; }
    public string zoneId { get; set; }
}

public class Ads2
{
    public __invalid_type__5 __invalid_name__5 { get; set; }
    public __invalid_type__6 __invalid_name__6 { get; set; }
}

public class Ads
{
    public Ads2 ads { get; set; }
}

public class Submodules8
{
    public string singleValue { get; set; }
}

public class LiveCycle
{
    public Submodules8 submodules { get; set; }
}

public class Submodules10
{
    public bool value { get; set; }
}

public class Requesting2
{
    public Submodules10 submodules { get; set; }
}

public class Submodules9
{
    public Requesting2 requesting { get; set; }
}

public class OnAir
{
    public List<object> items { get; set; }
    public int loadedPages { get; set; }
    public int pageTotal { get; set; }
    public Submodules9 submodules { get; set; }
}

public class Submodules12
{
    public bool value { get; set; }
}

public class Requesting3
{
    public Submodules12 submodules { get; set; }
}

public class Submodules11
{
    public Requesting3 requesting { get; set; }
}

public class BeforeOpen
{
    public List<object> items { get; set; }
    public int loadedPages { get; set; }
    public int pageTotal { get; set; }
    public Submodules11 submodules { get; set; }
}

public class Submodules14
{
    public bool value { get; set; }
}

public class Requesting4
{
    public Submodules14 submodules { get; set; }
}

public class Submodules13
{
    public Requesting4 requesting { get; set; }
}

public class Ended
{
    public List<object> items { get; set; }
    public int loadedPages { get; set; }
    public int pageTotal { get; set; }
    public Submodules13 submodules { get; set; }
}

public class Programs
{
    public OnAir on_air { get; set; }
    public BeforeOpen before_open { get; set; }
    public Ended ended { get; set; }
}

public class Focus2
{
    public LiveCycle liveCycle { get; set; }
    public Programs programs { get; set; }
}

public class PageError
{
    public object pageErrorName { get; set; }
}

public class TimeshiftReservations
{
    public List<object> reservationList { get; set; }
}

public class Programs2
{
}

public class ReservedPrograms
{
    public Programs2 programs { get; set; }
}

public class Reservations
{
    public TimeshiftReservations timeshiftReservations { get; set; }
    public ReservedPrograms reservedPrograms { get; set; }
}

public class Statistics
{
    public int watchCount { get; set; }
    public int commentCount { get; set; }
    public int? reservationCount { get; set; }
}

public class Program
{
    public string id { get; set; }
    public string title { get; set; }
    public string shortTitle { get; set; }
    public string userId { get; set; }
    public string thumbnailUrl { get; set; }
    public int providerType { get; set; }
    public string liveCycle { get; set; }
    public long beginAt { get; set; }
    public object endAt { get; set; }
    public bool isMemberOnly { get; set; }
    public Statistics statistics { get; set; }
    public string socialGroupName { get; set; }
    public string socialGroupThumbnailUrl { get; set; }
    public string ownerIconUrl { get; set; }
    public string liveScreenshotThumbnailUrl { get; set; }
    public object tsScreenshotThumbnailUrl { get; set; }
    public int? elapsedTimeSeconds { get; set; }
    public object durationSeconds { get; set; }
}

public class FavoritePrograms
{
    public List<Program> programs { get; set; }
}

public class Favorites2
{
    public FavoritePrograms favoritePrograms { get; set; }
}

public class Statistics2
{
    public object watchCount { get; set; }
    public object commentCount { get; set; }
    public object reservationCount { get; set; }
}

public class TrialWatch2
{
    public bool isTarget { get; set; }
    public bool isVideoEnabled { get; set; }
    public string commentMode { get; set; }
}

public class WatchRestriction2
{
    public bool payProgram { get; set; }
    public bool trialWatchAvailable { get; set; }
    public bool isMemberFree { get; set; }
    public TrialWatch2 trialWatch { get; set; }
}

public class Program2
{
    public object id { get; set; }
    public object title { get; set; }
    public object shortTitle { get; set; }
    public object userId { get; set; }
    public object description { get; set; }
    public object thumbnailUrl { get; set; }
    public object liveScreenshotThumbnailUrl { get; set; }
    public object tsScreenshotThumbnailUrl { get; set; }
    public int providerType { get; set; }
    public List<object> tags { get; set; }
    public string liveCycle { get; set; }
    public int openAt { get; set; }
    public int beginAt { get; set; }
    public int endAt { get; set; }
    public bool deleted { get; set; }
    public int excludeType { get; set; }
    public object deletedInfo { get; set; }
    public object visibleAt { get; set; }
    public bool isSpwebEnabled { get; set; }
    public bool isIosEnabled { get; set; }
    public bool isAndroidEnabled { get; set; }
    public object timeshiftType { get; set; }
    public bool isTimeshiftButtonEnabled { get; set; }
    public bool isTimeshiftEnabled { get; set; }
    public object timeshiftStartAt { get; set; }
    public object timeshiftEndAt { get; set; }
    public bool isTimeshiftViewLimited { get; set; }
    public bool isTimeshiftAvailable { get; set; }
    public bool isReliveEnabled { get; set; }
    public bool isPayProgram { get; set; }
    public bool showAds { get; set; }
    public object ticketPurchaseUrl { get; set; }
    public bool isMemberOnly { get; set; }
    public object socialGroup { get; set; }
    public object socialGroupId { get; set; }
    public object socialGroupName { get; set; }
    public object socialGroupThumbnailUrl { get; set; }
    public object ownerIconUrl { get; set; }
    public Statistics2 statistics { get; set; }
    public string twitterHashTags { get; set; }
    public object redirectTo { get; set; }
    public bool isDmc { get; set; }
    public bool isAllowedToPlay { get; set; }
    public int mediaServerType { get; set; }
    public object socialGroupDetail { get; set; }
    public object audienceLimitation { get; set; }
    public WatchRestriction2 watchRestriction { get; set; }
    public object programProvider { get; set; }
}

public class WsEndPoint
{
    public object url { get; set; }
    public object broadcastId { get; set; }
    public object audienceToken { get; set; }
}

public class CommentState
{
    public bool locked { get; set; }
    public string layout { get; set; }
}

public class OperatorComment2
{
    public string body { get; set; }
    public object link { get; set; }
    public string name { get; set; }
    public object decoration { get; set; }
    public bool isPermanent { get; set; }
}

public class PlayerParams
{
    public object audienceToken { get; set; }
    public object defaultJump { get; set; }
    public WsEndPoint wsEndPoint { get; set; }
    public CommentState commentState { get; set; }
    public OperatorComment2 operatorComment { get; set; }
    public int serverTime { get; set; }
}

public class WatchInformation
{
    public Program2 program { get; set; }
    public PlayerParams playerParams { get; set; }
    public object watchingError { get; set; }
    public object deletedInfo { get; set; }
    public bool isPlayable { get; set; }
    public bool isTimeshiftReserved { get; set; }
}

public class Redirect2
{
    public object permanentRedirectTo { get; set; }
    public object temporaryRedirectTo { get; set; }
}

public class Link
{
    public string text { get; set; }
    public string url { get; set; }
}

public class PortalLinks
{
    public List<Link> links { get; set; }
}

public class Watch2
{
    public WatchInformation watchInformation { get; set; }
    public Redirect2 redirect { get; set; }
    public PortalLinks portalLinks { get; set; }
}

public class Submodules15
{
    public string singleValue { get; set; }
}

public class LiveCycle2
{
    public Submodules15 submodules { get; set; }
}

public class Submodules17
{
    public bool value { get; set; }
}

public class Requesting5
{
    public Submodules17 submodules { get; set; }
}

public class Submodules16
{
    public Requesting5 requesting { get; set; }
}

public class Onair2
{
    public List<object> items { get; set; }
    public int loadedPages { get; set; }
    public int pageTotal { get; set; }
    public Submodules16 submodules { get; set; }
}

public class Submodules19
{
    public bool value { get; set; }
}

public class Requesting6
{
    public Submodules19 submodules { get; set; }
}

public class Submodules18
{
    public Requesting6 requesting { get; set; }
}

public class Past
{
    public List<object> items { get; set; }
    public int loadedPages { get; set; }
    public int pageTotal { get; set; }
    public Submodules18 submodules { get; set; }
}

public class Submodules21
{
    public bool value { get; set; }
}

public class Requesting7
{
    public Submodules21 submodules { get; set; }
}

public class Submodules20
{
    public Requesting7 requesting { get; set; }
}

public class Reserved
{
    public List<object> items { get; set; }
    public int loadedPages { get; set; }
    public int pageTotal { get; set; }
    public Submodules20 submodules { get; set; }
}

public class Programs3
{
    public Onair2 onair { get; set; }
    public Past past { get; set; }
    public Reserved reserved { get; set; }
}

public class RecentTab
{
    public string recentTab { get; set; }
}

public class Submodules22
{
    public string singleValue { get; set; }
}

public class SortOrder
{
    public Submodules22 submodules { get; set; }
}

public class Recent2
{
    public LiveCycle2 liveCycle { get; set; }
    public Programs3 programs { get; set; }
    public RecentTab recentTab { get; set; }
    public SortOrder sortOrder { get; set; }
}

public class Submodules24
{
    public bool value { get; set; }
}

public class Requesting8
{
    public Submodules24 submodules { get; set; }
}

public class Submodules23
{
    public Requesting8 requesting { get; set; }
}

public class Onair3
{
    public List<object> items { get; set; }
    public int loadedPages { get; set; }
    public int pageTotal { get; set; }
    public Submodules23 submodules { get; set; }
}

public class Submodules26
{
    public bool value { get; set; }
}

public class Requesting9
{
    public Submodules26 submodules { get; set; }
}

public class Submodules25
{
    public Requesting9 requesting { get; set; }
}

public class Past2
{
    public List<object> items { get; set; }
    public int loadedPages { get; set; }
    public int pageTotal { get; set; }
    public Submodules25 submodules { get; set; }
}

public class Submodules28
{
    public bool value { get; set; }
}

public class Requesting10
{
    public Submodules28 submodules { get; set; }
}

public class Submodules27
{
    public Requesting10 requesting { get; set; }
}

public class Reserved2
{
    public List<object> items { get; set; }
    public int loadedPages { get; set; }
    public int pageTotal { get; set; }
    public Submodules27 submodules { get; set; }
}

public class Programs4
{
    public Onair3 onair { get; set; }
    public Past2 past { get; set; }
    public Reserved2 reserved { get; set; }
}

public class Submodules29
{
    public string singleValue { get; set; }
}

public class LiveCycle3
{
    public Submodules29 submodules { get; set; }
}

public class Submodules30
{
    public List<object> singleValue { get; set; }
}

public class ProviderTypes
{
    public Submodules30 submodules { get; set; }
}

public class Submodules31
{
    public List<string> singleValue { get; set; }
}

public class SearchFilters
{
    public Submodules31 submodules { get; set; }
}

public class Submodules32
{
    public List<string> singleValue { get; set; }
}

public class SearchOptions
{
    public Submodules32 submodules { get; set; }
}

public class Submodules33
{
    public string singleValue { get; set; }
}

public class SortOrder2
{
    public Submodules33 submodules { get; set; }
}

public class SearchConditions
{
    public object date { get; set; }
    public string keyword { get; set; }
    public LiveCycle3 liveCycle { get; set; }
    public ProviderTypes providerTypes { get; set; }
    public SearchFilters searchFilters { get; set; }
    public SearchOptions searchOptions { get; set; }
    public SortOrder2 sortOrder { get; set; }
}

public class Search2
{
    public Programs4 programs { get; set; }
    public SearchConditions searchConditions { get; set; }
}

public class Tracking
{
    public string actionTrackId { get; set; }
    public string nicosId { get; set; }
}

public class Notification
{
    public object notification { get; set; }
}

public class Programs5
{
    public List<object> programs { get; set; }
}

public class Timetable
{
    public Programs5 programs { get; set; }
    public string date { get; set; }
}

public class NicoliveInfo
{
    public List<object> infoItems { get; set; }
    public List<object> maintenanceItems { get; set; }
}

public class NicoInfo
{
    public NicoliveInfo nicoliveInfo { get; set; }
}

public class RookiePrograms
{
    public List<object> programs { get; set; }
}

public class RecommendedPrograms
{
    public List<object> programs { get; set; }
}

public class PopularPrograms
{
    public List<object> commonCategoryPrograms { get; set; }
    public List<object> tryCategoryPrograms { get; set; }
    public List<object> gameCategoryPrograms { get; set; }
    public List<object> faceCategoryPrograms { get; set; }
    public List<object> smartPhoneBroadcastTagPrograms { get; set; }
    public List<object> reservedFocusedPrograms { get; set; }
    public List<object> reservedPrograms { get; set; }
    public List<object> pastPrograms { get; set; }
}

public class FocusedPrograms
{
    public List<object> programs { get; set; }
}

public class FavoritePrograms2
{
    public List<object> programs { get; set; }
}

public class Top2
{
    public NicoInfo nicoInfo { get; set; }
    public RookiePrograms rookiePrograms { get; set; }
    public RecommendedPrograms recommendedPrograms { get; set; }
    public PopularPrograms popularPrograms { get; set; }
    public FocusedPrograms focusedPrograms { get; set; }
    public FavoritePrograms2 favoritePrograms { get; set; }
}

public class PickupContents2
{
    public List<object> pickupItems { get; set; }
}

public class PickupContents
{
    public PickupContents2 pickupContents { get; set; }
}

public class FeaturedContents2
{
    public List<object> featuredItems { get; set; }
}

public class FeaturedContents
{
    public FeaturedContents2 featuredContents { get; set; }
}

public class Feature
{
    public PickupContents pickupContents { get; set; }
    public FeaturedContents featuredContents { get; set; }
}

public class HeatMap
{
    public bool isTarget { get; set; }
    public int sampleRate { get; set; }
}

public class ProgramSearchPanel
{
    public string keyword { get; set; }
}

public class HeaderParts
{
    public ProgramSearchPanel programSearchPanel { get; set; }
}

public class RankingPrograms
{
    public List<object> rankingPrograms { get; set; }
}

public class Submodules34
{
    public int singleValue { get; set; }
}

public class ProviderType
{
    public Submodules34 submodules { get; set; }
}

public class Ranking2
{
    public RankingPrograms rankingPrograms { get; set; }
    public ProviderType providerType { get; set; }
}

public class Cookie2
{
    public List<object> cookies { get; set; }
}

public class PageContents
{
    public Ads ads { get; set; }
    public Focus2 focus { get; set; }
    public PageError pageError { get; set; }
    public bool shouldFetchData { get; set; }
    public Reservations reservations { get; set; }
    public Favorites2 favorites { get; set; }
    public Watch2 watch { get; set; }
    public Recent2 recent { get; set; }
    public Search2 search { get; set; }
    public Tracking tracking { get; set; }
    public Notification notification { get; set; }
    public Timetable timetable { get; set; }
    public Top2 top { get; set; }
    public Feature feature { get; set; }
    public HeatMap heatMap { get; set; }
    public HeaderParts headerParts { get; set; }
    public Ranking2 ranking { get; set; }
    public Cookie2 cookie { get; set; }
}

public class EApiClient2
{
}

public class ApiClient
{
}

public class NicoliveEApiClient
{
    public ApiClient apiClient { get; set; }
}

public class EApiClient
{
    public EApiClient2 eApiClient { get; set; }
    public NicoliveEApiClient nicoliveEApiClient { get; set; }
}

public class ApiClient3
{
}

public class ApiClient2
{
    public ApiClient3 apiClient { get; set; }
}

public class WatchApiClient
{
    public ApiClient2 apiClient { get; set; }
}

public class ProgramService
{
    public EApiClient eApiClient { get; set; }
    public WatchApiClient watchApiClient { get; set; }
}

public class EApiClient4
{
}

public class ApiClient4
{
}

public class NicoliveEApiClient2
{
    public ApiClient4 apiClient { get; set; }
}

public class EApiClient3
{
    public EApiClient4 eApiClient { get; set; }
    public NicoliveEApiClient2 nicoliveEApiClient { get; set; }
}

public class AdsService
{
    public EApiClient3 eApiClient { get; set; }
}

public class Channel3
{
}

public class AccountServiceClient
{
    public List<object> __invalid_name__interceptors { get; set; }
public List<object> __invalid_name__interceptor_providers { get; set; }
    public Channel3 __invalid_name__channel { get; set; }
}

public class InternalRepr
{
}

public class MetaData
{
    public InternalRepr _internal_repr { get; set; }
}

public class ApiClient6
{
    public int timeoutMs { get; set; }
    public List<object> interceptors { get; set; }
    public AccountServiceClient accountServiceClient { get; set; }
    public MetaData metaData { get; set; }
}

public class ApiClient5
{
    public ApiClient6 apiClient { get; set; }
}

public class AccountService
{
    public ApiClient5 apiClient { get; set; }
}

public class TrackingService
{
}

public class ApiClient8
{
}

public class NicoInfoApiClient
{
    public ApiClient8 apiClient { get; set; }
}

public class ApiClient7
{
    public NicoInfoApiClient nicoInfoApiClient { get; set; }
}

public class NotificationService
{
    public ApiClient7 apiClient { get; set; }
}

public class ApiClient9
{
}

public class IzumoApiClient2
{
    public ApiClient9 apiClient { get; set; }
}

public class IzumoApiClient
{
    public IzumoApiClient2 izumoApiClient { get; set; }
}

public class EmacsClient2
{
}

public class ApiClient10
{
}

public class NicoliveEApiClient3
{
    public ApiClient10 apiClient { get; set; }
}

public class EmacsClient
{
    public EmacsClient2 emacsClient { get; set; }
    public NicoliveEApiClient3 nicoliveEApiClient { get; set; }
}

public class RecommendedProgramsService
{
    public IzumoApiClient izumoApiClient { get; set; }
    public EmacsClient emacsClient { get; set; }
}

public class SugoiSSApiClient
{
}

public class SearchService
{
    public SugoiSSApiClient sugoiSSApiClient { get; set; }
}

public class EApiClient6
{
}

public class ApiClient11
{
}

public class NicoliveEApiClient4
{
    public ApiClient11 apiClient { get; set; }
}

public class EApiClient5
{
    public EApiClient6 eApiClient { get; set; }
    public NicoliveEApiClient4 nicoliveEApiClient { get; set; }
}

public class TimeshiftService
{
    public EApiClient5 eApiClient { get; set; }
}

public class ApiClient13
{
}

public class NicoInfoApiClient2
{
    public ApiClient13 apiClient { get; set; }
}

public class ApiClient12
{
    public NicoInfoApiClient2 nicoInfoApiClient { get; set; }
}

public class NicoInfoService
{
    public ApiClient12 apiClient { get; set; }
}

public class ApiClient14
{
}

public class IndexStreamListApiClient2
{
    public ApiClient14 apiClient { get; set; }
}

public class IndexStreamListApiClient
{
    public IndexStreamListApiClient2 indexStreamListApiClient { get; set; }
}

public class SugoiSSApiClient2
{
}

public class FocusedProgramsService
{
    public IndexStreamListApiClient indexStreamListApiClient { get; set; }
    public SugoiSSApiClient2 sugoiSSApiClient { get; set; }
}

public class EApiClient8
{
}

public class ApiClient15
{
}

public class NicoliveEApiClient5
{
    public ApiClient15 apiClient { get; set; }
}

public class EApiClient7
{
    public EApiClient8 eApiClient { get; set; }
    public NicoliveEApiClient5 nicoliveEApiClient { get; set; }
}

public class FeaturedContentsService
{
    public EApiClient7 eApiClient { get; set; }
}

public class WakutkoolClient
{
    public int timeoutSeconds { get; set; }
}

public class RelatedContentsService
{
    public WakutkoolClient wakutkoolClient { get; set; }
}

public class EApiClient10
{
}

public class ApiClient16
{
}

public class NicoliveEApiClient6
{
    public ApiClient16 apiClient { get; set; }
}

public class EApiClient9
{
    public EApiClient10 eApiClient { get; set; }
    public NicoliveEApiClient6 nicoliveEApiClient { get; set; }
}

public class SubscribedProgramsService
{
    public EApiClient9 eApiClient { get; set; }
}

public class SuggestSearchService
{
}

public class ApiClient18
{
}

public class PtaApiClient
{
    public ApiClient18 apiClient { get; set; }
    public string serviceName { get; set; }
}

public class ApiClient17
{
    public PtaApiClient ptaApiClient { get; set; }
}

public class PtaService
{
    public ApiClient17 apiClient { get; set; }
}

public class EmacsClient4
{
}

public class ApiClient19
{
}

public class NicoliveEApiClient7
{
    public ApiClient19 apiClient { get; set; }
}

public class EmacsClient3
{
    public EmacsClient4 emacsClient { get; set; }
    public NicoliveEApiClient7 nicoliveEApiClient { get; set; }
}

public class NgFilteredOnairProgramsService
{
    public EmacsClient3 emacsClient { get; set; }
}

public class EmacsClient6
{
}

public class ApiClient20
{
}

public class NicoliveEApiClient8
{
    public ApiClient20 apiClient { get; set; }
}

public class EmacsClient5
{
    public EmacsClient6 emacsClient { get; set; }
    public NicoliveEApiClient8 nicoliveEApiClient { get; set; }
}

public class RankingService
{
    public EmacsClient5 emacsClient { get; set; }
}

public class Channel4
{
}

public class Client
{
    public List<object> __invalid_name__ { get; set; }
public List<object> __invalid_name___ { get; set; }
    public Channel4 __invalid_name____ { get; set; }
}

public class InternalRepr2
{
}

public class MetaData2
{
    public InternalRepr2 _internal_repr { get; set; }
}

public class ApiClient21
{
    public int timeoutMs { get; set; }
    public List<object> interceptors { get; set; }
    public Client client { get; set; }
    public MetaData2 metaData { get; set; }
}

public class DolphinApiClient
{
    public ApiClient21 apiClient { get; set; }
}

public class PopularProgramsService
{
    public DolphinApiClient dolphinApiClient { get; set; }
}

public class Channel5
{
}

public class Client2
{
    public List<object> __invalid_name__interceptors_ { get; set; }
public List<object> __invalid_name__interceptor_providers { get; set; }
    public Channel5 __invalid_name__channel { get; set; }
}

public class InternalRepr3
{
}

public class MetaData3
{
    public InternalRepr3 _internal_repr { get; set; }
}

public class ApiClient22
{
    public int timeoutMs { get; set; }
    public List<object> interceptors { get; set; }
    public Client2 client { get; set; }
    public MetaData3 metaData { get; set; }
}

public class DolphinApiClient2
{
    public ApiClient22 apiClient { get; set; }
}

public class RookieProgramsService
{
    public DolphinApiClient2 dolphinApiClient { get; set; }
}

public class Services2
{
    public ProgramService programService { get; set; }
    public AdsService adsService { get; set; }
    public AccountService accountService { get; set; }
    public TrackingService trackingService { get; set; }
    public NotificationService notificationService { get; set; }
    public RecommendedProgramsService recommendedProgramsService { get; set; }
    public SearchService searchService { get; set; }
    public TimeshiftService timeshiftService { get; set; }
    public NicoInfoService nicoInfoService { get; set; }
    public FocusedProgramsService focusedProgramsService { get; set; }
    public FeaturedContentsService featuredContentsService { get; set; }
    public RelatedContentsService relatedContentsService { get; set; }
    public SubscribedProgramsService subscribedProgramsService { get; set; }
    public SuggestSearchService suggestSearchService { get; set; }
    public PtaService ptaService { get; set; }
    public NgFilteredOnairProgramsService ngFilteredOnairProgramsService { get; set; }
    public RankingService rankingService { get; set; }
    public PopularProgramsService popularProgramsService { get; set; }
    public RookieProgramsService rookieProgramsService { get; set; }
}

public class Routing
{
    public object locationBeforeTransitions { get; set; }
}

public class RootObject
{
    public BrowserRuntime browserRuntime { get; set; }
    public Constants constants { get; set; }
    public PageContents pageContents { get; set; }
    public Services2 services { get; set; }
    public Routing routing { get; set; }
}