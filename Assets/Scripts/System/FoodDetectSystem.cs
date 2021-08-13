using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(SnakePieceDestroySystem))]
public class FoodDetectSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
          .WithoutBurst()
          .ForEach((ref Translation position, ref Rotation rotation,ref FoodDetectData data) =>
          {
              Translation headTransform = EntityManager.GetComponentData<Translation>(data.headTargetData);
              Rotation headRotation = EntityManager.GetComponentData<Rotation>(data.headTargetData);
              position.Value = headTransform.Value;
              rotation.Value = headRotation.Value;
              data.currentPos = position.Value;


          }).Run();
         return inputDeps;
    }

    
}
