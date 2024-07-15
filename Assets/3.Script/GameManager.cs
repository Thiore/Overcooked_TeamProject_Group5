using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0; // 현재 점수
    public int tip = 1; // 팁 배수
    public bool isPause = false; // 일시정지 상태
    public bool isPlaying = true; // 게임 진행 상태
    public GameObject pauseScreen; // 일시정지 화면
    public bool isInputEnabled = false; // 입력 가능 상태

    public int addScoreCount = 0; // 점수를 얻은 횟수
    public int subScoreCount = 0; // 점수를 잃은 횟수
    public int subScore = 0; // 차감된 점수 합계

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        isPause = false;
        isPlaying = true;
    }

    private void Update()
    {
        if (isPause && !isPlaying)
        {
            PauseScreen();
        }
    }

    // 특정 씬을 비동기 로드하는 메서드
    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneCoroutine(index));
    }

    // 비동기 로드 코루틴
    private IEnumerator LoadSceneCoroutine(int index)
    {
        AsyncOperation asyncLoad = null;
        switch (index)
        {
            case 0:
                asyncLoad = SceneManager.LoadSceneAsync("MangoScene");
                break;
            case 1:
                asyncLoad = SceneManager.LoadSceneAsync("MenuScene");
                break;
        }

        if (asyncLoad != null)
        {
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            pauseScreen = GameObject.Find("Pause_Screen");
            isPause = false;
            isPlaying = true;
        }
    }

    // 점수를 추가하는 메서드
    public void AddScore(int points)
    {
        score += points;
        addScoreCount++; // 점수를 얻은 횟수 증가
        ScoreManager.Instance.AddScore(points); // ScoreManager에 점수 추가
    }

    // 점수를 차감하는 메서드
    public void SubScore(int points)
    {
        score -= points;
        subScoreCount++; // 점수를 잃은 횟수 증가
        subScore += points; // 차감된 점수 합계 증가
        tip = 1; // 팁 배수 초기화
        ScoreManager.Instance.SubScore(points); // ScoreManager에 점수 차감
    }

    // 게임 씬으로 진입하는 메서드
    public void EnterGame()
    {
        isPlaying = true;
        SceneManager.LoadScene("GameScene");
    }

    // 게임 종료 메서드
    public void EndGame()
    {
        isPause = true;
        isPlaying = false;
        SceneManager.LoadScene("ResultScene");
    }

    // 일시정지 화면 활성화 메서드
    private void PauseScreen()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
        }
    }

    public void AllCorrect_Recipe()
    {
        ScoreManager.Instance.AddScore(10);
        if (tip < 4)
        {
            tip += 1;
        }
        Debug.Log("AllCorrect_Recipe 호출");
    }
    public void Incorrect_Recipe()
    {
        ScoreManager.Instance.AddScore(10);
        tip = 1;
        Debug.Log("InCorrect_Recipe 호출");
    }
    public void Wrong_Recipe()
    {
        ScoreManager.Instance.SubScore(10);
        tip = 1;
        Debug.Log("Wrong_Recipe 호출");
    }



}
