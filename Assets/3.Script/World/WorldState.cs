using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    private Animator animator;
    
    private int bestScore;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator ������Ʈ�� ã�� �� �����ϴ�!");
        }
        float savedTime = PlayerPrefs.GetFloat("AnimatorState", 0);
        //if (savedTime < 2)
            //SaveState();
        //if (savedTime > 1f)
            //RestoreState();
        //PlayerPrefs.DeleteKey("AnimatorState");
        ResetState(1);
        //SaveState();

    }

    public void SaveState()
    {
        if (animator != null)
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //���� �ִϸ��̼��� ��� �ð�(normalizedTime) ����
            PlayerPrefs.SetFloat("AnimatorState", animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Debug.Log("�ִϸ��̼� ��� �ð� �����: " + PlayerPrefs.GetFloat("AnimatorState", 0));
        }
        else
        {
            Debug.Log("���忡�� ���Ƴ�");
        }
    }

    public void RestoreState()
    {
        

        // ����� �ִϸ��̼� ��� �ð��� �ҷ��ͼ� �ִϸ��̼��� �ش� �ð��� ���缭 ���
        float savedTime = PlayerPrefs.GetFloat("AnimatorState", 0);
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, savedTime);
        Debug.Log("�ִϸ��̼� ��� �ð�: " + savedTime);
    }

    public void ResetState(int stageNumber)
    {
        bestScore = DataManager.Instance.GetStageInformation(stageNumber).bestScore;
        if (bestScore == 70)
        {
            //PlayerPrefs.DeleteKey("AnimatorState");
            SaveState();
            Debug.Log("�ִϸ��̼� ����");
        }
        else
        {
            RestoreState();
        }
    }
}
