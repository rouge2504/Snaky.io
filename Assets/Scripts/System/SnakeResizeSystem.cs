using Unity.Jobs;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Rendering;
using Unity.Physics;
using SphereCollider = Unity.Physics.SphereCollider;

[UpdateAfter(typeof(SnakePartMoveSystem))]
public class SnakeResizeSystem : JobComponentSystem
{


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

       //   NativeList<JobHandle> dependencies
        //     = new NativeList<JobHandle>();
        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((ref DynamicBuffer<SnakePartBuffer> snakeParts, ref SnakeHeadPartsData snakeHeadParts,ref SnakeLastPartData lastPart,in SnakeHeadData headData,in SnakePointsData snakePoints) =>
            {
                if (!headData.isDead)
                {


                    int newParts = 0;// snakeHeadParts.snakeParts;
                    /*if (snakePoints.points > 30000)
                    {
                        //newParts += (int)math.round(650 / (19));
                        //newParts += (int)math.round((1500-650) / (22));
                        //newParts += (int)math.round((5000 - 1500) / (60));
                        //newParts += (int)math.round((10000 - 5000) / (50));
                        //newParts += (int)math.round((snakePoints.points - 10000) / (100));
                        newParts += (int)math.round(snakePoints.points / (200));
                    }
                    else if (snakePoints.points > 10000 && snakePoints.points <= 30000)
                    {
                        //newParts += (int)math.round(650 / (19));
                        //newParts += (int)math.round((1500-650) / (22));
                        //newParts += (int)math.round((5000 - 1500) / (60));
                        //newParts += (int)math.round((10000 - 5000) / (50));
                        //newParts += (int)math.round((snakePoints.points - 10000) / (100));
                        newParts += (int)math.round(snakePoints.points / (800));
                    }
                    else if (snakePoints.points > 5000&& snakePoints.points <= 10000)
                    {
                        //newParts += (int)math.round(650 / (19));
                       // newParts += (int)math.round((1500 - 650) / (22));
                       // newParts += (int)math.round((5000 - 1500) / (30));
                        //newParts += (int)math.round((snakePoints.points - 5000) / (50));
                        newParts += (int)math.round(snakePoints.points / (200));
                    }
                    else if (snakePoints.points > 1500&& snakePoints.points <= 5000)
                    {
                        //newParts += (int)math.round(650 / (19));
                        //newParts += (int)math.round((1500 - 650) / (22));
                        //newParts += (int)math.round((snakePoints.points - 1500) / (30));
                        newParts += (int)math.round(snakePoints.points / (80));
                    }
                    else if (snakePoints.points > 650&& snakePoints.points <=1500)
                    {
                        //newParts += (int)math.round(650 / (19));
                        //newParts += (int)math.round((snakePoints.points - 650) / (22));
                        newParts += (int)math.round(snakePoints.points / (40));
                    }
                    else
                        newParts = (int)math.round(snakePoints.points / (19));*/


                    if (snakePoints.points <= 400)
                    {
                        newParts += 7;
                    }
                    else
                    {
                        newParts += 7;
                        newParts += (int)math.round((snakePoints.points - 400) / (400));
                    }

                 

                    snakeHeadParts.snakeNewParts = newParts;
                    ECSSnake snake = SnakeSpawner.Instance.snakes[headData.snakeId];
                    if (snake != null)
                    {
                        snake.points = snakePoints.points;
                        snake.scaleChanged = true;
                    }
                    if (snakeHeadParts.snakeParts != snakeHeadParts.snakeNewParts)
                    {
                        int diffPieces = snakeHeadParts.snakeNewParts - snakeHeadParts.snakeParts;
                        if (headData.snakeId >= SnakeSpawner.Instance.snakes.Length)
                        {
                            return;
                        }
                        
                        Mesh quadMesh = SnakeSpawner.Instance.quad;
                        if (diffPieces > 0)
                        {

                            // DynamicBuffer<SnakePartBuffer> snakeParts = EntityManager.GetBuffer<SnakePartBuffer>(snake.snakeHead);
                            for (int x = 0; x < diffPieces; x++)
                                snakeParts.Add(new SnakePartBuffer { savedPosition = snakeParts[snakeParts.Length - 1].savedPosition });

                            float3 lastEntityPos = EntityManager.GetComponentData<Translation>(lastPart.lastPiece).Value;

                            PieceData data = EntityManager.GetComponentData<PieceData>(lastPart.lastPiece);
                            int lastPieceIndex = data.pieceIndex;
                            int lastSnakeId = data.snakeId;
                            Entity nLastEntity = lastPart.lastPiece;

                            for (int x = 1; x < (diffPieces + 1); x++)
                            {
                                Entity newEntity = EntityManager.Instantiate(lastPart.lastPiece);

                                EntityManager.SetComponentData(newEntity, new Translation { Value = lastEntityPos });
                                EntityManager.AddComponentData(newEntity, new NonUniformScale
                                {
                                    Value = new float3(GameConstants.SNAKE_HEAD_SCALE, GameConstants.SNAKE_HEAD_SCALE, GameConstants.SNAKE_HEAD_SCALE)
                                });
                                EntityManager.SetComponentData(newEntity, new PieceData
                                {
                                    snakeId = (lastSnakeId),
                                    pieceIndex = (lastPieceIndex + x),
                                    positionToMove = new float3(0, x, 0),
                                    teamId = data.teamId
                                });
                                EntityManager.SetComponentData(newEntity, new PieceScaleData
                                {
                                    pieceIndex = (lastPieceIndex + x),
                                    scaleData = GameConstants.SNAKE_HEAD_SCALE
                                });
                                EntityManager.SetComponentData(newEntity, new PieceNodeData
                                {

                                    entityToFollow = nLastEntity
                                });
                                EntityManager.AddSharedComponentData(newEntity, new SnakeGroupData
                                {
                                    group = lastSnakeId
                                });
                                SnakeBallColorData ballData = EntityManager.GetComponentData<SnakeBallColorData>(newEntity);
                                EntityManager.SetSharedComponentData<RenderMesh>(ballData.snakeBallEntity, new RenderMesh
                                {
                                    mesh = quadMesh,
                                    material =  snake.GetNextColor(lastPieceIndex + x)
                                });
                                EntityManager.SetComponentData<Translation>(ballData.snakeBallEntity, new Translation
                                {
                                    Value = new float3(0, (((lastPieceIndex + x) + 1) * -GameConstants.TRANSLATION_OFFSET), 0)
                                });


                                nLastEntity = newEntity;
                            }
                        lastPart.lastPiece = nLastEntity;
                            

                        }
                        else
                        {
                            diffPieces *= -1;

                            //    DynamicBuffer<SnakePartBuffer> snakeParts = EntityManager.GetBuffer<SnakePartBuffer>(snake.snakeHead);
                            for (int x = 0; x < diffPieces; x++)
                                snakeParts.RemoveAt(snakeParts.Length - 1);

                            Entity nextEntity = lastPart.lastPiece;
                            for (int x = 0; x < (diffPieces); x++)
                            {
                                Entity lastEntity = nextEntity;

                                nextEntity = EntityManager.GetComponentData<PieceNodeData>(lastEntity).entityToFollow;

                                EntityManager.DestroyEntity(lastEntity);

                            }
                            lastPart.lastPiece = nextEntity;
                            if (snake != null)
                            {
                                snake.DecreaseNextColor(diffPieces);
                            }
                        }


                        snakeHeadParts.snakeParts = snakeHeadParts.snakeNewParts;
                        if (snake != null)
                        {
                            snake.points = snakePoints.points;
                            snake.scaleChanged = true;
                        }

                    }


                }
        }).Run();

        return inputDeps;// JobHandle.CombineDependencies(dependencies);
    }
}
