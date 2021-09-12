using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrefs
{
    public static string PLAYER_NAME
    {
        get
        {
            return PlayerPrefs.GetString("PlayerName", "You");
        }

        set
        {
            PlayerPrefs.SetString("PlayerName", value);
        }
    }
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

    public static int EGGS
    {
        get
        {
            return PlayerPrefs.GetInt("eggs", 9);
        }

        set
        {
            PlayerPrefs.SetInt("eggs", value);
        }
    }

    public static float PLAYER_TRASNPARENCY
    {
        get
        {
            return PlayerPrefs.GetFloat("PlayerTransparency", 1f);
        }

        set
        {
            PlayerPrefs.SetFloat("PlayerTransparency", value);
        }
    }
    public static bool GetBool(string pref, int defaultValue = 1)
    {
        if (!PlayerPrefs.HasKey(pref))
        {
            Debug.Log("No key");
        }
        int result = PlayerPrefs.GetInt(pref, defaultValue);
        PlayerPrefs.SetInt(pref, result == 1 ? 1 : 0);
        return result == 0 ? false : true;
    }

    public static void SetBool(string pref, bool value)
    {
        int result = value ? 1 : 0;
        PlayerPrefs.SetInt(pref, result);
    }


}
