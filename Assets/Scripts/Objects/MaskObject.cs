using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskObject : MonoBehaviour
{
    public string maskName;
    public Image maskImage;
    public Button buttonImage;
    public GameObject lockImage;

    public bool unlocked = true;

    public void SetImageOnLock()
    {
        lockImage.GetComponent<Image>().sprite = maskImage.sprite;
    }

}
