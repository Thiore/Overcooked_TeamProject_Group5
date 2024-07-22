using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellymesh : MonoBehaviour
{
    //젤리 오리지널과 복제 메쉬가 필요
    private Mesh OriginalMesh, MeshClone;
    private MeshRenderer renderer;
    private JellyVertex[] jv;
    private Vector3[] vertexArray;
    public float Intensity = 1f;
    public float Mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;
    private void Start()
    {
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;
        renderer = GetComponent<MeshRenderer>();
        jv = new JellyVertex[MeshClone.vertices.Length];
        for (int i = 0; i < MeshClone.vertices.Length; i++)
        {
            jv[i] = new JellyVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }
    }
    private void FixedUpdate()
    {
        vertexArray = OriginalMesh.vertices;
        for (int i = 0; i < jv.Length; i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jv[i].ID]);
            float intensity = (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * Intensity;
            jv[i].Shake(target, Mass, stiffness, damping);
            target = transform.InverseTransformPoint(jv[i].Position);
            vertexArray[jv[i].ID] = Vector3.Lerp(vertexArray[jv[i].ID], target, intensity);
        }
        MeshClone.vertices = vertexArray;
    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 Position;
        public Vector3 velocity, Force;

        public JellyVertex(int id, Vector3 pos)
        {
            ID = id;
            Position = pos;
        }
        public void Shake(Vector3 target, float m, float s, float d)
        {
            Force = (target - Position) * s;
            velocity = (velocity + Force / m) * d;
            Position += velocity;
            if ((velocity = Force + Force / m).magnitude < 0.001f)
                Position = target;
        }
    }

}
