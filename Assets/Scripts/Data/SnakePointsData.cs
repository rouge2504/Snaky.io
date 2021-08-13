using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct SnakePointsData : IComponentData
{
    public int points;
}
