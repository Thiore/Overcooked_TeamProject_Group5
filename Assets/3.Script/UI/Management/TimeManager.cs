using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float initTime = 60f; // �ʱ� �ð� ����
    public float playTime = 0;   // �÷��� �ð�
    public int tip = 1;          // �� ���
    private float tipTime = 0;   // �� ������ ���� �ð� ����
    private Animator timeAni;    // �ð� ���� �ִϸ��̼��� ���� �ִϸ�����
    private Text timeText;       // ���� �ð��� ǥ���� �ؽ�Ʈ UI

    private void Start()
    {
        // �ִϸ����Ϳ� �ؽ�Ʈ ������Ʈ ��������
        timeAni = GetComponent<Animator>();
        timeText = GetComponentInChildren<Text>();

        // ���� �Ŵ����� �Ͻ����� �� �÷��� ���� �ʱ�ȭ
        GameManager.Instance.isPause = false;
        GameManager.Instance.isPlaying = true;
    }

    private void Update()
    {
        // ������ �Ͻ����� ���°� �ƴϰ� �÷��� ���� ���� ����
        if (!GameManager.Instance.isPause && GameManager.Instance.isPlaying)
        {
            // ��Ÿ �ð��� ���� �÷��� �ð��� �� �ð��� ����
            tipTime += Time.deltaTime;
            playTime += Time.deltaTime;

            // ���� �ð� ���
            float remainingTime = initTime - playTime;

            // �� �ð��� 20�� �̻��� ��� �� ��� ����
            if (tipTime >= 20)
            {
                tip += 1;
                tipTime = 0f;
            }

            // ���� �ð��� 0 ������ ��� ���� ���� / //�ӽ÷� �����ΰڽ��ϴ�. (����)
            if (remainingTime <= 0)
            {
                playTime = 0;
                GameManager.Instance.EndGame();
            }

            // ���� �ð� UI ������Ʈ
            UpdateTimeDisplay(remainingTime);
        }
    }

    // ���� �ð��� UI�� ������Ʈ�ϴ� �޼���
    private void UpdateTimeDisplay(float remainingTime)
    {
        // ���� �ð��� 30�� ������ ��� �ִϸ��̼� Ʈ���� ����
        if (remainingTime <= 30f && remainingTime >= 29f)
        {
            timeAni.SetBool("isThirtySecLeft", true);
        }
        // ���� �ð��� 10�� ������ ��� �ִϸ��̼� Ʈ���� ����
        else if (remainingTime <= 10f && remainingTime >= 9f)
        {
            timeAni.SetBool("isThirtySecLeft", false);
            timeAni.SetBool("isTenSecLeft", true);
        }

        // ���� �ð��� �а� �ʷ� ��ȯ�Ͽ� �ؽ�Ʈ UI�� ǥ��
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
