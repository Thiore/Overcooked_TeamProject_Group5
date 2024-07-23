using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Flow : MonoBehaviour
{
    private GameObject[] flow = new GameObject[2];
    private RectTransform[] flowRects = new RectTransform[2];
    public float scrollSpeed = 50f; // ����� �������� �ӵ�

    private void Start()
    {
        flow[0] = GameObject.Find("Background_Flow_1");
        flow[1] = GameObject.Find("Background_Flow_2");

        for (int i = 0; i < 2; i++)
        {
            flowRects[i] = flow[i].GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            // �Ʒ��� �̵�
            flowRects[i].anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;

            // ��ġ�� Ư�� ������ ����� �缳��
            if (flowRects[i].anchoredPosition.y <= -1080f)
            {
                flowRects[i].anchoredPosition = new Vector2(flowRects[i].anchoredPosition.x, 1080f);
            }
        }
    }
}
