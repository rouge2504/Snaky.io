using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public GameObject dialogueObject;

    public void Start()
    {
        instance = this;
    }

    public void PopUp(string text)
    {
        dialogueObject.SetActive(true);
        dialogueObject.GetComponentInChildren<Text>().text = text;
    }
}
