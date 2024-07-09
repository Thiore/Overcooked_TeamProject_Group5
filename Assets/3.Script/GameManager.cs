using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    public float initTime = 60f;
    public float playTime = 0;
    public int tip = 1;
    public float tipTime = 0;
    private bool isPause = false;
    private bool isPlaying = false;
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

    private void Update()
    {
        if (!isPause)
        {
            if (initTime - playTime > 0)
            {
                tipTime += Time.deltaTime;
                playTime += Time.deltaTime;
                if (tipTime >= 20)
                {
                    tip += 1;
                    tipTime = 0f;
                }
                if (initTime - playTime <= 0)
                {
                    playTime = 0;
                    EndGame();
                }
            }
        }
        else if(isPlaying)
        {
            
        }
        else
        {
            PauseScreen();
        }
        
    }

    public void AddScore(int points)
    {
        score += points*tip;
    }

    public void SubScore(int points)
    {
        score -= points;
        tip = 1;
    }
    private void EnterGame()
    {
        isPlaying = true;
        SceneManager.LoadScene("GameScene");
    }
    private void EndGame()
    {
        isPause = true;
        isPlaying = false;
        // 게임 종료 로직 (예: 결과 씬으로 전환)
        SceneManager.LoadScene("ResultScene");
    }
    private void PauseScreen()
    {
        //일시종료 UI 활성화

    }

}
