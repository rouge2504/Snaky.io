using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Analytics;

[UpdateAfter(typeof(SnakePartMoveSystem))]
public class SnakeHeadMoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = 0.038f;// Time.DeltaTime;
                                 //   float rotationSpeed = 3f;
                                 //   float speed = 20f;
        int id = 0;
        Entities
            .WithoutBurst()
        .ForEach((ref Translation position,ref Rotation rotation,ref SnakeHeadData snakeHeadData, ref DynamicBuffer<SnakePartBuffer> snakeParts, ref SnakeHeadTargetData targetData) =>{
            id = snakeHeadData.snakeId;
            //SnakeEnvironment.Singleton.CheckHead(id, position.Value);
            if (!snakeHeadData.isBabySnake)
            {
                if (!snakeHeadData.isDead)
                {
                //    if (float.IsNaN(targetData.foodTarget.x))
                 //       Debug.LogError("IsNaN Found in food!");

                    float3 heading = targetData.foodTarget - position.Value;
                    heading.y = 0;
                    if (targetData.isAIMoveDirection)
                    {
                   //     if (float.IsNaN(targetData.moveDirection.x))
                      //      Debug.LogError("IsNaN Found in direction!");

                        heading = targetData.moveDirection;

                    }

                    quaternion targetDirection = quaternion.LookRotation(heading, math.up());


                    if (!targetData.isReachedPosition)
                    {
                        quaternion curRot = math.slerp(rotation.Value, targetDirection, deltaTime * snakeHeadData.snakeRotationSpeed);
                        if (float.IsNaN(curRot.value.x))
                        {
                            //Debug.LogError("IsNaN Found!");
                        }
                        else
                            rotation.Value = curRot;

                        //math.slerp(rotation.Value, targetDirection, deltaTime * snakeHeadData.snakeRotationSpeed);
                    }



                    position.Value += deltaTime * snakeHeadData.speed * snakeHeadData.speedMultiplier * math.forward(rotation.Value);

                    SnakePartBuffer buffer = snakeParts[0];
                    buffer.savedPosition = position.Value;
                    snakeParts[0] = buffer;

                    /*   if (targetData.directionToMove.x != targetData.newDirectionToMove.x
                       || targetData.directionToMove.z != targetData.newDirectionToMove.z)
                       {
                           targetData.directionToMove = targetData.newDirectionToMove;
                       }*/
                    if (!targetData.isAIMoveDirection)
                    {
                        if (math.distance(position.Value, targetData.foodTarget) < 3)
                        {
                            targetData.isReachedPosition = true;

                        }
                    }
                    if (!snakeHeadData.isDuelMode)
                    {
                        if (math.distance(position.Value, float3.zero) > GameConstants.FIELD_SCALE)
                        {
                            targetData.foodTarget = float3.zero;
                            targetData.isReachedPosition = false;
                            targetData.isAIMoveDirection = false;
                        }
                    }
                    else
                    {
                        Vector3 screen = FoodSpawner.Instance.duelModeSpawnSize;
                        if (position.Value.x > screen.x || position.Value.x < (-screen.x) || position.Value.z < screen.z || position.Value.z > (-screen.z))
                        {
                            targetData.foodTarget = float3.zero;
                            targetData.isReachedPosition = false;
                            targetData.isAIMoveDirection = false;
                        }
                    }



                }
            }

        }).Run();
        return inputDeps;
    }
}
