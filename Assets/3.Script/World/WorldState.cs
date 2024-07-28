using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    private Animator animator;

    private FlagUIController flagController;
    private int bestScore_W;

    private void Start()
    {
        flagController = FindObjectOfType<FlagUIController>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator ������Ʈ�� ã�� �� �����ϴ�!");
        }
        
        ResetState(1);
        

    }

    public void SaveState()
    {
        if (animator != null)
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //���� �ִϸ��̼��� ��� �ð�(normalizedTime) ����
            PlayerPrefs.SetFloat("AnimatorState", animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            //Debug.Log("�ִϸ��̼� ��� �ð� �����: " + PlayerPrefs.GetFloat("AnimatorState", 0));
            Debug.Log("�ִϸ��̼� ��� �ð� �����: " + normalizedTime);
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
        //bestScore = DataManager.Instance.GetStageInformation(stageNumber).bestScore;
        bestScore_W = flagController.bestScore;
        if (bestScore_W <= 39)
        {
            //PlayerPrefs.DeleteKey("AnimatorState");
            SaveState();
            Debug.Log("ResetState �޼��� ���Դ�.");
        }
        else
        {
            RestoreState();
            Debug.Log("else���� ResetState �޼��� ���Դ�.");
        }
    }

   
}
