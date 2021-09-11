using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{
    [SerializeField] public GameObject control;
    [SerializeField] public GameObject basecontrol;
    [SerializeField] public GameObject bust;
    [SerializeField] public GameObject arrow;

    public void ControlsActive(bool active)
    {
        
        if (arrow != null)
        {
            bust.GetComponent<Image>().enabled = active;
            arrow.GetComponent<Image>().enabled = active;
            

        }
        else
        {
            control.GetComponent<Image>().enabled = active;
            bust.GetComponent<Image>().enabled = active;
            basecontrol.GetComponent<Image>().enabled = active;

        }
    }

}
