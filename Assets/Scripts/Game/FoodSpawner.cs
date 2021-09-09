using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using Unity.Rendering;

public enum FoodSpawnType
{
    Normal = 0,
    DuelMode = 1
}
public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance;
    public FoodSpawnType foodSpawnType = FoodSpawnType.Normal;
    public FoodSpawnType lastFoodSpawnType = FoodSpawnType.Normal;
    public GameObject foodPrefab;
    public float minFoodSize = 2f;
    public float maxFoodSize = 6f;
    public int foodQuantity = 800;
    Entity foodEntity;
    World world;
    EntityManager manager;
    public bool isReset = false;
    public bool isWiped = true;
    public Material[] foodMats;
    public Mesh foodMesh;
    public Vector3 duelModeSpawnSize;
    BlobAssetStore blobAssetStore;
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        world = World.DefaultGameObjectInjectionWorld;
        manager = world.EntityManager;
        blobAssetStore = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(world, blobAssetStore);// (new BlobAssetStore()));

        foodEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(foodPrefab, settings);
        duelModeSpawnSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Screen.height));
    }
    // Start is called before the first frame update
    void Start()
    {
        //   SpawnFoods();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isWiped)
        {
            isWiped = false;
            if (foodSpawnType == FoodSpawnType.Normal)
            {
                SpawnFoods();
            }
            else
            {
                SpawnDuelFoods();
            }

            lastFoodSpawnType = foodSpawnType;
        }
    }

    public void SwitchToDuelModeAndReset()
    {
        lastFoodSpawnType = foodSpawnType;
        foodSpawnType = FoodSpawnType.DuelMode;
        isReset = true;
    }

    public void SwitchToNormalModeAndReset()
    {
        lastFoodSpawnType = foodSpawnType;
        foodSpawnType = FoodSpawnType.Normal;
        isReset = true;
    }

    private void OnDestroy()
    {
        // Dispose of the BlobAssetStore, else we're get a message:
        // A Native Collection has not been disposed, resulting in a memory leak.
        if (blobAssetStore != null) { blobAssetStore.Dispose(); }
    }

    public Entity GetFoodEntity()
    {
        return foodEntity;
    }

    public void SpawnFoods()
    {
        //NativeArray<Entity> foods = new NativeArray<Entity>(foodQuantity,Allocator.TempJob);


        for (int i = 0; i < foodQuantity; i++)
        {
            Entity food = manager.Instantiate(foodEntity);
            float x = Mathf.Sin(i) * UnityEngine.Random.Range(0, GameConstants.FIELD_SCALE);
            float z = Mathf.Cos(i) * UnityEngine.Random.Range(0, GameConstants.FIELD_SCALE);
            float scale = UnityEngine.Random.Range(minFoodSize, maxFoodSize);
            manager.SetComponentData(food, new Translation
            {
                Value = new float3(x, 0, z)
            });
            manager.SetSharedComponentData(food, new RenderMesh
            {
                mesh = foodMesh,
                material = foodMats[UnityEngine.Random.Range(0, foodMats.Length)]
            });
            manager.AddComponentData(food, new NonUniformScale
            {
                Value = new float3(scale, scale, scale)
            });
            manager.SetComponentData(food, new FoodData
            {
                foodValue = (int)(5 * (scale / GameConstants.FIELD_SCALE)),
                shouldDestroy = false,
                isNewSpawn = true,
                absorbed = false,
                isAbsorbing = false
            });
        }

        // foods.Dispose();


    }

    public void SpawnDuelFoods()
    {
        //NativeArray<Entity> foods = new NativeArray<Entity>(foodQuantity,Allocator.TempJob);

        Vector3 screen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Screen.height));

        // foodTransform.position = new Vector3(width, 0, height);
        for (int i = 0; i < 60; i++)
        {
            Entity food = manager.Instantiate(foodEntity);
            var x = UnityEngine.Random.Range(-screen.x, screen.x);
            var z = UnityEngine.Random.Range(-screen.z, screen.z);
            float scale = UnityEngine.Random.Range(minFoodSize, maxFoodSize);
            manager.SetComponentData(food, new Translation
            {
                Value = new float3(x, 0, z)
            });
            manager.SetSharedComponentData(food, new RenderMesh
            {
                mesh = foodMesh,
                material = foodMats[UnityEngine.Random.Range(0, foodMats.Length)]
            });
            manager.AddComponentData(food, new NonUniformScale
            {
                Value = new float3(scale, scale, scale)
            });
            manager.SetComponentData(food, new FoodData
            {
                foodValue = (int)(0.05  * scale),
                shouldDestroy = false,
                isNewSpawn = true,
                absorbed = false,
                isAbsorbing = false
            });
        }

        // foods.Dispose();


    }

    public void SpawnFoods(Vector3[] positions, int points)
    {
        //  NativeArray<Entity> foods = new NativeArray<Entity>(positions.Length, Allocator.TempJob);


        for (int i = 0; i < positions.Length; i++)
        {
            Entity foodEn = manager.Instantiate(foodEntity);
            Vector3 randomCircle = UnityEngine.Random.insideUnitSphere * 3;
            Vector3 newPosition = positions[i];//new Vector3(positions[i].x, 0, positions[i].z);
            newPosition += randomCircle;
            int value = UnityEngine.Random.Range(Mathf.RoundToInt(points / 2 / positions.Length), Mathf.RoundToInt(points / 3 / positions.Length));
            value = Mathf.Clamp(value, 1, 20);
            float scale = UnityEngine.Random.Range(minFoodSize, maxFoodSize);
            manager.SetComponentData(foodEn, new Translation
            {
                Value = new float3(newPosition.x, 0, newPosition.z)
            });
            manager.SetSharedComponentData(foodEn, new RenderMesh
            {
                mesh = foodMesh,
                material = foodMats[UnityEngine.Random.Range(0, foodMats.Length)]
            });
            manager.AddComponentData(foodEn, new NonUniformScale
            {
                Value = new float3(scale, scale, scale)
            });
            manager.SetComponentData(foodEn, new FoodData
            {
                foodValue = value
            });
        }

        // foods.Dispose();


    }
}
