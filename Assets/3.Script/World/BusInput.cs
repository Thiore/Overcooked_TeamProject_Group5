using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusInput : MonoBehaviour
{
    [SerializeField] private string _moveAxis_name = "Vertical";
    [SerializeField] private string _rotateAxis_name = "Horizontal";

    public float Move_Value { get; private set; }
    public float Rotate_Value { get; private set; }

    private void Update()
    {
        Move_Value = Input.GetAxis(_moveAxis_name);
        Rotate_Value = Input.GetAxis(_rotateAxis_name);
    }
}
