using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct SnakeHeadPartsData : IComponentData
{
 
    public int snakeParts;
    public int snakeNewParts;
}
