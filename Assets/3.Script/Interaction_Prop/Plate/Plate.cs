using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Crate_Data Ingredients_Info;
    [SerializeField] private GameObject[] IngrePrefabs;
    //[SerializeField] 
    
    [SerializeField] private Transform[] Sink_Pos;
    [SerializeField] private MeshFilter[] Plate_Mesh;
    [SerializeField] private Material[] materials;

    private Plate_Spawn spawner;

    public bool isWash { get; private set; }
    public bool isPass;

    private void Awake()
    {
        isWash = false;
    }
    private void OnEnable()
    {
        isPass = false;
    }

    private void Update()
    {
        
    }
    private void SetWash(bool isWash)
    {
        this.isWash = isWash;
    }

    public void SetIngredient(GameObject Ingredient)
    {
        isPass = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            if (obj.activeSelf)
            {
                
                return;
            }
        }
    }



}
