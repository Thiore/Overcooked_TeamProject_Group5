using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private Transform player;

    private void Awake()
    {
        camera = Camera.main;

    }

    private void FixedUpdate()
    {
        camera.transform.position = player.position + new Vector3(0, 6, -4);
        camera.transform.LookAt(player);
    }
}
