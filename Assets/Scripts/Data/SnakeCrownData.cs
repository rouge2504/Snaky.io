using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct SnakeCrownData : IComponentData
{
    public Entity crownEntity;
}
