using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationController : MonoBehaviour
{
    private Vector2 mousePosition = Vector2.zero;
    private bool isFacingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        FlipCheck();
    }

    private void FlipCheck()
    {
        if(mousePosition.x < transform.position.x && isFacingRight)
        {
            Flip();
        }else if(mousePosition.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }
}
