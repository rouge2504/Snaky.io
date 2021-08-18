using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Collections;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class SnakeAICollisionSystem : JobComponentSystem
{
    BuildPhysicsWorld physicsWorld;
    StepPhysicsWorld stepsWorld;

    protected override void OnCreate()
    {
        physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }


    struct aiTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PieceData> pieceDataGroup;
        [ReadOnly] public ComponentDataFromEntity<FoodData> foodDataGroup;
        public ComponentDataFromEntity<AIData> aiDataGroup;
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;
           
            bool isBodyTrigger2A = pieceDataGroup.HasComponent(entityA);
            bool isBodyTrigger2B = pieceDataGroup.HasComponent(entityB);

            bool isBodyDynamicA = aiDataGroup.HasComponent(entityA);
            bool isBodyDynamicB = aiDataGroup.HasComponent(entityB);

            if ((isBodyTrigger2A || isBodyTrigger2B)&& (isBodyDynamicA|| isBodyDynamicB))
            {
                //    if (isBodyTrigger2A && !isBodyDynamicB ||
                //   isBodyTrigger2B && !isBodyDynamicA) return;
                var trigger2Entity = isBodyTrigger2A ? entityA : entityB;
                var dynamic2Entity = isBodyTrigger2A ? entityB : entityA;

                //Debug.Log(trigger2Entity);

                var pieceComponent = pieceDataGroup[trigger2Entity];
                var component2 = aiDataGroup[dynamic2Entity];
                
                    if ((pieceComponent.snakeId != component2.snakeId))
                    {
                        if (!component2.isDead)
                        {
                            component2.enemyEntity = trigger2Entity;
                            component2.counter++;
                        //    component2.isChase = false;
                        //    component2.isEscape = false;
                        if (component2.isDuelMode||component2.isBabySnake)
                        {
                            
                            if (component2.isDuelMode)
                            {
                                if (component2.counter % 2 == 0)
                                {
                                    component2.isEscape = true;
                                    component2.isChase = false;
                                }
                            }
                            else
                            {
                                component2.isEscape = true;
                                component2.isChase = false;
                            }

                        }
                        else
                        {
                            if (component2.counter % 8 == 6)
                            {
                                component2.isChase = true;
                                component2.isEscape = false;
                            }
                            else if (component2.counter % 2 == 0)
                            {
                                component2.isEscape = true;
                                component2.isChase = false;
                            }
                        }

                            if (component2.counter > 100)
                                component2.counter = 0;

                            aiDataGroup[dynamic2Entity] = component2;
                        }
                    }
                
            }
            else if ((isBodyDynamicA || isBodyDynamicB))
            {
                bool isBodyTriggerA = foodDataGroup.HasComponent(entityA);
                bool isBodyTriggerB = foodDataGroup.HasComponent(entityB);

                if (isBodyTriggerA || isBodyTriggerB)
                {
                    //   if (isBodyTriggerA && !isBodyDynamicB ||
                    //   isBodyTriggerB && !isBodyDynamicA) return;
                    var triggerEntity = isBodyTriggerA ? entityA : entityB;
                    var dynamicEntity = isBodyTriggerA ? entityB : entityA;


                    var component = aiDataGroup[dynamicEntity];
                    //    component.isEscape = true;
                    if (!component.isDead)
                    {
                        if (component.isEscape || component.isChase)
                        {
                        }
                        else
                        {
                            if (component.isReachedFood)
                            {
                                component.isReachedFood = false;
                                component.entityToChase = triggerEntity;


                                aiDataGroup[dynamicEntity] = component;
                            }
                        }
                    }
                }
            }

           // if (isBodyTriggerA && isBodyTriggerB) return;

         //   bool isBodyDynamicA = aiDataGroup.Exists(entityA);
        //    bool isBodyDynamicB = aiDataGroup.Exists(entityB);

         //   if (isBodyTriggerA && !isBodyDynamicB ||
          //      isBodyTriggerB && !isBodyDynamicA) return;

       /*     var triggerEntity = isBodyTriggerA ? entityA : entityB;
            var dynamicEntity = isBodyTriggerA ? entityB : entityA;


            var component = aiDataGroup[dynamicEntity];
            //    component.isEscape = true;
            if (!component.isEscape)
            {
                if (component.isReachedFood)
                {
                    component.isReachedFood = false;
                    component.entityToChase = triggerEntity;


                    aiDataGroup[dynamicEntity] = component;
                }
            }*/


        }
    }

    float timeToExecute = 0;
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        if (timeToExecute <= 0)
        {

            var sjobHandle = new aiTriggerJob
            {
                pieceDataGroup = GetComponentDataFromEntity<PieceData>(),
                foodDataGroup = GetComponentDataFromEntity<FoodData>(),
                aiDataGroup = GetComponentDataFromEntity<AIData>()
            }.Schedule(stepsWorld.Simulation, ref physicsWorld.PhysicsWorld, inputDeps);
            sjobHandle.Complete();

            timeToExecute = 0.2f;
        }

        timeToExecute -= Time.DeltaTime;

        

        return inputDeps;
    }

    
}
