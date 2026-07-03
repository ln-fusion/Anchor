using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField]private float speed = 5f;
    [SerializeField]private Transform leftBound = null;
    private Vector2 leftBoundPosition = Vector2.zero;
    private Vector2 startPostion = Vector2.zero;

    protected virtual void Awake()
    {
        startPostion = transform.position;
        if (leftBound == null)
        {
            Debug.LogWarning("No leftbound!");
        }
        else
        {
            leftBoundPosition = leftBound.position;
        }
    }

    protected virtual void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftBoundPosition.x)
        {
            ResetLaser();
        }
    }

    protected virtual void ResetLaser()
    {
        Vector3 pos = transform.position;
        pos = startPostion;
        transform.position = pos;
    }
}
