using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager intance;
    [SerializeField] private Text scoreGame;
    [SerializeField] private List<UserInfoPoints> infoPoints;
    [SerializeField] private UserInfoPoints gameOverScore;
    // Start is called before the first frame update
    void Start()
    {
        intance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance != null)
        if (GameManager.instance.InGame)
        {
            switch (GameManager.instance.state)
            {
                case GameManager.STATE.IN_GAME:
                    CheckLeaderBoard();
                    break;
                case GameManager.STATE.TEAM2X2:
                    CheckLeaderBoardTeam(GameManager.instance.state);  
                    break;
                case GameManager.STATE.TEAM3X3:
                    CheckLeaderBoardTeam(GameManager.instance.state);
                    break;
                case GameManager.STATE.IN_DUEL:
                    CheckLeaderDuelBoard();
                    break;
            }
            if (SnakeSpawner.Instance.playerSnake != null)
            {
                scoreGame.text = SnakeSpawner.Instance.playerSnake.points.ToString("0");

                gameOverScore.points.text = SnakeSpawner.Instance.playerSnake.points.ToString("0");
                PlayerStatsManager.instance.SaveScore(SnakeSpawner.Instance.playerSnake.points);
                CheckAchievement();


            }
        }
    }

    private void CheckAchievement()
    {
        AchievementManager.instance.PointCounter(SnakeSpawner.Instance.playerSnake.points);
    }

    public void ResetInfoPoints(int it)
    {
        for (int i = 0; i < it; i++)
        {
            infoPoints[i].gameObject.SetActive(false);
        }
    }

    private void CheckLeaderBoardTeam(GameManager.STATE state)
    {
        int limit = 0;

        if (state == GameManager.STATE.TEAM2X2)
        {
            limit = 2;
        }else if (state == GameManager.STATE.TEAM3X3)
        {
            limit = 3;
        }
        for (int i = 0; i < infoPoints.Count; i++)
        {
            infoPoints[i].image.SetActive(false);
            if (i >= limit)
            {
                infoPoints[i].gameObject.SetActive(false);
            }
        }

        
        infoPoints[0].SetScoreOnTeam("Team A", Population.instance.GetTotalTeamScore("A"), SkinsManager.instance.teamColors[0].color);
        infoPoints[1].SetScoreOnTeam("Team B", Population.instance.GetTotalTeamScore("B"), SkinsManager.instance.teamColors[1].color);
        infoPoints[2].SetScoreOnTeam("Team C", Population.instance.GetTotalTeamScore("C"), SkinsManager.instance.teamColors[2].color);
    }

   

    private void CheckLeaderBoard()
    {
        List<ECSSnake> tempListSnake = new List<ECSSnake>(); /*SnakeSpawner.Instance.snakes.ToList();*/

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
                infoPoints[i].SetMainColor();
                infoPoints[i].gameObject.SetActive(true);
                infoPoints[i].image.SetActive(true);
                infoPoints[i].nameSnake.text = tempSnake.snakeName;
                infoPoints[i].points.text = tempSnake.points.ToString("0");
            }




        }

        //print(debug);

    }

    private void CheckLeaderDuelBoard()
    {
        List<ECSSnake> tempListSnake = new List<ECSSnake>(); /*SnakeSpawner.Instance.snakes.ToList();*/

        for (int i = 0; i < SnakeSpawner.Instance.snakes.Length; i++)
        {
            if (SnakeSpawner.Instance.snakes[i] != null)
            {
                tempListSnake.Add(SnakeSpawner.Instance.snakes[i]);
            }

            if (i < infoPoints.Count)
            {
                infoPoints[i].gameObject.SetActive(false);
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

            if (i < 4)
            {
                infoPoints[i].gameObject.SetActive(true);
                infoPoints[i].SetMainColor();
                infoPoints[i].image.SetActive(true);
                infoPoints[i].nameSnake.text = tempSnake.snakeName;
                infoPoints[i].points.text = tempSnake.points.ToString("0");
            }

        }

        //print(debug);

    }
}
