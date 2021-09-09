using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoPoints : MonoBehaviour
{
    public GameObject image;
    public Text nameSnake;
    public Text points;

    public Color mainColor;


    public void SetMainColor()
    {
        nameSnake.color = mainColor;
        this.points.color = mainColor;
    }


    public void SetScoreOnTeam(string name, int points, Color color)
    {
        nameSnake.text = name;
        this.points.text = points.ToString("0");
        nameSnake.color = color;
        this.points.color = color;
    }
}
