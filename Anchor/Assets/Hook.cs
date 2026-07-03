using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Hook : MonoBehaviour{
    public bool hasCollidedWithObstacles;//一旦发生碰撞，被收回之前就不要再动了
    public bool isBeingRetracted;//正在被收回
    public Rigidbody2D rigidbody2D;
    public GameObject handPosMarker;
    public GameObject player;
    public Player playerScript;
    public float retractingSpeed;
    public float2 shootingVelocity;//出去的速度
    void Start(){
        hasCollidedWithObstacles=false;
        isBeingRetracted=false;
        rigidbody2D=GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision2D){
        hasCollidedWithObstacles=true;
    }
    void Update(){
        if(!isBeingRetracted){
            if(hasCollidedWithObstacles){
                rigidbody2D.velocity=new Vector2(0,0);
            }
            else{
                rigidbody2D.velocity=shootingVelocity;
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
