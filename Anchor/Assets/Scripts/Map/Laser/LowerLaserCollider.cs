using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerLaserController: LaserController
{
    [SerializeField] private Transform trigger1;
    [SerializeField] private Transform trigger2;
    private BoxCollider2D boxCollider;
    private int laserStage = 0;

    protected override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        if(trigger1 == null||trigger2 == null)
        {
            Debug.LogError("触发器未设置");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(transform.position.x < trigger1.position.x && laserStage==0)
        {
            boxCollider.enabled = true;
            laserStage++;
        }
        else if (transform.position.x < trigger2.position.x && laserStage==1)
        {
            boxCollider.enabled = false;
            laserStage++;
        }
    }

    protected override void ResetLaser()
    {
        base.ResetLaser();
        laserStage = 0;
    }
}
