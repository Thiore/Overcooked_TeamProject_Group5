using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassArrow : MonoBehaviour
{
    private Material ArrowMaterial;

    private void Awake()
    {
        TryGetComponent(out Renderer r);
        ArrowMaterial = r.material;
    }

    private void FixedUpdate()
    {
        ArrowMaterial.mainTextureOffset -= new Vector2(Time.deltaTime,0);
    }


}
