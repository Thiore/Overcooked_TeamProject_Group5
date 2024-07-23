using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static LoadingSceneManager Instance { get; private set; }
    public static string nextScene;
    private Text Title;
    private Text Complete_Text;

    private void Awake()
    {
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Title�� Complete_Text ���� �Ҵ�
        Title = GameObject.Find("Title").GetComponent<Text>();
        Complete_Text = GameObject.Find("Complete_Text").GetComponent<Text>();

        if (Title != null)
        {
            if (GameManager.Instance.stage_index != 0) { 
                Title.text = $"Stage {GameManager.Instance.stage_index}";
            }
            else
            {
                Title.text = "L Shift to Switch";
            }
        }
        else
        {
            Debug.LogError("Title ������Ʈ�� ã�� �� �����ϴ�.");
        }

        if (Complete_Text != null)
        {
            Complete_Text.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Complete_Text ������Ʈ�� ã�� �� �����ϴ�.");
        }

        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        // �ε��� ���� ������ ���
        while (!op.isDone)
        {
            if (op.progress >= 0.9f)
            {
                if (Complete_Text != null)
                {
                    Complete_Text.gameObject.SetActive(true);
                }

                // Space Ű �Է� ���
                while (!Input.GetKeyDown(KeyCode.Space))
                {
                    yield return null;
                }
                // Space Ű �Է� �� �� Ȱ��ȭ
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
