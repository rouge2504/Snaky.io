using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(SnakeScaleSystem))]
public class SnakeSinSystem : JobComponentSystem
{
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      //  float deltaTime = Time.DeltaTime;
        float elapsedTime = -((float)Time.ElapsedTime);
        float frequency = 0.6f;
        float speed = 16f;
        float amplitude =  0.06f;
       var jobHandle = Entities
            .ForEach((ref NonUniformScale scale, in PieceScaleData data) =>
            {
               
                float scaleVal = amplitude * math.sin(((elapsedTime*speed)+ data.pieceIndex)*frequency);

                float tempScale = GameConstants.SNAKE_HEAD_SCALE + ((data.scaleData + scaleVal)); 
                scale.Value = (new float3(tempScale, 1f, tempScale));


            }).Schedule(inputDeps);
       
        return jobHandle;
    }
}
