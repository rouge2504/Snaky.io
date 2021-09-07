using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using SphereCollider = Unity.Physics.SphereCollider;

[UpdateAfter(typeof(SnakeResizeSystem))]
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
                if (/*snake.scaleChanged && */!snake.isDestroyed)
                {
                    float nscale = snake.GetSnakeScale();
                    float points = snake.points;
                    int playerId = SnakeSpawner.Instance.playerID;
                    int snakeId = snake.snakeId;
                    var jobhandle = Entities.
                                        WithSharedComponentFilter(new SnakeGroupData { group = snake.snakeId }).
                                          ForEach((ref PieceScaleData data, ref PhysicsCollider collider, ref NonUniformScale scale) =>
                                          {
                                              data.scaleData = nscale;
                                              //data.scaleData = 10000;

                                              unsafe
                                              {
                                                  float offset = 1.8f;
                                                  /*if (points > 5000)
                                                  {
                                                      offset = 0.6f;
                                                  }
                                                  else if (points > 15000 && points < 27000)
                                                  {
                                                      offset = 0.5f;
                                                  }
                                                  else if (points > 27000 && points < 100000){
                                                      offset = 0.4f;
                                                  }*/
                                                  /*if (playerId == snakeId)
                                                  {
                                                      offset = 9.7f;
                                                  }*/
                                                  float oldRadius = 1.0f;
                                                  float newRadius = 10.0f;
                                                  SphereCollider* scPtr = (SphereCollider*)collider.ColliderPtr;
                                                  oldRadius = scPtr->Radius;
                                                  
                                                  newRadius = math.lerp(oldRadius, nscale * offset, 0.05f);
                                                  var sphereGeometry = scPtr->Geometry;
                                                  sphereGeometry.Radius = /*nscale * offset*/ (scale.Value.x / 2) * offset;
                                                  
                                                  scPtr->Geometry = sphereGeometry;
                                              }
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
