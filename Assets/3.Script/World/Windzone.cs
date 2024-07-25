using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windzone : MonoBehaviour
{
    public WindZone windZone;
    public float rotationSpeed = 10f;

    void Update()
    {
        if (windZone != null)
        {
            // 바람의 세기를 기반으로 터빈 회전 속도 조정
            float windStrength = windZone.windMain;
            transform.Rotate(Vector3.right, windStrength * rotationSpeed * Time.deltaTime);
        }
    }
}
