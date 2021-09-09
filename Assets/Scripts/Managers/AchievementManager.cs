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

    public void EditSnake()
    {
        if (PlayerPrefs.HasKey(GameUtils.PREFS_EDIT_SNAKE))
        {
            GamePrefs.SetBool(GameUtils.PREFS_EDIT_SNAKE, true);
        }
    }

  
}
