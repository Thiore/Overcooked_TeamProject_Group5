using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    public int tip = 1;
    public bool isPause = false;
    public bool isPlaying = true;
    public GameObject pauseScreen;
    public bool isInputEnabled = false;
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

    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneCoroutine(index));
    }

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

    public void AddScore(int points)
    {
        score += points * tip;
    }

    public void SubScore(int points)
    {
        score -= points;
        tip = 1;
    }

    public void EnterGame()
    {
        isPlaying = true;
        SceneManager.LoadScene("GameScene");
    }

    public void EndGame()
    {
        isPause = true;
        isPlaying = false;
        SceneManager.LoadScene("ResultScene");
    }

    private void PauseScreen()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
        }
    }
}
