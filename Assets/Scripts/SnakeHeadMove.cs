using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SnakeHeadMove : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;
    public float diff;

    public GameObject bodyPrefab;
    Vector3 axis;

    public List<GameObject> bodyList;

    private float timingToDelay;
    public float timeToDelay;

    private int it_position;


    public void Init()
    {
        bodyList = new List<GameObject>();
        bodyList.Add(this.gameObject);
        it_position = 0;
    }

    public void AddBody(GameObject body)
    {
        Vector3 position = new Vector3(body.transform.position.x, body.transform.position.y - (it_position + 1), body.transform.position.z);
        body.transform.position = position;
        bodyList.Add(body);
        it_position++;
    }

    // Update is called once per frame
    void Update()
    {
        axis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    }

    private void LateUpdate()
    {
        transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        transform.Rotate(0, axis.x * rotSpeed * Time.deltaTime, 0);

        timingToDelay += Time.deltaTime;

        if (timingToDelay > timeToDelay)
        {
            for (int x = 1; x < bodyList.Count; x++)
            {
                /* if (x == 1)
                     tempDiff *= 5;*/
                Vector3 position = Vector3.Lerp(bodyList[x].transform.position, bodyList[x - 1].transform.position, diff);
                bodyList[x].transform.position = new Vector3(position.x, bodyList[x].transform.position.y, position.z);
                //snakeParts[x] = buffer;

            }
        }
    }

}
