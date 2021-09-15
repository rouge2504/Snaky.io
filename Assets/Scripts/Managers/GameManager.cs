using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

#if UNITY_ANDROID

using Unity.Notifications.Android;

#endif

public class GameManager : MonoBehaviour
{
    public enum STATE { IN_GAME, GO_TO_HOME_FROM_GAMEPLAY, IN_MENU, TEAM2X2, TEAM3X3, IN_DUEL }

    public STATE state;

    public string STATE_STRING
    {
        get
        {
            string _state = null;

            switch (state)
            {
                case STATE.IN_GAME:
                    _state = "InGame";
                    break;
                case STATE.GO_TO_HOME_FROM_GAMEPLAY:
                    _state = "GO_TO_HOME_FROM_GAMEPLAY";
                    break;
                case STATE.IN_MENU:
                    _state = "IN_MENU";
                    break;
                case STATE.TEAM2X2:
                    _state = "TEAM2X2";
                    break;
                case STATE.TEAM3X3:
                    _state = "TEAM3X3";
                    break;              
                case STATE.IN_DUEL:
                    _state = "IN_DUEL";
                    break;
            }

            return _state;
        }
    }

    private Action actRewardDoubleEgg;

    public static GameManager instance;
    [Header("MENUS")]
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject loadingUI;
    [SerializeField] public GameObject searchUI;
    [SerializeField] public GameObject gameplayMenu;
    [SerializeField] public GameObject gameOverMenu;
    [SerializeField] public GameObject tutorialMenu;
    [SerializeField] public GameObject gdprMenu;

    public enum CONTROL_MODE { LEFT, RIGHT, ARROW_LEFT, ARROW_RIGHT}
    [Header("Controls")]

    [SerializeField] public GameObject controlLeft;
    [SerializeField] public GameObject controlRight;
    [SerializeField] public GameObject arrow;
    [SerializeField] public GameObject arrowBoostLeft;
    [SerializeField] public GameObject arrowBoostRight;
    [SerializeField] public GameObject parentControl;
    private GameObject selectedMobileControl;
    public long SurvivalTimeTicks { get; set; }

    public DateTime SpawnTime { get; set; }

    public GameObject mainCamera;

    public InputField nameField;

    public bool InGame;
    public bool dev_SpeedUp { get; private set; }

    public void SetControls()
    {
        int control = PlayerPrefs.GetInt(GameUtils.PREFS_JOYSTICK_CONTROL_MODE, 0);
        int hand = PlayerPrefs.GetInt(GameUtils.PREFS_JOYSTICK_CONTROL_HAND, 0);
        CONTROL_MODE controlMode = CONTROL_MODE.RIGHT/*= (CONTROL_MODE)PlayerPrefs.GetInt(GameUtils.PREFS_JOYSTICK_CONTROL_MODE, 0)*/;

        if (control == 0 && hand == 0 /*Left*/)
        {
            controlMode = CONTROL_MODE.ARROW_LEFT;

        }else  if (control == 1 && hand == 0 )
        {
            controlMode = CONTROL_MODE.LEFT;

        }else  if (control == 0 && hand == 1 )
        {
            controlMode = CONTROL_MODE.ARROW_RIGHT;

        }else if (control == 1 && hand == 1 )
        {
            controlMode = CONTROL_MODE.RIGHT;

        }
        controlLeft.SetActive(false);
        controlRight.SetActive(false);
        arrow.SetActive(false);
        switch (controlMode)
        {
            case CONTROL_MODE.LEFT:
                controlLeft.SetActive(true);
                break;
            case CONTROL_MODE.RIGHT:
                controlRight.SetActive(true);
                break;
            case CONTROL_MODE.ARROW_RIGHT:
                arrow.SetActive(true);
                arrowBoostLeft.SetActive(true);
                arrowBoostRight.SetActive(false);
                break;
            case CONTROL_MODE.ARROW_LEFT:
                arrowBoostRight.SetActive(true);
                arrowBoostLeft.SetActive(false);
                arrow.SetActive(true);
                break;
        }

        //selectedMobileControl = arrowLeft; 
    }
    public void ControlsActive(bool active)
    {

        parentControl.SetActive(active);

    }

    public bool IsOnTutorial
    {
        get
        {
            return GamePrefs.GetBool(GameUtils.ON_TUTORIAL, 1);
        }

        set
        {
            GamePrefs.SetBool(GameUtils.ON_TUTORIAL, value); 
        }
    }

    public bool IsFirstTime
    {
        get
        {
            return GamePrefs.GetBool(GameUtils.PLAYER_FIRST_TIME, 1);
        }

        set
        {
            GamePrefs.SetBool(GameUtils.PLAYER_FIRST_TIME, value);
        }
    }
    public bool IsDuelMode
    {
        get
        {
            return state == STATE.IN_DUEL;
        }
    }

    public void Awake()
    {
        instance = this;
        SetControls();
        ControlsActive(false);
        InGame = false;
        nameField.text = GamePrefs.PLAYER_NAME;
    }

    void Start()
    {
        SetLoading();
        state = STATE.IN_MENU;
        tutorialMenu.SetActive(IsOnTutorial);
        gdprMenu.SetActive(GamePrefs.GetBool(GameUtils.GDPR_WINDOW, 1));
        SetupNotifications();   

    }

    public void PlayDuel()
    {
        SnakeSpawner.Instance.DestroyAllSnakes();


        InGame = true;
        state = STATE.IN_DUEL;
        ScoreManager.intance.ResetInfoPoints(4);
        StartCoroutine(ShowSearchingMenu(DuelManager.instance.Init, gameplayMenu));
    }

    public void PlayWithTeam3x3()
    {
        SnakeSpawner.Instance.DestroyAllSnakes();
        StartCoroutine(ShowLoadingMenu(Play3x3, gameplayMenu));
    }

    public void PlayWithTeam2x2()
    {
        SnakeSpawner.Instance.DestroyAllSnakes();
        StartCoroutine(ShowLoadingMenu( Play2x2, gameplayMenu));
    }

    void Play2x2()
    {
        Camera.main.GetComponent<Camera>().orthographicSize = 50;
        mainCamera.GetComponent<CameraManager>().enabled = true;
        FoodSpawner.Instance.isReset = true;
        FoodSpawner.Instance.isWiped = true;
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        ControlsActive(true);
        InGame = true;
        state = STATE.TEAM2X2;
        SnakeSpawner.Instance.StartGame2x2();
    }

    void Play3x3()
    {
        Camera.main.GetComponent<Camera>().orthographicSize = 50;
        mainCamera.GetComponent<CameraManager>().enabled = true;
        FoodSpawner.Instance.isReset = true;
        FoodSpawner.Instance.isWiped = true;
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        ControlsActive(true);
        InGame = true;
        state = STATE.TEAM3X3;
        SnakeSpawner.Instance.StartGame3x3();
    }



    public void PlayWithAI()
    {
        IsOnTutorial = false;
        if (SnakeEnvironment.Singleton.CounterSnake > GameConstants.TOTAL_SNAKES - 20)
        {
            SnakeSpawner.Instance.DestroyAllSnakes(SnakeEnvironment.Singleton.CounterSnake - 20);
        }
        Camera.main.GetComponent<Camera>().orthographicSize = 50;

        //SnakeSpawner.Instance.DestroyAllSnakes();
        StartCoroutine(PlayAI());
    }

    public void GoToMainMenu()
    {
        ShowAds();
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "interstitial" },
            { "placement", "revive" },
            { "result", "available" },
            { "connection", 1 }
        };
        AppMetricaManager.instance.SendReportAppMetrica(AppMetricaManager.Reports.VIDEO_AVAILABLE, vals);
        switch (state)
        {
            case STATE.TEAM2X2:
                SnakeSpawner.Instance.DestroyAllSnakes();
                state = STATE.IN_MENU;
                StartCoroutine(ShowLoadingMenu(Population.instance.Initialize, mainMenu));
                break;
            case STATE.TEAM3X3:
                SnakeSpawner.Instance.DestroyAllSnakes();
                state = STATE.IN_MENU;
                StartCoroutine(ShowLoadingMenu(Population.instance.Initialize, mainMenu));
                break;
            case STATE.IN_GAME:
                state = STATE.IN_MENU;
                mainCamera.GetComponent<CameraManager>().enabled = true;
                StartCoroutine(ShowLoadingMenu(mainMenu));
                break;
            case STATE.IN_DUEL:
                SnakeSpawner.Instance.DestroyAllSnakes();
                FoodSpawner.Instance.foodSpawnType = FoodSpawnType.Normal;
                mainCamera.GetComponent<CameraManager>().enabled = true;
                state = STATE.IN_MENU;
                StartCoroutine(ShowLoadingMenu(Population.instance.Initialize, mainMenu));
                break;
        }
    }

    public void RefreshPlay()
    {
        #region APP_METRICA
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "interstitial" },
            { "placement", "replay" },
            { "result", "start " },
            { "connection", 1 }
        };
        AppMetricaManager.instance.SendReportAppMetrica(AppMetricaManager.Reports.VIDEO_STARTED, vals);
        #endregion
        ShowAds();
        switch (state)
        {
            case STATE.IN_GAME:
                PlayWithAI();
                break;

            case STATE.TEAM2X2:
                PlayWithTeam2x2();
                break;
            case STATE.TEAM3X3:
                PlayWithTeam3x3 ();
                break;

            case STATE.IN_DUEL:
                PlayDuel();
                break;
        }
    }

    public void Revive()
    {
        switch (state)
        {
            case STATE.IN_GAME:
                PlayWithAI();
                break;

            case STATE.TEAM2X2:
                PlayWithTeam2x2();
                break;
            case STATE.TEAM3X3:
                PlayWithTeam3x3();
                break;

            case STATE.IN_DUEL:
                PlayDuel();
                break;
        }
    }

    IEnumerator PlayAI()
    {
        //SnakeSpawner.Instance.DestroyAllSnakes();
        FoodSpawner.Instance.isReset = true;
        FoodSpawner.Instance.isWiped = true;
        FoodSpawner.Instance.SwitchToNormalModeAndReset();
        mainCamera.GetComponent<CameraManager>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        ControlsActive(true);
        InGame = true;
        state = STATE.IN_GAME;
        SnakeSpawner.Instance.StartGameWithAI();
    }

    public void LooseWithAI()
    {

        if (state == STATE.IN_DUEL)
        {
            AppMetricaManager.instance.LevelFinish(PlayerPrefs.GetInt("level_number"), STATE_STRING, PlayerPrefs.GetInt("level_count"), "normal", 1, false, "normal", "classic", "Lose", (int)DuelManager.instance.timingToDuel, 0, 0);

            PlayerStatsManager.instance.SaveDuelLooses(1);
        }
        AppMetricaManager.instance.LevelFinish(PlayerPrefs.GetInt("level_number"), STATE_STRING, PlayerPrefs.GetInt("level_count"), "normal", 1, false, "normal", "classic", "Lose", (int)DuelManager.instance.timingToDuel, 0, 0);

        ControlsActive(false);
        gameplayMenu.SetActive(false);
        DuelManager.instance.timerDuel.SetActive(false);
        DuelManager.instance.isOnDuel = false;

        if (state != STATE.GO_TO_HOME_FROM_GAMEPLAY)
        {
            gameOverMenu.SetActive(true);
        }
        IncreaseSurvivingTime();
        PlayerStatsManager.instance.SaveSurvivalTime(SurvivalTimeTicks);
    }

    private void IncreaseSurvivingTime()
    {
        SurvivalTimeTicks += DateTime.Now.Ticks - SpawnTime.Ticks;
    }

    public void SnakeSpeedManager(bool anyValue)
    {
        dev_SpeedUp = anyValue;
        if (SnakeSpawner.Instance.playerSnake != null)
        {
            if (anyValue)
            {
                SnakeSpawner.Instance.StartSprint(SnakeSpawner.Instance.playerSnake);
            }
            else
            {
                SnakeSpawner.Instance.playerSnake.sprinting = false;
            }
        }
    }


    public void GoHomeFromGame()
    {
        if (state == STATE.IN_DUEL)
        {
            DuelManager.instance.FinishDuel();
        }
        /*SnakeSpawner.Instance.snakes[SnakeSpawner.Instance.playerID].isDestroyed = true;
        SnakeSpawner.Instance.snakes[SnakeSpawner.Instance.playerID].isPlayerSnake = false;*/
        state = STATE.GO_TO_HOME_FROM_GAMEPLAY;
        SnakeSpawner.Instance.DestroyAllSnakes();
        gameplayMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        StartCoroutine(ShowLoadingMenu (mainMenu, STATE.IN_MENU));
        //SetLoading();
    }

    public void SetLoading(GameObject additionalMenu = null)
    {
        StartCoroutine(ShowLoadingMenu(additionalMenu));
    }

    IEnumerator ShowLoadingMenu(GameObject additionalMenu = null)
    {
        loadingUI.SetActive(true);
        yield return new WaitForSeconds(GameConstants.loadingMenuDelay);
        loadingUI.SetActive(false);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }


    }

    IEnumerator ShowLoadingMenu(Action action, GameObject additionalMenu = null)
    {
        loadingUI.SetActive(true);
        yield return new WaitForSeconds(GameConstants.loadingMenuDelay);
        loadingUI.SetActive(false);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }
        action();


    }

    IEnumerator ShowLoadingMenu(GameObject additionalMenu, STATE state)
    {
        loadingUI.SetActive(true);
        yield return new WaitForSeconds(GameConstants.loadingMenuDelay);
        loadingUI.SetActive(false);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }
        this.state = state;


    }

    IEnumerator ShowLoadingMenuWithReward()
    {
        loadingUI.SetActive(true);
        yield return new WaitForSeconds(GameConstants.loadingMenuDelay);
        Invoke("WatchRewardToDoubleEgg", 0.5f);
        loadingUI.SetActive(false);


    }

    IEnumerator ShowSearchingMenu(Action action, GameObject additionalMenu = null)
    {
        searchUI.SetActive(true);
        yield return new WaitForSeconds(GameConstants.searchiingMenuDelay);
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        searchUI.SetActive(false);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }
        ControlsActive(true);
        action();


    }

    IEnumerator ShowSearchingMenu(Action<Action> action, Action action1, GameObject additionalMenu = null)
    {
        searchUI.SetActive(true);
        yield return new WaitForSeconds(GameConstants.searchiingMenuDelay);
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        searchUI.SetActive(false);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }
        ControlsActive(true);
        action(action1);


    }


    public void SetPlayerName(Text name)
    {
        GamePrefs.PLAYER_NAME = name.text;
    }

    public void GDPR_Acept()
    {
        GamePrefs.SetBool(GameUtils.GDPR_WINDOW, false);
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }


    public void ShowAds()
    {

#if IRONSOURCE
      ISManager.instance.ShowInterstitial();
#endif
#if APPLOVIN
        AppsLovinManager.instance.ShowInterstitial("duelgameover");
#endif
#if YODO
          YodoManager.instance.ShowInterstitial();
#endif

    }
    public void GetEggReward(Action act)
    {
        loadingUI.SetActive(true);
        actRewardDoubleEgg = act;
        Invoke("WatchRewardToDoubleEgg", 0.5f);
        Invoke("DisableLoading", 1f);
    }

    public void DisableLoading()
    {
        loadingUI.SetActive(false);
    }

    public void WatchRewardToDoubleEgg()
    {
#if APPLOVIN
        AppsLovinManager.instance.ShowRewardVideo(AppsLovinManager.RewardType.dailyDoubleEgg, "free_eggs", () =>
        {
            actRewardDoubleEgg();
            actRewardDoubleEgg = null;
        });
#endif
    }
    public void WatchRewardedVideoEgg()
    {
        //Debug.Log("Rewarded video opened!");
#if IRONSOURCE
        ISManager.instance.ShowRewardVideo("egg");
#endif
#if APPLOVIN
        AppsLovinManager.instance.ShowRewardVideo(AppsLovinManager.RewardType.egg, "free_eggs");
#endif
#if YODO
        YodoManager.instance.ShowRewardAd("egg");
#endif
    }

    public void WatchRewardedVideo()
    {
        //Debug.Log("Rewarded video opened!");

#if IRONSOURCE
        ISManager.instance.ShowRewardVideo("");
#endif
#if APPLOVIN
        AppsLovinManager.instance.ShowRewardVideo(AppsLovinManager.RewardType.revive, "revive");
#endif

#if YODO
            YodoManager.instance.ShowRewardAd("");
#endif


        #region APP_METRICA
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "interstitial" },
            { "placement", "revive" },
            { "result", "start " },
            { "connection", 1 }
        };
        AppMetricaManager.instance.SendReportAppMetrica(AppMetricaManager.Reports.VIDEO_STARTED, vals);
        #endregion
        /*if (GamePrefs.GetBool(GamePrefs.ON_TUTORIAL))
        {
            OnReplayButtonPressed();
            GamePrefs.SetBool(GamePrefs.ON_TUTORIAL, false);
            freeImage.SetActive(false);
            mainGameOverButtons.SetActive(true);
        }*/
    }

    public void SetNotification(string title, string desc, System.DateTime time)
    {
#if UNITY_ANDROID

        AndroidNotification notification = new AndroidNotification
        {
            Title = title,
            Text = desc,

            FireTime = time
        };

        AndroidNotificationCenter.SendNotification(notification, "channel_id");

#endif

#if UNITY_IPHONE

            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan((24), 0, 0),
                Repeats = false
            };

            var notification = new iOSNotification()
            {
                // You can optionally specify a custom identifier which can later be
                // used to cancel the notification, if you don't set one, a unique
                // string will be generated automatically.
                Identifier = "_notification_0"+10,
                Title = title,
                Body = desc,
                Subtitle = "",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);

#endif
    }


    public void SetupNotifications()
    {
#if UNITY_ANDROID
        for (int x = 1; x < 8; x++)
        {
            AndroidNotification notification = new AndroidNotification
            {
                Title = "You friends are waiting in Duels!",
                Text = "Don't miss out our new Snake Masks!",
                //  notification.RepeatInterval = TimeSpan.FromMinutes(1f);

                // notification.SmallIcon = "icon_0";
                //  notification.LargeIcon = "icon_1";
                FireTime = System.DateTime.Now.AddHours(24 * x)
            };

            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
#endif

#if UNITY_IPHONE
        for (int x = 1; x < 8; x++)
        {
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan((24*x), 0, 0),
                Repeats = false
            };

            var notification = new iOSNotification()
            {
                // You can optionally specify a custom identifier which can later be
                // used to cancel the notification, if you don't set one, a unique
                // string will be generated automatically.
                Identifier = "_notification_0"+x,
                Title = "You friends are waiting in Duels!",
                Body = "Don't miss out our new Snake Masks!",
                Subtitle = "",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
        }
#endif
        /*     Notifications.CancelAllPendingLocalNotifications();

             var notif = new NotificationContent();
             notif.title = "You friends are waiting in Duels!";
             notif.body = "Don't miss out our new Snake Masks!";
             // notif.categoryId = repeatCategoryId;
           //  DateTime triggerDate = DateTime.Now + new TimeSpan(0, 2, 0);
           //  Notifications.ScheduleLocalNotification(triggerDate, notif);
             Notifications.ScheduleLocalNotification(new TimeSpan(24, 0, 0), notif, NotificationRepeat.EveryDay);*/
    }
}
