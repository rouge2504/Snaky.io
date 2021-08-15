using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateBefore(typeof(DestroyFoodSystem))]
public class FoodAbsorbingSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deltaTime = Time.DeltaTime;
        float speed = 5f;
        var jobHandle = Entities
             .ForEach((ref Translation postion, ref FoodAbsorbData absorbData,ref FoodData foodData) =>
             {
                 if (absorbData.isAbsorbing)
                 {
                     if (math.distance(postion.Value, absorbData.positionToMove) > 0.5f)
                     {
                         float3 diff = absorbData.positionToMove - postion.Value;
                         postion.Value += deltaTime * speed * diff;


                     }
                     else
                     {
                         absorbData.isAbsorbing = false;
                         foodData.absorbed = false;
                         foodData.isAbsorbing = false;
                     }
                 }

             }).Schedule(inputDeps);
        jobHandle.Complete();
        return jobHandle;
    }

   
}
