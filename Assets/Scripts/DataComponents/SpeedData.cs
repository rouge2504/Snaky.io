using Unity.Entities;

[GenerateAuthoringComponent]public struct SpeedData : IComponentData
{
    public float speed;
    public float speedMultiplier;
    public float speedRotation;
    public float limitSpeed;
}