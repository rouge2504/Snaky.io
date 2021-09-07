using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    public enum STATE { IN_GAME, GO_TO_HOME_FROM_GAMEPLAY, IN_MENU, TEAM2X2, TEAM3X3 }

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

    public void PlayWithTeam3x3()
    {
        SnakeSpawner.Instance.DestroyAllSnakes();
        StartCoroutine(ShowLoadingMenu(Play3x3, gameplayMenu));
    }

    public void PlayWithTeam2x2()
    {
        SnakeSpawner.Instance.DestroyAllSnakes();
        StartCoroutine(ShowLoadingMenu( Play2x2, gameplayMenu));
    }

    void Play2x2()
    {
        FoodSpawner.Instance.isReset = true;
        FoodSpawner.Instance.isWiped = true;
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        control.GetComponent<Image>().enabled = true;
        bust.GetComponent<Image>().enabled = true;
        InGame = true;
        state = STATE.TEAM2X2;
        SnakeSpawner.Instance.StartGame2x2();
    }

    void Play3x3()
    {
        FoodSpawner.Instance.isReset = true;
        FoodSpawner.Instance.isWiped = true;
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        control.GetComponent<Image>().enabled = true;
        bust.GetComponent<Image>().enabled = true;
        InGame = true;
        state = STATE.TEAM3X3;
        SnakeSpawner.Instance.StartGame3x3();
    }



    public void PlayWithAI()
    {
        /*if (SnakeEnvironment.Singleton.CounterSnake > GameConstants.TOTAL_SNAKES - 20)
        {
            SnakeSpawner.Instance.DestroyAllSnakes(SnakeEnvironment.Singleton.CounterSnake - 20);
        }*/
        SnakeSpawner.Instance.DestroyAllSnakes();
        StartCoroutine(PlayAI());
    }

    public void GoToMainMenu()
    {
        switch (state)
        {
            case STATE.TEAM2X2:
                SnakeSpawner.Instance.DestroyAllSnakes();
                state = STATE.IN_MENU;
                StartCoroutine(ShowLoadingMenu(Population.instance.Initialize, mainMenu));
                break;
            case STATE.TEAM3X3:
                SnakeSpawner.Instance.DestroyAllSnakes();
                state = STATE.IN_MENU;
                StartCoroutine(ShowLoadingMenu(Population.instance.Initialize, mainMenu));
                break;
            case STATE.IN_GAME:
                StartCoroutine(ShowLoadingMenu(mainMenu));
                break;
        }
    }

    public void RefreshPlay()
    {
        switch (state)
        {
            case STATE.IN_GAME:
                PlayWithAI();
                break;

            case STATE.TEAM2X2:
                PlayWithTeam2x2();
                break;
            case STATE.TEAM3X3:
                PlayWithTeam3x3 ();
                break;
        }
    }

    IEnumerator PlayAI()
    {
        //SnakeSpawner.Instance.DestroyAllSnakes();
        FoodSpawner.Instance.isReset = true;
        FoodSpawner.Instance.isWiped = true;
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

    IEnumerator ShowLoadingMenu(Action action, GameObject additionalMenu = null)
    {
        loadingUI.SetActive(true);
        yield return new WaitForSeconds(GameConstants.loadingMenuDelay);
        loadingUI.SetActive(false);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }
        action();


    }
}
