using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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

    [SerializeField] Material normalSkin;
    [SerializeField] Material boostSkin;

    public Vector2 amplitude;
    public Vector2 speed;
    public Vector2 frequency;
    float timer;
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
            axis = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), 0, CrossPlatformInputManager.GetAxis("Vertical"));


    }

    private void LateUpdate()
    {
        Move();
        OnRangeCollision();
    }
    void OnDrawGizmosSelected()
    {
        //Gizmos.DrawSphere(transform.position, GameConstants.VISION_SNAKE);
    }

    private void Move()
    {
        tempDiff = diff;
        Boost();


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
                bodyList[x].transform.eulerAngles = bodyList[x - 1].transform.eulerAngles; 
            }
        }

        if (GamePlayManager.instance.SnakeOutField(this.transform))
        {
            Debug.Log("Snake Out!");
            transform.Rotate(0, 30, 0);
        }
    }

    private void Boost()
    {
        timer += Time.deltaTime;
        foreach (GameObject skin in bodyList)
        {
            MeshRenderer mesh = Helper.FindComponentInChildWithTag<MeshRenderer>(skin, "Skin");
            mesh.material = normalSkin;
        }
        if (isPlayer && /*Input.GetKey(KeyCode.Space)*/ CrossPlatformInputManager.GetButton("Boost"))
        {
            boost = 0.1f;
            tempDiff = 0.3f;
            int i = 0;
            foreach(GameObject skin in bodyList)
            {
                MeshRenderer mesh = Helper.FindComponentInChildWithTag<MeshRenderer>(skin, "Skin");
                float scaleValX = amplitude.x * Mathf.Sin(((timer * speed.x) + i) * frequency.x);
                float scaleValY = amplitude.y * Mathf.Cos(((timer * speed.y)) * frequency.y);

                if (scaleValX < 0.8f && scaleValX > -0.8f)
                {
                    scaleValX = 0.8f;
                }

                if (scaleValY < 0.5f && scaleValY > -0.5f)
                {
                    scaleValY = 0.5f;
                }


                if (axis.x > 0 || axis.x < 0)
                {
                    scaleValX = 1f;
                    scaleValY = 1.5f;
                }
                mesh.gameObject.transform.localScale = new Vector3(scaleValX, scaleValY, mesh.gameObject.transform.localScale.z);
                mesh.material = boostSkin;
                i++;
            }
        }
        else if (isPlayer && /*Input.GetKeyUp(KeyCode.Space)*/ CrossPlatformInputManager.GetButtonUp("Boost"))        {
            boost = 0;
            timer = 0;
            foreach (GameObject skin in bodyList)
            {
                MeshRenderer mesh = Helper.FindComponentInChildWithTag<MeshRenderer>(skin, "Skin");
                mesh.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnRangeCollision()
    {
        snakeVision = SnakeEnvironment.Singleton.GetCollisionWithAnotherSnake(this.gameObject);
    }

}

public static class Helper
{
    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }
}


