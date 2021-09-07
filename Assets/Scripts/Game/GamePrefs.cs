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

    
}
