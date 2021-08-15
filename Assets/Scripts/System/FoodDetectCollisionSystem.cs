using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Collections;

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
                        foodComponent.absorbed = true;
                        foodComponent.positionToMove = pointcomponent.currentPos;
                        foodDataGroup[triggerEntity] = foodComponent;
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
            foodDetectDataGroup = GetComponentDataFromEntity<FoodDetectData>()
        }.Schedule(stepsWorld.Simulation, ref physicsWorld.PhysicsWorld, inputDeps);


        
        jobHandle.Complete();

        

        return inputDeps;
    }

    
}
