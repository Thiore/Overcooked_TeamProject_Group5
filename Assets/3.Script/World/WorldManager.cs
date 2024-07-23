using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Animator worldAnimator;
    private bool animationPlayed = false;
    private GameObject bus; // 버스 오브젝트
    

    private void Start()
    {
        // 버스 오브젝트 찾기
        bus = GameObject.Find("Van_Mesh (1)");
        // 플레이어 오브젝트 찾기
        

        // 애니메이션 상태를 확인하여 재생합니다.
        if (!animationPlayed)
        {
            worldAnimator.SetTrigger("1Stage_Ani");
            animationPlayed = true;
        }
    }

   

    public void SetBusPosition(Vector3 position)
    {
        if (bus != null)
        {
            bus.transform.position = position;
        }
    }

    public Vector3 GetBusPosition()
    {
        if (bus != null)
        {
            return bus.transform.position;
        }
        return Vector3.zero;
    }

   

   

    
}
