using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct SnakeHeadData : IComponentData
{
    public int snakeId;
    public float speed;
    public float snakeRotationSpeed;
    public int speedMultiplier;
    public float headDiff;
    public bool shouldDestroy;
    public bool isDead;
    public bool isImmune;
    public int teamId;
    public bool isDuelMode;
    public bool isBabySnake;

}
