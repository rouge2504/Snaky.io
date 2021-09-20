using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if APPLOVIN

public class AppsLovinManager : MonoBehaviour
{
    public enum RewardType
    {
        none,
        revive,
        egg,
        duelmode,
        additionSnakeLenght,
        dailyDoubleEgg
    }

    private const string MaxSdkKey = "6AQkyPv9b4u7yTtMH9PT40gXg00uJOTsmBOf7hDxa_-FnNZvt_qTLnJAiKeb5-2_T8GsI_dGQKKKrtwZTlCzAR";
    private string InterstitialAdUnitId = "9a1d74c55376a20d";
    private string RewardedAdUnitId = "0ee2015c0a703852";

    private Action onDoubleEggAction;

    public static AppsLovinManager instance;     //Commented out recently
    public RewardType rewardType = RewardType.none;
    private string placement = "default";

    public float timeToCoolDown = 120;
    public float timinToCoolDown;
    public bool activeCoolDown = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("AppsLovinManager add more then need");
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (activeCoolDown)
        {
            timinToCoolDown += Time.deltaTime;
            if (timinToCoolDown > timeToCoolDown)
            {
                timinToCoolDown = 0;
                activeCoolDown = false;
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
#if UNITY_IPHONE
        InterstitialAdUnitId = "0d511d95bd573d06";
        RewardedAdUnitId = "f0301b5ed5bb3a0d";
#endif

        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            // AppLovin SDK is initialized, configure and start loading ads.
            Debug.LogWarning("MAX SDK Initialized");

            InitializeInterstitialAds();
            InitializeRewardedAds();

            if (MaxSdk.IsInitialized())
            {
                Debug.LogWarning("MaxSdk.InitializeSdk Initialized x3");
            }
            else
            {
                Debug.LogError("MaxSdk.InitializeSdk NOT Initialized! x3");
            }
        };

        //MaxSdk.SetSdkKey(MaxSdkKey);
        //MaxSdk.InitializeSdk();

        if (MaxSdk.IsInitialized())
        {
            Debug.LogWarning("MaxSdk.InitializeSdk Initialized x2");
        }
        else
        {
            Debug.LogError("MaxSdk.InitializeSdk NOT Initialized! x2");
        }
        StartCoroutine(tryMaxSdkInitialize());
    }
    public IEnumerator tryMaxSdkInitialize()
    {
        while (!MaxSdk.IsInitialized())
        {
            Debug.LogWarning("TRY MaxSdk.InitializeSdk Initialized Coroutine");
            MaxSdk.SetSdkKey(MaxSdkKey);
            MaxSdk.InitializeSdk();
            yield return new WaitForSeconds(1);
        }
        Debug.LogWarning("MaxSdk.InitializeSdk Initialized Coroutine");
    }
    public void ShowInterstitial(string pl)
    {
        if (PlayerPrefs.GetInt("ads", 1) == 0)
        {
            return;
        }


            placement = pl;
            if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
            {
                MaxSdk.ShowInterstitial(InterstitialAdUnitId);
            }
            else
            {
            }

    }

    public bool isVideoAvailable()
    {
        bool isVideoAvailable = MaxSdk.IsRewardedAdReady(RewardedAdUnitId);

        return isVideoAvailable;
    }

    public void ShowRewardVideo(RewardType type, string pl, Action onDoubleEgg = null)
    {
        onDoubleEggAction = onDoubleEgg;
        rewardType = type;
        placement = pl;

            if (MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
            {
                MaxSdk.ShowRewardedAd(RewardedAdUnitId);
            }
            else
            {
                Debug.LogError("ShowRewardVideo: IsRewardedAdReady -> False");
            }


        //GameManager.instance.gameOverCanvas.SetActive(false);
        // AchievementManager._instance.PlaySessionCount();
    }

    #region Interstitial Ad Methods

    private void InitializeInterstitialAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;
        placement = "default";
        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    }

    #region Rewarded Ad Methods

    private void OnInterstitialLoadedEvent(string adUnitId)
    {
#if EVENTS
            Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "Interstitial" },
            { "placement", placement },
            { "result", "success" },
            { "connection", 1 }
        };
            AppMetrica.Instance.ReportEvent("video_ads_available", vals);

        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
#endif
    }

    private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
    {
#if EVENTS
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "Interstitial" },
            { "placement", placement },
            { "result", "not_available" },
            { "connection", 1 }
        };
        AppMetrica.Instance.ReportEvent("video_ads_available", vals);
        // Interstitial ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
#endif
        //   interstitialRetryAttempt++;
        //   double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));

        Invoke("LoadInterstitial", 6f);
        Debug.LogError("OnInterstitialFailedEvent " + errorCode);
    }

    private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        Debug.LogError("InterstitialFailedToDisplayEvent " + errorCode);

#if EVENTS
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "Interstitial" },
            { "placement", placement },
            { "result", "failed" },
            { "connection", 1 }
        };
        AppMetrica.Instance.ReportEvent("video_ads_started", vals);
#endif
        LoadInterstitial();
    }

    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        Debug.LogWarning("OnInterstitialDismissedEvent ");
#if EVENTS
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "Interstitial" },
            { "placement", placement },
            { "result", "watched" },
            { "connection", 1 }
        };
        AppMetrica.Instance.ReportEvent("video_ads_watch", vals);
#endif
        LoadInterstitial();
    }

    private void InitializeRewardedAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first RewardedAd
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId)
    {
#if EVENTS
            Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "rewarded" },
            { "placement", placement },
            { "result", "success" },
            { "connection", 1 }
        };
            AppMetrica.Instance.ReportEvent("video_ads_available", vals);

#endif
    }

    private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
    {
#if EVENTS
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "rewarded" },
            { "placement", placement },
            { "result", "not_available" },
            { "connection", 1 }
        };
        AppMetrica.Instance.ReportEvent("video_ads_available", vals);
#endif

        Invoke("LoadRewardedAd", 6f);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {
#if EVENTS
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "rewarded" },
            { "placement", placement },
            { "result", "not_available" },
            { "connection", 1 }
        };
        AppMetrica.Instance.ReportEvent("video_ads_available", vals);
#endif
        // Rewarded ad failed to display. We recommend loading the next ad
        Debug.Log("Rewarded ad failed to display with error code: " + errorCode);
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId)
    {
#if EVENTS
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "rewarded" },
            { "placement", placement },
            { "result", "start" },
            { "connection", 1 }
        };
        AppMetrica.Instance.ReportEvent("video_ads_started", vals);
#endif
        // Debug.Log("Rewarded ad displayed");
    }

    private void OnRewardedAdClickedEvent(string adUnitId)
    {
#if EVENTS
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "rewarded" },
            { "placement", placement },
            { "result", "clicked" },
            { "connection", 1 }
        };
        AppMetrica.Instance.ReportEvent("video_ads_started", vals);
#endif
        Debug.Log("Rewarded ad clicked");
    }

    private void OnRewardedAdDismissedEvent(string adUnitId)
    {
#if EVENTS
        if (!ifRewarded)
        {
            Dictionary<string, object> vals = new Dictionary<string, object>
            {
                { "ad_type", "rewarded" },
                { "placement", placement },
                { "result", "canceled" },
                { "connection", 1 }
            };
            AppMetrica.Instance.ReportEvent("video_ads_watch", vals);
            if (rewardType == RewardType.revive)
            {
                GameManager.instance.gameOverCanvas.SetActive(true);
            }
        }

        ifRewarded = false;
#endif
        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded ad dismissed");
        LoadRewardedAd();
    }

    private bool ifRewarded = false;

    #endregion Rewarded Ad Methods

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
    {
#if EVENTS
        Dictionary<string, object> vals = new Dictionary<string, object>
        {
            { "ad_type", "rewarded" },
            { "placement", placement },
            { "result", "watched" },
            { "connection", 1 }
        };
        AppMetrica.Instance.ReportEvent("video_ads_watch", vals);
        ifRewarded = true;
#endif
        if (rewardType == RewardType.revive)
        {
            //Debug.Log("is rewarded :- " + isRewarded);
            /*GameManager.instance.watchedAd = true;
            GameManager.instance.loadingOverlay.SetActive(true);
            GameManager.instance.KillAllSnakes();*/
            GameManager.instance.Revive();
        }
        else if (rewardType == RewardType.egg)
        {
            int gotEggs = 2;
            StoreManager.instance.IncreaseEggs(gotEggs);
            DialogueManager.instance.PopUp("You got " + gotEggs.ToString() + " Eggs!");
            rewardType = RewardType.none;
        }
        else if (rewardType == RewardType.duelmode)
        {
            //GameManager.instance.ExtendDuelModeTimer();
            rewardType = RewardType.none;
        }
        else if (rewardType == RewardType.additionSnakeLenght)
        {
           /* GameManager.instance.IsRewardedVideo = true;
            GameManager.instance.OnPlayWithAI();*/
            rewardType = RewardType.none;
        }
        else if (rewardType == RewardType.dailyDoubleEgg)
        {
            //GameManager.instance.IsRewardedVideo = true;
            onDoubleEggAction?.Invoke();
            onDoubleEggAction = null;
            rewardType = RewardType.none;
        }
        // Rewarded ad was displayed and user should receive the reward
        Debug.Log("Rewarded ad received reward");
    }

    #endregion Interstitial Ad Methods
}

#endif