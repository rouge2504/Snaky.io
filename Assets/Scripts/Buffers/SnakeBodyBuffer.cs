using Unity.Entities;
using Unity.Mathematics;

[InternalBufferCapacity(300)]
public struct SnakeBodyBuffer : IBufferElementData
{
    public float3 savedPosition;
}
