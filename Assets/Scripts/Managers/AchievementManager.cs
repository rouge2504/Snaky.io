using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
#if GPG
using GooglePlayGames.BasicApi;
using GooglePlayGames;
#endif
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void SetAchievementGoal(string achievement, string gpfsid, string pro)
    {

            

#if UNITY_ANDROID
        Social.Active.ReportProgress(gpfsid, 100.0f, (bool success) =>  //Social.Active.ReportProgress(SnakeMask.achievement_kill_your_first_snake, 100.0f, (bool success) =>
        {
            // handle success or failure
            if (success)
            {
                GamePrefs.SetBool(achievement, true);
                GamePrefs.SetBool(pro, true);
                PlayerPrefs.Save();

            }
        });
#endif
#if UNITY_EDITOR_WIN
        GamePrefs.SetBool(achievement, true);
#endif
    }

    public void KillerCounter(int counter)
    {
        if (counter >= 1)
        {
            SetAchievementGoal(GameUtils.PREFS_FIRST_KILL, GPGSID.achievement_first_slithering_kill, "pro0");

        }
        if (counter >= 5)
        {
            SetAchievementGoal(GameUtils.PREFS_FIVE_KILL, GPGSID.achievement_five_slither_killing, "pro1");
        }

        if (counter >= 10)
        {
            SetAchievementGoal(GameUtils.PREFS_TEN_KILL, GPGSID.achievement_ten_slithering_kills, "pro2");
        }

        if (counter >= 100)
        {
            SetAchievementGoal(GameUtils.PREFS_HUNDRED_KILL, GPGSID.achievement_hundred_slithering_kills, "pro3");
        }
    }

    public void PointCounter(int counter)
    {
        if (counter >= 200)
        {
            SetAchievementGoal(GameUtils.PREFS_TWO_HUNDRED_POINTS, GPGSID.achievement_get_200_points, "pro6");
        }

        if (counter >= 100000)
        {
            SetAchievementGoal(GameUtils.PREFS_ONE_LAKH_POINTS, GPGSID.achievement_get_100000_points, "pro7");

        }
    }

    public void PlaySecondTime()
    {
        SetAchievementGoal(GameUtils.PREFS_PLAY_SECOND_TIME, GPGSID.achievement_play_for_the_second_time, "pro8");
    }

    public void BuyTransparency()
    {

        SetAchievementGoal(GameUtils.PREFS_BUY_TRANSPARENCY, GPGSID.achievement_buy_transparency, "pro9");
    }

    public void EditSnake()
    {
        SetAchievementGoal(GameUtils.PREFS_EDIT_SNAKE, GPGSID.achievement_edit_your_snake, "pro4");

    }

    public void SocialMediaShare()
    {

        SetAchievementGoal(GameUtils.PREFS_SOCIAL_MEDIA, GPGSID.achievement_share_you_snake, "pro5");
    }


    public void PlaySessionCount()
    {
        int playSessionCounter;
        if (!PlayerPrefs.HasKey(GameUtils.PREFS_SECOND_PLAY))
        {
            playSessionCounter = 1;
        }
        else
        {
            playSessionCounter = PlayerPrefs.GetInt(GameUtils.PREFS_SECOND_PLAY);
            playSessionCounter++;
        }

        PlayerPrefs.SetInt(GameUtils.PREFS_SECOND_PLAY, playSessionCounter);
        PlayerPrefs.Save();

        if (playSessionCounter >= 1)
        {
            PlaySecondTimeNotifier();
        }

        UnlockAllProMasks();
    }

    void PlaySecondTimeNotifier()
    {
        PlaySecondTime();
    }

    public void UnlockAllProMasks()
    {
        Social.LoadAchievements(achievements => {
            if (achievements.Length > 0)
            {
                int count = 0;
                //    Debug.Log("Got " + achievements.Length + " achievement instances");
                //   string myAchievements = "My achievements:\n";
                foreach (IAchievement achievement in achievements)
                {
                    /*    myAchievements += "\t" +
                            achievement.id + " " +
                            achievement.percentCompleted + " " +
                            achievement.completed + " " +
                            achievement.lastReportedDate + "\n";*/
                    if (achievement.completed)
                    {

                        PlayerPrefs.SetInt("pro" + count, 1);
                    }
                    count++;
                }
                //  Debug.Log(myAchievements);
            }
            else
                Debug.Log("No achievements returned");
        });
    }

    public void OnAchievementButtonClicked()
    {

        Debug.Log("Achievement button clicked");
        if (Social.localUser.authenticated)
        {
#if UNITY_ANDROID
#if GPG
             Social.ShowAchievementsUI();
            ((PlayGamesPlatform)Social.Active).ShowAchievementsUI();  //Commented out recently
#endif
#else
            //Social.ShowLeaderboardUI();
            Social.Active.ShowAchievementsUI();
#endif

        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Login Sucess");
#if UNITY_ANDROID
#if GPG
                        Social.ShowAchievementsUI();
                        ((PlayGamesPlatform)Social.Active).ShowAchievementsUI();   //Commented out recently
#endif
#else
                    //Social.ShowLeaderboardUI();
                    Social.Active.ShowAchievementsUI();
#endif
                }
                else
                {
                    Debug.Log("Login failed");
                }
            });
        }

    }

    public void OnLeaderboardClick()
    {
        if (Social.localUser.authenticated)
        {
#if UNITY_ANDROID
#if GPG
            //Social.ShowLeaderboardUI();
            // ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSID.leaderboard_leaderboard);   //Commented out recently
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
#endif
#else
            //Social.ShowLeaderboardUI();
            Social.Active.ShowLeaderboardUI ();
#endif

        }
        else
        {
            Debug.Log("Inside leaderboard else");
#if GPG
            Social.localUser.Authenticate((bool success) =>
               {
                   if (success)
                   {
#if UNITY_ANDROID
                       Social.ShowLeaderboardUI();
                       //((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSID.leaderboard_leaderboard);    //Commented out recently
                       PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSID.leaderboard_slither_high_score);
#else
						//Social.ShowLeaderboardUI();
						Social.Active.ShowLeaderboardUI ();
#endif
                   }
                   else
                   {
                       //Debugger.instance.DebugLog("Authentication failed");
                   }
               });
#endif
        }
    }

}
