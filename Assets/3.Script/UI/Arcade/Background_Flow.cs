using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Flow : MonoBehaviour
{
    private GameObject[] flow = new GameObject[2];
    private RectTransform[] flowRects = new RectTransform[2];
    public float scrollSpeed = 50f; // 배경이 내려가는 속도

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
            // 아래로 이동
            flowRects[i].anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;

            // 위치가 특정 범위를 벗어나면 재설정
            if (flowRects[i].anchoredPosition.y <= -1080f)
            {
                flowRects[i].anchoredPosition = new Vector2(flowRects[i].anchoredPosition.x, 1080f);
            }
        }
    }
}
