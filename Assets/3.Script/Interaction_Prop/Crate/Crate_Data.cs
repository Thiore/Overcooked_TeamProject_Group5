using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Crate_Info
{
    public Ingredients Ingredients_Name;
    public Vector3 Position;
    public Quaternion Rotation;
}

[CreateAssetMenu(fileName = "_Crate", menuName = "ScriptableObjects/Crate_Data", order = 1)]
public class Crate_Data : ScriptableObject
{
    [SerializeField] private Crate_Info[] Install_Pos;


}
