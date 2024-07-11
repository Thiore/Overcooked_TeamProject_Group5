using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveTransformData
{
    public string Type;
    public int branch;
    public float PosX;
    public float PosY;
    public float PosZ;
    public float RotX;
    public float RotY;
    public float RotZ;

}

[CreateAssetMenu(fileName = "Counter_Data", menuName = "ScriptableObjects/Counter_Data", order = 1)]
public class Counter_Data : ScriptableObject
{
    [SerializeField] private SaveTransformData[] Top_Data;
    [SerializeField] private SaveTransformData[] No_Edge_Data;
    [SerializeField] private Material Top_Material;
    [SerializeField] private Material No_Edge_Material;

}
