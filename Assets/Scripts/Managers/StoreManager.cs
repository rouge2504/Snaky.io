using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;

    public Text eggsMainMenu;
    public Text eggsSkinsMenu;

    public GameObject EggsMenu;

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
        SuccesPurchase("You purchased " + count + " Eggs!");
    }

    public void OnSuccessfulPurchaseEggPack(int eggs)
    {
        Debug.Log("eggs purchased");
        IncreaseEggs(eggs);
        DialogueManager.instance.PopUp("You purchased " + eggs + " Eggs!");
    }

    public void OnSuccessfulPurchaseRemoveAD()
    {
        PlayerPrefs.SetInt(GameUtils.ADS, 0);
        //AdRemoveBtn.SetActive(false);
        Debug.Log("Ad button successfully removed");
    }

    public void OnSuccessfulPurchaseBabySnake()
    {
        PlayerPrefs.SetInt(GameUtils.BABY_SNAKE, 1);
        //babySnakeBtn.SetActive(false);
        Debug.Log("Baby Snake button successfully removed");
    }

    public void OnSuccessfulPurchaseTransperancy()
    {
        BuyTransparency();
    }

    public void OnSuccessfulPurchaseBundle()
    {
        BuyBundle();
    }

    public void IncreaseEggs(int count)
    {
        PlayerStatsManager.instance.EggsCounter = count;
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

    public void BuyTransparency()
    {
        SkinsManager.instance.UpdateTransparencyState(GamePrefs.GetBool(GameUtils.TRANSPARENCY));

        AchievementManager.instance.BuyTransparency();
    }

    public void BuyBundle()
    {
        BuyTransparency();
        IncreaseEggs(15);
        NoADS();
        SuccesPurchase("You purchased Bundle!");
    }

    public void NoADS()
    {
        GamePrefs.SetBool(GameUtils.ADS, true);
    }

    public void SuccesPurchase(string text)
    {
        DialogueManager.instance.PopUp(text);
        EggsMenu.SetActive(false);

    }


    public void OnRemoveAdsButtonClick()
    {
#if UIP
#if UNITY_ANDROID
        UnityIAP.instance.BuyItem("com.camc.slitherio.snakes.worms.adremove");    //Commented out recently
#endif
#if UNITY_IPHONE
        UnityIAP.instance.BuyItem("com.camc.removeads");
#endif
#endif
        //GameManager.instance.ifAdRemoveClicked = true;
    }

    public void RemoveAds(string key)
    {
        PlayerPrefs.SetInt(key, 1);
    }

    public void OnBabySnakeButtonClick()
    {
#if UIP
#if UNITY_ANDROID
        UnityIAP.instance.BuyItem("com.camc.slitherio.snakes.worms.babysnake");    //Commented out recently
#endif
#if UNITY_IPHONE
        UnityIAP.instance.BuyItem("com.camc.babysnake");
#endif
#endif
        //  GameManager.instance.ifAdRemoveClicked = true;
    }

    public void OnBundleButtonClick()
    {
#if UIP
#if UNITY_ANDROID
        UnityIAP.instance.BuyItem("com.camc.slitherio.snakes.worms.bundle");    //Commented out recently
#endif
#if UNITY_IPHONE
        UnityIAP.instance.BuyItem("com.camc.bundle");
#endif
#endif
        //  GameManager.instance.ifAdRemoveClicked = true;
    }

    public void OnTestPurchaseClicked()
    {
#if UIP
#if UNITY_ANDROID
        Debug.Log("purchased test id");
        UnityIAP.instance.BuyItem("android.test.purchased");    //Commented out recently
#endif
#endif
    }

    public void OnEggButtonClick(string eggName)
    {
#if UIP
#if UNITY_ANDROID
        UnityIAP.instance.BuyItem("com.camc.slitherio.snakes.worms." + eggName);    //Commented out recently
#endif
#if UNITY_IPHONE
        UnityIAP.instance.BuyItem("com.camc."+eggName);
#endif
#endif
        //  GameManager.instance.ifAdRemoveClicked = true;
    }

    public void OnRestoreAdsButtonClick()
    {
#if UIP
        UnityIAP.instance.RestorePurchase();  //Commented out recently
#endif
    }
}
