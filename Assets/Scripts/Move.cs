using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 translation;
    public MeshFilter mf;
    private Vector3[] origVerts;
    private Vector3[] newVerts;

    public MeshFilter head;

    void Start()
    {
        mf = GetComponent<MeshFilter>();
        origVerts = mf.mesh.vertices;
        newVerts = new Vector3[origVerts.Length];
        print(origVerts.Length);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        origVerts = mf.mesh.vertices;
        for (int i = 0; i < origVerts.Length; i++)
        {
            newVerts[i] = origVerts[i];
        }

         Matrix4x4 localToWorld = head.transform.localToWorldMatrix;
        Vector3[] world_v = new Vector3[head.mesh.vertices.Length];

         /*for (int i = 0; i < head.mesh.vertices.Length; ++i)
         {
             world_v = localToWorld.MultiplyPoint3x4(head.mesh.vertices[i]);
         print(world_v);

         }
         */


        float scale = 0.0003f;
        for (int i = 0; i < 20; i++)    
        {
            Matrix4x4 m = Matrix4x4.TRS(head.transform.position, Quaternion.Euler(0,0,0), new Vector3(scale, scale, scale));
            newVerts[i] = m.MultiplyPoint3x4(world_v[i]);
        }

        mf.mesh.vertices = newVerts;
        //mf.mesh.RecalculateNormals();

    }

    void DisplaceVertices(Vector3 targetVertexPos, float force, float radius)
    {
        Vector3 currentVertexPos = Vector3.zero;
        float sqrRadius = radius * radius; //1

        for (int i = 0; i < 20; i++) //2
        {
            currentVertexPos = newVerts[i];
            float sqrMagnitude = (currentVertexPos - targetVertexPos).sqrMagnitude; //3
            if (sqrMagnitude > sqrRadius)
            {
                continue; //4
            }
            float distance = Mathf.Sqrt(sqrMagnitude); //5
            float falloff = GaussFalloff(distance, radius);
            Vector3 translate = (currentVertexPos * force) * falloff; //6
            translate.z = 0f;
            Quaternion rotation = Quaternion.Euler(translate);
            Matrix4x4 m = Matrix4x4.TRS(translate, rotation, Vector3.one);
            newVerts[i] = m.MultiplyPoint3x4(currentVertexPos);
        }
        mf.mesh.vertices = newVerts; //7
        mf.mesh.RecalculateNormals();
    }

    static float GaussFalloff(float dist, float inRadius)
    {
        return Mathf.Clamp01(Mathf.Pow(360, -Mathf.Pow(dist / inRadius, 2.5f) - 0.01f));
    }
}
