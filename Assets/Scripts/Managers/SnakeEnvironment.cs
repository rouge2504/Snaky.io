using System.Collections.Generic;
using UnityEngine;

public sealed class SnakeEnvironment
{
    private static SnakeEnvironment instance;
    private List<SnakeObject> snakes = new List<SnakeObject>();
    
    public int CounterSnake
    {
        get
        {
            return snakes.Count;
        }
    }
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

    public SnakeVision GetCollisionWithAnotherSnake(GameObject requestSnake)
    {
        SnakeVision snakeVision = new SnakeVision();
        SnakeObject snake = snakes.Find(x => x.head.name == requestSnake.name);
        //Debug.Log(snake.head.name);
        Vector2 headPosition = new Vector2(snake.head.transform.position.x, snake.head.transform.position.z);
        foreach(SnakeObject temp_snake in snakes)
        {
            if (snake.head.name != temp_snake.head.name)
            {

                /*foreach (GameObject part in temp_snake.parts)
                {
                    Vector2 partPosition = new Vector2(part.transform.position.x, part.transform.position.z);
                    Vector2 direction = headPosition - partPosition;
                    float angle = Vector2.Angle(direction, part.transform.forward);
                    if (direction.magnitude < GameConstants.VIS_DIST && angle < GameConstants.VIS_ANGLE)
                    {
                        //Debug.Log("Part: " + part.name + " near to me: " + snake.head.name);
                        snakeVision.seeAnotherSnake = true;

                    }
                    if (Vector2.Distance(headPosition, partPosition) < GameConstants.VISION_SNAKE)
                    {
                        //Debug.Log("Part: " + part.name + " near to: " + snake.head.name);
                        snakeVision.onCollision = true;
                    }
                }*/

                for (int i = 0; i < temp_snake.parts.Count; i++)
                {
                    Vector2 partPosition = new Vector2(temp_snake.parts[i].transform.position.x, temp_snake.parts[i].transform.position.z);
                    float x = 0;
                    float z = 0;
                    if ((i + 1) < temp_snake.parts.Count) {
                         x = (temp_snake.parts[i].transform.position.x + temp_snake.parts[i + 1].transform.position.x) / 2;
                         z = (temp_snake.parts[i].transform.position.z + temp_snake.parts[i + 1].transform.position.z) / 2;
                    }

                    Vector2 midlePosition = new Vector2(x, z);
                    Vector2 direction = headPosition - partPosition;
                    float angle = Vector2.Angle(direction, temp_snake.parts[i].transform.forward);
                    if (direction.magnitude < GameConstants.VIS_DIST && angle < GameConstants.VIS_ANGLE)
                    {
                        //Debug.Log("Part: " + part.name + " near to me: " + snake.head.name);
                        snakeVision.seeAnotherSnake = true;

                    }
                    if (Vector2.Distance(headPosition, partPosition) < GameConstants.VISION_SNAKE || Vector2.Distance(headPosition, midlePosition) < GameConstants.VISION_SNAKE)
                    {
                        //Debug.Log("Part: " + part.name + " near to: " + snake.head.name);
                        snakeVision.onCollision = true;
                    }
                }
            }
        }

        foreach(Food food in FoodManager.instance.foodList)
        {
            if (Vector2.Distance(headPosition, food.foodPosition) < GameConstants.VISION_FOOD)
            {
                snakeVision.seeFood = true;
                snakeVision.food = food;
            }
        }

        return snakeVision;
    }

    public void PopUpSnake(GameObject part)
    {
        part.SetActive(false);
        SnakeObject snake = snakes.Find(x => x.head.name == part.name);
        snakes.Remove(snake);
    }

    public void DestroyAll()
    {
        snakes.Clear();
    }
}

public class SnakeVision
{
    public bool onCollision;
    public bool seeAnotherSnake;
    public bool seeFood;
    public Food food;

    public SnakeVision()
    {
        onCollision = false;
        seeAnotherSnake = false;
        seeFood = false;
        food = null;
    }

    public void Log()
    {
        Debug.Log("onCollision: " + onCollision +",  seeAnotherSnakes: "+seeAnotherSnake +", seeFood: " + seeFood );
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
        head.transform.position = SnakeManager.instance.SetPosition(SnakeEnvironment.Singleton.Snakes);
        snakeHeadMove.isPlayer = isPlayer;
        snakeHeadMove.Init();
        parts.Add(head);
        
        for (int i = 0; i < Length; i++)
        {
            GameObject body = PoolManager.instance.GetSnake();
            body.transform.localScale = head.transform.localScale;
            body.transform.position = head.transform.position;  
            snakeHeadMove.AddBody(body);
            parts.Add(body);
        }
    }
}
