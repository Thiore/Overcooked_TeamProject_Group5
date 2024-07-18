using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus_Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 360f;
    [SerializeField] private BusInput busInput;


    private Rigidbody bus_rb;
    private Vector3 moveDirection;

    private void Awake()
    {
        busInput = GetComponent<BusInput>();
        bus_rb = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        
    }

    private void Move()
    {
        //이동 전 회전 주고 비교
        moveDirection = new Vector3(busInput.Rotate_Value, 0, busInput.Move_Value) * moveSpeed * Time.deltaTime;


    }
}
