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
            Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다!");
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
            //현재 애니메이션의 재생 시간(normalizedTime) 저장
            PlayerPrefs.SetFloat("AnimatorState", animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Debug.Log("애니메이션 재생 시간 저장됨: " + PlayerPrefs.GetFloat("AnimatorState", 0));
        }
        else
        {
            Debug.Log("저장에서 ㅈ됐네");
        }
    }

    public void RestoreState()
    {
        

        // 저장된 애니메이션 재생 시간을 불러와서 애니메이션을 해당 시간에 맞춰서 재생
        float savedTime = PlayerPrefs.GetFloat("AnimatorState", 0);
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, savedTime);
        Debug.Log("애니메이션 재생 시간: " + savedTime);
    }

    public void ResetState(int stageNumber)
    {
        bestScore = DataManager.Instance.GetStageInformation(stageNumber).bestScore;
        if (bestScore == 0)
        {
            //PlayerPrefs.DeleteKey("AnimatorState");
            SaveState();
            Debug.Log("애니메이션 리셋");
        }
        else
        {
            RestoreState();
        }
    }
}
