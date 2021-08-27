using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using SphereCollider = Unity.Physics.SphereCollider;

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
                    float points = snake.points;
                    var jobhandle = Entities.
                                        WithSharedComponentFilter(new SnakeGroupData { group = snake.snakeId }).
                                          ForEach((ref PieceScaleData data, ref PhysicsCollider collider) =>
                                          {
                                              data.scaleData = nscale;
                                              //data.scaleData = 10000;

                                              /*unsafe
                                              {
                                                  float offset = 0.8f;
                                                  if (points > 40000)
                                                  {
                                                      offset = 1.9f;
                                                  }
                                                  float oldRadius = 1.0f;
                                                  float newRadius = 10.0f;
                                                  SphereCollider* scPtr = (SphereCollider*)collider.ColliderPtr;
                                                  oldRadius = scPtr->Radius;
                                                  newRadius = math.lerp(oldRadius, nscale * offset, 0.05f);
                                                  var sphereGeometry = scPtr->Geometry;
                                                  sphereGeometry.Radius = nscale * offset;
                                                  scPtr->Geometry = sphereGeometry;
                                              }*/
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
