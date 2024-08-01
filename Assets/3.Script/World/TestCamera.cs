using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private Transform player;
    private bool followVan = true;

    private void Awake()
    {
        camera = Camera.main;

    }

    private void LateUpdate()
    {
        if (followVan)
        { 
        transform.position = player.position + new Vector3(0, 4, -3);
        camera.transform.LookAt(player);
        }
    }
    public void StartFollowingPlayer()
    {
        followVan = true;
        Debug.Log("followPlayer가 true로 설정되었습니다."); // 디버깅을 위한 로그 추가
    }

    public void StopFollowingPlayer()
    {
        followVan = false;
    }
}
