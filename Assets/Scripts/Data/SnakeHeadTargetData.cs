using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct SnakeHeadTargetData : IComponentData
{
    public Entity ai;
    public float3 moveDirection;
    public float3 foodTarget;
    public bool isReachedPosition;
    public bool isAIMoveDirection;

}
