using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance;
    public GameObject foodPrefab;
    Entity foodEntity;
    World world;
    EntityManager manager;
    BlobAssetStore blobAssetStore;
    public Material[] foodMats;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        world = World.DefaultGameObjectInjectionWorld;
        manager = world.EntityManager;
        blobAssetStore = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(world, blobAssetStore);// (new BlobAssetStore()));

        foodEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(foodPrefab, settings);
    }
    public Entity GetFoodEntity()
    {
        return foodEntity;
    }
}
