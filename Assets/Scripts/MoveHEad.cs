using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHEad : MonoBehaviour
{

    public float moveSpeed;
    public float boost;
    public float rotSpeed;
    public float diff;
    private Vector3 axis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        axis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Move();
    }

    private void Move()
    {


            transform.Translate(0, 0, moveSpeed * Time.deltaTime + boost);

            transform.Rotate(0, axis.x * rotSpeed * Time.deltaTime, 0);


    }
}
