using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Cinemachine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private GameObject snakeHeadPrefab;
    [SerializeField] private CinemachineVirtualCamera vcam;

    // Start is called before the first frame update
    void Start()
    {
        GameObject clone = Instantiate(snakeHeadPrefab);
        clone.name = "Player";
        SnakeEnvironment.Singleton.CreateSnake(clone, 5, true);
        vcam.Follow = clone.transform;

        GameObject clone2 = Instantiate(snakeHeadPrefab);
        clone2.name = "CPU";
        SnakeEnvironment.Singleton.CreateSnake(clone2, 15, false);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
