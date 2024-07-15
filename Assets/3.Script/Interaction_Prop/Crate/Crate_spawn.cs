using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate_spawn : MonoBehaviour
{
    [SerializeField] private Material Crate1;
    [SerializeField] private Material Crate2;
    [SerializeField] private Material Crate3;
    [SerializeField] private Material Crate4;
    [SerializeField] private Crate_Pos_Data pos;
    List<string> ingredient_list = new List<string>();

    private void Awake()
    {
        
    }

}
