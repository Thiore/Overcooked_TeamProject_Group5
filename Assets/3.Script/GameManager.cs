using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int stage_index;
    public bool isPause = false; // 일시정지 상태
    public bool isPlaying = true; // 게임 진행 상태
    public GameObject pauseScreen; // 일시정지 화면
    public int isInputEnabled = 0; // 입력 가능 상태
    public WorldState worldState;


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
    public void LoadScene(string SceneName)
    {
        LoadingSceneManager.LoadScene(SceneName);
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
        if (ScoreManager.Instance.tip < 4)
        {
            ScoreManager.Instance.tip += 1;
        }
    }

    public void Incorrect_Recipe()
    {
        ScoreManager.Instance.tip = 1;
        ScoreManager.Instance.AddScore(10);
    }

    public void Wrong_Recipe()
    {
        ScoreManager.Instance.SubScore(10);
        ScoreManager.Instance.tip = 1;
    }

    //Game Load
    public void LoadGame(int stage_index)
    {
        //worldState.SaveState();
        this.stage_index = stage_index;
        switch (stage_index)
        {
            case 1:
                LoadingSceneManager.LoadScene("MangoScene");
                break;
        }
    }

    //IntroScene Button Onclick Method
    public void Menu_Button(int button_index)
    {
        switch (button_index)
        {
            case 0:
                LoadingSceneManager.LoadScene("BSJScene");
                break;
            case 1:
                LoadingSceneManager.LoadScene("Arcade_Menu");
                break;
            case 2:
                SceneManager.LoadScene("BSJScene");
                break;
            case 3:
                SceneManager.LoadScene("Set_Menu");
                break;
            default:
                return;
        }
    }
}
