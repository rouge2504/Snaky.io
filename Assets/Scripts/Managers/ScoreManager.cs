using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreGame;
    [SerializeField] private List<UserInfoPoints> infoPoints;
    [SerializeField] private UserInfoPoints gameOverScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {;
        if (GameManager.instance.InGame)
        {
            CheckLeaderBoard();
            if (SnakeSpawner.Instance.playerSnake != null)
            {
                scoreGame.text = SnakeSpawner.Instance.playerSnake.points.ToString("0");

                gameOverScore.points.text = SnakeSpawner.Instance.playerSnake.points.ToString("0");
            }
        }
    }

    private void CheckLeaderBoard()
    {
        List<ECSSnake> tempListSnake = new List<ECSSnake>(); /*SnakeSpawner.Instance.snakes.ToList();*/

            /*tempListSnake.Sort(delegate (ECSSnake a, ECSSnake b)
            {
                if (a == null || b == null)
                {
                    return 0;
                }
                return a.points.CompareTo(b);
            });*/
            for (int i = 0; i < SnakeSpawner.Instance.snakes.Length; i++)
            {
                if (SnakeSpawner.Instance.snakes[i] != null)
                {
                    tempListSnake.Add(SnakeSpawner.Instance.snakes[i]);
                }
            }
        tempListSnake.Sort(delegate (ECSSnake a, ECSSnake b)
        {
            return a.points.CompareTo(b.points);
        });
        //string debug = null;
        
        for (int i = 0; i < tempListSnake.Count; i++)
        {
            ECSSnake tempSnake = tempListSnake[(tempListSnake.Count - 1) - i];
            //debug += "Points: " + tempListSnake[(tempListSnake.Count - 1) - i].points + "\n";

            if (i < infoPoints.Count)
            {
                infoPoints[i].nameSnake.text = tempSnake.snakeName;
                infoPoints[i].points.text = tempSnake.points.ToString("0");
            }
        }

        //print(debug);

    }
}
