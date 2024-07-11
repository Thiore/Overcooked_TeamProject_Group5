using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PosData
{
    public string Object_Name;
    public Vector3 Position;
    public Quaternion Rotation;
}

[CreateAssetMenu(fileName = "_InterProp_Pos", menuName = "ScriptableObjects/InterProp_Pos_Data", order = 1)]
public class InterProp_Pos_Data : ScriptableObject
{
    [SerializeField] private PosData[] Install_Pos;


}
