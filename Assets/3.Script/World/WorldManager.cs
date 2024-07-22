using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Animator worldAnimator;
    private bool animationPlayed = false;
    private GameObject bus; // ���� ������Ʈ
    

    private void Start()
    {
        // ���� ������Ʈ ã��
        bus = GameObject.Find("Van_Mesh (1)");
        // �÷��̾� ������Ʈ ã��
        

        // �ִϸ��̼� ���¸� Ȯ���Ͽ� ����մϴ�.
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
