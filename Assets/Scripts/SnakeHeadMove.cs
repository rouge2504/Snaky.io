using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadMove : MonoBehaviour
{
    public float moveSpeed;
    public float boost;
    public float rotSpeed;
    public float diff;

    Vector3 axis;

    public List<GameObject> bodyList;

    private float timingToDelay;
    public float timeToDelay;

    private float it_position;

    public bool isPlayer;

    private float tempDiff;

    [HideInInspector] public SnakeVision snakeVision;

    public void Init()
    {
        bodyList = new List<GameObject>();
        bodyList.Add(this.gameObject);
        it_position = GameConstants.OFFSET_BODY_Y_POSITION;
        if (!isPlayer)
        {
            this.gameObject.AddComponent<AI>();
        }
    }

    public void AddBody(GameObject body)
    {
        Vector3 position = new Vector3(body.transform.position.x, body.transform.position.y - (it_position), body.transform.position.z);
        body.transform.position = position;
        bodyList.Add(body);
        it_position += GameConstants.OFFSET_BODY_Y_POSITION;
    }

    void Update()
    {
        if (isPlayer)
            axis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


    }

    private void LateUpdate()
    {
        Move();
        OnRangeCollision();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, GameConstants.VISION_SNAKE);
    }

    private void Move()
    {
        tempDiff = diff;
        if (isPlayer && Input.GetKey(KeyCode.Space))
        {
            boost = 0.1f;
            tempDiff = 0.3f;
        }else if (isPlayer && Input.GetKeyUp(KeyCode.Space))
        {
            boost = 0;
        }


        if (isPlayer)
        {
            transform.Translate(0, 0, moveSpeed * Time.deltaTime + boost);

            transform.Rotate(0, axis.x * rotSpeed * Time.deltaTime, 0);
        }

        timingToDelay += Time.deltaTime;

        if (timingToDelay > timeToDelay)
        {
            for (int x = 1; x < bodyList.Count; x++)
            {
                /* if (x == 1)
                     tempDiff *= 5;*/
                Vector3 position = Vector3.Lerp(bodyList[x].transform.position, bodyList[x - 1].transform.position, tempDiff);
                bodyList[x].transform.position = new Vector3(position.x, bodyList[x].transform.position.y, position.z);

            }
        }

        if (GamePlayManager.instance.SnakeOutField(this.transform))
        {
            Debug.Log("Snake Out!");
            transform.Rotate(0, 30, 0);
        }
    }

    private void OnRangeCollision()
    {
        snakeVision = SnakeEnvironment.Singleton.GetCollisionWithAnotherSnake(this.gameObject);
    }

}
