using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    StateAI currentState;
    void Start()
    {
        currentState = new Sleep(this.gameObject, GamePlayManager.instance.player, this.GetComponent<SnakeHeadMove>().snakeVision);
    }

    void Update()
    {
        currentState = currentState.Process();
    }
}
