using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    public enum STATE { IN_GAME, GO_TO_HOME_FROM_GAMEPLAY, IN_MENU }

    public STATE state;

    public static GameManager instance;
    [Header("MENUS")]
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject loadingUI;
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

    void Start()
    {
        SetLoading();
        state = STATE.IN_MENU;
    }



    public void PlayWithAI()
    {
        if (SnakeEnvironment.Singleton.CounterSnake > GameConstants.TOTAL_SNAKES - 20)
        {
            SnakeSpawner.Instance.DestroyAllSnakes(SnakeEnvironment.Singleton.CounterSnake - 20);
        }
        //SnakeSpawner.Instance.DestroyAllSnakes();
        StartCoroutine(PlayAI());
    }

    IEnumerator PlayAI()
    {
        //SnakeSpawner.Instance.DestroyAllSnakes();
        yield return new WaitForSeconds(2);
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        control.GetComponent<Image>().enabled = true;
        bust.GetComponent<Image>().enabled = true;
        InGame = true;
        state = STATE.IN_GAME;
        SnakeSpawner.Instance.StartGameWithAI();
    }

    public void LooseWithAI()
    {
        control.GetComponent<Image>().enabled = false;
        bust.GetComponent<Image>().enabled = false;
        gameplayMenu.SetActive(false);

        if (state != STATE.GO_TO_HOME_FROM_GAMEPLAY)
        {
            gameOverMenu.SetActive(true);
        }
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

    public void GoHomeFromGame()
    {
        /*SnakeSpawner.Instance.snakes[SnakeSpawner.Instance.playerID].isDestroyed = true;
        SnakeSpawner.Instance.snakes[SnakeSpawner.Instance.playerID].isPlayerSnake = false;*/
        state = STATE.GO_TO_HOME_FROM_GAMEPLAY;
        SnakeSpawner.Instance.DestroyAllSnakes();
        gameplayMenu.SetActive(false);
        SetLoading(mainMenu);
        //SetLoading();
    }

    public void SetLoading(GameObject additionalMenu = null)
    {
        StartCoroutine(ShowLoadingMenu(additionalMenu));
    }

    IEnumerator ShowLoadingMenu(GameObject additionalMenu = null)
    {
        loadingUI.SetActive(true);
        yield return new WaitForSeconds(GameConstants.loadingMenuDelay);
        loadingUI.SetActive(false);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }


    }
}
