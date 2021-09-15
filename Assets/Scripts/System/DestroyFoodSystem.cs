using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Entities.UniversalDelegates;

public class DestroyFoodSystem : JobComponentSystem
{
   
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        bool isReset = FoodSpawner.Instance.isReset;
        Vector3 screen = FoodSpawner.Instance.duelModeSpawnSize;
        FoodSpawnType newSpawnType = FoodSpawner.Instance.foodSpawnType;
        FoodSpawnType lastFoodSpawnType = FoodSpawner.Instance.lastFoodSpawnType;
        bool isDuelMode = GameManager.instance.IsDuelMode;
        bool completeFoodWipe = false;
        if (isReset)
        {
            if (newSpawnType != lastFoodSpawnType)
            {
                completeFoodWipe = true;
            }

           var jobHandle = Entities
                .ForEach((ref FoodData foodData) =>
                {
                    foodData.isNewSpawn = !completeFoodWipe;
                    foodData.shouldDestroy = true;
                    foodData.absorbed = false;
                    foodData.isAbsorbing = false;
                }).Schedule(inputDeps);
            jobHandle.Complete();
            if (newSpawnType != lastFoodSpawnType)
            {
                FoodSpawner.Instance.isWiped = true;
            }
            FoodSpawner.Instance.isReset = false;

        }

                Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity,ref FoodData foodData) =>
            {
                if (foodData.absorbed && !foodData.isAbsorbing)
                {

                    foodData.isAbsorbing  = true;
                    EntityManager.AddComponentData<FoodAbsorbData>(entity, new FoodAbsorbData
                    {
                        isAbsorbing = true,
                        positionToMove = foodData.positionToMove
                    });
                }
                if (foodData.shouldDestroy)
                {
                    if (foodData.isNewSpawn)
                    {
                        Entity ent= EntityManager.Instantiate(entity);
                        Vector2 rangeVectorRange = UnityEngine.Random.insideUnitCircle * GameConstants.FIELD_SCALE;
                        SnakeEnvironment.Singleton.counterPiece += 1;

                        float offset = 0.05f;
                        float scale = UnityEngine.Random.Range(35, 100);
                        if (GameManager.instance.IsDuelMode)
                        {
                            var width = UnityEngine.Random.Range(-screen.x, screen.x);
                            var height = UnityEngine.Random.Range(-screen.z, screen.z);
                            rangeVectorRange = new Vector2(width,height);
                            scale = UnityEngine.Random.Range(20, 50);
                            offset = 0.05f;
                        }
                        float3 rangeVector = new float3(rangeVectorRange.x, 0, rangeVectorRange.y);

                        

                        EntityManager.SetComponentData<Translation>(ent, new Translation
                        {
                            Value = rangeVector
                        });
                        EntityManager.SetComponentData<NonUniformScale>(ent, new NonUniformScale
                        {
                            Value = new float3(scale,scale,scale)
                        });
                        EntityManager.SetComponentData<FoodData>(ent, new FoodData
                        {
                            foodValue = (int)(offset * scale),
                            shouldDestroy = false,
                            isNewSpawn = true,
                            absorbed = false,
                            isAbsorbing = false
                        });
                        EntityManager.RemoveComponent<FoodAbsorbData>(ent);
                    }
                   
                    EntityManager.DestroyEntity(entity);
                    

                }
                

            }).Run();
       
        return inputDeps;
    }
}
