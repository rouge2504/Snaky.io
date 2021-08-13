using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[GenerateAuthoringComponent]
public struct PieceData : IComponentData
{
    public int snakeId;
    public int pieceIndex;
    
    public float3 positionToMove;
    public int teamId;
   
}
