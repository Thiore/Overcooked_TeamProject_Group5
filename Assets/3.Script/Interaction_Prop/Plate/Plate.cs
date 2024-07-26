using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform[] Sink_Pos;
    [SerializeField] private Mesh[] Plate_Mesh;

    private MeshFilter mesh;
    private Renderer renderer;
    private MeshCollider meshcol;

    public bool isWash { get; private set; }//true면 설거지를 해야하는상태 false면 올릴 수 있는상태

    private List<Recipe> recipes;


    private void Awake()
    {
        
        TryGetComponent(out mesh);
        TryGetComponent(out renderer);
        TryGetComponent(out meshcol);
    }

    private void OnEnable()
    {
        
        Change_Plate(isWash);
        
    }

   
    public void SetWash(bool isWash)
    {
        this.isWash = isWash;
    }

    private void Change_Plate(bool isWash)
    {
        if (!isWash)
        {
            mesh.mesh = Plate_Mesh[0];
            renderer.material.SetFloat("_DetailAlbedoMapScale", 0f);
            meshcol.sharedMesh = Plate_Mesh[0];
        }
        else
        {
            mesh.mesh = Plate_Mesh[1];
            renderer.material.SetFloat("_DetailAlbedoMapScale", 1f);
            meshcol.sharedMesh = Plate_Mesh[1];
        }
    }

    private void Check()
    {
        if (!isWash)
        {
            
        }
    }
}
