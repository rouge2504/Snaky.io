using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rate : MonoBehaviour
{
    public Image[] rateStars;
    public GameObject rateThanks;
    private bool activeThanks;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Image star in rateStars)
        {
            star.color = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RateStar(int rate)
    {
        foreach(Image star in rateStars)
        {
            star.color = Color.black;
        }

        for (int i = 0; i < rate; i++)
        {
            rateStars[i].color = Color.white;
        }

        if (rate <= 4)
        {
            activeThanks = true;

        }
    }

    public void ButtonRate()
    {
        if (activeThanks)
        {
            //rateThanks.SetActive(true);
            DialogueManager.instance.PopUp("Thanks for rating");
        }
        else
        {
            this.gameObject.SetActive(false);
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.carmin.slitherio.snake.worm");   
        }
        GamePrefs.SetBool(GameUtils.ONE_TIME_RATE, false);
    }

    public void NotRate()
    {
        GamePrefs.SetBool(GameUtils.ONE_TIME_RATE, false);

    }
}
