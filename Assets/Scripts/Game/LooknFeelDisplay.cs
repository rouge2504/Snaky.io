using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LooknFeelDisplay : MonoBehaviour
{
    public GameObject[] colorRender;
    // Start is called before the first frame update
    void Start()
    {

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
