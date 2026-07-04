using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LowerLaserController: LaserController
{
    [SerializeField] private Transform trigger1;
    [SerializeField] private Transform trigger2;
    [SerializeField] private LayerMask playerLayer;
    private BoxCollider2D laserCollider;
    private int laserStage = 0;

    protected override void Awake()
    {
        base.Awake();
        laserCollider = GetComponent<BoxCollider2D>();
        laserCollider.enabled = false;
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
            ActivateLaser();
            laserCollider.enabled = true;
            laserStage++;
        }
        else if (transform.position.x < trigger2.position.x && laserStage==1)
        {
            laserCollider.enabled = false;
            laserStage++;
        }
    }

    protected override void ResetLaser()
    {
        base.ResetLaser();
        laserStage = 0;
    }

    void ActivateLaser()
    {
        laserCollider.enabled = true;

        // 关键：开启瞬间检测玩家是否已经在里面
        Collider2D player = Physics2D.OverlapBox(
            laserCollider.bounds.center,
            laserCollider.bounds.size,
            0f,
            playerLayer
        );

        if (player != null)
        {
            Vector2 position = player.transform.position;
            player.transform.position = new Vector2(position.x + laserCollider.bounds.size.x, position.y);
        }
    }
}
