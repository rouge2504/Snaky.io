using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class MovementHeadSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = 0.04f;//Time.DeltaTime;
        float3 axis = new float3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Entities.WithoutBurst().ForEach((ref Rotation rotation, ref Translation position, ref DynamicBuffer<SnakeBodyBuffer> snakeParts, in SpeedData speedData) =>
        {
            float3 heading = float3.zero;
            if (axis.x == 0 && axis.y == 0)
            {

            }
            else
            {

                heading = new float3(axis.x, 0, axis.z);

            }
            if (math.distance(position.Value, float3.zero) > 350f)
            {
                heading = float3.zero - position.Value;

            }

            if (math.distance(position.Value, float3.zero) > 350f)
            {
                heading = float3.zero - position.Value;

            }

            if (heading.x != 0 || heading.z != 0)
            {
                heading.y = 0;
                Quaternion rot = Quaternion.LookRotation(heading, Vector3.up);
                rotation.Value = math.slerp(rotation.Value, rot, deltaTime * speedData.speedRotation);
            }

            position.Value += deltaTime * speedData.speed * speedData.speedMultiplier * math.forward(rotation.Value);

            SnakeBodyBuffer buffer = snakeParts[0];
            buffer.savedPosition = position.Value;
            snakeParts[0] = buffer;


        }).Run();

        return default;
    }
}
