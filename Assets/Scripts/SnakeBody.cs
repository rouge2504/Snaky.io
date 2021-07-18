using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Debug.Log("Object is no longer visible");
    }

    private void OnBecameVisible()
    {
        Debug.Log("Object is visible");
    }
}
