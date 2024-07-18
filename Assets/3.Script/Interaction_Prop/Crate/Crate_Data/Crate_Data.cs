using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Crate_Info
{
    public eIngredients Ingredients;
    public Vector3 Position;
    public Quaternion Rotation;
    public eCookingProcess CookingProcess;
    public bool Chop_Anim;
}

[CreateAssetMenu(fileName = "_Crate", menuName = "ScriptableObjects/Crate_Data", order = 1)]
public class Crate_Data : ScriptableObject
{
    public Crate_Info[] Install_Pos;


}
