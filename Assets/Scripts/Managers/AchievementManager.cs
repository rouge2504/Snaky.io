using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void SetAchievementGoal(string achievement)
    {

            GamePrefs.SetBool(achievement, true);
    }

    public void KillerCounter(int counter)
    {
        if (counter >= 1)
        {
            GamePrefs.SetBool(GameUtils.PREFS_FIRST_KILL, true);

        }
        if (counter >= 5)
        {
            SetAchievementGoal(GameUtils.PREFS_FIVE_KILL);
        }

        if (counter >= 10)
        {
            SetAchievementGoal(GameUtils.PREFS_TEN_KILL);
        }

        if (counter >= 100)
        {
            SetAchievementGoal(GameUtils.PREFS_HUNDRED_KILL);
        }
    }

    public void EditSnake()
    {
        if (PlayerPrefs.HasKey(GameUtils.PREFS_EDIT_SNAKE))
        {
            GamePrefs.SetBool(GameUtils.PREFS_EDIT_SNAKE, true);
        }
    }

  
}
