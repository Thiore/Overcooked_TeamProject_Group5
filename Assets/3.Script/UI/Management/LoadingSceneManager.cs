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
        // 싱글톤 패턴 구현
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
        // Title과 Complete_Text 변수 할당
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
            Debug.LogError("Title 오브젝트를 찾을 수 없습니다.");
        }

        if (Complete_Text != null)
        {
            Complete_Text.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Complete_Text 오브젝트를 찾을 수 없습니다.");
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

        // 로딩이 끝날 때까지 대기
        while (!op.isDone)
        {
            if (op.progress >= 0.9f)
            {
                if (Complete_Text != null)
                {
                    Complete_Text.gameObject.SetActive(true);
                }

                // Space 키 입력 대기
                while (!Input.GetKeyDown(KeyCode.Space))
                {
                    yield return null;
                }
                // Space 키 입력 후 씬 활성화
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
