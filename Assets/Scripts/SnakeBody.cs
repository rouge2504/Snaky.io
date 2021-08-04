using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public int id;
    public float frequency = 0.6f;
    public float speed = 16f;
    public float amplitude = 0.06f; 
    private void Update()
    {
        //float scale = amplitude * Mathf.Sin(Time.deltaTime * speed) * frequency;
        //this.transform.localScale = new Vector3(scale, this.transform.localScale.y, this.transform.localScale.z);
    }
}
