using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAC : MonoBehaviour
{
    public Animator animator;
    private bool isThrowing;
    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponentInChildren<Animator>();
        isThrowing= false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isThrowing", isThrowing);
    }
}
