using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Vector2 moveDir=(Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position).normalized;
        if (Input.GetKey(KeyCode.Q))
        {
            rb.velocity = moveDir*5;
        }
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = moveDir*10;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rb.velocity = moveDir*20;
        }
    }
}
