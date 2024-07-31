using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player_SwapManager : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private GameObject currentPlayer;
    public GameObject CurrentPlayer { get => currentPlayer; }

    [SerializeField] private Image currentMark1;
    [SerializeField] private Image currentMark2;
    [SerializeField] private Transform player1_Respawn_pivot;
    [SerializeField] private Transform player2_Respawn_pivot;
    private bool isRespawn_player1 = false;
    private bool isRespawn_player2 = false;
    private float respawnTime = 5f;
    private float player1_currentTime;
    private float player2_currentTime;

    [SerializeField] private GameObject player1_respawnObj;
    [SerializeField] private GameObject player2_respawnObj;
    private Text player1_respawnTimeText;
    private Text player2_respawnTimeText;


    private void Awake()
    {
        //���� �ʿ��� ������ ������Ʈ ��Ź�帳�ϴ�. �ش� ������ ������Ʈ�� ã�� �Ѱ̴ϴ�. 
        //������ �ʿ����� �����հ� if�� �ɱ�?

        player1_Respawn_pivot = GameObject.Find("player1_Respawn_pivot").transform;
        player2_Respawn_pivot = GameObject.Find("player2_Respawn_pivot").transform;

        player1.transform.position = player1_Respawn_pivot.position;
        player2.transform.position = player2_Respawn_pivot.position;

        player1_respawnTimeText = player1_respawnObj.transform.GetChild(0).GetComponent<Text>();
        player2_respawnTimeText = player2_respawnObj.transform.GetChild(0).GetComponent<Text>();

    }
    void Start()
    {
        currentPlayer = player1;
        SetActivePlayer(player1);
        currentMark1.enabled = true;
        currentMark2.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SwapCharacter();
        }


        if (currentPlayer != null && currentPlayer.transform.position.y < -2f)
        {
            StartCoroutine(FallingCharacter());
        }

        if (currentPlayer == null)
        {
            if (!isRespawn_player1)
            {
                SetActivePlayer(player1);
            }
            else if (!isRespawn_player2)
            {
                SetActivePlayer(player2);
            }
        }

    }

    public IEnumerator FallingCharacter()
    {
        // �������� ������Ʈ�� �������� 5�� �ִٰ� 
        // Ư�� ��ġ�� �ٽ� �����Ѵ� 

        if (currentPlayer == player1)
        {
            if (!isRespawn_player2)
            {
                SwapCharacter();
            }
            else
            {
                currentPlayer = null;
            }

            isRespawn_player1 = true;
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
                yield return null;
            }

            player1.transform.position = player1_Respawn_pivot.position;
            player1_respawnObj.SetActive(false);
            isRespawn_player1 = false;

            yield break;
        }
        else
        {
            if (!isRespawn_player1)
            {
                SwapCharacter();
            }
            else
            {
                currentPlayer = null;
            }

            isRespawn_player2 = true;
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
                yield return null;
            }

            player2.transform.position = player2_Respawn_pivot.position;
            player2_respawnObj.SetActive(false);
            isRespawn_player2 = false;
            yield return null;
        }


        if (currentPlayer == null)
        {

            SetActivePlayer(player1);
        }

    }

    public void SwapCharacter()
    {

        if (currentPlayer == player1)
        {
            if (!isRespawn_player1)
            {
                SetActivePlayer(player2);
                var ani = player1.transform.GetComponent<Animator>();
                ani.SetBool("IsWalking", false);
            }
        }
        else if (currentPlayer == player2)
        {
            if (!isRespawn_player2)
            {
                SetActivePlayer(player1);
                var ani = player2.transform.GetComponent<Animator>();
                ani.SetBool("IsWalking", false);
            }
        }
    }

    void SetActivePlayer(GameObject player)
    {

        player1.GetComponent<Player_Movent>().enabled = player == player1;
        player1.GetComponent<PlayerStateControl>().enabled = player == player1;
        currentMark1.enabled = player == player1;

        player2.GetComponent<Player_Movent>().enabled = player == player2;
        player2.GetComponent<PlayerStateControl>().enabled = player == player2;
        currentMark2.enabled = player == player2;

        currentPlayer = player;

    }

    public void AniWalkingSetbool(GameObject player)
    {
        var ani = player.transform.GetComponent<Animator>();
        ani.SetBool("IsWalking", false);
    }
}
