using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Transforms;

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
        public ComponentDataFromEntity<NonUniformScale> foodScaleDataGroup;
        public ComponentDataFromEntity<Translation> foodTranslateDataGroup;
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


                        var foodScaleComponent = foodScaleDataGroup[triggerEntity];
                        var foodScaleHeadComponent = foodScaleDataGroup[dynamicEntity];


                        var foodTransformComponent = foodTranslateDataGroup[triggerEntity];
                        var foodHeadTransformComponent = foodTranslateDataGroup[dynamicEntity];

                        float distPiece = foodScaleComponent.Value.x / 12;
                        float distHead = foodScaleHeadComponent.Value.x / 2;

                        float allDist = distHead + distPiece;

                        float distVector = Vector3.Distance(foodTransformComponent.Value, foodHeadTransformComponent.Value);
                        if (distVector < allDist)//if (distVector < allDist)
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
    }




    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

      

        var jobHandle = new foodTriggerJob
        {
            foodDataGroup = GetComponentDataFromEntity<FoodData>(),
            snakePointDataGroup = GetComponentDataFromEntity<SnakePointsData>(),
            foodScaleDataGroup = GetComponentDataFromEntity<NonUniformScale>(),
            foodTranslateDataGroup = GetComponentDataFromEntity<Translation>(),
        }.Schedule(stepsWorld.Simulation, ref physicsWorld.PhysicsWorld, inputDeps);


        
        jobHandle.Complete();

        

        return inputDeps;
    }

    
}
