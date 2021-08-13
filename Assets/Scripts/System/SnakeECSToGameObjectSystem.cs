using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;


[UpdateBefore(typeof(AIDetectSystem))]
[UpdateAfter(typeof(SnakeHeadMoveSystem))]
public class SnakeECSToGameObjectSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity,ref SnakeHeadData headData,in SnakeHeadTargetData targetData) =>
            {

                if (headData.shouldDestroy&&headData.isDead)
                {
                       EntityManager.DestroyEntity(targetData.ai);
                       EntityManager.RemoveComponent<SnakeHeadTargetData>(entity);
                       EntityManager.RemoveComponent<PhysicsCollider>(entity);
                       EntityManager.RemoveComponent<PhysicsVelocity>(entity);
                       EntityManager.RemoveComponent<PhysicsMass>(entity);
                  //     EntityManager.RemoveComponent<SnakePartBuffer>(entity);
                    //  DynamicBuffer<SnakePartBuffer> buffer = EntityManager.GetBuffer<SnakePartBuffer>(entity);
                    //  buffer.
                    /*      AIData ai = EntityManager.GetComponentData<AIData>(targetData.ai);
                             ai.isDead = true;
                             EntityManager.SetComponentData<AIData>(targetData.ai,ai);*/

                    headData.shouldDestroy = false;
                    //headData.isDead = true;
                    SnakeSpawner.Instance.snakes[headData.snakeId].isDestroyed = true;

                    //   ECSSnake snake = SnakeSpawner.Instance.snakes[headData.snakeId];
                    //   SnakeSpawner.Instance.DestroySnake(snake);
                }


            }).Run();

        Entities
           .WithoutBurst()
           .WithStructuralChanges()
           .ForEach((Entity entity, ref SnakeHeadData headData,in PlayerData player) =>
           {

               if (headData.shouldDestroy && headData.isDead)
               {
                       headData.shouldDestroy = false;
                       SnakeSpawner.Instance.snakes[headData.snakeId].sprinting = false;
                      SnakeSpawner.Instance.snakes[headData.snakeId].isDuelModeDestroyed = true;



               }


           }).Run();

        return inputDeps;
    }
}
