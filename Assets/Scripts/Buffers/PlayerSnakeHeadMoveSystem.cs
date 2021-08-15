using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class PlayerSnakeHeadMoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = 0.04f;// UnityEngine.Time.deltaTime;
                                //   float trueDelta = Time.DeltaTime;
        float3 axis = new float3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float3 trackerNewPos = float3.zero;

        int id = 0;
        //   Debug.Log("Axis : " + axis);
        Entities
            .WithoutBurst()
            .ForEach((ref Translation position, ref Rotation rotation, ref SnakeHeadData snakeHeadData, ref DynamicBuffer<SnakePartBuffer> snakeParts, ref SnakePointsData pointsData, in PlayerData playerData) =>
            {
                id = snakeHeadData.snakeId;
                if (!snakeHeadData.isDead)
                {
                    //float3 heading = new float3(axis.x, 0, axis.z);
                    float3 heading = float3.zero;
                    if (axis.x == 0 && axis.y == 0)
                    {

                    }
                    else
                    {

                        heading = new float3(axis.x, 0, axis.z);
                        //   Debug.Log("Movement heading : " + heading);

                    }

                    if (!snakeHeadData.isDuelMode)
                    {
                        if (math.distance(position.Value, float3.zero) > GameConstants.FIELD_SCALE)
                        {
                            heading = float3.zero - position.Value;

                        }
                    }
                    else
                    {
                       /* //Vector3 screen = FoodSpawner.Instance.duelModeSpawnSize;
                        if (position.Value.x > screen.x || position.Value.x < (-screen.x) || position.Value.z < screen.z || position.Value.z > (-screen.z))
                        {
                            // Debug.Log("going zero x " + position.Value.x + " " + screen.x + " " + (-screen.x) + " z " + position.Value.z + " " + screen.z + " " + (-screen.z));
                            heading = float3.zero - position.Value;

                        }*/

                    }

                    if (heading.x != 0 || heading.z != 0)
                    {
                        heading.y = 0;
                        quaternion targetDirection = quaternion.LookRotation(heading, math.up());
                        rotation.Value = math.slerp(rotation.Value, targetDirection, deltaTime * snakeHeadData.snakeRotationSpeed);
                    }

                    position.Value += deltaTime * snakeHeadData.speed * snakeHeadData.speedMultiplier * math.forward(rotation.Value);
                    trackerNewPos = position.Value;

                    SnakePartBuffer buffer = snakeParts[0];
                    buffer.savedPosition = position.Value;
                    snakeParts[0] = buffer;

                    /*if (snakeHeadData.speedMultiplier > 1f)
                    {
                        if (pointsData.points > 200)
                            pointsData.points -= (int)(1f);
                    }*/

                }


            }).Run();
        //Debug.Log(id);
        SnakeSpawner.Instance.playerTracker.transform.position = new Vector3(trackerNewPos.x, trackerNewPos.y, trackerNewPos.z);
        return inputDeps;
    }
}
