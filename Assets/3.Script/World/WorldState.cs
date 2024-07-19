using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    private Animator animator;


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        animator.SetTrigger("dd");
    //    }
    //}
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator ������Ʈ�� ã�� �� �����ϴ�!");
        }
        //float savedTime = PlayerPrefs.GetFloat("AnimatorState", 0);
        //if(savedTime<2)
        //SaveState();
        //if (savedTime>1f)
            RestoreState();

    }

    public void SaveState()
    {
        if (animator != null)
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //���� �ִϸ��̼��� ��� �ð�(normalizedTime) ����
            PlayerPrefs.SetFloat("AnimatorState", animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Debug.Log("�ִϸ��̼� ��� �ð� �����: " + normalizedTime);
        }
        else
        {
            Debug.Log("���忡�� ���Ƴ�");
        }
    }

    public void RestoreState()
    {
        //if (animator != null)
        //{
        //    // ����� �ִϸ��̼� ��� �ð��� �ҷ�����
        //    float savedtime = PlayerPrefs.GetFloat("animatorstate", 0);
        //    AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        //    // �ִϸ��̼� Ŭ���� ���̸� ��������
        //    float cliplength = info.length;

        //    // ����� �ð��� Ŭ�� ���̷� ������ normalized time���� ��ȯ
        //    float normalizedtime = savedtime;

        //    // �ִϸ��̼��� �ش� �ð��� ���缭 ��� (�� �κп��� ����)
        //    animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, normalizedtime);

        //    Debug.Log("�ִϸ��̼� �� �κ� ��� �ð�: " + normalizedtime);
        //}
        //else 
        //{
        //    Debug.Log("������� ���Ƴ�");
        //}



        // ����� �ִϸ��̼� ��� �ð��� �ҷ��ͼ� �ִϸ��̼��� �ش� �ð��� ���缭 ���
        float savedTime = PlayerPrefs.GetFloat("AnimatorState", 0);
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, savedTime);
        Debug.Log("�ִϸ��̼� ��� �ð�: " + savedTime);
    }
}
