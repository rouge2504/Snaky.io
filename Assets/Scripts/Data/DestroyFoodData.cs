using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct DestroyFoodData : IComponentData
{
    public bool shouldDestroy;
}
