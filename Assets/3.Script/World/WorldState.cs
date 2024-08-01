using System.Collections;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    [SerializeField] private GameObject stage1; // FlagUIController가 있는 부모 오브젝트를 할당합니다.
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private TestCamera testCamera; // 카메라 스크립트
    [SerializeField] private Animator stage2;
    [SerializeField] private GameObject stage2Object;
    

    private FlagUIController flagController;
    private FlagUIController flagControllerStage2;
    private Animator animator;
    private int bestScore_W;
    private Vector3 vanPosition;

    // 애니메이션 상태를 추적하는 변수
    public bool isCameraAnimationPlaying { get; private set; }

    private void Start()
    {
        testCamera = FindObjectOfType<TestCamera>();
        if (stage1 == null)
        {
            Debug.LogError("stage1 게임 오브젝트가 할당되지 않았습니다!");
            return;
        }

        flagController = stage1.GetComponentInChildren<FlagUIController>(); // 자식 오브젝트에서 FlagUIController를 찾습니다.
        if (flagController == null)
        {
            Debug.LogError("FlagUIController 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        flagControllerStage2 = stage2Object.GetComponentInChildren<FlagUIController>(); // stage2Object의 자식 오브젝트에서 FlagUIController를 찾습니다.
        if (flagControllerStage2 == null)
        {
            Debug.LogError("Stage2의 FlagUIController 컴포넌트를 찾을 수 없습니다!");
            return;
        }

        flagController.OnUISet += OnFlagUISet;

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다!");
        }

        // 지연 호출로 FlagUIController 초기화를 기다림
        //StartCoroutine(DelayedResetState());
    }

    private void OnFlagUISet()
    {
        ResetState(1); // UI 설정 완료 후 ResetState 호출
    }

    private IEnumerator DelayedResetState()
    {
        // 1초 대기 (1초 후에도 이벤트가 설정되지 않았다면 ResetState를 호출)
        yield return new WaitForSeconds(1);

        if (flagController != null && flagController.bestScore != 0)
        {
            ResetState(1);
        }
    }

    public void SaveState()
    {
        if (animator != null)
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            PlayerPrefs.SetFloat("AnimatorState", normalizedTime);
            Debug.Log("애니메이션 재생 시간 저장됨: " + normalizedTime);
        }
        else
        {
            Debug.Log("저장에서 문제가 발생했습니다.");
        }
    }

    public void RestoreState()
    {
        float savedTime = PlayerPrefs.GetFloat("AnimatorState", 0);
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, savedTime);
        Debug.Log("애니메이션 재생 시간: " + savedTime);
    }

    public void ResetState(int stageNumber)
    {
        if (flagController == null)
        {
            Debug.LogError("ResetState 호출 시 FlagUIController가 없습니다.");
            return;
        }

        bestScore_W = flagController.bestScore;
        Debug.Log($"ResetState called with bestScore_W: {bestScore_W}");
        if (bestScore_W <= 39)
        {
            SaveState();
            Debug.Log("ResetState 메서드에 들어왔습니다.");
        }
        else
        {
            RestoreState();

            HandleStage2Animation();



            Debug.Log("else에서 ResetState 메서드에 들어왔습니다.");
        }
    }
    private void HandleStage2Animation()
    {
        if (flagControllerStage2 == null)
        {
            Debug.LogError("Stage2의 플레그컨트롤러를 찾을 수 없습니다.");
            return;
        }
        // 애니메이션 상태를 직접 확인
        AnimatorStateInfo stateInfo = stage2.GetCurrentAnimatorStateInfo(0);
        bool isStage2AnimationPlaying = stateInfo.IsName("Stage2");

        if (!isStage2AnimationPlaying)
        {
            // Stage2 애니메이션이 재생되지 않은 경우에만 트리거를 설정
            stage2.SetTrigger("Stage2_");
        }

        // Stage2 애니메이션이 완료된 상태로 만들기
        if (flagControllerStage2.bestScore >= 50)
        {
            // 애니메이션을 끝으로 이동시키는 방법
            stage2.Play("Stage2", 0, 1f);

            
        }
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    //카메라 애니메이션 메서드
    public void StartCameraAnimation_Stage1()
    {
        if (cameraAnimator != null)
        {
            isCameraAnimationPlaying = true;
            cameraAnimator.SetTrigger("Camera_Stage1");
        }
        else
        {
            Debug.Log("카메라 애니메이션 재생 안됨.");
        }
    }
    public void StartCameraAnimation_Stage2()
    {
        if (cameraAnimator != null)
        {
            isCameraAnimationPlaying = true;
            cameraAnimator.SetTrigger("Camera_Stage2");
        }
    }
    public void OnCameraAnimationFinished()
    { 
    
        Debug.Log("카메라 애니메이션이 끝났습니다."); // 디버깅을 위한 로그 추가
        isCameraAnimationPlaying = false;
        if (testCamera != null)
        {
            testCamera.StartFollowingPlayer();
            Debug.Log("카메라가 플레이어를 따라갑니다."); // 디버깅을 위한 로그 추가
        }
        else
        {
            Debug.Log("testCamera가 할당되지 않았습니다.");
        }
    }
    public void OnAnimationStart()
    {
        if (testCamera != null)
        {
            testCamera.StopFollowingPlayer();
        }
    }
    public void OnAnimationEnd()
    {
        if (testCamera != null)
        {
            testCamera.StartFollowingPlayer();
        }
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    //Van 포지션 저장

    public void SaveVanPosition(Vector3 position)
    {
        vanPosition = position;
        Debug.Log("벤 위치 저장 값" + vanPosition);
    }

    public Vector3 GetVanPosition()
    {
        return vanPosition;
    }
    
}
