using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrefs
{
    public static string MASK
    {
        get
        {
            return PlayerPrefs.GetString("MASK_PLAYER");
            
        }

        set
        {
            PlayerPrefs.SetString("MASK_PLAYER", value);
        }
    }

    public static bool GetBool(string pref, int defaultValue = 1)
    {
        int result = PlayerPrefs.GetInt(pref, defaultValue);

        return result == 0 ? false : true;
    }

    public static void SetBool(string pref, bool value)
    {
        PlayerPrefs.SetInt(pref, value ? 1 : 0);
    }


}
