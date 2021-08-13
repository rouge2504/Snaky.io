using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct AIData : IComponentData
{
    public int snakeId;
    public Entity headTargetData;
    public Entity entityToChase;
    public Entity enemyEntity;
    public bool isReachedFood;
    public bool isEscape;
    public bool isChase;
    public bool isSprint;
    public int counter;
    public bool isDead;
    public int teamId;
    public bool isDuelMode;
    public bool isBabySnake;
   
}
