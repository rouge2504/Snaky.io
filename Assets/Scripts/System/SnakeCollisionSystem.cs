using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Collections;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class SnakeCollisionSystem : JobComponentSystem
{
    BuildPhysicsWorld physicsWorld;
    StepPhysicsWorld stepsWorld;

    protected override void OnCreate()
    {
        physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

   
    struct pieceTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PieceData> pieceDataGroup;
        public ComponentDataFromEntity<SnakeHeadData> snakeHeadDataGroup;
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

         

            bool isBodyTrigger2A = pieceDataGroup.HasComponent(entityA);
            bool isBodyTrigger2B = pieceDataGroup.HasComponent(entityB);
            if (isBodyTrigger2A || isBodyTrigger2B)
            {
                bool isBodyDynamic2A = snakeHeadDataGroup.HasComponent(entityA);
                bool isBodyDynamic2B = snakeHeadDataGroup.HasComponent(entityB);

                if (isBodyDynamic2A || isBodyDynamic2B)
                {


                     var triggerEntity = isBodyTrigger2A ? entityA : entityB;
                     var dynamicEntity = isBodyTrigger2A ? entityB : entityA;


                            var component = snakeHeadDataGroup[dynamicEntity];
                            var piececomponent = pieceDataGroup[triggerEntity];

                            if ((piececomponent.snakeId != component.snakeId)&&
                                (piececomponent.teamId!=component.teamId))
                            {
                                if (!component.isDead)
                                {
                                    if (!component.isImmune)
                                    {
                                       
                                        if (!component.shouldDestroy)
                                        {
                                            component.isDead = true;
                                            component.shouldDestroy = true;// foodComponent.foodValue;
                                            snakeHeadDataGroup[dynamicEntity] = component;
                                    Debug.Log("muerto");
                                }
                            }
                                }
                            }
                     
                    

                }
            }
        }
    }



    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        var pjobHandle = new pieceTriggerJob
        {
            pieceDataGroup = GetComponentDataFromEntity<PieceData>(),
            snakeHeadDataGroup = GetComponentDataFromEntity<SnakeHeadData>()
        }.Schedule(stepsWorld.Simulation, ref physicsWorld.PhysicsWorld, inputDeps);



        pjobHandle.Complete();

    

        return inputDeps;
    }

    
}
