using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private Transform player;
    private bool followPlayer = false;

    private void Awake()
    {
        camera = Camera.main;

    }

    private void Update()
    {
        if (followPlayer == true)
        { 
        camera.transform.position = player.position + new Vector3(0, 4, -3);
        camera.transform.LookAt(player);
        }
    }
    public void StartFollowingPlayer()
    {
        followPlayer = true;
        Debug.Log("followPlayer�� true�� �����Ǿ����ϴ�."); // ������� ���� �α� �߰�
    }

    public void StopFollowingPlayer()
    {
        followPlayer = false;
    }
}
