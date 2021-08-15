using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Mathematics;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class SnakeFoodCollisionSystem : JobComponentSystem
{
    BuildPhysicsWorld physicsWorld;
    StepPhysicsWorld stepsWorld;

    protected override void OnCreate()
    {
        physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    struct foodTriggerJob : ITriggerEventsJob
    {
       
        public ComponentDataFromEntity<FoodData> foodDataGroup;
        public ComponentDataFromEntity<SnakePointsData> snakePointDataGroup;
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isBodyTriggerA = foodDataGroup.HasComponent(entityA);
            bool isBodyTriggerB = foodDataGroup.HasComponent(entityB);

            if (isBodyTriggerA || isBodyTriggerB)
            {

                bool isBodyDynamicA = snakePointDataGroup.HasComponent(entityA);
                bool isBodyDynamicB = snakePointDataGroup.HasComponent(entityB);

                if (isBodyDynamicA || isBodyDynamicB)
                {


                    var triggerEntity = isBodyTriggerA ? entityA : entityB;
                    var dynamicEntity = isBodyTriggerA ? entityB : entityA;

                    var foodComponent = foodDataGroup[triggerEntity];
                    var pointcomponent = snakePointDataGroup[dynamicEntity];

                    if (!foodComponent.shouldDestroy)
                    {
                        foodComponent.shouldDestroy = true;
                        pointcomponent.points += foodComponent.foodValue;
                        snakePointDataGroup[dynamicEntity] = pointcomponent;
                        foodDataGroup[triggerEntity] = foodComponent;
                    }
                }
            }
           
            
          
        }
    }




    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

      

        var jobHandle = new foodTriggerJob
        {
            foodDataGroup = GetComponentDataFromEntity<FoodData>(),
            snakePointDataGroup = GetComponentDataFromEntity<SnakePointsData>()
        }.Schedule(stepsWorld.Simulation, ref physicsWorld.PhysicsWorld, inputDeps);


        
        jobHandle.Complete();

        

        return inputDeps;
    }

    
}
