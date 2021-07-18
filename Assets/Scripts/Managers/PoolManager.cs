using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public List<GameObject> bodySnake = new List<GameObject>();
    public int it_snake;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject body = this.transform.GetChild(i).gameObject;
            bodySnake.Add(this.transform.GetChild(i).gameObject);
            body.SetActive(false);
            
        }
    }

    public GameObject GetSnake()
    {
        GameObject snake = bodySnake[NextSnake()];
        snake.SetActive(true);
        return snake;
    }

    public int NextSnake()
    {
        int temp = it_snake + 1;
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
        it_snake = temp;
        return it_snake;


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
}
