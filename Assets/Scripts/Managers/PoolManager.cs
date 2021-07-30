using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public Transform bodyContent;
    public Transform headContent;
    public Transform foodContent;
    public List<GameObject> bodySnake = new List<GameObject>();
    public List<GameObject> headSnake = new List<GameObject>();
    public List<GameObject> food = new List<GameObject>();
    public int it_snakeBody;
    public int it_snakeHead;
    private int it_food;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        
        for (int i = 0; i < bodyContent.childCount; i++)
        {
            GameObject body = bodyContent.GetChild(i).gameObject;
            bodySnake.Add(bodyContent.GetChild(i).gameObject);
            body.SetActive(false);
            
        }

        for (int i = 0; i < headContent.childCount; i++)
        {
            GameObject head = headContent.GetChild(i).gameObject;
            headSnake.Add(headContent.GetChild(i).gameObject);
            head.SetActive(false);

        }
    }

    public GameObject GetSnake()
    {
        GameObject snake = bodySnake[NextSnakeBody()];
        snake.SetActive(true);
        return snake;
    }

    public GameObject GetHead()
    {
        GameObject snake = headSnake[NextSnakeHead()];
        snake.SetActive(true);
        return snake;
    }

    public GameObject GetFood()
    {
        GameObject foodObject = food[NextFood()];
        foodObject.SetActive(true);
        return foodObject;
    }

    public int NextSnakeBody()
    {
        int temp = it_snakeBody + 1;
        int limit = temp;
        if (temp > bodySnake.Count - 1)
        {
            temp = 0;
        }

        while (bodySnake[temp].activeSelf)
        {
            temp = NextIterator(temp);
            if (limit == temp)
            {
                Debug.LogError("All snakes are on uses");
                break;
            }
        }
        it_snakeBody = temp;
        return it_snakeBody;


    }

    public void DestroyAll()
    {
        int counter = 0;
        foreach(GameObject body in bodySnake)
        {
            body.SetActive(false);
            body.name = "BodySnake " + counter;
            counter++;
        }
        counter = 0;
        foreach (GameObject head in headSnake)
        {
            head.SetActive(false);
            head.name = "SnakeHead " + counter;
            counter++;
        }
    }


    public int NextIterator(int temp)
    {
        temp++;
        if (temp > bodySnake.Count - 1)
        {
            temp = 0;
        }
        return temp;
    }


    public int NextSnakeHead()
    {
        int temp = it_snakeHead + 1;
        int limit = temp;
        if (temp > headSnake.Count - 1)
        {
            temp = 0;
        }

        while (headSnake[temp].activeSelf)
        {
            temp = NextIteratorHead(temp);
            if (limit == temp)
            {
                Debug.LogError("All heads are on uses");
                break;
            }
        }
        it_snakeHead = temp;
        return it_snakeHead;


    }

    public int NextFood()
    {
        int temp = it_food + 1;
        int limit = temp;
        if (temp > food.Count - 1)
        {
            temp = 0;
        }

        while (food[temp].activeSelf)
        {
            temp = NextIteratorFood(temp);
            if (limit == temp)
            {
                Debug.LogError("All heads are on uses");
                break;
            }
        }
        it_snakeHead = temp;
        return it_snakeHead;


    }
    public int NextIteratorFood(int temp)
    {
        temp++;
        if (temp > food.Count - 1)
        {
            temp = 0;
        }
        return temp;
    }

    public int NextIteratorHead(int temp)
    {
        temp++;
        if (temp > headSnake.Count - 1)
        {
            temp = 0;
        }
        return temp;
    }
}
