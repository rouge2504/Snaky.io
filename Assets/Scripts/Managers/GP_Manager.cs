using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP_Manager : MonoBehaviour
{
    public static GP_Manager instance;
    [HideInInspector] public bool isConnectedToGooglePlayServices;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
.Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        SignInToGooglePlayService();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SignInToGooglePlayService()
    {


        PlayGamesPlatform.Instance.Authenticate((success, message) =>
        {
            /* switch (result)
             {
                 case SignInStatus.Success:
                     isConnectedToGooglePlayServices = true;
                     PlayGamesPlatform.Instance.ShowAchievementsUI();
                     Debug.Log("Sign in success");
                     break;
                 default:

                     isConnectedToGooglePlayServices = false;
                     Debug.Log("Sign in failed: "  );
                     break;
             }*/
            if (success)
            {
                isConnectedToGooglePlayServices = true;
                //PlayGamesPlatform.Instance.ShowAchievementsUI();
                Debug.Log("Sign in success");
            }
            else
            {
                isConnectedToGooglePlayServices = false;
                Debug.Log("Sign in failed: pas to corutine: " + message);
                StartCoroutine(WaitForAuthenticationCoroutine());
            }
        }, false);

    }

    private const float AuthenticationWaitTimeSeconds = 10;

    private IEnumerator WaitForAuthenticationCoroutine()
    {
        var startTime = Time.realtimeSinceStartup;

        while (!Social.localUser.authenticated)
        {
            if (Time.realtimeSinceStartup - startTime > AuthenticationWaitTimeSeconds)
            {
                // X seconds have passed and we are still not authenticated, time to give up.
                break;
            }

            yield return null;
        }

        if (Social.localUser.authenticated)
        {
            Debug.Log("Sign in success");
        }
        else
        {
            Debug.Log("Sign in failed: ");
        }
    }
}
