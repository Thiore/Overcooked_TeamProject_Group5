using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_SwapManager : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private GameObject currentPlayer;

    [SerializeField] private Image currentMark1;
    [SerializeField] private Image currentMark2;
    [SerializeField] private Transform player1_Respawn_pivot;
    [SerializeField] private Transform player2_Respawn_pivot;
    private bool isRespawn = false;
    private float respawnTime = 5f;
    private float player1_currentTime;
    private float player2_currentTime;

    [SerializeField] private GameObject player1_respawnObj;
    [SerializeField] private GameObject player2_respawnObj;
    private Text player1_respawnTimeText;
    private Text player2_respawnTimeText;

    private Coroutine coroutine;

    private void Awake()
    {
        //���� �ʿ��� ������ ������Ʈ ��Ź�帳�ϴ�. �ش� ������ ������Ʈ�� ã�� �Ѱ̴ϴ�. 
        //������ �ʿ����� �����հ� if�� �ɱ�?

        player1_Respawn_pivot = GameObject.Find("player1_Respawn_pivot").transform;
        player2_Respawn_pivot = GameObject.Find("player2_Respawn_pivot").transform;

        player1_respawnTimeText = player1_respawnObj.transform.GetChild(0).GetComponent<Text>();
        player2_respawnTimeText = player2_respawnObj.transform.GetChild(0).GetComponent<Text>();
    }
    void Start()
    {
        currentPlayer = player1;
        currentMark1.enabled = true;
        currentMark2.enabled = false;
        SetActivePlayer(player1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SwapCharacter();
        }


        if (currentPlayer.transform.position.y < -2f)
        {
            StartCoroutine(FallingCharacter());
        }

    }

    public IEnumerator FallingCharacter()
    {
        // �������� ������Ʈ�� �������� 5�� �ִٰ� 
        // Ư�� ��ġ�� �ٽ� �����Ѵ� 
        // 

        if (currentPlayer == player1)
        {
            SwapCharacter();

            player1.transform.position = player1_Respawn_pivot.position;
            player1.transform.rotation = Quaternion.identity;
            player1_respawnObj.SetActive(true);
            player1.GetComponent<Player_Movent>().enabled = false;
            player1.GetComponent<PlayerStateControl>().enabled = false;
            player1_currentTime = respawnTime;
            while (0f <= player1_currentTime)
            {
                player1_currentTime -= Time.deltaTime;
                player1_respawnTimeText.text = player1_currentTime.ToString(string.Format("N0"));
                Debug.Log(player1_currentTime);
                yield return new WaitForEndOfFrame();
            }

            player1.transform.position = player1_Respawn_pivot.position;
            player1_respawnObj.SetActive(false);
            yield return new WaitForSeconds(respawnTime);
        }
        else
        {
            SwapCharacter();

            player2.transform.position = player2_Respawn_pivot.position;
            player2.transform.rotation = Quaternion.identity;
            player2_respawnObj.SetActive(true);
            player2.GetComponent<Player_Movent>().enabled = false;
            player2.GetComponent<PlayerStateControl>().enabled = false;
            player2_currentTime = respawnTime;
            while (0f <= player2_currentTime)
            {
                player2_currentTime -= Time.deltaTime;
                player2_respawnTimeText.text = player2_currentTime.ToString(string.Format("N0"));
                Debug.Log(player2_currentTime);
                yield return new WaitForEndOfFrame();
            }


            player2.transform.position = player2_Respawn_pivot.position;
            player2_respawnObj.SetActive(false);
            yield return new WaitForSeconds(respawnTime);
        }

        yield return null;
    }

    public void SwapCharacter()
    {
        if (currentPlayer == player1)
        {
            SetActivePlayer(player2);
            currentMark1.enabled = false;
            currentMark2.enabled = true;
            var ani = player1.transform.GetComponent<Animator>();
            ani.SetBool("IsWalking", false);
        }
        else
        {
            SetActivePlayer(player1);
            currentMark1.enabled = true;
            currentMark2.enabled = false;
            var ani = player2.transform.GetComponent<Animator>();
            ani.SetBool("IsWalking", false);
        }
    }

    void SetActivePlayer(GameObject player)
    {
        player1.GetComponent<Player_Movent>().enabled = player == player1;
        player1.GetComponent<PlayerStateControl>().enabled = player == player1;

        player2.GetComponent<Player_Movent>().enabled = player == player2;
        player2.GetComponent<PlayerStateControl>().enabled = player == player2;

        currentPlayer = player;
    }
}
