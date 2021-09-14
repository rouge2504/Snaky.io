using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DailyTimer : MonoBehaviour
{
    public DailyReward dailyReward;
    public Image timeSlider;
    public Button btn;
    public Text timer;
    public GameObject shiny;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        
    }

    public void WaitRewardButton()
    {

        StartCoroutine(WaitReward());
        dailyReward.GetRewardDoubleEgg();
    }

    private IEnumerator WaitReward()
    {
        dailyReward.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        dailyReward.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (dailyReward.CheckAvailableReward())
        {
            timer.enabled = false;
            timeSlider.fillAmount = 0f;
            btn.interactable = true;
            shiny.SetActive(true);
        }
        else
        {
            int minutesLeft = dailyReward.GetRemainingTimeNormal();
           // Debug.Log(minutesLeft + " minutes left");
            if (minutesLeft < 1)
            {
                timer.enabled = false;
                timeSlider.fillAmount = 0f;
                btn.interactable = true;
                shiny.SetActive(true);
            }
            else
            {
                timer.enabled = true;
                btn.interactable = false;
                float normalizedValue = Mathf.InverseLerp(0, (24 * 60*60), minutesLeft);
                float result = Mathf.Lerp(0f, 1f, normalizedValue);
                //Debug.Log(result + " time left");
                TimeSpan t = TimeSpan.FromSeconds(minutesLeft);
                timer.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                t.Hours,
                t.Minutes,
                t.Seconds);

                timeSlider.fillAmount = result;
                shiny.SetActive(false);
            }
        }

    }
}
