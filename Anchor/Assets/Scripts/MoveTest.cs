using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [SerializeField] private float velocity = 20f;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckMoving();
    }

    void CheckMoving()
    {
        int moveDir = 0;
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir -= 1;
        }
        rb.velocity = new Vector2(velocity * moveDir, rb.velocity.y);
    }
}
