using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class MovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = 0.04f;//Time.DeltaTime;
        float3 axis = new float3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Entities.WithoutBurst().ForEach((ref PhysicsVelocity vel, ref Rotation rotation, ref Translation position, in SpeedData speedData) =>
        {
            float3 heading = float3.zero;
            float2 newVel = vel.Linear.xz;
            if (axis.x == 0 && axis.y == 0)
            {

            }
            else
            {

                heading = new float3(axis.x, 0, axis.z);
                //   Debug.Log("Movement heading : " + heading);

            }
            if (newVel.x > speedData.limitSpeed)
            {
                newVel.x = speedData.limitSpeed;
            }else if (newVel.x < (speedData.limitSpeed * -1) )
            {
                newVel.x = (speedData.limitSpeed * -1);
            }

            if (newVel.y > speedData.limitSpeed)
            {
                newVel.y = speedData.limitSpeed;
            }
            else if (newVel.y < (speedData.limitSpeed * -1))
            {
                newVel.y = (speedData.limitSpeed * -1);
            }
            if (math.distance(position.Value, float3.zero) > 350f)
            {
                heading = float3.zero - position.Value;

            }
            /*if (heading.x != 0 || heading.z != 0)
            {
                heading.y = 0;
                quaternion targetDirection = quaternion.LookRotation(heading, math.up());
                rotation.Value = math.slerp(rotation.Value, targetDirection, deltaTime * 0.2f);
            }*/
            //position.Value += deltaTime * 0.02f * 0.2f  * math.forward(rotation.Value);
            //newVel += axis.xz * speedData.speed * deltaTime;
            //vel.Linear.xz = newVel;
            if (math.distance(position.Value, float3.zero) > 350f)
            {
                heading = float3.zero - position.Value;

            }
            /*if (position.Value.x != 0)
            {

                float angle =Mathf.Rad2Deg * Mathf.Atan(position.Value.z / position.Value.x);

                Debugger.instance.Log(angle);
                float3 rot = new float3(0, angle, 0);
                Debugger.instance.Log(rot);
                rotation.Value = quaternion.Euler(rot);
            }*/
            if (heading.x != 0 || heading.z != 0)
            {
                heading.y = 0;
                Quaternion rot = Quaternion.LookRotation(heading, Vector3.up);
                rotation.Value = math.slerp(rotation.Value, rot, deltaTime * speedData.speedRotation);
            }

            position.Value += deltaTime * speedData.speed * speedData.speedMultiplier * math.forward(rotation.Value);

        }).Run();

        return default;
    }
}
