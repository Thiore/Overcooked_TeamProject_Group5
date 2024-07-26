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
    }

    void SwapCharacter()
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
        player2.GetComponent<Player_StateController>().enabled = player == player2;

        currentPlayer = player;
    }
}
