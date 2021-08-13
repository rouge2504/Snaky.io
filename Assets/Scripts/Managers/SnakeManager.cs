using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class SnakeManager : MonoBehaviour
{
    public static SnakeManager instance;
    public Text counterText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InvokeRepeating("CheckSpawnSnake", 30f, 40);
        Init();

    }

    public void Init()
    {


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
     
        SnakeSpawner.Instance.CreateNewSnake(100, "CPU_" + i, Vector3.zero, SnakeSpawner.Instance.selectedColorTemplate, null, false, "");
    }

    public Vector3 SetPosition(List<SnakeObject> parts)
    {
        float offsetField = 0/*(field.transform.localScale.x/2) - GameConstants.OFFSET_FIELD*/;
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
            for (int i = length; i < GameConstants.TOTAL_SNAKES; i++)
            {
                StartCoroutine(NewSnake(i));
            }
        }

    }
}
