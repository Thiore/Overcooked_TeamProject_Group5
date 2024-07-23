using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float initTime = 60f; // 초기 시간 설정
    public float playTime = 0;   // 플레이 시간
    public int tip = 1;          // 팁 배수
    private float tipTime = 0;   // 팁 증가를 위한 시간 추적
    private Animator timeAni;    // 시간 관련 애니메이션을 위한 애니메이터
    private Text timeText;       // 남은 시간을 표시할 텍스트 UI

    private void Start()
    {
        // 애니메이터와 텍스트 컴포넌트 가져오기
        timeAni = GetComponent<Animator>();
        timeText = GetComponentInChildren<Text>();

        // 게임 매니저의 일시정지 및 플레이 상태 초기화
        GameManager.Instance.isPause = false;
        GameManager.Instance.isPlaying = true;
    }

    private void Update()
    {
        // 게임이 일시정지 상태가 아니고 플레이 중일 때만 실행
        if (!GameManager.Instance.isPause && GameManager.Instance.isPlaying)
        {
            // 델타 시간을 통해 플레이 시간과 팁 시간을 증가
            tipTime += Time.deltaTime;
            playTime += Time.deltaTime;

            // 남은 시간 계산
            float remainingTime = initTime - playTime;

            // 팁 시간이 20초 이상일 경우 팁 배수 증가
            if (tipTime >= 20)
            {
                tip += 1;
                tipTime = 0f;
            }

            // 남은 시간이 0 이하일 경우 게임 종료 / //임시로 만들어두겠습니다. (승주)
            if (remainingTime <= 0)
            {
                playTime = 0;
                GameManager.Instance.EndGame();
            }

            // 남은 시간 UI 업데이트
            UpdateTimeDisplay(remainingTime);
        }
    }

    // 남은 시간을 UI에 업데이트하는 메서드
    private void UpdateTimeDisplay(float remainingTime)
    {
        // 남은 시간이 30초 이하일 경우 애니메이션 트리거 설정
        if (remainingTime <= 30f && remainingTime >= 29f)
        {
            timeAni.SetBool("isThirtySecLeft", true);
        }
        // 남은 시간이 10초 이하일 경우 애니메이션 트리거 설정
        else if (remainingTime <= 10f && remainingTime >= 9f)
        {
            timeAni.SetBool("isThirtySecLeft", false);
            timeAni.SetBool("isTenSecLeft", true);
        }

        // 남은 시간을 분과 초로 변환하여 텍스트 UI에 표시
        if (remainingTime >= 60f)
        {
            int min = (int)remainingTime / 60;
            int sec = (int)remainingTime % 60;
            timeText.text = sec > 9 ? $"0{min}:{sec}" : $"0{min}:0{sec}";
        }
        else
        {
            int sec = (int)remainingTime;
            timeText.text = sec > 9 ? $"00:{sec}" : $"00:0{sec}";
        }
    }
}
