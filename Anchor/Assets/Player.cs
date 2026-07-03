using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


//已知的问题：
//人可以借助这个东西把自己卡在距离钩爪有一定距离的墙上。这是特性！不用修复！
//钩爪飞得太远不会收回
//如果人跑得太快，且正好处于收回阶段，钩爪可能永远都不会被收回
//收回阶段没有限制时间
//可以通过反复拉来拉去把切向速度拉到很高
//钩爪收回的时候没有禁用collider导致收回障碍
//忘记实现钩爪长度了！！！！！！！！！！！！！！！！！！！！
//命名==一坨

//钩爪不会在很长一段时间之后收回，是故意的


public enum PlayerState:int{//状态机
    Idle,//空闲
    ShootingHook,//钩子往外飞//命名可能不是很好
    RetractingHook,//撤回钩子
    BeingPulledToHook//被钩子拉过去
}

public class Player : MonoBehaviour{
    public Camera mainCamera;

    public GameObject handPosMarker;//标记手的位置，人物左右翻转的时候处理起来比较方便

    public GameObject hookPrefab;
    private GameObject hook;
    private Hook hookScript;

    private PlayerState state;//状态机

    private const float shootingCooldown=0.2f;//两次发射钩爪之间需要间隔一段时间
    private float remainingShootingCooldown;
    private const float hookMaxLivingTime=5f;
    private float remainingHookLivingTime;
    

    private Vector3 defaultLocalScale;//朝右

    public const float hookSpeed=14f;

    private const float eps=1e-5f;

    public const float hookRetractingSpeed=28f;
    public bool hookHasBeenRetracted;

    //钩子拉的那一刻，
    //法向速度不变，径向速度如果小于下面这个东西就会被重置为它，这里的速度有正负之分
    //接下来会获得一个持续的朝向钩子的加速度
    //这里我吸的都是人而不是人的手
    public const float beingPulledToHook_Vr0=8f;//径向初速度
    public const float beingPulledToHook_A=10f;//朝向钩爪的加速度
    public const float beingPulledToHook_VrLimit=12f;//径向速度上限，超过了就不能加速了

    public Rigidbody2D rigidbody2D;
    void DestroyHookAndReset(){
        Destroy(hook);
        hook=null;
        hookScript=null;
        hookHasBeenRetracted=true;
    }


    void Awake(){
        state=PlayerState.Idle;
        remainingShootingCooldown=0;
        remainingHookLivingTime=0;
        defaultLocalScale=transform.localScale;
        hook=null;
        hookScript=null;
        hookHasBeenRetracted=true;
        rigidbody2D=GetComponent<Rigidbody2D>();
    }

    void Update(){
        //杂项
        var dt=Time.deltaTime;
        Vector2 screenCenterPos=new Vector2(Screen.width,Screen.height)/2.0f;
        //更新
        remainingShootingCooldown=math.max(0,remainingShootingCooldown-dt);
        remainingHookLivingTime=math.max(0,remainingHookLivingTime-dt);
        //更新人物朝向，默认为右
        Vector3 newLocalScale=defaultLocalScale;
        if(Input.mousePosition.x<screenCenterPos.x){
            newLocalScale.x=-newLocalScale.x;
        }
        transform.localScale=newLocalScale;
        //
        float2 v,r,vr,vn,er;
        float signedAbsVr;
        switch(state){
            case PlayerState.Idle:
                if(Input.GetMouseButtonDown(0) && math.abs(remainingShootingCooldown)<eps){//左键+已冷却
                    //尝试发射钩子，如果归一化出现异常则不发射
                    var handPosMarkerScreenPos=mainCamera.WorldToScreenPoint(handPosMarker.transform.position);
                    var dir=math.normalizesafe(new float2(Input.mousePosition.x-handPosMarkerScreenPos.x,Input.mousePosition.y-handPosMarkerScreenPos.y));
                    if(math.length(dir)>eps){//否则视作异常
                        var deg=-math.atan2(dir.y,dir.x)*(180.0f/math.PI);//Deg!
                        hook=Instantiate(hookPrefab,handPosMarker.transform.position,Quaternion.Euler(0,0,deg));
                        hookScript=hook.GetComponent<Hook>();
                        hookScript.player=gameObject;
                        hookScript.playerScript=this;
                        hookScript.handPosMarker=handPosMarker;
                        hookScript.retractingSpeed=hookRetractingSpeed;
                        hookScript.shootingVelocity=dir*hookSpeed;
                        hookHasBeenRetracted=false;
                        //更新冷却
                        remainingShootingCooldown=shootingCooldown;
                        remainingHookLivingTime=hookMaxLivingTime;
                        //状态转移
                        state=PlayerState.ShootingHook;
                    }
                }
                break;
            case PlayerState.ShootingHook:
                if(hookScript.hasCollidedWithObstacles){
                    //处理径向速度
                    r=new float2(
                        hook.transform.position.x-transform.position.x,
                        hook.transform.position.y-transform.position.y
                    );//人->钩子
                    v=new float2(rigidbody2D.velocity.x,rigidbody2D.velocity.y);//原来的V_人
                    vr=math.projectsafe(v,r);//v在r的径向上的分量
                    vn=v-vr;//v在r的法向上的分量
                    er=math.normalizesafe(r);//r方向上的单位向量，与r同方向
                    signedAbsVr=math.dot(vr,er);//vr的有方向的模长，正负根据r的方向决定
                    float2 newV=er*math.max(signedAbsVr,beingPulledToHook_Vr0)+vn;//好了喵
                    rigidbody2D.velocity=newV;
                    //
                    state=PlayerState.BeingPulledToHook;
                }
                else if(Input.GetMouseButtonDown(0)){//撤回钩子
                    hookScript.isBeingRetracted=true;
                    state=PlayerState.RetractingHook;
                }
                else if(!hookScript.hasCollidedWithObstacles && math.abs(remainingHookLivingTime)<eps){
                    DestroyHookAndReset();
                    state=PlayerState.Idle;
                }
                break;
            case PlayerState.RetractingHook:
                if(hookHasBeenRetracted){
                    DestroyHookAndReset();
                    state=PlayerState.Idle;
                }
                break;
            case PlayerState.BeingPulledToHook:
                if(Input.GetMouseButtonDown(0)){//撤回钩子
                    hookScript.isBeingRetracted=true;
                    state=PlayerState.RetractingHook;
                    break;
                }
                //处理径向速度
                r=new float2(
                    hook.transform.position.x-transform.position.x,
                    hook.transform.position.y-transform.position.y
                );//人->钩子
                v=new float2(rigidbody2D.velocity.x,rigidbody2D.velocity.y);//原来的V_人
                vr=math.projectsafe(v,r);//v在r的径向上的分量
                vn=v-vr;//v在r的法向上的分量
                er=math.normalizesafe(r);//r方向上的单位向量，与r同方向
                signedAbsVr=math.dot(vr,er);//vr的有方向的模长，正负根据r的方向决定
                if(signedAbsVr>=beingPulledToHook_VrLimit){
                    break;
                }
                float mass=rigidbody2D.mass;
                rigidbody2D.AddForce(
                    mass*beingPulledToHook_A*
                    math.normalizesafe(new float2(hook.transform.position.x-transform.position.x,hook.transform.position.y-transform.position.y))
                );
                break;
            default:
                Debug.LogError("?????????");
                break;
        }
        //
    }
}
