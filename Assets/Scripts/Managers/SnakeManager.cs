using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private GameObject snakeHeadPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject clone = Instantiate(snakeHeadPrefab);
        SnakeEnvironment.Singleton.CreateSnake(clone, 5, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
