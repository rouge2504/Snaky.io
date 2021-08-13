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
    public SpawnPoint[] aiSpawnPoints;
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

        
        snakes = new ECSSnake[80];


        //StartCoroutine(Create());
        //CreateNewSnake(100, "PlayerName", playerSpawnPoints[0].position, colorTemp, null, true, "");

    }

    IEnumerator Create()
    {
        yield return new WaitForSeconds(1);
        ColorTemplate colorTemp = selectedColorTemplate;
        CreateNewSnake(100, "PlayerName", playerSpawnPoints[0].position, colorTemp, null, true, "");

    }

    public void CreateNewSnake(int snakeSize, string name, Vector3 position, ColorTemplate colortemp, Sprite maskSelected = null, bool isPlayer = false, string team = "", bool isBabySnake = false)
    {
        int snakeNum = GetEmptySnakeArrayPos();
        ECSSnake newSnake = new ECSSnake(snakeNum, name, snakeSize, position, colortemp, maskSelected, isPlayer, team, isBabySnake);
        //  numOfSnake++;
        Debug.Log("Snake num index chosen : " + snakeNum);
        snakes[snakeNum] = newSnake;

        if (isPlayer)
        {
            playerSnake = newSnake;
            camerManager.playerSnake = playerSnake;
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
        float x = Mathf.Sin(snakeIndex) * UnityEngine.Random.Range(0, 1000);
        float z = Mathf.Cos(snakeIndex) * UnityEngine.Random.Range(0, 1000);
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
        Debug.Log("Team id : " + teamid);

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
            speed = 25,
            snakeRotationSpeed = 8,
            speedMultiplier = 1,
            headDiff = isPlayer ? GameConstants.SNAKE_DIFF : 0.25f,
            shouldDestroy = false,
            isDead = false,
            isImmune = true,
            teamId = (teamid == -1) ? snakeIndex : teamid,
            //isDuelMode = GameManager.instance.IsDuelMode(),
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
            material = snake.sprintMat
        });

        SnakeBallColorData ballData = manager.GetComponentData<SnakeBallColorData>(snakeHead);
        manager.SetSharedComponentData<RenderMesh>(ballData.snakeBallEntity, new RenderMesh
        {
            mesh = quad,
            material = snake.GetNextColor()
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

        for (int u = 0; u < numberOfPieces; u++)
        {
            snakePartBufferList.Add(new SnakePartBuffer { savedPosition = spawnPos });
        }

        //    NativeArray<Entity> pieces = new NativeArray<Entity>(numberOfPieces,Allocator.TempJob);
        //  manager.Instantiate(snakePieceEntity,pieces);
        snakeFirstAndLast[0] = snakeHead;
        Entity lastEntity = snakeHead;
        for (int i = 0; i < numberOfPieces; i++)
        {
            // if (snake.isDestroyed)
            //    break;

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
            manager.SetSharedComponentData<RenderMesh>(glowData.glowEntity, new RenderMesh
            {
                mesh = square,
                material = snake.sprintMat
            });

            ballData = manager.GetComponentData<SnakeBallColorData>(pieceEntity);
            manager.SetSharedComponentData<RenderMesh>(ballData.snakeBallEntity, new RenderMesh
            {
                mesh = quad,
                material = snake.GetNextColor()
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
        snake.sprintMat.color = normalColor;
        yield return new WaitForSeconds(timer);
        snake.sprintMat.color = alphaColor;
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
        SnakeHeadData headData = manager.GetComponentData<SnakeHeadData>(snake.snakeHead);
        headData.speedMultiplier = snake.speedMultiplier;
        headData.speed = snake.speed;
        headData.speedMultiplier = snake.speedMultiplier;
        headData.headDiff = snake.MOVlerpTime;
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
        if (!snake.defaultMask)
        {
            DestroyImmediate(snake.maskMat, true);
        }

        DestroyImmediate(snake.sprintMat, true);
        snakes[snake.snakeId] = null;

        // snakes.Remove(snake);
    }
}
