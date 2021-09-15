using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Collections;

[UpdateAfter(typeof(SnakeScaleSystem))]
public class SnakePieceDestroySystem : JobComponentSystem
{
    Material[] foodMats;
    Entity foodEnt;
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (foodEnt == Entity.Null)
            foodEnt = FoodSpawner.Instance.GetFoodEntity();

        if (foodMats == null)
            foodMats = FoodSpawner.Instance.foodMats;

        foreach (ECSSnake snake in SnakeSpawner.Instance.snakes)
        {
            if (snake != null)
            {
                if (snake.isDestroyed)
                {


                    int snakeLength = snake.GetSnakeParts();
                    int snakePoints = snake.points;
                    Entities
                                        .WithoutBurst()
                                        .WithStructuralChanges()
                                       .WithSharedComponentFilter(new SnakeGroupData { group = snake.snakeId }).
                                         ForEach((Entity entity, ref PieceData pieceData, in Translation position) =>
                                         {

                                             EntityManager.DestroyEntity(entity);


                                          if (!snake.dontSpawnFood)
                                          {

                                                 Entity foodEn = EntityManager.Instantiate(foodEnt);
                                              float3 randomCircle = UnityEngine.Random.insideUnitSphere * 5;
                                              float3 newPosition = position.Value;//new Vector3(positions[i].x, 0, positions[i].z);
                                              newPosition += randomCircle;
                                              int value = UnityEngine.Random.Range(Mathf.RoundToInt(snakePoints / 2 / snakeLength), Mathf.RoundToInt(snakePoints / 2 / snakeLength));
                                              value = Mathf.Clamp(value, 1, 40);
                                          
                                              float scale = (float)value;

                                                 if (value <= 10)
                                                 {
                                                     scale *= 30;
                                                 }


                                              EntityManager.SetComponentData(foodEn, new Translation
                                              {
                                                  Value = new float3(newPosition.x, 0, newPosition.z)
                                              });
                                              RenderMesh foodRender = EntityManager.GetSharedComponentData<RenderMesh>(foodEn);
                                              foodRender.material = foodMats[UnityEngine.Random.Range(0, foodMats.Length)];
                                              EntityManager.SetSharedComponentData(foodEn, foodRender);
                                              //scale *= 2;
                                              EntityManager.AddComponentData(foodEn, new NonUniformScale
                                              {
                                                  Value = new float3(scale, scale, scale)
                                              });
                                              EntityManager.SetComponentData(foodEn, new FoodData
                                              {
                                                  foodValue = value,
                                                  shouldDestroy = false,
                                                  isNewSpawn = false
                                              });
                                          }

                                         }).Run();
                    SnakeSpawner.Instance.RemoveSnake(snake);
                    break;

                }
            }


        }

            return inputDeps;
    }

   
}
