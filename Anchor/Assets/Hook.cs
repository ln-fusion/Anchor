using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Hook: MonoBehaviour{
    public int obstacleCollisionType;//0 未击中 1 击中正常 2击中不让勾上去
    public bool isBeingRetracted;//正在被收回
    public Rigidbody2D rigidbody2D;
    public Collider2D collider2D;
    public GameObject handPosMarker;
    public GameObject player;
    public Player playerScript;
    public float retractingSpeed;
    public float2 shootingVelocity;//出去的速度
    void Start(){
        obstacleCollisionType=0;
        isBeingRetracted=false;
        rigidbody2D=GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision2D){
        obstacleCollisionType=1;
        if(!collision2D.gameObject.GetComponent<ObstacleComponentData>().allowHooks){
            obstacleCollisionType=2;
        }
    }
    void Update(){
        if(!isBeingRetracted){
            if(obstacleCollisionType==1){
                rigidbody2D.velocity=new Vector2(0,0);
            }
            else if(obstacleCollisionType==0){
                rigidbody2D.velocity=shootingVelocity;
            }
            else{//其他，这里设计得不好
                rigidbody2D.velocity=new Vector2(0,0);
            }
        }
        else{
            rigidbody2D.velocity=math.normalizesafe(
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
