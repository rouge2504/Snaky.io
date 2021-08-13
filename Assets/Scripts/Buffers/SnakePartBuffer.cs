using Unity.Entities;
using Unity.Mathematics;

[InternalBufferCapacity(300)]
public struct SnakePartBuffer : IBufferElementData
{
    public float3 savedPosition;
}
