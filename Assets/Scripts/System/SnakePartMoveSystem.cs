using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class SnakePartMoveSystem : JobComponentSystem
{
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
       
        var jobHandle = Entities
            .ForEach((DynamicBuffer<SnakePartBuffer> snakeParts, ref SnakeHeadData headData) =>
            {
                System.Collections.Generic.List<Vector3> bufferPosition = new System.Collections.Generic.List<Vector3>();
                for (int x = 1; x < snakeParts.Length; x++)
                {
                    float diff = headData.headDiff;
                    if (x == 1)
                        diff *= 2;
                    SnakePartBuffer buffer = snakeParts[x];
                    buffer.savedPosition = math.lerp(snakeParts[x].savedPosition, snakeParts[x - 1].savedPosition, diff);
                    snakeParts[x] = buffer;
                    bufferPosition.Add(buffer.savedPosition);
                    SnakeEnvironment.Singleton.CheckBodyParts(headData.snakeId, bufferPosition);
                }
            
            }).Schedule(inputDeps);
        jobHandle.Complete();
        foreach (ECSSnake snake in SnakeSpawner.Instance.snakes)
        {
            if (snake != null)
            {
                if (!snake.isDestroyed)
                {
                    DynamicBuffer<SnakePartBuffer> snakeParts = EntityManager.GetBuffer<SnakePartBuffer>(snake.snakeHead);

                    NativeArray<float3> positions = new NativeArray<float3>(snakeParts.Length, Allocator.TempJob);
                    for (int x = 0; x < positions.Length; x++)
                    {
                        positions[x] = snakeParts[x].savedPosition;
                    }

                    var njobHandle = Entities
                        .WithSharedComponentFilter(new SnakeGroupData { group = snake.snakeId })
                        .ForEach((ref PieceData piece, ref Translation position) =>
                        {
                            if (piece.pieceIndex < positions.Length)
                            {
                                position.Value = positions[piece.pieceIndex];
                            }

                        }).Schedule(inputDeps);
                    njobHandle.Complete();
                    positions.Dispose();
                }
            }
        }



        return default;
    }
}

struct Job : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> positionsInput;
    [WriteOnly] public NativeArray<float3> positionsOutput;
    public void Execute(int index)
    {
        positionsOutput[index] = positionsInput[index];
    }
}
