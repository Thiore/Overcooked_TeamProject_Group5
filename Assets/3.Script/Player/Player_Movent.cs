using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class Player_Movent : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 90f;
    [SerializeField] private PlayerIntput _playerInput;

    private Rigidbody _player_rb;
    private Animator _animator;
    private Vector3 moveDirection;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerIntput>();
        _player_rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Walking();
    }

    private void Walking()
    {
        if (_playerInput.Move_Value != 0 || _playerInput.Rotate_Value != 0)
        {
            _animator.SetBool("IsWalking", true);
            Move();
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }

    private void Move()
    {
        // 회전하고 돌고 이전 값과 동일하면 돌지 않는다 ? 

        //회전
        float x = 0;
        float y = 0;
        if (x < 1 && x > -1)
        {
            x = _playerInput.Rotate_Value * _rotateSpeed * Time.deltaTime;
        }

        if(y < 1 && y > -1)
        {
            y = _playerInput.Move_Value * _rotateSpeed * Time.deltaTime;
        }
        this.transform.LookAt(transform.position + new Vector3(x, 0, y).normalized);

        //이동
        moveDirection = new Vector3(_playerInput.Rotate_Value, 0, _playerInput.Move_Value) * _moveSpeed * Time.deltaTime;
        _player_rb.MovePosition(_player_rb.position + moveDirection);

    }

    private void Rotate()
    {
        float turn = _playerInput.Rotate_Value * _rotateSpeed * Time.deltaTime;
        _player_rb.rotation = _player_rb.rotation * Quaternion.Euler(0, turn, 0);

    }


}
