using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float canSpawn = 0f;
    public ECSSnake snake;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    private void FixedUpdate()
    {
        if (canSpawn > 0f)
        {
            canSpawn -= Time.fixedDeltaTime;
            if (canSpawn <= 0)
            {
                if (snake != null)
                {
                    if (snake.isDestroyed)
                    {

                        snake = null;
                    }
                }
                canSpawn = 0;
            }
        }

        if (snake != null)
        {
            if (snake.isDestroyed)
            {
                canSpawn = 0;
            }
        }
    }
    public void CannotSpawn(ECSSnake snakeParams)
    {
        if (snakeParams == null)
            return;

        snake = snakeParams;
        if (snakeParams.points > 5000)
        {
            canSpawn = 30f;
        }
        else if (snakeParams.points > 1000)
        {
            canSpawn = 20f;
        }
        else
            canSpawn = 10f;
    }

    public void Reset()
    {
        canSpawn = 0f;
    }
}
