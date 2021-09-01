using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LooknFeelDisplay : MonoBehaviour
{
    public Transform contentSnake;
    private Image[] colorRender;
    // Start is called before the first frame update
    void Start()
    {
        colorRender = new Image[contentSnake.childCount];

        for (int i = 0; i < colorRender.Length; i++)
        {
            colorRender[i] = contentSnake.GetChild(i).GetComponent<Image>();
            colorRender[i].gameObject.GetComponent<Floater>().enabled = false;

        }

        StartCoroutine(StartAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {

        //StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation()
    {

        for (int i = 0; i < colorRender.Length; i++)
        {
            colorRender[i].gameObject.GetComponent<Floater>().enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

    }
}
