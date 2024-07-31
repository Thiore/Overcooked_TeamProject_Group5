using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_Ani : MonoBehaviour
{
    [SerializeField] private GameObject stage2; // FlagUIController�� �ִ� �θ� ������Ʈ�� �Ҵ��մϴ�.
    private FlagUIController flagController;
    private Animator animator;
    public int bestScore_W;

    // �ִϸ��̼� ���¸� �����ϴ� ����
    public bool isCameraAnimationPlaying { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (stage2 == null)
        {
            Debug.LogError("stage2 ���� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }
        flagController = stage2.GetComponentInChildren<FlagUIController>(); // �ڽ� ������Ʈ���� FlagUIController�� ã���ϴ�.
        if (flagController == null)
        {
            Debug.LogError("Stage2���� FlagUIController ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }
        flagController.OnUISet += OnFlagUISet;
        // ���� ȣ��� FlagUIController �ʱ�ȭ�� ��ٸ�
        StartCoroutine(DelayedResetState());
    }
    private void OnFlagUISet()
    {
        ResetState_Stage2(2); // UI ���� �Ϸ� �� ResetState ȣ��
    }

    private IEnumerator DelayedResetState()
    {
        // 1�� ��� (1�� �Ŀ��� �̺�Ʈ�� �������� �ʾҴٸ� ResetState�� ȣ��)
        yield return new WaitForSeconds(1);

        if (flagController != null && flagController.bestScore != 0)
        {
            ResetState_Stage2(2);
        }
    }

    public void SaveState_Stage2()
    {
        if (animator != null)
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            PlayerPrefs.SetFloat("State_Stage2", normalizedTime);
            Debug.Log("Stage2 �ִϸ��̼� ��� �ð� �����: " + normalizedTime);
        }
        else
        {
            Debug.Log("���忡�� ������ �߻��߽��ϴ�.");
        }
    }
        public void RestoreState_Stage2()
        {
            float savedTime = PlayerPrefs.GetFloat("State_Stage2", 0);
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, savedTime);
            Debug.Log("�ִϸ��̼� ��� �ð�: " + savedTime);
        }

    public void ResetState_Stage2(int stageNumber)
    {
        if (flagController == null)
        {
            Debug.LogError("ResetState ȣ�� �� FlagUIController�� �����ϴ�.");
            return;
        }

        bestScore_W = flagController.bestScore;
        Debug.Log($"Stage2: {bestScore_W}");
        if (bestScore_W <= 39)
        {
            SaveState_Stage2();
            Debug.Log("Stage2 ResetState �޼��忡 ���Խ��ϴ�.");
        }
        else
        {
            RestoreState_Stage2();
            //stage2.SetTrigger("Stage2_");
            Debug.Log("Stage2 else���� ResetState �޼��忡 ���Խ��ϴ�.");
        }

    }
}
