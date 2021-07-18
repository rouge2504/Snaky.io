using System.Collections.Generic;
using UnityEngine;

public sealed class SnakeEnvironment
{
    private static SnakeEnvironment instance;
    private List<SnakeObject> snakes = new List<SnakeObject>();

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
        snakes.Add(snakeObject);
    }

    public void GetCollisionWithAnotherSnake(GameObject requestSnake)
    {
        SnakeObject snake = snakes.Find(x => x.head.name == requestSnake.name);
        //Debug.Log(snake.head.name);
        Vector2 headPosition = new Vector2(snake.head.transform.position.x, snake.head.transform.position.z);
        foreach(SnakeObject temp_snake in snakes)
        {
            if (snake.head.name != temp_snake.head.name)
            {
                foreach (GameObject part in temp_snake.parts)
                {
                    Vector2 partPosition = new Vector2(part.transform.position.x, part.transform.position.z);
                    if (Vector2.Distance(headPosition, partPosition) < GameConstants.VISION_SNAKE)
                    {
                        Debug.Log("Part: " + part.name + " near to: " + snake.head.name);
                    }
                }
            }
        }
    }
}

public class SnakeObject
{
    public GameObject head;
    public List<GameObject> parts;

    public SnakeObject (GameObject snakeHead, int Length, bool isPlayer)
    {
        parts = new List<GameObject>();
        head = snakeHead;
        SnakeHeadMove snakeHeadMove = head.GetComponent<SnakeHeadMove>();
        snakeHeadMove.isPlayer = isPlayer;
        snakeHeadMove.Init();
        parts.Add(head);
        
        for (int i = 0; i < Length; i++)
        {
            GameObject body = PoolManager.instance.GetSnake();
            body.transform.localScale = head.transform.localScale;
            snakeHeadMove.AddBody(body);
            parts.Add(body);
        }
    }
}
