﻿using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;
using System.Collections.Generic;

[AlwaysSynchronizeSystem]

public class SnakePartMoveSystem : JobComponentSystem
{
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
       
        var jobHandle = Entities
            .ForEach((DynamicBuffer<SnakePartBuffer> snakeParts, ref SnakeHeadData headData, ref NonUniformScale scale) =>
            {
                //float3[] bufferPosition = new float3[snakeParts.Length];

               
                for (int x = 1; x < snakeParts.Length; x++)
                {
                    float diff = headData.headDiff;
                    if (!headData.isPlayer)
                    {
                        diff = 3.8f / scale.Value.x;
                    }

                    if (x == 1)
                        diff *= 2;
                    SnakePartBuffer buffer = snakeParts[x];
                    float3 index = snakeParts[x].savedPosition;
                    float3 last =snakeParts[x - 1].savedPosition;
                    buffer.savedPosition = math.lerp(index, last, diff);
                    snakeParts[x] = buffer;
                    
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
                    SnakeEnvironment.Singleton.BufferTemp = new List<Vector3>();

                    NativeArray<float3> positions = new NativeArray<float3>(snakeParts.Length, Allocator.TempJob);
                    for (int x = 0; x < positions.Length; x++)
                    {
                        positions[x] = new float3 (snakeParts[x].savedPosition.x, snakeParts[x].savedPosition.y, snakeParts[x].savedPosition.z);
                        SnakeEnvironment.Singleton.BufferTemp.Add(positions[x]);
                        SnakeEnvironment.Singleton.CheckBodyParts(snake.snakeId, SnakeEnvironment.Singleton.BufferTemp);
                    }



                    int id = snake.snakeId;
                    var njobHandle = Entities
                        .WithSharedComponentFilter(new SnakeGroupData { group = snake.snakeId })
                        .ForEach((ref PieceData piece, ref Translation position) =>
                        {
                                position.Value = positions[piece.pieceIndex];



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
    [ReadOnly] public Translation position;
    public void Execute(int index)
    {
        position.Value = positionsInput[index];
    }
}
