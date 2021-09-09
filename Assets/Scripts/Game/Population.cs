using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Population : MonoBehaviour
{

    public int baby;   //20;
    public int small;   //20;
    public int medium;  //10;
    public int big; //5;
    public int reallybig;   //5;
    public int superbig;    //1;
    public int MaxPopulation;   //20
    public static Population instance;
    public float spawnCircleLenght;   //300
    public float minimalSpawnDistanceToPlayer = 60f;  
    // public GameObject snakePrefab;
    public int updateScoreFrequency = 0;

    public Text[] nameText, scoreText;
    public Text[] nameTextDuel, scoreTextDuel;
    public Text[] teamsScore,teamsLabels;
    public GameObject[] teamsScoreObj;
  //  public List<Snake> allSnakes;
    // Dictionary<Snake, string> currSnakes = new Dictionary<Snake, string>();

    string[] defaultNames = new string[]{"I should study", "Lol", "Pop", "Your mooom", "eater eatest", "nom nom nom",
        "longer", "lilli", "areum", "kom", "nuna", "ophidiophobia", "viper", "anaconda", "remember me?", "ignore me !!",
        "YOUR KING", "Eat Me", "What’s this?", "smile more", "cari", "beea", "Im lost", "undeath", "fluffy", "eat him",
        "boom", "(*_*)", "Help! I’m Lost!", "I’m Doomed", "((^▽^))", "AlaskanBullWorm", "Ion lee", "babayaga", "babushka",
        "antonio demaciado", "lodesea", "playas", "the D", "I m batman!", "your bla bla", "jsjsjsjs"};

    public List<string> tempNames = new List<string>();
    public string topperName = "none";
    public string lastTopperName = "none";

    public float transparentPercent;
    public float noMaskPercent;
    public float transparentCount;
    public float noMaskCount;
    public float snakeCounter;
    public Vector3 duelBot1_position;
    public Vector3 duelBot2_position;
    public Vector3 duelBot3_position;
    public Vector3 duelPlayer_position;
    public Vector3[] duelMode_Position;

    [HideInInspector] public int realCount;
    public bool activeCounter;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //SetDuelPositions();
        snakeCounter = 1;
        transparentCount = (transparentPercent / 100) * GameConstants.TOTAL_SNAKES;
        noMaskCount = (noMaskPercent / 100) * GameConstants.TOTAL_SNAKES;
        noMaskCount = Mathf.RoundToInt(noMaskCount);
        //MaxPopulation = GameConstants.TOTAL_SNAKES;
        //SpawnSnake(Random.Range(30000, 35000), SnakeEnvironment.snakeType.superbig, "");
        Initialize();
    }

    //private void SetDuelPositions()
    //{
    //    var lowerLeftScreen = new Vector2(0, 0);
    //    var lowerRightScreen = new Vector2(Screen.width, 0);
    //    var upperLeftScreen = new Vector2(0, Screen.height);
    //    var upperRightScreen = new Vector2(Screen.width, Screen.height);

    //    duelPlayer_position = Camera.main.ScreenToWorldPoint(lowerLeftScreen) + GameManager.instance.duelScreenOffsetPosition;
    //    duelBot1_position = Camera.main.ScreenToWorldPoint(lowerRightScreen) - new Vector3(GameManager.instance.duelScreenOffsetPosition.x , 0f, -GameManager.instance.duelScreenOffsetPosition.z); 
    //    duelBot2_position = Camera.main.ScreenToWorldPoint(upperRightScreen) - GameManager.instance.duelScreenOffsetPosition;
    //    duelBot3_position = Camera.main.ScreenToWorldPoint(upperLeftScreen) + new Vector3(GameManager.instance.duelScreenOffsetPosition.x, 0f, -GameManager.instance.duelScreenOffsetPosition.z);

    //    duelPlayer_position.y = duelBot1_position.y = duelBot2_position.y = duelBot3_position.y = 0f;
    //}

    public void StopAllRoutines()
    {
        StopAllCoroutines();
        StopCoroutine("checkSpawnSnake");
        StopCoroutine("spawnSnakePopulation");
    }

    public void Initialize()
    {
        StartCoroutine(ImposeDelay());
    }

    IEnumerator ImposeDelay()
    {
        yield return new WaitForSeconds(GameConstants.TIME_TO_SPAWN_SNAKE);
        tempNames = defaultNames.ToList();

        while (!SnakeManager.instance.isLoaded)
        {
            yield return null;
        }

        SpawnPopulation();
    }



    public void SpawnPopulation()
    {
        Debug.Log("spawning snake population");
        StopCoroutine("checkSpawnSnake");
        StopCoroutine(SpawnSnakePopulation());
        StartCoroutine(SpawnSnakePopulation());
    }

    public void StopSnakeLoading()
    {
        StopAllCoroutines();
       // Debug.Log("Snake loading stopped");
    }

    float TimeToSpawnSnakes()
    {
        float time;
        if (GameManager.instance.InGame)
        {
            time = 5;
        }
        else
        {
            time = 0.5f;
        }
        return time;
    }

    IEnumerator SpawnSnakePopulation()
    {

        if (GameManager.instance.state == GameManager.STATE.IN_GAME)
        {
            float time = 0.5f;
            //    for (int i = 0; i < mega; i++)
            //     {
            if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
            {
                SpawnSnake(Random.Range(20000, 35000), SnakeEnvironment.snakeType.superbig, "");  //10000
                                                                                                  //SpawnSnake(Random.Range(60000, 80000), SnakeEnvironment.snakeType.superbig, "");  //10000
                realCount++;
                yield return new WaitForSeconds(time);
            }
            //   }
            time = TimeToSpawnSnakes();
            for (int i = 0; i < superbig; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(15000, 20000), SnakeEnvironment.snakeType.superbig, "");  //10000
                    realCount++;
                    yield return new WaitForSeconds(time);
                }
            }
            time = TimeToSpawnSnakes();

            for (int i = 0; i < reallybig; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(15000, 30000), SnakeEnvironment.snakeType.reallybig, "");
                    realCount++;
                    yield return new WaitForSeconds(time);
                }
            }
            time = TimeToSpawnSnakes();
            for (int i = 0; i < big; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(5000, 8000), SnakeEnvironment.snakeType.big, "");
                    realCount++;
                    yield return new WaitForSeconds(time);
                }
            }
            time = TimeToSpawnSnakes();
            for (int i = 0; i < medium; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(2000, 4000), SnakeEnvironment.snakeType.medium, "");
                    realCount++;
                    yield return new WaitForSeconds(time);
                }
            }
            time = TimeToSpawnSnakes();
            for (int i = 0; i < small; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(250, 1500), SnakeEnvironment.snakeType.small, "");
                    realCount++;
                    yield return new WaitForSeconds(time);
                }

            }
            time = TimeToSpawnSnakes();
            for (int i = 0; i < baby; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(250, 500), SnakeEnvironment.snakeType.small, "");
                    realCount++;
                    yield return new WaitForSeconds(time);
                }

            }
            time = TimeToSpawnSnakes();



        




            if (realCount <= MaxPopulation && !activeCounter)
            {
                for (int i = realCount; i < 80; i++)
                {
                    realCount++;
                    SnakeEnvironment.Singleton.counterPiece += Random.Range(100, 500);
                    yield return new WaitForSeconds(time);
                }
            }
        //   Debug.Log("spawning snake with check up");
        StartCoroutine("checkSpawnSnake");
        }else if(GameManager.STATE.TEAM2X2 == GameManager.instance.state)
        {
            for (int x = 0; x < GameConstants.TOTAL_SNAKES; x++)
            {
                string nSelectedTeam = "B";
                Debug.Log(((x + 2) % 2).ToString());
                if ((x + 2) % 2 == 0)
                    nSelectedTeam = "A";


                SpawnSnakeAITeam(nSelectedTeam, "2/4/6/8/11");
                yield return new WaitForSeconds(0.5f);
            }
        }else if (GameManager.STATE.TEAM3X3 == GameManager.instance.state)
        {
            for (int x = 0; x < 32; x++)
            {
                string nSelectedTeam = "C";
                Debug.Log(((x + 3) % 3).ToString());
                if ((x + 3) % 3 == 0)
                    nSelectedTeam = "A";
                else if ((x + 3) % 3 == 1)
                    nSelectedTeam = "B";


                SpawnSnakeAITeam(nSelectedTeam, "1/3/4/9/14");
                yield return new WaitForSeconds(0.5f);
            }
        }else if (GameManager.STATE.IN_MENU == GameManager.instance.state)
        {
            float time = 0.5f;
            //    for (int i = 0; i < mega; i++)
            //     {
            if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
            {
                SpawnSnake(Random.Range(20000, 35000), SnakeEnvironment.snakeType.superbig, "");  //10000
                                                                                                  //SpawnSnake(Random.Range(60000, 80000), SnakeEnvironment.snakeType.superbig, "");  //10000
                realCount++;
                yield return new WaitForSeconds(time);
            }
            //   }
            time = TimeToSpawnSnakes();
            for (int i = 0; i < superbig; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(15000, 20000), SnakeEnvironment.snakeType.superbig, "");  //10000
                    realCount++;
                    yield return new WaitForSeconds(time);
                }
            }
            time = TimeToSpawnSnakes();

            for (int i = 0; i < reallybig; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(15000, 30000), SnakeEnvironment.snakeType.reallybig, "");
                    realCount++;
                    yield return new WaitForSeconds(time);
                }
            }
            time = TimeToSpawnSnakes();
            for (int i = 0; i < big; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(5000, 8000), SnakeEnvironment.snakeType.big, "");
                    realCount++;
                    yield return new WaitForSeconds(time);
                }
            }
            time = TimeToSpawnSnakes();
            for (int i = 0; i < medium; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(2000, 4000), SnakeEnvironment.snakeType.medium, "");
                    realCount++;
                    yield return new WaitForSeconds(time);
                }
            }
            time = TimeToSpawnSnakes();
            for (int i = 0; i < small; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(250, 1500), SnakeEnvironment.snakeType.small, "");
                    realCount++;
                    yield return new WaitForSeconds(time);
                }

            }
            time = TimeToSpawnSnakes();
            for (int i = 0; i < baby; i++)
            {
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(250, 500), SnakeEnvironment.snakeType.small, "");
                    realCount++;
                    yield return new WaitForSeconds(time);
                }

            }
            time = TimeToSpawnSnakes();








            if (realCount <= MaxPopulation && !activeCounter)
            {
                for (int i = realCount; i < 80; i++)
                {
                    realCount++;
                    SnakeEnvironment.Singleton.counterPiece += Random.Range(100, 500);
                    yield return new WaitForSeconds(time);
                }
            }
            //   Debug.Log("spawning snake with check up");
            StartCoroutine("checkSpawnSnake");
        }
    }

    public void SpawnSnakeAITeam(string team, string numbers)
    {
        string[] nNums = numbers.Split('/');
        int superbigSnakes = int.Parse(nNums[0]);
        int realybigSnakes = int.Parse(nNums[1]);
        int bigSnakes = int.Parse(nNums[2]);
        int medSnakes = int.Parse(nNums[3]);
        int smallSnakes = int.Parse(nNums[4]);

        int NsuperbigSnakes = 0;
        int NrealybigSnakes = 0;
        int NbigSnakes = 0;
        int NmedSnakes = 0;
        int NsmallSnakes = 0;
        int x = 0;
        for(int i= 0;i< SnakeSpawner.Instance.snakes.Length; i++)
        {
            
            if (SnakeSpawner.Instance.snakes[i] != null)
            {
                ECSSnake ecsSnake = SnakeSpawner.Instance.snakes[i];
                if (!ecsSnake.isDestroyed)
                {
                    if (ecsSnake.points>=10000)
                        NsuperbigSnakes++;

                    if (ecsSnake.points >= 6000)
                        NrealybigSnakes++;

                    if (ecsSnake.points >= 4000)
                        NbigSnakes++;

                    if (ecsSnake.points >= 2000)
                        NmedSnakes++;

                    if (ecsSnake.points >= 150)
                        NsmallSnakes++;
                }
            }
        }

        if (NsuperbigSnakes < superbigSnakes)
            x = 0;
        else if (NrealybigSnakes < realybigSnakes)
            x = 1;
        else if (NbigSnakes < bigSnakes)
            x = 2;
        else if (NmedSnakes < medSnakes)
            x = 3;
        else if (NsmallSnakes < smallSnakes)
            x = 4;

            switch (x)
            {
                case 0:
                   
                        if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                        {
                            SpawnSnake(20000, SnakeEnvironment.snakeType.superbig, team);  //10000
                            return;
                        }
                   
                    break;
                case 1:
                   
                        if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                        {
                            SpawnSnake(10000, SnakeEnvironment.snakeType.reallybig, team);  //10000
                            return;
                        }
                 
                    break;
                 case 2:
                  
                        if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                        {
                            SpawnSnake(6000, SnakeEnvironment.snakeType.big, team);  //10000
                            return;
                        }
                   
                    break;
                case 3:
                   
                        if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                        {
                            SpawnSnake(Random.Range(1000, 2000), SnakeEnvironment.snakeType.medium, team);  //10000
                            return;
                        }
                  
                    break;
                case 4:
                  
                        if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                        {
                            SpawnSnake(Random.Range(150, 500), SnakeEnvironment.snakeType.small, team);  //10000
                            return;
                        }
                   
                    break;

            }
        
    }

    public void AddSnakesDuel()
    {


        for (int i = 1; i < 4; i++)
        {
            ColorTemplate colorChosen = SnakeSpawner.Instance.selectedColorTemplate;

            bool ifMask = (Random.Range(0, 3) == 0);

            SnakeSpawner.Instance.CreateNewSnake(250, GetRandomName(), duelMode_Position[i], colorChosen, SkinsManager.instance.GetRandomMask(), false);

        }
    }

    public void SpawnSnake(int points, SnakeEnvironment.snakeType type,string team="")
    {
        Debug.Log("spawning snake: " + type);
        if (SnakeEnvironment.Singleton.CounterSnake >= MaxPopulation)
            return;

        //ColorTemplate colorChosen = SnakeSpawner.Instance.stockColorTemplates[Random.Range(0, SnakeSpawner.Instance.stockColorTemplates.Count)];//SkinManager._instance.availableColorTemplate[Random.Range(2, SkinManager._instance.availableColorTemplate.Length - 1)];
        ColorTemplate colorChosen = SnakeSpawner.Instance.selectedColorTemplate;
        /*if (team!="")
            colorChosen = SkinManager._instance.GetTeamBasedColorTemplate(team);*/

        bool ifMask = (Random.Range(0, 4) == 0);
        SnakeSpawner.Instance.CreateNewSnake(points, GetRandomName(), Vector3.zero, colorChosen,/*ifMask ? MaskManager.instance.GetRandomMask() : null*/ SkinsManager.instance.GetRandomMask() ,false, team);
        // newsnake.SetActive(true);
        Debug.Log("snake is created successfully");


        snakeCounter++;

    }

    /*private bool IsSpawnNearPlayer(Vector3 spawnPoint)
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.GetPlayerSnakeParams() != null)
            {
                return GameManager.instance.IsPlayerActive && Vector3.Distance(spawnPoint, GameManager.instance.GetPlayerSnakeParams().GetSnakeHeadPosition()) < minimalSpawnDistanceToPlayer;
            }
            else
                return false;
        }
        else
            return false;
    }*/

   /* public Snake SpawnRemoteSnake(string name, float posx, float posy, int points)
    {
        if (allSnakes.Count >= MaxPopulation)
        {
            foreach (Snake snake in allSnakes)
            {
                if (snake.IsAutoPlayer())
                {
                    allSnakes.Remove(snake);
                    GameManager.instance.DestroySnake(snake);
                    break;
                }
            }
        }
        GameObject newsnake = null;
        Snake snakeparams = null;
        newsnake = SnakeManager.instance.InstantiateSnake(Vector3.zero, Quaternion.identity);
        snakeparams = newsnake.GetComponent<Snake>();
        snakeparams.points = points;
        snakeparams.snakeHead.IsPlayer = false;
        snakeparams.isPlayer = false;
        newsnake.transform.position = new Vector3(posx, newsnake.transform.position.y, posy);
        AddSnake(snakeparams, name);
        return snakeparams;
    }*/

    public string GetRandomName()
    {
        if (tempNames.Count == 0)
        {
            tempNames = defaultNames.ToList();
        }
        int rand = Random.Range(0, tempNames.Count);
        var randName = tempNames[rand];
        tempNames.RemoveAt(rand);

        return randName;
    }

 /*   public void AddSnake(Snake snake, string name)
    {
        snake.snakeName = name.ToString();
       
        allSnakes.Add(snake);
        // currSnakes.Add(snake, name);
    }*/

 /*   public void RemoveSnake(Snake snake)
    {
        allSnakes.Remove(snake);
       
    }*/

    public void StopAllSnakeIncrease()
    {
        StopCoroutine("checkSpawnSnake");
        StopCoroutine("SpawnSnakePopulation");
    }

    IEnumerator checkSpawnSnake()
    {
        Debug.Log("Spawner started");
        while (true)
        {
            
                //Debug.Log("Check spawn snake running!");
                if (SnakeEnvironment.Singleton.CounterSnake < MaxPopulation)
                {
                    SpawnSnake(Random.Range(150, 500), SnakeEnvironment.snakeType.small, "");

                }

              

            

            if (SnakeSpawner.Instance.playerSnake==null)
            {
                //AddSnake(GameManager.instance.playerSnake.GetComponent<Snake>(), PlayerPrefs.GetString("PlayerName", "You"));
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UpdateScoreBoard()
    {
        if (SnakeEnvironment.Singleton.CounterSnake < 6)
            return;


            Dictionary<string, int> individualSnakes = new Dictionary<string, int>();
            individualSnakes.Clear();
            foreach (ECSSnake snake in SnakeSpawner.Instance.snakes)
            {
                if (snake != null)
                {
                    if (!snake.isDestroyed)
                    {
                        if (!individualSnakes.ContainsKey(snake.snakeName))
                            individualSnakes.Add(snake.snakeName, snake.points);
                    }
                }
            }

            List<KeyValuePair<string, int>> scoreList = individualSnakes.ToList();

            scoreList.Sort(
                delegate (KeyValuePair<string, int> pair1,
                    KeyValuePair<string, int> pair2)
                {
                    return pair1.Value.CompareTo(pair2.Value);
                }
            );


                int highScoreDisplayCount = nameText.Length;
                if (highScoreDisplayCount >= 1 && individualSnakes.Count > 0)
                {
                    for (int i = 1; i <= highScoreDisplayCount; i++)
                    {
                        if (i == 1)
                            topperName = scoreList[scoreList.Count - i].Key.ToString();
                        var t = i - 1;
                        var d = scoreList.Count;
                        nameText[i - 1].text = scoreList[scoreList.Count - i].Key.ToString();
                        scoreText[i - 1].text = scoreList[scoreList.Count - i].Value.ToString();//individualSnakes [scoreList [scoreList.Count - i].ToString()].ToString();
                    }
                }

                if (lastTopperName != topperName)
                    UpdateCrownedSnake(topperName);


            
        
    }

    public void RemoveAllSnakes()
    {

    }

    public int GetTotalTeamScore(string team)
    {
        int totalTeamScore = 0;
        for (int x = 0; x < SnakeSpawner.Instance.snakes.Length; x++)
        {
            
            if (SnakeSpawner.Instance.snakes[x] != null)
            {
                ECSSnake snake = SnakeSpawner.Instance.snakes[x];
                if (snake.team == team)
                    totalTeamScore += snake.points;
            }
        }

        return totalTeamScore;

    }

    public void UpdateCrownedSnake(string _topperName)
    {
        string lastTopperNameFound = lastTopperName;
        lastTopperName = topperName;
        topperName = _topperName;
        for (int j = 0; j < SnakeSpawner.Instance.snakes.Length; j++)
        {
            if (SnakeSpawner.Instance.snakes[j] != null)
            {
                ECSSnake snake = SnakeSpawner.Instance.snakes[j];
                /*if (snake.snakeName == lastTopperNameFound)
                {
                    SnakeSpawner.Instance.ToggleCrown(snake, false);
                }
                if (snake.snakeName == topperName)
                {
                    SnakeSpawner.Instance.ToggleCrown(snake, true);
                }*/
            }
        }
    }

    void FixedUpdate()
    {
        //if (!GameManager.instance.isDuelModeOn)
        //{
        if (SnakeSpawner.Instance == null)
            return;

        if (SnakeSpawner.Instance.snakes == null)
            return;

            if (SnakeEnvironment.Singleton.CounterSnake > 0)
            {
                if (updateScoreFrequency % 10 == 0)
                {
                    UpdateScoreBoard();
                }
                updateScoreFrequency++;
            }
        }
   // }
}
