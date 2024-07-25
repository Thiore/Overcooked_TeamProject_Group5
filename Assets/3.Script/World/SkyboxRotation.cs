using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public Material skybox;
    public float rotationSpeed = 1.0f;

    private void Update()
    {
        float rotation = Time.time * rotationSpeed;
        skybox.SetFloat("_Rotation", rotation);
    }
}
