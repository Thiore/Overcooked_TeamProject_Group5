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
            Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다!");
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
            //현재 애니메이션의 재생 시간(normalizedTime) 저장
            PlayerPrefs.SetFloat("AnimatorState", animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Debug.Log("애니메이션 재생 시간 저장됨: " + normalizedTime);
        }
        else
        {
            Debug.Log("저장에서 ㅈ됐네");
        }
    }

    public void RestoreState()
    {
        //if (animator != null)
        //{
        //    // 저장된 애니메이션 재생 시간을 불러오기
        //    float savedtime = PlayerPrefs.GetFloat("animatorstate", 0);
        //    AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        //    // 애니메이션 클립의 길이를 가져오기
        //    float cliplength = info.length;

        //    // 저장된 시간을 클립 길이로 나누어 normalized time으로 변환
        //    float normalizedtime = savedtime;

        //    // 애니메이션을 해당 시간에 맞춰서 재생 (끝 부분에서 시작)
        //    animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, normalizedtime);

        //    Debug.Log("애니메이션 끝 부분 재생 시간: " + normalizedtime);
        //}
        //else 
        //{
        //    Debug.Log("재생에서 ㅈ됐네");
        //}



        // 저장된 애니메이션 재생 시간을 불러와서 애니메이션을 해당 시간에 맞춰서 재생
        float savedTime = PlayerPrefs.GetFloat("AnimatorState", 0);
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, savedTime);
        Debug.Log("애니메이션 재생 시간: " + savedTime);
    }
}
