using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public static Debugger instance;
    void Start()
    {
        instance = this;
    }

    public void Log(object message)
    {
        Debug.Log(message);
    }


}
