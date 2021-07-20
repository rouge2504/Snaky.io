using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private GameObject snakeHeadPrefab;
    [SerializeField] private CinemachineVirtualCamera vcam;

    // Start is called before the first frame update
    void Start()
    {
        GameObject clone = PoolManager.instance.GetHead();
        clone.name = "Player";
        GamePlayManager.instance.player = clone;
        SnakeEnvironment.Singleton.CreateSnake(clone, 5, true);
        vcam.Follow = clone.transform;


        for (int i = 0; i < 20; i++)
        {
            StartCoroutine(NewSnake(i));
        }


    }

    IEnumerator NewSnake(int i)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 10));
        GameObject clone2 = PoolManager.instance.GetHead();
        clone2.name = "CPU_" + i;
        SnakeEnvironment.Singleton.CreateSnake(clone2, UnityEngine.Random.Range(8, 30), false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
