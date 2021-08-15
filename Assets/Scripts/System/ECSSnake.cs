using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public class ECSSnake 
{
    public int snakeId;
    public string snakeName;
    public float speed = 25;
    public float rotatingSpeed = 8f;
    public int points;
    public int speedMultiplier = 2;
    int originalSpeedMultiplier;
    int startingPoints = 50; // You can't go lower than this value
    float pieceForPoints = 19f; // Each 50p gets 1 piece //18
    public float referenceScale = 1f;
    int pointsForScale = 500; // Each 250p the scale increases 
    float scaleOffset = 0.04f;
    public float MOVlerpTime = 0.25f;
    public int snakePieces;
    public bool scaleChanged = false;
    public bool sprinting = false;
    public bool isDestroyed = false;
    public bool isPlayerSnake = false;
    public bool isPaused = false;
    public Entity snakeHead;
    public Entity lastPiece;
    public Material sprintMat;
    public Material maskMat;
    public Material[] colorTempMats;
    public ColorTemplate colorTemplate;
    public bool dontSpawnFood = false;
    public bool defaultMask = false;
    public bool isDuelModeDestroyed = false;
    public int teamId=-1;
    public string team = "";
    int nextColor = -1;
  
    public ECSSnake(int id,string name,int snakePoints,Vector3 spawnPos,ColorTemplate colorTemp,Sprite mask=null,bool isPlayer=false,string team="",bool isBabySnake = false) {
        mask = null;
        originalSpeedMultiplier = speedMultiplier;
        snakeId = id;
        
        points = snakePoints;
        snakeName = name;
        this.team = team;
        switch (team)
        {
            case "A":
                this.teamId = 0;
                break;
            case "B":
                this.teamId = 1;
                break;
            case "C":
                this.teamId = 2;
                break;
            default:
                this.teamId = -1;
                break;
        }
        
        snakePieces = GetSnakeParts();
        Debug.Log(snakePieces);
        if (SnakeSpawner.Instance.playerSnake != null)
        {
            if (spawnPos == Vector3.zero)
            {
                for (int x = 0; x < 3; x++)
                {
                    var randomSpawnCircleVector2 = UnityEngine.Random.insideUnitCircle * 350;
                    var playerHeadSpot = new Vector2(SnakeSpawner.Instance.playerTracker.transform.position.x, SnakeSpawner.Instance.playerTracker.transform.position.z);
                    if (Vector2.Distance(randomSpawnCircleVector2, playerHeadSpot) > 50f)
                    {
                        spawnPos = new Vector3(randomSpawnCircleVector2.x, 0,
                        randomSpawnCircleVector2.y);
                        break;
                    }

                }
            }
        }
        else
        {
            if (spawnPos == Vector3.zero)
            {
                var randomSpawnCircleVector2 = UnityEngine.Random.insideUnitCircle * 350;
                spawnPos = new Vector3(randomSpawnCircleVector2.x, 0,
                    randomSpawnCircleVector2.y);
            }
        }

        if (spawnPos == Vector3.zero)
            spawnPos = SnakeSpawner.Instance.GetRandomSpawnPoint(this);

        isPlayerSnake = isPlayer;
        //newSnakePieces = snakePieces;
        SetupMaterialsFromTemplate(colorTemp);

        if (mask != null)
        {
            defaultMask = false;
            //maskMat = new Material(SnakeSpawner.Instance.maskMat);

            maskMat.mainTexture = mask.texture;
        }
        else
        {
            defaultMask = true;
            maskMat = SnakeSpawner.Instance.maskMat;
        }

        sprintMat  = new Material(SnakeSpawner.Instance.sprintMat);
        colorTemplate = colorTemp;
        Entity[] snakeFirstAndLast = SnakeSpawner.Instance.SpawnSnake(this,id, snakePieces, spawnPos,isPlayer, team,isBabySnake);
       
        snakeHead = snakeFirstAndLast[0];
        lastPiece = snakeFirstAndLast[1];
        scaleChanged = true;
        SetSnakePieceMoveLerp();
        SnakeSpawner.Instance.DisableImmune(this);
    }

   

    public void SetupMaterialsFromTemplate(ColorTemplate colorTemp)
    {
        colorTempMats = new Material[colorTemp.colors.Length];
        for (int x = 0; x < colorTemp.colors.Length; x++)
        {
            //colorTempMats[x] = SnakeSpawner.Instance.GetMaterial(colorTemp.colors[x]);
            colorTempMats[x] = SnakeSpawner.Instance.testMaterial;
            //  colorTempMats[x].color = colorTemp.colors[x];
        }
    /*    bool isTransparency = colorTemp.colors[0].a < 1f;

        if (isTransparency)
        {

        }
        else
        {
           
        }*/
    }

    public void Destroy()
    {
      

        isDestroyed = true;
    }

    

    public Material GetNextColor()
    {
        /*int colorLength = colorTempMats.Length;
        nextColor++;
        int colorToChoose = nextColor % colorLength;
        return colorTempMats[colorToChoose];*/
        return SnakeSpawner.Instance.testMaterial;
    }

    public void DecreaseNextColor(int num)
    {
        nextColor -= num;
    }

    public void SetupSnake()
    { 
        
    }

    public Vector3 GetSnakeHeadPosition()
    {
        return SnakeSpawner.Instance.GetSnakeHeadPosition(this);
    }


    public IEnumerator sprint()
    {
       
        sprinting = true;
        SpeedBoost();
        float lerp = 0f;
        Color normalColor = Color.white;
        Color alphaColor = new Color(1, 1, 1, 0);
        while (sprinting)
        {

            if (lerp <= 1.0f)
            {
                lerp += Time.fixedDeltaTime * 2f;
                sprintMat.color = Color.Lerp(alphaColor,normalColor, lerp);
            }
            else
                sprintMat.color = normalColor;
            yield return new WaitForFixedUpdate();
        }

        while (lerp>0f&&!sprinting)
        {

                lerp -= Time.fixedDeltaTime * 5f;
                sprintMat.color = Color.Lerp(alphaColor, normalColor, lerp);

            yield return new WaitForFixedUpdate();
        }
        if (!sprinting)
        {
            sprintMat.color = alphaColor;
            SpeedNormal();
        }
        
        yield return null;
    }

    public void IncreasePoints(int p)
    {

        points += p;

        SnakeSpawner.Instance.UpdateSnakeHeadPoints(this);
        snakePieces = GetSnakeParts();
    }

    public void DecreasePoints(int p)
    {
        if (points - p < 100)
        {
            points = 100;


        }
        else
            points -= p;

        SnakeSpawner.Instance.UpdateSnakeHeadPoints(this);
        // newSnakePieces = GetSnakeParts();
        snakePieces = GetSnakeParts();
    }
    bool isBoosted = false;
    public void SpeedBoost()
    {
        speedMultiplier = 2;
        SetSnakePieceMoveLerp();
        SnakeSpawner.Instance.UpdateSnakeHead(this);
    }
    public void SpeedNormal()
    {
        speedMultiplier = 1;
        SetSnakePieceMoveLerp();
        SnakeSpawner.Instance.UpdateSnakeHead(this);
    }
  

    public void SetSnakePieceMoveLerp()
    {
        if (isPlayerSnake)
        {
            Debug.Log("player move lerp set");
            if (speedMultiplier > 1f)
                MOVlerpTime = .45f;
            else
                MOVlerpTime = .35f;
        }
        else
        {
            if (speedMultiplier > 1f)
                MOVlerpTime = .35f;
            else
                MOVlerpTime = .25f;
        }
    }

    public int GetSnakeParts()
    {
        
        int partsDiff = 0;
        if (points > 10000)
        {

            partsDiff += (int)math.round((650) / (19));
            partsDiff += Mathf.RoundToInt((1500 - 650) / (22));
            partsDiff += Mathf.RoundToInt((5000 - 1500) / (30));
            partsDiff += Mathf.RoundToInt((10000 - 5000) / (50));
            partsDiff += Mathf.RoundToInt((points - 10000) / (100));
            return partsDiff;// (int)math.round((points) / (30));
        }
        else if(points > 5000 && points <= 10000)
        {

            partsDiff += (int)math.round((650) / (19));
            partsDiff += Mathf.RoundToInt((1500 - 650) / (22));
            partsDiff += Mathf.RoundToInt((5000 - 1500) / (30));
            partsDiff += Mathf.RoundToInt((points - 5000) / (50));
            return partsDiff;// (int)math.round((points) / (30));
        }
        else if (points > 1500&&points<=5000)
        {

            partsDiff += (int)math.round((650) / (19));
            partsDiff += Mathf.RoundToInt((1500 - 650) / (22));
            partsDiff += Mathf.RoundToInt((points - 1500) / (30));
         
            return partsDiff;// (int)math.round((points) / (30));
        }
        else if (points > 650&&points<=1500)
        {
            partsDiff += (int)math.round((650) / (19));
            partsDiff += Mathf.RoundToInt((points-650) / (22));
            return partsDiff;
        }
        else
        {
            
            return (int)math.round((points) / (19));
        }
    }

  

    public float GetSnakeScale()
    {
        
        float scalechange = (float)points / pointsForScale;

        float scale = 1 + scalechange * scaleOffset;
        if (scale > GameConstants.MAXSNAKESCALE)
        {
            scale = GameConstants.MAXSNAKESCALE;
        }

       
        return scale;
    }

   
}
