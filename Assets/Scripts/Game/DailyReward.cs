using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    public Image[] dailyRewards;
    public GameObject[] claimedObject;
    public GameObject[] claimButtons;
    public GameObject[] shineButtons;
    public Color normalRewardColor;
    public Color takenRewardColor;

    // Start is called before the first frame update
    private void Start()
    {
        /*for (int x = 0; x < claimedObject.Length; x++)
        {
            claimedObject[x].SetActive(false);

        }*/
        
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnEnable()
    {
        SetupDailyRewardScreen();
    }

    public void SetupDailyRewardScreen()
    {
        RefreshPage();
    }

    public bool CheckAvailableReward()
    {
        if (PlayerPrefs.GetString("lastclaimtime", "") == "")
        {
            Debug.Log("no time");
            return true;
        }

        string strDate = PlayerPrefs.GetString("lastclaimtime", "");

        DateTime now1 = DateTime.FromBinary(long.Parse(strDate));
        DateTime newTime = now1.AddHours(24);
        //  Debug.Log(newTime + " " + DateTime.Now);
        DateTime newTime2 = now1.AddHours(48);
        if (DateTime.Now > newTime2)
        {
            PlayerPrefs.SetInt("rewardNum", 0);
            return true;
        }
        else if (DateTime.Now > newTime)
        {
            Debug.Log(newTime + " " + DateTime.Now + " is great ");
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetRemainingTimeNormal()
    {
        if (PlayerPrefs.GetString("lastclaimtime", "") == "")
        {
            //  Debug.Log("no time");
            return 0;
        }

        string strDate = PlayerPrefs.GetString("lastclaimtime", "");

        DateTime now1 = DateTime.FromBinary(long.Parse(strDate));
        //   DateTime newTime = now1.AddHours(24);
        //  TimeSpan subNow = now1.Subtract(newTime);
        DateTime currentTime = DateTime.Now;
        TimeSpan newSpan = currentTime.Subtract(now1);
        return ((24 * 60 * 60) - (int)newSpan.TotalSeconds);
        /*  float normalizedValue = Mathf.InverseLerp(0, subNow.Minutes, newSpan.Minutes);
          float result = Mathf.Lerp(0f, 1f, normalizedValue);

          return result;*/
    }

    public void GetRewardDoubleEgg()
    {
        GameManager.instance.GetEggReward(() => { GetRewardEgg(1); });
    }

    public void GetRewardEgg(int multiplier = 1)
    {
        int num = PlayerPrefs.GetInt("rewardNum", 0);
        int gotEggs = 0;
        switch (num)
        {
            case 0:
            case 1:

                gotEggs = 3;
                break;

            case 2:
            case 3:
                gotEggs = 5;
                break;

            case 4:
            case 5:
                gotEggs = 7;
                break;

            case 6:
                gotEggs = 10;
                break;
        }
        num++;

        gotEggs *= multiplier;

        PlayerPrefs.SetInt("rewardNum", num);
        DateTime now1 = DateTime.Now;
        string strDate = now1.ToBinary().ToString();
        // DateTime now2 = DateTime.FromBinary(long.Parse(strDate));

        PlayerPrefs.SetString("lastclaimtime", strDate);
        DateTime newTime = now1.AddHours(24);
        //GameManager.instance.SetNotification("Don't miss out yoir daily rewards", "Get a new mask now 🐍 !!!", newTime);
        StoreManager.instance.IncreaseEggs(gotEggs);
        DialogueManager.instance.PopUp("You got " + gotEggs.ToString() + " Eggs!");

        //EggsManager.instance.ToggleDailyRewardBtns(false);
        gameObject.SetActive(false);
    }

    public void RefreshPage()
    {
        int num = PlayerPrefs.GetInt("rewardNum", 0);
        bool rewardAvail = CheckAvailableReward();
        for (int x = 0; x < 7; x++)
        {
            //shineButtons[x].SetActive(false);
            claimButtons[x].SetActive(false);
            dailyRewards[x].color = normalRewardColor;
            claimedObject[x].SetActive(false);

            //  claimedText[x].SetActive(false);
            //    selectedImg[x].sprite = selectedSpr;
            //    claimBtn[x].SetActive(false);
        }

        //  if (num > 4)
        //    num = 3;
        if (rewardAvail && num == 7)
        {
            PlayerPrefs.SetInt("rewardNum", 0);
            num = 0;
            //return;
        }

        for (int x = 0; x < num + 1; x++)
        {
            if (x == num && rewardAvail)
            {
                claimButtons[x].SetActive(true);
                //shineButtons[x].SetActive(true);
                //     claimBtn[x].SetActive(true);
            }
            else if (x < num)
            {
                dailyRewards[x].color = takenRewardColor;
                claimedObject[x].SetActive(true);
                //  claimedText[x].SetActive(true);
                //    selectedImg[x].sprite = unselectedSpr;
                //  claimBtn[x].SetActive(false);
            }
        }

        claimButtons[0].SetActive(true);
    }
}