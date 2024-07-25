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
            // �ٶ��� ���⸦ ������� �ͺ� ȸ�� �ӵ� ����
            float windStrength = windZone.windMain;
            transform.Rotate(Vector3.right, windStrength * rotationSpeed * Time.deltaTime);
        }
    }
}
