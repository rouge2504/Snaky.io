using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    public enum STATE { IN_GAME, GO_TO_HOME_FROM_GAMEPLAY, IN_MENU, TEAM2X2, TEAM3X3, IN_DUEL }

    public STATE state;

    public static GameManager instance;
    [Header("MENUS")]
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject loadingUI;
    [SerializeField] public GameObject searchUI;
    [SerializeField] public GameObject gameplayMenu;
    [SerializeField] public GameObject gameOverMenu;

    [Header("Controls")]
    [SerializeField] public GameObject control;
    [SerializeField] public GameObject basecontrol;
    [SerializeField] public GameObject bust;
    public long SurvivalTimeTicks { get; set; }

    public DateTime SpawnTime { get; set; }

    public GameObject mainCamera;

    public InputField nameField;

    public bool InGame;
    public bool dev_SpeedUp { get; private set; }

    public void ControlsActive(bool active)
    {
        control.GetComponent<Image>().enabled = active;
        bust.GetComponent<Image>().enabled = active;
        basecontrol.GetComponent<Image>().enabled = active;
    }

    public bool IsDuelMode
    {
        get
        {
            return state == STATE.IN_DUEL;
        }
    }

    public void Awake()
    {
        instance = this;
        ControlsActive(false);
        InGame = false;
        nameField.text = GamePrefs.PLAYER_NAME;
    }

    void Start()
    {
        SetLoading();
        state = STATE.IN_MENU;
    }

    public void PlayDuel()
    {
        SnakeSpawner.Instance.DestroyAllSnakes();


        InGame = true;
        state = STATE.IN_DUEL;
        ScoreManager.intance.ResetInfoPoints(4);
        StartCoroutine(ShowSearchingMenu(DuelManager.instance.Init, gameplayMenu));
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
        Camera.main.GetComponent<Camera>().orthographicSize = 50;
        mainCamera.GetComponent<CameraManager>().enabled = true;
        FoodSpawner.Instance.isReset = true;
        FoodSpawner.Instance.isWiped = true;
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        ControlsActive(true);
        InGame = true;
        state = STATE.TEAM2X2;
        SnakeSpawner.Instance.StartGame2x2();
    }

    void Play3x3()
    {
        Camera.main.GetComponent<Camera>().orthographicSize = 50;
        mainCamera.GetComponent<CameraManager>().enabled = true;
        FoodSpawner.Instance.isReset = true;
        FoodSpawner.Instance.isWiped = true;
        mainMenu.SetActive(false);
        gameplayMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        ControlsActive(true);
        InGame = true;
        state = STATE.TEAM3X3;
        SnakeSpawner.Instance.StartGame3x3();
    }



    public void PlayWithAI()
    {
        if (SnakeEnvironment.Singleton.CounterSnake > GameConstants.TOTAL_SNAKES - 20)
        {
            SnakeSpawner.Instance.DestroyAllSnakes(SnakeEnvironment.Singleton.CounterSnake - 20);
        }
        Camera.main.GetComponent<Camera>().orthographicSize = 50;

        //SnakeSpawner.Instance.DestroyAllSnakes();
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
                state = STATE.IN_MENU;
                StartCoroutine(ShowLoadingMenu(mainMenu));
                break;
            case STATE.IN_DUEL:
                SnakeSpawner.Instance.DestroyAllSnakes();
                FoodSpawner.Instance.foodSpawnType = FoodSpawnType.Normal;
                mainCamera.GetComponent<CameraManager>().enabled = true;
                state = STATE.IN_MENU;
                StartCoroutine(ShowLoadingMenu(Population.instance.Initialize, mainMenu));
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

            case STATE.IN_DUEL:
                PlayDuel();
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
        ControlsActive(true);
        InGame = true;
        state = STATE.IN_GAME;
        SnakeSpawner.Instance.StartGameWithAI();
    }

    public void LooseWithAI()
    {
        if(state == STATE.IN_DUEL)
        {
            PlayerStatsManager.instance.SaveDuelLooses(1);
        }
        ControlsActive(false);
        gameplayMenu.SetActive(false);
        DuelManager.instance.timerDuel.SetActive(false);
        DuelManager.instance.isOnDuel = false;

        if (state != STATE.GO_TO_HOME_FROM_GAMEPLAY)
        {
            gameOverMenu.SetActive(true);
        }
        IncreaseSurvivingTime();
        PlayerStatsManager.instance.SaveSurvivalTime(SurvivalTimeTicks);
    }

    private void IncreaseSurvivingTime()
    {
        SurvivalTimeTicks += DateTime.Now.Ticks - SpawnTime.Ticks;
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
        if (state == STATE.IN_DUEL)
        {
            DuelManager.instance.FinishDuel();
        }
        /*SnakeSpawner.Instance.snakes[SnakeSpawner.Instance.playerID].isDestroyed = true;
        SnakeSpawner.Instance.snakes[SnakeSpawner.Instance.playerID].isPlayerSnake = false;*/
        state = STATE.GO_TO_HOME_FROM_GAMEPLAY;
        SnakeSpawner.Instance.DestroyAllSnakes();
        gameplayMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        StartCoroutine(ShowLoadingMenu (mainMenu, STATE.IN_MENU));
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

    IEnumerator ShowLoadingMenu(GameObject additionalMenu, STATE state)
    {
        loadingUI.SetActive(true);
        yield return new WaitForSeconds(GameConstants.loadingMenuDelay);
        loadingUI.SetActive(false);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }
        this.state = state;


    }

    IEnumerator ShowSearchingMenu(Action action, GameObject additionalMenu = null)
    {
        searchUI.SetActive(true);
        yield return new WaitForSeconds(GameConstants.searchiingMenuDelay);
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        searchUI.SetActive(false);
        if (additionalMenu != null)
        {
            additionalMenu.SetActive(true);
        }
        ControlsActive(true);
        action();


    }


    public void SetPlayerName(Text name)
    {
        GamePrefs.PLAYER_NAME = name.text;
    }
}
