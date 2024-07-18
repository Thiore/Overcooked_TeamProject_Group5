using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private Rigidbody player;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out player);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        player.MovePosition(player.position +new Vector3(x, 0, y)*Time.deltaTime*4f);
        
    }
}
