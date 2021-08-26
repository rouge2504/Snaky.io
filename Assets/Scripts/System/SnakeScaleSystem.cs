using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

//[UpdateAfter(typeof(SnakeResizeSystem))]
public class SnakeScaleSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        //    NativeList<JobHandle> dependencies
        //    = new NativeList<JobHandle>();
        foreach (ECSSnake snake in SnakeSpawner.Instance.snakes)
        {
            if (snake != null)
            {
                if (snake.scaleChanged && !snake.isDestroyed)
                {
                    float nscale = snake.GetSnakeScale();
                    var jobhandle = Entities.
                                        WithSharedComponentFilter(new SnakeGroupData { group = snake.snakeId }).
                                          ForEach((ref PieceScaleData data) =>
                                          {
                                              data.scaleData = nscale;
                                              //data.scaleData = 10000;
                                              

                                              //scale.Value = new float3(nscale, 1f, nscale);
                                          }).Schedule(inputDeps);
                    jobhandle.Complete();
                    snake.scaleChanged = false;
                }
            }
            //  dependencies.Add(inputDeps);
        }
        return default;// JobHandle.CombineDependencies(dependencies); ;
    }

   
}
