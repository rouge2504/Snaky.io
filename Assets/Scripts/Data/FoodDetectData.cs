using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct FoodDetectData : IComponentData
{
    public Entity headTargetData;
    public float3 currentPos;
}
