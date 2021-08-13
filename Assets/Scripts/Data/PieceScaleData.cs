using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PieceScaleData : IComponentData
{
    public int pieceIndex;
    public float scaleData;
}
