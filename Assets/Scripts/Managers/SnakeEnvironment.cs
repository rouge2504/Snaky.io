using System.Collections.Generic;
using UnityEngine;

public sealed class SnakeEnvironment
{
    private static SnakeEnvironment instance;
    private List<SnakeObject> snakes;

    public List<SnakeObject> Snakes {
        get
        {
            return snakes;
        }
    }

    public static SnakeEnvironment Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new SnakeEnvironment();
            }
            return instance;
        }
    }

    public void CreateSnake(GameObject snakeHead, int Length, bool isPlayer)
    {
        SnakeObject snakeObject = new SnakeObject(snakeHead, Length, isPlayer);
    }
}

public class SnakeObject
{
    public GameObject head;
    public List<GameObject> parts;

    public SnakeObject (GameObject snakeHead, int Length, bool isPlayer)
    {
        parts = new List<GameObject>();
        GameObject head = snakeHead;
        SnakeHeadMove snakeHeadMove = head.GetComponent<SnakeHeadMove>();
        snakeHeadMove.Init();
        parts.Add(head);
        for (int i = 0; i < Length; i++)
        {
            GameObject body = PoolManager.instance.GetSnake();
            snakeHeadMove.AddBody(body);
            parts.Add(body);
        }
    }
}
