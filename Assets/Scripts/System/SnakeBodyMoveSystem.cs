using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;

public class SnakeBodyMoveSystem : ComponentSystem
{

    protected override void OnUpdate()
    {

        Entities
            .ForEach((DynamicBuffer<SnakeBodyBuffer> snakeParts, ref Translation position) =>
            {

                for (int x = 1; x < snakeParts.Length; x++)
                {
                    float diff = 5;
                    if (x == 1)
                        diff *= 2;
                    SnakeBodyBuffer buffer = snakeParts[x];
                    buffer.savedPosition = math.lerp(snakeParts[x].savedPosition, snakeParts[x - 1].savedPosition, diff);
                    snakeParts[x] = buffer;
                }

            });

    }

}
