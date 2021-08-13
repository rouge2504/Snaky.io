using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct SnakeLastPartData : IComponentData
{
    public Entity lastPiece;
}
