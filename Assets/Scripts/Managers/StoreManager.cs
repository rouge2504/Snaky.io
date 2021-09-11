using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;

    public Text eggsMainMenu;
    public Text eggsSkinsMenu;

    private void Start()
    {
        instance = this;
        UpdateEggsView();
        
    }

    private void UpdateEggsView()
    {
        eggsMainMenu.text = GamePrefs.EGGS.ToString();
        eggsSkinsMenu.text = GamePrefs.EGGS.ToString();
    }

    public void BuyEggs(int count)
    {
        GamePrefs.EGGS += count;
        UpdateEggsView();
    }

    public bool DecreaseEggs(SkinMask skinMask)
    {
        if (skinMask.eggValue > GamePrefs.EGGS || !GamePrefs.GetBool(skinMask.id))
        {
            DialogueManager.instance.PopUp("You need to get more eggs win more duels to earn eggs");
            return false;
        }
        else
        {
            GamePrefs.SetBool(skinMask.id, true);
            GamePrefs.EGGS -= skinMask.eggValue;
            UpdateEggsView();
            return true;
        }
    }
}
