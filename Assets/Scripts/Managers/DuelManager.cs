using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuelManager : MonoBehaviour
{
    public static DuelManager instance;

    public GameObject camera;

    public Text counterDown;

    [SerializeField] public GameObject gameFinishDuelUI;
    [SerializeField] public GameObject timerDuel;

    public bool isOnDuel;

    public float timingToDuel = 30;
    private void Start()
    {
        instance = this;
        isOnDuel = false;
        timerDuel.SetActive(false);
    }
    public void Init()
    {
        timingToDuel = 30;
        gameFinishDuelUI.SetActive(false);
        timerDuel.SetActive(false);
        camera.GetComponent<CameraManager>().enabled = false;
        camera.transform.position = new Vector3(0f, 50f, 0f);
        camera.GetComponent<Camera>().orthographicSize = 150f;
        FoodSpawner.Instance.duelModeSpawnSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Screen.height));
        SnakeSpawner.Instance.SpawnDuelPlayer(Population.instance.duelMode_Position[0]);
        Population.instance.AddSnakesDuel();
        Population.instance.StopSnakeLoading();
        //  playerSnake.SetActive(true);

        SnakeSpawner.Instance.PauseAllSnakes();


        FoodSpawner.Instance.SwitchToDuelModeAndReset();
        InitCounter();
    }

    private void Update()
    {
        if (isOnDuel)
        {
            bool timer = DuelTimeOver();
            if (SnakeEnvironment.Singleton.CounterSnake < 2 && !timer && SnakeSpawner.Instance.playerSnake != null)
            {
                WinDuel();
                isOnDuel = false;
            }
        }
    }

    public void FinishDuel()
    {
        isOnDuel = false;
    }

    private bool DuelTimeOver()
    {
        timingToDuel -= Time.deltaTime;
        timerDuel.GetComponentInChildren<Text>().text = timingToDuel.ToString("0");
        if (timingToDuel < 0)
        {
            timingToDuel = 30;
            return true;
        }
        return false;
    }

    public void InitCounter()
    {
        StartCoroutine(InitTimer(3));
    }

    IEnumerator InitTimer(int time)
    {
        counterDown.gameObject.SetActive(true);
        for (int i = time; i > 0; i--)
        {
            counterDown.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        counterDown.gameObject.SetActive(false);
        SnakeSpawner.Instance.ActivateSnakesAI_Duel();
        timerDuel.SetActive(true);
        isOnDuel = true;

    }

    public void WinDuel()
    {
        gameFinishDuelUI.SetActive(true);
        GameManager.instance.ControlsActive(false);
        StoreManager.instance.IncreaseEggs(1);
        PlayerStatsManager.instance.SaveDuelVictories(1);
    }
}
