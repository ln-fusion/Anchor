using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Hook: MonoBehaviour{
    public int obstacleCollisionType;//0 未击中 1 击中正常 2击中不让勾上去
    public bool isBeingRetracted;//正在被收回
    public Rigidbody2D rb;
    public Collider2D colider;
    public GameObject handPosMarker;
    public GameObject player;
    public Player playerScript;
    public float retractingSpeed;
    public float2 shootingVelocity;//出去的速度

    private bool shouldPlaySoundOnHittingObstacles;
    public SoundManager.SoundManager soundManager;
    void Start(){
        obstacleCollisionType=0;
        isBeingRetracted=false;
        rb=GetComponent<Rigidbody2D>();
        shouldPlaySoundOnHittingObstacles=true;
    }
    void OnCollisionEnter2D(Collision2D collision2D){
        if(shouldPlaySoundOnHittingObstacles){
            //soundManager.PlaySFXReplace("AnchorHit");
        }
        shouldPlaySoundOnHittingObstacles=false;
        obstacleCollisionType=1;
        if(collision2D.gameObject.TryGetComponent<ObstacleComponentData>(out var tmp)){
            if(!tmp.allowHooks){
                obstacleCollisionType=2;
            }
        }
    }
    void Update(){
        if(!isBeingRetracted){
            if(obstacleCollisionType==1){
                rb.velocity=new Vector2(0,0);
            }
            else if(obstacleCollisionType==0){
                rb.velocity=shootingVelocity;
            }
            else{//其他，这里设计得不好
                rb.velocity=new Vector2(0,0);
            }
        }
        else{
            rb.velocity=math.normalizesafe(
                new float2(
                    handPosMarker.transform.position.x-transform.position.x,
                    handPosMarker.transform.position.y-transform.position.y
                )
            )*retractingSpeed;
            if(math.distance(handPosMarker.transform.position,transform.position)<0.5){//这里没法用碰撞了
                playerScript.hookHasBeenRetracted=true;
            }
        }
    }
}
