using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    private Text Title;
    private Text Complete_Text;
    private Image Tutorial;
    private Image Shift;


    private void Start()
    {
        InitializeUI();
        StartCoroutine(LoadScene());
    }

    private void InitializeUI()
    {
        // Title�� Complete_Text ���� �Ҵ�
        Title = GameObject.Find("Title").GetComponent<Text>();
        Complete_Text = GameObject.Find("Complete_Text").GetComponent<Text>();
        Tutorial = GameObject.Find("Tutorial").GetComponent<Image>();
        Shift = GameObject.Find("Shift").GetComponent<Image>();


        if (Title != null && Tutorial != null && Shift != null)
        {
            if (GameManager.Instance.stage_index != 0)
            {
                Title.text = $"Stage {GameManager.Instance.stage_index}";
                Debug.Log($"Stage{GameManager.Instance.stage_index}_Tutorial");
                Tutorial.sprite = Resources.Load<Sprite>($"Stage{GameManager.Instance.stage_index}_Tutorial");
                Shift.gameObject.SetActive(false);
            }
            else
            {
                Title.text = "L Shift to Switch";
                Tutorial.sprite = Resources.Load<Sprite>("Default_Tutorial");
                Shift.gameObject.SetActive(true);
            }
        }
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
