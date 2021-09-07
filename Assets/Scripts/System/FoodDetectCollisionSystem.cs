using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class FoodDetectCollisionSystem : JobComponentSystem
{
    BuildPhysicsWorld physicsWorld;
    StepPhysicsWorld stepsWorld;

    protected override void OnCreate()
    {
        physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    struct foodDetectTriggerJob : ITriggerEventsJob
    {
       
        public ComponentDataFromEntity<FoodData> foodDataGroup;
        [ReadOnly] public ComponentDataFromEntity<FoodDetectData> foodDetectDataGroup;
        public ComponentDataFromEntity<NonUniformScale> foodScaleDataGroup;
        [ReadOnly] public ComponentDataFromEntity<Translation> foodTranslateDataGroup;
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isBodyTriggerA = foodDataGroup.HasComponent(entityA);
            bool isBodyTriggerB = foodDataGroup.HasComponent(entityB);

            if (isBodyTriggerA || isBodyTriggerB)
            {

                bool isBodyDynamicA = foodDetectDataGroup.HasComponent(entityA);
                bool isBodyDynamicB = foodDetectDataGroup.HasComponent(entityB);

                if (isBodyDynamicA || isBodyDynamicB)
                {


                    var triggerEntity = isBodyTriggerA ? entityA : entityB;
                    var dynamicEntity = isBodyTriggerA ? entityB : entityA;

                    var foodComponent = foodDataGroup[triggerEntity];
                    var pointcomponent = foodDetectDataGroup[dynamicEntity];

                    if (!foodComponent.absorbed)
                    {
                        /*foodComponent.absorbed = true;
                        foodComponent.positionToMove = pointcomponent.currentPos;
                        foodDataGroup[triggerEntity] = foodComponent;*/

                        var foodScaleComponent = foodScaleDataGroup[triggerEntity];
                        var foodScaleHeadComponent = foodScaleDataGroup [pointcomponent.headTargetData];


                        var foodTransformComponent = foodTranslateDataGroup[triggerEntity];
                        var foodHeadTransformComponent = foodTranslateDataGroup[dynamicEntity];

                        float distPiece = foodScaleComponent.Value.x / 2;
                        float distHead = foodScaleHeadComponent.Value.x / 2;

                        float allDist = distHead + distPiece;

                        float distVector = Vector3.Distance(foodTransformComponent.Value, foodHeadTransformComponent.Value);
                        if (distVector < allDist)
                        {
                            foodComponent.absorbed = true;
                            foodComponent.positionToMove = pointcomponent.currentPos;
                            foodDataGroup[triggerEntity] = foodComponent;
                        }
                    }
                }
            }
           
            
          
        }
    }




    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

      

        var jobHandle = new foodDetectTriggerJob
        {
            foodDataGroup = GetComponentDataFromEntity<FoodData>(),
            foodDetectDataGroup = GetComponentDataFromEntity<FoodDetectData>(),
            foodScaleDataGroup = GetComponentDataFromEntity<NonUniformScale>(),
            foodTranslateDataGroup = GetComponentDataFromEntity<Translation>(),
        }.Schedule(stepsWorld.Simulation, ref physicsWorld.PhysicsWorld, inputDeps);


        
        jobHandle.Complete();

        

        return inputDeps;
    }

    
}
