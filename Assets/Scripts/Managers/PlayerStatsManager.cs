using System;
//using GooglePlayGames.BasicApi;
//using GooglePlayGames.Native.Cwrapper;    //Commented out recently
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{

  private const string KEY_BEST_SCORE = "KEY_BEST_SCORE";
  private const string KEY_BEST_KILLS = "KEY_BEST_KILS";
  private const string KEY_BEST_TOP = "KEY_BEST_TOP";
  private const string KEY_BEST_SURVIVAL_TIME = "KEY_BEST_SURVIVAL_TIME";
  
  private const string KEY_LAST_SCORE = "KEY_LAST_SCORE";
  private const string KEY_LAST_KILLS = "KEY_LAST_KILLS";
  private const string KEY_KILLS = "KEY_KILLS";
  private const string KEY_LAST_TOP = "KEY_LAST_TOP";
  private const string KEY_LAST_SURVIVAL_TIME = "KEY_LAST_SURVIVAL_TIME";
  private const string KEY_DUEL_VITORIES = "KEY_DUEL_VITORIES";
  private const string KEY_DUEL_LOSES = "KEY_DUEL_LOSES";

    private const string KEY_EGGS_COUNTER = "KEY_EGGS_COUNTER";


    public static PlayerStatsManager instance;

    [Header("Menu")]
    [SerializeField] private StatObject highScore;
    [SerializeField] private StatObject totalKills;
    [SerializeField] private StatObject achievements;
    [SerializeField] private StatObject mostKillinMatch;
    [SerializeField] private StatObject duelVictories;
    [SerializeField] private StatObject duelLoses;
    [SerializeField] private StatObject totalEggsWon;
    [SerializeField] private StatObject totalTimeInFirstPlace;
    [SerializeField] private StatObject TotalTimePlayed;
    public void Start()
    {
        instance = this;
    }
    public int EggsCounter
    {
        get
        {
            return PlayerPrefs.GetInt(KEY_EGGS_COUNTER, 0);
        }
        set
        {
            PlayerPrefs.SetInt(KEY_EGGS_COUNTER, value);
        }
    }

    public void OpenStats()
    {

        var bestSurvivingTime = TimeSpan.FromTicks(LoadBestSurvivalTime());
        var lastSurvivingTime = TimeSpan.FromTicks(LoadLastSurvivalTime());

        var bestTopOneTime = TimeSpan.FromTicks(LoadBestTopOneTime());
        var lastTopOneTime = TimeSpan.FromTicks(LoadLastTopOneTime());
        highScore.points.text = LoadLastScore().ToString();
        totalKills.points.text = LoadKills().ToString();
        int achivmentCounter = 0;
        for (int i = 0; i < 10; i++)
        {
            achivmentCounter += PlayerPrefs.GetInt("pro" + i);

        }

        achievements.points.text = achivmentCounter + "/10";

        mostKillinMatch.points.text = LoadBestKills().ToString();

        duelVictories.points.text = LoadDuelVictories().ToString();
        duelLoses.points.text = LoadDuelLooses().ToString();

        totalEggsWon.points.text = EggsCounter.ToString();
        totalTimeInFirstPlace.points.text = string.Format("{0:D2}:{1:D2}:{2:D2}", lastTopOneTime.Hours, lastTopOneTime.Minutes, lastTopOneTime.Seconds);
        TotalTimePlayed.points.text = string.Format("{0:D2}:{1:D2}:{2:D2}", bestSurvivingTime.Hours, bestSurvivingTime.Minutes, bestSurvivingTime.Seconds);

    }

    public void SaveKillsRed(int count)
    {
        int iterator = PlayerPrefs.GetInt(KEY_KILLS, 0);
        iterator += count;
        PlayerPrefs.SetInt(KEY_KILLS, iterator);

        if (count > LoadBestKills())
        {
            PlayerPrefs.SetInt(KEY_BEST_KILLS, count);
        }
    }

    public int LoadKills()
    {
        return PlayerPrefs.GetInt(KEY_KILLS, 0);
    }


  public void SaveScore(int score)
  {
    PlayerPrefs.SetInt(KEY_LAST_SCORE, score);

    if (score > LoadBestScore())
    {
      PlayerPrefs.SetInt(KEY_BEST_SCORE, score);
    }
  }

  public void SaveKills(int kills)
  {
        int iterator = PlayerPrefs.GetInt(KEY_KILLS, 0);
        iterator += kills;
        PlayerPrefs.SetInt(KEY_KILLS, iterator);

    if (kills > LoadBestKills())
    {
      PlayerPrefs.SetInt(KEY_BEST_KILLS, kills);
    }
  }
  
  public void SaveSurvivalTime(long time)
  {
    PlayerPrefs.SetString(KEY_LAST_SURVIVAL_TIME, time.ToString());

    if (time > LoadBestSurvivalTime())
    {
      PlayerPrefs.SetString(KEY_BEST_SURVIVAL_TIME, time.ToString());
    }
  }
  
  public void SaveTopOneTime(long time)
  {
    PlayerPrefs.SetString(KEY_LAST_TOP, time.ToString());

    if (time > LoadBestTopOneTime())
    {
      PlayerPrefs.SetString(KEY_BEST_TOP, time.ToString());
    }
  }

    public void SaveDuelVictories(int victories)
    {
        int iterator = PlayerPrefs.GetInt(KEY_DUEL_VITORIES, 0);
        iterator += victories;
        PlayerPrefs.SetInt(KEY_DUEL_VITORIES, iterator);
    }

    public void SaveDuelLooses(int looses)
    {
        int iterator = PlayerPrefs.GetInt(KEY_DUEL_LOSES, 0);
        iterator += looses;
        PlayerPrefs.SetInt(KEY_DUEL_LOSES, iterator);
    }

    public int LoadDuelLooses()
    {
        return PlayerPrefs.GetInt(KEY_DUEL_LOSES, 0);
    }


    public int LoadDuelVictories()
    {
        return PlayerPrefs.GetInt(KEY_DUEL_VITORIES, 0);
    }
  
  public int LoadBestScore()
  {
    return PlayerPrefs.GetInt(KEY_BEST_SCORE, 0);
  }

  public int LoadBestKills()
  {
    return PlayerPrefs.GetInt(KEY_BEST_KILLS, 0);
  }

  public long LoadBestSurvivalTime()
  {
    return Convert.ToInt64(PlayerPrefs.GetString(KEY_BEST_SURVIVAL_TIME, "0"));
  }
  
  public long LoadBestTopOneTime()
  {
    return Convert.ToInt64(PlayerPrefs.GetString(KEY_BEST_TOP, "0"));
  }
  
  public int LoadLastScore()
  {
    return PlayerPrefs.GetInt(KEY_LAST_SCORE, 0);
  }

  public int LoadLastKills()
  {
    return PlayerPrefs.GetInt(KEY_LAST_KILLS, 0);
  }
  
  public long LoadLastSurvivalTime()
  {
    return Convert.ToInt64(PlayerPrefs.GetString(KEY_LAST_SURVIVAL_TIME, "0"));
  }
  
  public long LoadLastTopOneTime()
  {
    return Convert.ToInt64(PlayerPrefs.GetString(KEY_LAST_TOP, "0"));
  }

}