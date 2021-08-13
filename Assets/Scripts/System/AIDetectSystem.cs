using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using System;

[UpdateAfter(typeof(SnakePieceDestroySystem))]
public class AIDetectSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithoutBurst()
            .ForEach((ref Translation position, ref Rotation rotation, ref AIData data) =>
            {
                if (!data.isDead)
                {
                    Translation headTransform = EntityManager.GetComponentData<Translation>(data.headTargetData);
                    Rotation headRotation = EntityManager.GetComponentData<Rotation>(data.headTargetData);
                    position.Value = headTransform.Value;
                    rotation.Value = headRotation.Value;
                    SnakeHeadTargetData headTargetDirection = EntityManager.GetComponentData<SnakeHeadTargetData>(data.headTargetData);

                    if (data.isEscape || data.isChase)
                    {

                        if (data.enemyEntity != Entity.Null)
                        {
                            try
                            {
                                Translation enemyEntityTransform = EntityManager.GetComponentData<Translation>(data.enemyEntity);
                                float3 moveDirection = headTransform.Value - enemyEntityTransform.Value;
                                if (data.isChase)
                                    moveDirection = enemyEntityTransform.Value - headTransform.Value;

                                moveDirection.y = 0;
                                headTargetDirection.moveDirection = moveDirection;
                                headTargetDirection.isAIMoveDirection = true;
                            }
                            catch (Exception e)
                            {
                                headTargetDirection.isAIMoveDirection = false;
                            }
                            //  if(data.isEscape)
                           


                            headTargetDirection.isReachedPosition = false;
                            headTargetDirection.foodTarget = float3.zero;
                            EntityManager.SetComponentData<SnakeHeadTargetData>(data.headTargetData, headTargetDirection);
                            data.enemyEntity = Entity.Null;
                            data.entityToChase = Entity.Null;
                            data.isEscape = false;
                            data.isChase = false;
                            //  }
                            //    catch (Exception e)
                            //   {
                            //       data.isEscape = false;
                            //        data.isChase = false;
                            //  }
                        }

                    }
                    else
                    {

                        if (data.entityToChase != Entity.Null)
                        {

                            try
                            {
                                Translation entityToChaseTransform = EntityManager.GetComponentData<Translation>(data.entityToChase);


                                headTargetDirection.foodTarget = entityToChaseTransform.Value;
                                headTargetDirection.isAIMoveDirection = false;

                                headTargetDirection.isReachedPosition = false;
                                EntityManager.SetComponentData<SnakeHeadTargetData>(data.headTargetData, headTargetDirection);
                                data.entityToChase = Entity.Null;
                                data.enemyEntity = Entity.Null;
                            }
                            catch (Exception e)
                            {
                                data.isReachedFood = true;
                            }

                        }
                        else
                        {

                            if (headTargetDirection.foodTarget.x == 0
                            || headTargetDirection.foodTarget.z == 0)
                                data.isReachedFood = true;
                            else
                                data.isReachedFood = headTargetDirection.isReachedPosition;
                            //   if (headTargetDirection.isReachedPosition)



                        }

                    }

                }
            }).Run();
        return inputDeps;
    }
}
