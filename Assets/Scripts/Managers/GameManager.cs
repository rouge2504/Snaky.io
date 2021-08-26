using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("MENUS")]
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject gameplayMenu;
    [SerializeField] public GameObject gameOverMenu;
    [Header("Controls")]
    [SerializeField] public GameObject control;
    [SerializeField] public GameObject bust;

    public bool InGame;
    public bool dev_SpeedUp { get; private set; }

    public void Awake()
    {
        instance = this;
        control.GetComponent<Image>().enabled = false;
        bust.GetComponent<Image>().enabled = false;
        InGame = false;
    }



    public void PlayWithAI()
    {
        if (SnakeEnvironment.Singleton.CounterSnake > 40)
        {
            SnakeSpawner.Instance.DestroyAllSnakes(40);
        }
            //SnakeSpawner.Instance.DestroyAllSnakes();
        StartCoroutine(PlayAI());
    }

    IEnumerator PlayAI()
    {
        yield return new WaitForSeconds(2);
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        control.GetComponent<Image>().enabled = true;
        bust.GetComponent<Image>().enabled = true;
        InGame = true;
        SnakeSpawner.Instance.StartGameWithAI();
    }

    public void LooseWithAI()
    {
        control.GetComponent<Image>().enabled = false;
        bust.GetComponent<Image>().enabled = false;
        gameplayMenu.SetActive(false);
        
        gameOverMenu.SetActive(true);
    }

    public void SnakeSpeedManager(bool anyValue)
    {
        dev_SpeedUp = anyValue;
        if (SnakeSpawner.Instance.playerSnake != null)
        {
            if (anyValue)
            {
                SnakeSpawner.Instance.StartSprint(SnakeSpawner.Instance.playerSnake);
            }
            else
            {
                SnakeSpawner.Instance.playerSnake.sprinting = false;
            }
        }
    }
}
