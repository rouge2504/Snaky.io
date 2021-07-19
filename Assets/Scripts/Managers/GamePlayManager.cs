using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;

    [SerializeField] private GameObject field;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public bool SnakeOutField(Transform position)
    {
        Vector2 fieldDistance = new Vector2(field.transform.position.x,field.transform.position.z);
        Vector2 snakePosition = new Vector2(position.position.x, position.position.z);

        float distance = (field.transform.localScale.x / 2);

        return Vector2.Distance(fieldDistance, snakePosition) > distance ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
