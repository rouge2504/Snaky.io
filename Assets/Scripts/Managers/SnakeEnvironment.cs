using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class SnakeEnvironment
{
    private static SnakeEnvironment instance;
    private List<SnakeObject> snakes = new List<SnakeObject>();
    private List<Vector3> bufferTemp = new List<Vector3>();
    
    public List<Vector3> BufferTemp
    {
        get
        {
            return bufferTemp;
        }

        set
        {
            bufferTemp = value;
        }
    }
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

    public void CreateSnake(GameObject snakeHead, int Length, bool isPlayer, int id)
    {
        SnakeObject snakeObject = new SnakeObject(snakeHead, Length, isPlayer, id);
        snakeHead.GetComponent<SnakeHeadMove>().snakeObject = snakeObject;
        snakes.Add(snakeObject);
    }

    public void CreateSnake (int id, List<GameObject> body)
    {
        SnakeObject snakeObject = new SnakeObject(id, body);
        snakes.Add(snakeObject);
    }

    public void CheckHead(int id, Vector3 position)
    {
        SnakeObject snake = snakes.Find(x => x.id == id);
        if (snake == null)
        {
            return;
        }

        snake.head.transform.position = position;

    }

    public void CheckBodyParts(int id, List<Vector3> buffer)
    {
        SnakeObject snake = snakes.Find(x => x.id == id);

        if (snake == null)
        {
            return;
        }
        snake.bufferPosition = buffer;
    }

    public void UpdateBuffer()
    {
        foreach(SnakeObject snake in snakes)
        {
            /*for (int i = 0; i < snake.body.Count; i++)
            {
                snake.body[i].transform.position = snake.bufferPosition[i];
            }*/
            SnakeObject snakeTemp = snakes.Find(x => x.head.name == snake.name);
            SnakeVision snakeVision = new SnakeVision();
            Vector2 headPosition = new Vector2(snakeTemp.head.transform.position.x, snakeTemp.head.transform.position.z);
            if (snakeTemp == null)
            {
                return;
            }
            foreach (SnakeObject temp_snake in snakes)
            {
                if (snake.head.name != temp_snake.head.name)
                {

                    for (int i = 0; i < snake.bufferPosition.Count; i++)
                    {
                        Vector2 partPosition = new Vector2(snake.bufferPosition[i].x, snake.bufferPosition[i].z);
                        float x = 0;
                        float z = 0;
                        if ((i + 1) < snake.bufferPosition.Count)
                        {
                            x = (snake.bufferPosition[i].x + snake.bufferPosition[i].x) / 2;
                            z = (snake.bufferPosition[i].z + snake.bufferPosition[i].z) / 2;
                        }

                        Vector2 midlePosition = new Vector2(x, z);
                        Vector2 direction = headPosition - partPosition;
                        float angle = Vector2.Angle(direction, snake.head.transform.forward/* snake.bufferPosition[i].normalized*/);
                        if (direction.magnitude < GameConstants.VIS_DIST && angle < GameConstants.VIS_ANGLE)
                        {
                            //Debug.Log("Part: " + temp_snake.head.name + " near to me: " + snake.head.name);
                            //snakeVision.seeAnotherSnake = true;

                        }
                        if (Vector2.Distance(headPosition, partPosition) < GameConstants.VISION_SNAKE || Vector2.Distance(headPosition, midlePosition) < GameConstants.VISION_SNAKE)
                        {
                            Debug.Log("Part: " + temp_snake.head.name + " near to die: " + snake.head.name);
                            //snakeVision.onCollision = true;
                        }
                    }
                }
            }
        }
    }

    public SnakeVision GetCollisionWithAnotherSnake(GameObject requestSnake)
    {
        SnakeVision snakeVision = new SnakeVision();
        SnakeObject snake = snakes.Find(x => x.head.name == requestSnake.name);
        //Debug.Log(snake.head.name);
        if (snake == null)
        {
            return null;
        }
        Vector2 headPosition = new Vector2(snake.head.transform.position.x, snake.head.transform.position.z);
        foreach(SnakeObject temp_snake in snakes)
        {
            if (snake.head.name != temp_snake.head.name)
            {

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
    public List<GameObject> body;
    public List<Vector3> bufferPosition;
    public string name;
    public int id;
    public int bodyID;

    public SnakeObject(int id, List<GameObject> body)
    {
        this.id = id;
        head = PoolManager.instance.GetHead();
        name = "Head_" + id;
        head.name = name;
        this.body = body;
        bufferPosition = new List<Vector3>(body.Count);
    }

    public SnakeObject(GameObject snakeHead, int Length, bool isPlayer, int id)
    {
        parts = new List<GameObject>();
        float scale = isPlayer ? 30 : Random.Range(30, 55);
        head = snakeHead;
        head.transform.localScale =  new Vector3(scale, scale, scale);
        SnakeHeadMove snakeHeadMove = head.GetComponent<SnakeHeadMove>();
        head.transform.position = SnakeManager.instance.SetPosition(SnakeEnvironment.Singleton.Snakes);
        name = isPlayer ? "Player" : "CPU_" + parts.Count;
        this.id = id;
        snakeHeadMove.isPlayer = isPlayer;
        snakeHeadMove.Init();
        parts.Add(head);
        
        for (int i = 0; i < Length; i++)
        {
            /*GameObject body = PoolManager.instance.GetSnake();
            body.name = name + "_" + i;
            body.transform.localScale = head.transform.localScale;
            body.transform.position = head.transform.position;
            body.GetComponent<SnakeBody>().id = id;
            snakeHeadMove.AddBody(body);
            parts.Add(body);*/
            AddBody(name + "_" + i, head.transform.localScale, head.transform.position, id, snakeHeadMove);
        }
    }

    public void AddBody(string name, Vector3 scale, Vector3 position, int id, SnakeHeadMove snakeHeadMove)
    {
        GameObject body = PoolManager.instance.GetSnake();
        body.name = name;
        body.transform.localScale = scale;
        body.transform.position = position;
        body.GetComponent<SnakeBody>().id = id;
        snakeHeadMove.AddBody(body);
        parts.Add(body);
    }
}
