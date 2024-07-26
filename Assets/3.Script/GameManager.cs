using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int stage_index;
    public bool isPause = false; // �Ͻ����� ����
    public bool isPlaying = true; // ���� ���� ����
    public GameObject pauseScreen; // �Ͻ����� ȭ��
    public int isInputEnabled = 0; // �Է� ���� ����
    public WorldState worldState;
    public int isFire = 0;

    
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
    public void LoadScene(string SceneName)
    {
        LoadingSceneManager.LoadScene(SceneName);
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
        //worldState.ResetState(1);
        this.stage_index = stage_index;
        switch (stage_index)
        {
            case 1:
                LoadingSceneManager.LoadScene("Stage1");
                break;
        }
    }

    //IntroScene Button Onclick Method
    public void Menu_Button(int button_index)
    {
        MenuController menu = GameObject.Find("Menu_Panel").GetComponent<MenuController>();
        switch (button_index)
        {
            case 0:
                LoadingSceneManager.LoadScene("BSJScene");
                break;
            case 1:
                LoadingSceneManager.LoadScene("Arcade_Menu");
                break;
            case 2:
                menu.CharacterChoose();
                break;
            case 3:
                menu.Active_Audio_UI();
                break;
            case 4:
                GameObject shutter = GameObject.Find("m_van_02").transform.Find("m_vanshutter_01_0").gameObject;

                shutter.SetActive(true);
                shutter.GetComponent<Animator>().SetTrigger("Close");
                break;
            default:
                return;
        }
    }

    public void Enter_Game(int button_index)
    {
        if (button_index == 0)
        {
            //���ο� ���� �����ϴ� ����
        }
        else
        {
            //���� ���� �̾ �ϴ� ����
        }
    }
}
