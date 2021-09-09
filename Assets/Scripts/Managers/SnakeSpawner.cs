using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    public static SnakeSpawner Instance;
    public GameObject snakePiecePrefab;
    public GameObject snakeHeadPrefab;
    public GameObject playerSnakeHeadPrefab;
    public ECSSnake[] snakes;
    public ECSSnake playerSnake;
    public GameObject playerTracker;
    public Transform SpawnPointAIContent;
    private SpawnPoint[] aiSpawnPoints;
    public UnityEngine.Material sprintMat;
    public UnityEngine.Material solidColor;
    public UnityEngine.Material transparentColor;
    public UnityEngine.Material maskMat;
    public UnityEngine.Material noCrown;
    public UnityEngine.Material crown;

    public Material testMaterial;

    private Entity snakePieceEntity;
    private Entity snakeHeadEntity;
    private Entity playerSnakeHeadEntity;
    private World world;
    private EntityManager manager;
    private BlobAssetStore blobAssetStore;
    public int playerID;
    public Mesh square;
    public Mesh quad;
    private List<UnityEngine.Material> stockMaterials;


    public CameraManager camerManager;

    public Transform[] playerSpawnPoints;


    public ColorTemplate selectedColorTemplate;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        stockMaterials = new List<UnityEngine.Material>();
        world = World.DefaultGameObjectInjectionWorld;
        manager = world.EntityManager;
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(world, blobAssetStore);// (new BlobAssetStore()));
        snakePieceEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(snakePiecePrefab, settings);
        snakeHeadEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(snakeHeadPrefab, settings);
        playerSnakeHeadEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerSnakeHeadPrefab, settings);

        
        snakes = new ECSSnake[GameConstants.TOTAL_SNAKES];

        aiSpawnPoints = new SpawnPoint[SpawnPointAIContent.childCount];
        for (int i = 0; i < SpawnPointAIContent.childCount; i++)
        {
            aiSpawnPoints[i] = SpawnPointAIContent.GetChild(i).GetComponent<SpawnPoint>();
        }
        //StartCoroutine(Create());
        //CreateNewSnake(100, "PlayerName", playerSpawnPoints[0].position, colorTemp, null, true, "");


    }


    public void DestroyAllSnakes()
    {
        StopAllCoroutines();
        foreach (ECSSnake snake in snakes)
        {
            if (snake != null)
            {
                snake.dontSpawnFood = true;

                DestroySnake(snake);
            }
        }
        Population.instance.StopAllRoutines();
        Population.instance.realCount = 0;
        SnakeEnvironment.Singleton.counterPiece = 0;
        //SetupNewColorTemplatesAndMaterialsForBots();
    }

    public void DestroyAllSnakes(int it)
    {
        StopAllCoroutines();
        for (int i = it; i < snakes.Length; i++)
        {
            if (snakes[i] != null)
            {
                snakes[i].dontSpawnFood = true;
                DestroySnake(snakes[i]);
                //snakes[i] = null;
            }
        }
        //SetupNewColorTemplatesAndMaterialsForBots();
    }

    public void DestroySnake(ECSSnake snake)
    {
        SnakeHeadData headData = manager.GetComponentData<SnakeHeadData>(snake.snakeHead);

        headData.shouldDestroy = true;
        headData.isDead = true;
        //SnakeSpawner.Instance.RemoveSnake(snake);
        SnakeEnvironment.Singleton.PopUpSnake(headData.snakeId, snake.snakePieces);
        manager.SetComponentData<SnakeHeadData>(snake.snakeHead, headData);

    }

    public void StartSprint(ECSSnake snake)
    {
        if (!snake.sprinting)
        {
            StartCoroutine(snake.sprint());
        }
    }

    public void StartGame2x2()
    {
        string team = null;
        int rnd = UnityEngine.Random.Range(0, 2);

        switch (rnd)
        {
            case 0:
                team = "A";
                break;
            case 1:
                team = "B";
                break;
        }
        ColorTemplate colorTemp = selectedColorTemplate;
        CreateNewSnake(150, "PlayerName", playerSpawnPoints[0].position, colorTemp, PlayerProgress.instance.skinMask.maskSprite, true, team);
        Population.instance.Initialize();
    }

    public void StartGame3x3()
    {
        string team = null;
        int rnd = UnityEngine.Random.Range(0, 3);

        switch (rnd)
        {
            case 0:
                team = "A";
                break;
            case 1:
                team = "B";
                break;
            case 2:
                team = "C";
                break;
        }
        ColorTemplate colorTemp = selectedColorTemplate;
        CreateNewSnake(150, "PlayerName", playerSpawnPoints[0].position, colorTemp, PlayerProgress.instance.skinMask.maskSprite, true, team);
        Population.instance.Initialize();
    }
    public void StartGameWithAI()
    {
        StartCoroutine(Create());
    }



    IEnumerator Create()
    {
        yield return new WaitForSeconds(0.5f);
        ColorTemplate colorTemp = selectedColorTemplate;
        CreateNewSnake(150, "PlayerName", playerSpawnPoints[0].position, colorTemp, PlayerProgress.instance.skinMask.maskSprite, true, "");
        Population.instance.Initialize();

    }

    public void CreateNewSnake(int snakeSize, string name, Vector3 position, ColorTemplate colortemp, Sprite maskSelected = null, bool isPlayer = false, string team = "", bool isBabySnake = false)
    {
        int snakeNum = GetEmptySnakeArrayPos();
        ECSSnake newSnake = new ECSSnake(snakeNum, name, snakeSize, position, colortemp, maskSelected, isPlayer, team, isBabySnake);
        //  numOfSnake++;
        //Debug.Log("Snake num index chosen : " + snakeNum);
        if (snakeNum >= snakes.Length - 1)
        {
            return;
        }
        snakes[snakeNum] = newSnake;

        if (isPlayer)
        {
            playerSnake = newSnake;
            camerManager.playerSnake = playerSnake;
        }
    }

    public void GetPlayerDead(int id)
    {
        if (id == playerID)
        {
            print("muerto player");
            //CreateNewSnake(50, "PlayerName", playerSpawnPoints[0].position, selectedColorTemplate, null, true, "");
            playerSnake = null;
            GameManager.instance.LooseWithAI();
        }
    }

    public int GetEmptySnakeArrayPos()
    {
        for (int x = 0; x < snakes.Length; x++)
        {
            if (snakes[x] == null)
            {
                return x;
            }
        }

        return -1;
    }
    public Vector3 GetRandomSpawnPoint(ECSSnake snake)
    {
        List<SpawnPoint> spawnPointAvailable = new List<SpawnPoint>();
        for (int x = 0; x < aiSpawnPoints.Length; x++)
        {
            if (aiSpawnPoints[x].canSpawn == 0f)
            {
                if (SnakeSpawner.Instance.playerSnake != null)
                {
                    if (Vector3.Distance(aiSpawnPoints[x].transform.position, playerTracker.transform.position) > 200f)
                    {
                        spawnPointAvailable.Add(aiSpawnPoints[x]);
                    }
                }
                else
                {
                    spawnPointAvailable.Add(aiSpawnPoints[x]);
                }
            }
        }
        if (spawnPointAvailable.Count == 0)
        {
            return Vector3.zero;
        }

        int xrnd = UnityEngine.Random.Range(0, spawnPointAvailable.Count);

        Debug.Log("spawn point available : " + spawnPointAvailable.Count);
        spawnPointAvailable[xrnd].CannotSpawn(snake);
        return spawnPointAvailable[xrnd].transform.position;
    }

    public Entity[] SpawnSnake(ECSSnake snake, int snakeIndex, int snakeSize, Vector3 spawnPos, bool isPlayer = false, string team = "", bool isBaby = false)
    {
        Entity[] snakeFirstAndLast = new Entity[2];
        float x = Mathf.Sin(snakeIndex) * UnityEngine.Random.Range(0, GameConstants.FIELD_SCALE);
        float z = Mathf.Cos(snakeIndex) * UnityEngine.Random.Range(0, GameConstants.FIELD_SCALE);
        int numberOfPieces = snakeSize;

        if (isPlayer)
        {
            playerID = snakeIndex;
        }
        // Debug.Log("player spawned " + isPlayer);
        Entity snakeHead = Entity.Null;// manager.Instantiate(snakeHeadEntity);
        if (!isPlayer)
        {
            snakeHead = manager.Instantiate(snakeHeadEntity);
        }
        else
        {
            snakeHead = manager.Instantiate(playerSnakeHeadEntity);
        }

        //     manager.RemoveComponent<PhysicsVelocity>(snakeHead);
        int teamid = 0;
        switch (team)
        {
            case "A":
                teamid = 0;
                break;

            case "B":
                teamid = 1;
                break;

            case "C":
                teamid = 2;
                break;

            default:
                teamid = -1;
                break;
        }
        
        snake.AssignMaterial(teamid);
        //Debug.Log("Team id : " + teamid);

        /*if (isBaby)
        {
            manager.AddComponentData<BabySnakeData>(snakeHead, new BabySnakeData());
            if (playerSnake != null)
            {
                if (playerSnake.teamId == -1)
                {
                    teamid = playerSnake.snakeId;
                }
                else
                {
                    teamid = playerSnake.teamId;
                }

            }
        }*/

        manager.SetComponentData(snakeHead, new LocalToWorld
        {
            Value = new float4x4((new quaternion(0, 0, 0, 1)), spawnPos)
        });

        manager.SetComponentData(snakeHead, new Translation
        {
            Value = spawnPos
        });
        manager.SetComponentData(snakeHead, new Rotation
        {
            Value = new quaternion(0, 0, 0, 1)
        });
        manager.SetComponentData(snakeHead, new SnakeHeadData
        {
            snakeId = snakeIndex,
            speed = GameConstants.SPEED,
            isPlayer = isPlayer,
            snakeRotationSpeed = 5,
            speedMultiplier = 1,
            headDiff = /*isPlayer ? GameConstants.SNAKE_DIFF :  (snake.points > 10000) ? 0.003f : GameConstants.SNAKE_DIFF*/ snake.Diff(),
            shouldDestroy = false,
            isDead = false,
            isImmune = true,
            teamId = (teamid == -1) ? snakeIndex : teamid,
            isDuelMode = GameManager.instance.IsDuelMode,
            isBabySnake = isBaby
        });

        manager.AddComponentData(snakeHead, new NonUniformScale
        {
            Value = new float3(GameConstants.SNAKE_HEAD_SCALE  , GameConstants.SNAKE_HEAD_SCALE, GameConstants.SNAKE_HEAD_SCALE)
        });
        manager.AddSharedComponentData(snakeHead, new SnakeGroupData
        {
            group = snakeIndex
        });
        int ssnakeParts = snake.GetSnakeParts();
        manager.SetComponentData(snakeHead, new SnakeHeadPartsData
        {
            snakeParts = ssnakeParts,
            snakeNewParts = ssnakeParts
        });
        manager.SetComponentData(snakeHead, new SnakePointsData
        {
            points = snake.points
        });
        manager.SetComponentData(snakeHead, new PieceScaleData
        {
            pieceIndex = 0,
            scaleData = GameConstants.SNAKE_HEAD_SCALE
        });
        if (!isPlayer)
        {
            SnakeHeadTargetData snakeHeadTargetData = manager.GetComponentData<SnakeHeadTargetData>(snakeHead);
            snakeHeadTargetData.foodTarget = float3.zero;
            snakeHeadTargetData.moveDirection = float3.zero;
            snakeHeadTargetData.isReachedPosition = false;
            snakeHeadTargetData.isAIMoveDirection = false;

            Entity aiEntity = snakeHeadTargetData.ai;
            AIData aiData = manager.GetComponentData<AIData>(aiEntity);
            aiData.snakeId = snakeIndex;
            aiData.counter = 0;

            aiData.teamId = (teamid == -1) ? snakeIndex : teamid;
            //aiData.isDuelMode = GameManager.instance.IsDuelMode();
            aiData.isBabySnake = isBaby;
            manager.SetComponentData<AIData>(aiEntity, aiData);
            manager.SetComponentData<SnakeHeadTargetData>(snakeHead, snakeHeadTargetData);
        }

        SnakeGlowData glowData = manager.GetComponentData<SnakeGlowData>(snakeHead);
        manager.SetSharedComponentData<RenderMesh>(glowData.glowEntity, new RenderMesh
        {
            mesh = square,
            material = snake.GetNextSprintColor(0)
        });

        SnakeBallColorData ballData = manager.GetComponentData<SnakeBallColorData>(snakeHead);
        manager.SetSharedComponentData<RenderMesh>(ballData.snakeBallEntity, new RenderMesh
        {
            mesh = quad,
            material = snake.GetNextColor(0)
        });
        manager.SetComponentData<Translation>(ballData.snakeBallEntity, new Translation
        {
            Value = new float3(0, 0.1f, 0)
        });

        SnakeMaskData maskData = manager.GetComponentData<SnakeMaskData>(snakeHead);
        manager.SetSharedComponentData<RenderMesh>(maskData.entityData, new RenderMesh
        {
            mesh = square,
            material = snake.maskMat
        });

        DynamicBuffer<SnakePartBuffer> snakePartBufferList = manager.AddBuffer<SnakePartBuffer>(snakeHead);
        snakePartBufferList.Add(new SnakePartBuffer { savedPosition = spawnPos });

        SnakeEnvironment.Singleton.counterPiece += numberOfPieces;
        for (int u = 0; u < numberOfPieces; u++)
        {
            snakePartBufferList.Add(new SnakePartBuffer { savedPosition = spawnPos });
        }

        //    NativeArray<Entity> pieces = new NativeArray<Entity>(numberOfPieces,Allocator.TempJob);
        //  manager.Instantiate(snakePieceEntity,pieces);
        snakeFirstAndLast[0] = snakeHead;
        Entity lastEntity = snakeHead;
        List<GameObject> body = new List<GameObject>();
        print("Number of piece: " + numberOfPieces);
        for (int i = 0; i < numberOfPieces; i++)
        {
            // if (snake.isDestroyed)
            //    break;
            /*GameObject temp = PoolManager.instance.GetSnake();
            temp.name = "Body_" + snakeIndex + "_" + i;
            body.Add(temp);*/


        Entity pieceEntity = manager.Instantiate(snakePieceEntity);


            manager.SetComponentData(pieceEntity, new Translation
            {
                Value = spawnPos
            });
            manager.AddComponentData(pieceEntity, new NonUniformScale
            {
                Value = new float3(GameConstants.SNAKE_HEAD_SCALE, GameConstants.SNAKE_HEAD_SCALE, GameConstants.SNAKE_HEAD_SCALE)
            });

            manager.SetComponentData(pieceEntity, new PieceData
            {
                snakeId = snakeIndex,
                pieceIndex = i + 1,
                positionToMove = new float3(0, 0, 0),
                teamId = (teamid == -1) ? snakeIndex : teamid
            });
            manager.SetComponentData(pieceEntity, new PieceScaleData
            {
                pieceIndex = i + 1,
                scaleData = 100f
            });
            manager.SetComponentData(pieceEntity, new PieceNodeData
            {
                entityToFollow = lastEntity
            });

            manager.AddSharedComponentData(pieceEntity, new SnakeGroupData
            {
                group = snakeIndex
            });

            glowData = manager.GetComponentData<SnakeGlowData>(pieceEntity);

            //Material colorNew = new Material(sprintMat);
            //colorNew.color = Color.green;
            manager.SetSharedComponentData<RenderMesh>(glowData.glowEntity, new RenderMesh
            {
                mesh = square,
                material = /*snake.sprintMat[0]*/  snake.GetNextSprintColor(i + 1)
            });

            

            ballData = manager.GetComponentData<SnakeBallColorData>(pieceEntity);
            manager.SetSharedComponentData<RenderMesh>(ballData.snakeBallEntity, new RenderMesh
            {
                mesh = quad,
                material = snake.GetNextColor(i + 1) 
            });
            manager.SetComponentData<Translation>(ballData.snakeBallEntity, new Translation
            {
                Value = new float3(0, ((i + 1) * -0.001f), 0)
            });
            lastEntity = pieceEntity;
        }
        manager.SetComponentData(snakeHead, new SnakeLastPartData
        {
            lastPiece = lastEntity
        });

        //  pieces.Dispose();

        snakeFirstAndLast[1] = lastEntity;

        // manager.RemoveComponent<SnakePartBuffer>(snakeHead);
        SnakeEnvironment.Singleton.CreateSnake(snakeIndex, body);
        return snakeFirstAndLast;
    }

    public void DisableImmune(ECSSnake snake, float timer = 2f)
    {
        StartCoroutine(disableImmune(snake, timer));
    }

    private IEnumerator disableImmune(ECSSnake snake, float timer = 2f)
    {
        Color normalColor = Color.white;
        Color alphaColor = new Color(1, 1, 1, 0);
        //snake.sprintMat[0].color = normalColor;
        yield return new WaitForSeconds(timer);
        //snake.sprintMat[0].color = alphaColor;
        SnakeHeadData headData = manager.GetComponentData<SnakeHeadData>(snake.snakeHead);
        headData.isImmune = false;
        manager.SetComponentData<SnakeHeadData>(snake.snakeHead, headData);
    }

    public UnityEngine.Material GetMaterial(Color color)
    {
        foreach (UnityEngine.Material mat in stockMaterials)
        {
            if (mat.color == color && mat != null)
            {
                return mat;
            }
        }

        if (color.a == 1f)
        {
            UnityEngine.Material newMat = new UnityEngine.Material(solidColor)
            {
                color = color
            };
            stockMaterials.Add(newMat);
            return newMat;
        }
        else
        {
            UnityEngine.Material newMat = new UnityEngine.Material(transparentColor)
            {
                color = color
            };
            stockMaterials.Add(newMat);
            return newMat;
        }
    }


    public Vector3 GetSnakeHeadPosition(ECSSnake snake)
    {
        Translation snakeHeadPos = manager.GetComponentData<Translation>(snake.snakeHead);
        Vector3 getHeadPos = new Vector3(snakeHeadPos.Value.x, 0, snakeHeadPos.Value.z);

        return getHeadPos;
    }

    public void UpdateSnakeHeadPoints(ECSSnake snake)
    {
        manager.SetComponentData(snake.snakeHead, new SnakePointsData
        {
            points = snake.points
        });
    }

    public void UpdateSnakeHead(ECSSnake snake)
    {
        if (!manager.HasComponent<SnakeHeadData>(snake.snakeHead))
        {
            return;
        }
        SnakeHeadData headData = manager.GetComponentData<SnakeHeadData>(snake.snakeHead);
        headData.speedMultiplier = snake.speedMultiplier;
        headData.speed = snake.speed;
        headData.speedMultiplier = snake.speedMultiplier;
        headData.headDiff = snake.MOVlerpTime; /*GameConstants.SNAKE_DIFF;*/
        manager.SetComponentData<SnakeHeadData>(snake.snakeHead, headData);
        /* manager.SetComponentData(snake.snakeHead, new SnakeHeadData {
                 snakeId = snake.snakeId,
                 speed = snake.speed,
                 snakeRotationSpeed = snake.rotatingSpeed,
                 speedMultiplier = snake.speedMultiplier,
                 headDiff = snake.MOVlerpTime
         });*/
    }

    public void RemoveSnake(ECSSnake snake)
    {
        SnakeEnvironment.Singleton.PopUpSnake(snake.snakeId, snakes[snake.snakeId].snakePieces);
        if (!snake.defaultMask)
        {
            DestroyImmediate(snake.maskMat, true);
        }
        DestroyImmediate(snake.sprintMat[0], true);
        snakes[snake.snakeId] = null;
        Population.instance.realCount = 0;
        // snakes.Remove(snake);
    }

    public void FixedUpdate()
    {
    }


    public void SpawnDuelPlayer(Vector3 pos)
    {
        Debug.Log("spawning duel player");
        int randomSpawnPoint = UnityEngine.Random.Range(0, playerSpawnPoints.Length);

        // GameManager.instance.chosenPlayerSpawnPoint = playerSpawnPoints[randomSpawnPoint];
        ColorTemplate colortemp = selectedColorTemplate;


        Sprite mask = null;



        CreateNewSnake(250, PlayerPrefs.GetString("PlayerName", "You"), pos, colortemp, PlayerProgress.instance.skinMask.maskSprite, true);
    }

    public void PauseAllSnakes()
    {
        for (int x = 0; x < snakes.Length; x++)
        {
            if (snakes[x] != null)
            {
                PauseSnake(snakes[x]);
            }
        }
    }

    public void PauseSnake(ECSSnake snake)
    {
        snake.isPaused = true;
        SnakeHeadData headData = manager.GetComponentData<SnakeHeadData>(snake.snakeHead);
        headData.isDead = true;
        manager.SetComponentData<SnakeHeadData>(snake.snakeHead, headData);
    }

    public void ActivateSnakesAI_Duel()
    {
        UnPauseAllSnakes();
        /*  for(int i = 0; i < usedSnakes.Count; i++)
          {
              var snakeItem = usedSnakes[i].GetComponent<Snake>();
              snakeItem.isLoaded = true;
          }*/
    }

    public void UnPauseAllSnakes(float time = 1f)
    {
        for (int x = 0; x < snakes.Length; x++)
        {
            if (snakes[x] != null)
            {
                UnPauseSnake(snakes[x], time);
            }
        }
    }

    public void UnPauseSnake(ECSSnake snake, float time = 1f)
    {
        snake.isPaused = false;
        SnakeHeadData headData = manager.GetComponentData<SnakeHeadData>(snake.snakeHead);
        headData.isDead = false;
        headData.isImmune = true;
        manager.SetComponentData<SnakeHeadData>(snake.snakeHead, headData);
        DisableImmune(snake, time);
    }
}
