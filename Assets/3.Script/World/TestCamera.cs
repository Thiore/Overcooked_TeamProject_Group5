using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private Transform player;

    private void Awake()
    {
        this.camera = Camera.main;

    }

    private void FixedUpdate()
    {
        this.camera.transform.position = player.position + new Vector3(0, 4, -3);
        this.camera.transform.LookAt(player);
    }
}
