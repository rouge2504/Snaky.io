using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct SnakeGlowData : IComponentData
{
    public Entity glowEntity;
}
