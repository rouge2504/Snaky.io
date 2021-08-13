using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct FoodData : IComponentData
{
    public int foodValue;
    public bool shouldDestroy;
    public bool isNewSpawn;
    public bool absorbed;
    public bool isAbsorbing;
    public float3 positionToMove;
}
