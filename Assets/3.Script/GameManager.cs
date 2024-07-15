using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0; // ���� ����
    public int tip = 1; // �� ���
    public bool isPause = false; // �Ͻ����� ����
    public bool isPlaying = true; // ���� ���� ����
    public GameObject pauseScreen; // �Ͻ����� ȭ��
    public bool isInputEnabled = false; // �Է� ���� ����

    public int addScoreCount = 0; // ������ ���� Ƚ��
    public int subScoreCount = 0; // ������ ���� Ƚ��
    public int subScore = 0; // ������ ���� �հ�

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

    // Ư�� ���� �񵿱� �ε��ϴ� �޼���
    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneCoroutine(index));
    }

    // �񵿱� �ε� �ڷ�ƾ
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

    // ������ �߰��ϴ� �޼���
    public void AddScore(int points)
    {
        score += points;
        addScoreCount++; // ������ ���� Ƚ�� ����
        ScoreManager.Instance.AddScore(points); // ScoreManager�� ���� �߰�
    }

    // ������ �����ϴ� �޼���
    public void SubScore(int points)
    {
        score -= points;
        subScoreCount++; // ������ ���� Ƚ�� ����
        subScore += points; // ������ ���� �հ� ����
        tip = 1; // �� ��� �ʱ�ȭ
        ScoreManager.Instance.SubScore(points); // ScoreManager�� ���� ����
    }

    // ���� ������ �����ϴ� �޼���
    public void EnterGame()
    {
        isPlaying = true;
        SceneManager.LoadScene("GameScene");
    }

    // ���� ���� �޼���
    public void EndGame()
    {
        isPause = true;
        isPlaying = false;
        SceneManager.LoadScene("ResultScene");
    }

    // �Ͻ����� ȭ�� Ȱ��ȭ �޼���
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
        Debug.Log("AllCorrect_Recipe ȣ��");
    }
    public void Incorrect_Recipe()
    {
        ScoreManager.Instance.AddScore(10);
        tip = 1;
        Debug.Log("InCorrect_Recipe ȣ��");
    }
    public void Wrong_Recipe()
    {
        ScoreManager.Instance.SubScore(10);
        tip = 1;
        Debug.Log("Wrong_Recipe ȣ��");
    }



}
