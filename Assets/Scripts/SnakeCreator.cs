using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;

public class SnakeCreator : MonoBehaviour
{
    public Material material;
    public Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        MakeEntity();
    }

    private void MakeEntity()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof (Translation),
            typeof (Rotation),
            typeof (RenderMesh),
            typeof (RenderBounds),
            typeof (LocalToWorld)
            );
        Entity myEntity = entityManager.CreateEntity(archetype);

        entityManager.AddComponentData(myEntity, new Translation
        {
            Value = new float3(2f, 0f, 0f)
        });

        entityManager.AddSharedComponentData(myEntity, new RenderMesh
        {
            material = this.material,
            mesh = this.mesh
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
