using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_Ani : MonoBehaviour
{
    [SerializeField] private GameObject stage2; // FlagUIController가 있는 부모 오브젝트를 할당합니다.
    private FlagUIController flagController;
    private Animator animator;
    public int bestScore_W;

    // 애니메이션 상태를 추적하는 변수
    public bool isCameraAnimationPlaying { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (stage2 == null)
        {
            Debug.LogError("stage2 게임 오브젝트가 할당되지 않았습니다!");
            return;
        }
        flagController = stage2.GetComponentInChildren<FlagUIController>(); // 자식 오브젝트에서 FlagUIController를 찾습니다.
        if (flagController == null)
        {
            Debug.LogError("Stage2에서 FlagUIController 컴포넌트를 찾을 수 없습니다!");
            return;
        }
        flagController.OnUISet += OnFlagUISet;
        // 지연 호출로 FlagUIController 초기화를 기다림
        StartCoroutine(DelayedResetState());
    }
    private void OnFlagUISet()
    {
        ResetState_Stage2(2); // UI 설정 완료 후 ResetState 호출
    }

    private IEnumerator DelayedResetState()
    {
        // 1초 대기 (1초 후에도 이벤트가 설정되지 않았다면 ResetState를 호출)
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
            Debug.Log("Stage2 애니메이션 재생 시간 저장됨: " + normalizedTime);
        }
        else
        {
            Debug.Log("저장에서 문제가 발생했습니다.");
        }
    }
        public void RestoreState_Stage2()
        {
            float savedTime = PlayerPrefs.GetFloat("State_Stage2", 0);
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, savedTime);
            Debug.Log("애니메이션 재생 시간: " + savedTime);
        }

    public void ResetState_Stage2(int stageNumber)
    {
        if (flagController == null)
        {
            Debug.LogError("ResetState 호출 시 FlagUIController가 없습니다.");
            return;
        }

        bestScore_W = flagController.bestScore;
        Debug.Log($"Stage2: {bestScore_W}");
        if (bestScore_W <= 39)
        {
            SaveState_Stage2();
            Debug.Log("Stage2 ResetState 메서드에 들어왔습니다.");
        }
        else
        {
            RestoreState_Stage2();
            //stage2.SetTrigger("Stage2_");
            Debug.Log("Stage2 else에서 ResetState 메서드에 들어왔습니다.");
        }

    }
}
