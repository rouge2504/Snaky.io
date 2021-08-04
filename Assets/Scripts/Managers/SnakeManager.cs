using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class SnakeManager : MonoBehaviour
{
    public static SnakeManager instance;
    [SerializeField] private GameObject snakeHeadPrefab;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Transform field;
    [SerializeField] private Text counterText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InvokeRepeating("CheckSpawnSnake", 30f, 5.0f);
        Init();

    }

    public void Init()
    {
        GameObject clone = PoolManager.instance.GetHead();
        clone.name = "Player";
        GamePlayManager.instance.player = clone;
        SnakeEnvironment.Singleton.CreateSnake(clone, 15, true);
        vcam.Follow = clone.transform;

        for (int i = 0; i < GameConstants.TOTAL_SNAKES; i++)
        {
            StartCoroutine(NewSnake(i));
        }
    }

    IEnumerator NewSnake(int i)
    {

        yield return new WaitForSeconds(GameConstants.TIME_TO_SPAWN_SNAKE);
        if (SnakeEnvironment.Singleton.CounterSnake >= GameConstants.TOTAL_SNAKES)
        {
            yield break;
        }
        GameObject clone2 = PoolManager.instance.GetHead();
        clone2.name = "CPU_" + i;
        SnakeEnvironment.Singleton.CreateSnake(clone2, GameConstants.LENGTH_SNAKE, false);
    }

    public Vector3 SetPosition(List<SnakeObject> parts)
    {
        float offsetField = (field.transform.localScale.x/2) - GameConstants.OFFSET_FIELD;
        Vector3 snakePosition = new Vector3(Random.Range(-offsetField, offsetField), 0, Random.Range(-offsetField, offsetField));
        bool findPosition = false;
        while (!findPosition)
        {
            findPosition = true;
            foreach (SnakeObject part in parts)
            {
                if (Vector3.Distance(part.head.transform.position, snakePosition) < 5)
                {
                    findPosition = false;
                }
            }

            snakePosition = new Vector3(Random.Range(-offsetField, offsetField), 0, Random.Range(-offsetField, offsetField));
        }

        return snakePosition;
    }

    // Update is called once per frame
    void Update()
    {
        counterText.text = SnakeEnvironment.Singleton.CounterSnake + "/" + GameConstants.TOTAL_SNAKES;
    }

    void CheckSpawnSnake()
    {
        if (SnakeEnvironment.Singleton.CounterSnake < GameConstants.TOTAL_SNAKES)
        {
            int length = GameConstants.TOTAL_SNAKES - SnakeEnvironment.Singleton.CounterSnake;
            for (int i = 0; i < length; i++)
            {
                StartCoroutine(NewSnake(i));
            }
        }

    }
}
