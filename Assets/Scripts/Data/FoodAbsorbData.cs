using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


[GenerateAuthoringComponent]
public struct FoodAbsorbData : IComponentData
{
    public bool isAbsorbing;
    public float3 positionToMove; 

}
