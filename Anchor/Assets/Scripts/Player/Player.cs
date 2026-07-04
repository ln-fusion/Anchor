using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


//已知的问题：
//人可以借助这个东西把自己卡在距离钩爪有一定距离的墙上。这是特性！不用修复！
//可以通过反复拉来拉去把切向速度拉到很高
//命名==一坨

//钩住东西的钩爪不会在很长一段时间之后收回，是故意的

public enum PlayerState: int{//状态机
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
    public GameObject chainPrefab;
    private GameObject chain;
    private Hook hookScript;

    private PlayerState state;//状态机

    [Header("抓钩参数设置")]
    [SerializeField]private float shootingCooldown=0.2f;//两次发射钩爪之间需要间隔一段时间
    [SerializeField] private float retractingCooldown = 1f;//抓钩收回后至下一次发射的冷却时间
    [SerializeField]private float playerStateShootingHook_HookMaxLivingTime=3f;
    [SerializeField]private float playerStateRetractingHook_HookMaxLivingTime=3f;
    private float remainingShootingCooldown;
    private float remainingRetractingCooldown;
    private float playerStateShootingHook_RemainingHookLivingTime;
    private float playerStateRetractingHook_RemainingHookLivingTime;
    

    private Vector3 defaultLocalScale;//朝右

    private const float hookSpeed=14f;

    private const float eps=1e-5f;

    private const float hookRetractingSpeed=28f;
    public bool hookHasBeenRetracted;

    //钩子拉的那一刻，
    //法向速度不变，径向速度如果小于下面这个东西就会被重置为它，这里的速度有正负之分
    //接下来会获得一个持续的朝向钩子的加速度
    //这里我吸的都是人而不是人的手
    [Header("抓钩的手感参数")]
    [Header("1.径向初速度")]
    public float beingPulledToHook_Vr0=8f;//径向初速度
    [Header("2.朝向钩爪的加速度")]
    public float beingPulledToHook_A=4f;//朝向钩爪的加速度
    [Header("3.径向速度上限，超过无法加速（不代表最高速度）")]
    public float beingPulledToHook_VrLimit=8f;//径向速度上限，超过了就不能加速了（不代表最高速度就是这个东西）
    [Header("4.抓钩长度")]
    [SerializeField] private float hookLength=7.5f;

    public Rigidbody2D rb;

    [SerializeField] private SoundManager.SoundManager soundManager;

    void DestroyHookAndReset(){
        Destroy(hook);
        hook=null;
        hookScript=null;
        hookHasBeenRetracted=true;
        Destroy(chain);
        chain=null;
    }


    void Awake(){
        state=PlayerState.Idle;
        remainingShootingCooldown=0;
        playerStateShootingHook_RemainingHookLivingTime=0;
        playerStateRetractingHook_RemainingHookLivingTime=0;
        defaultLocalScale=transform.localScale;
        hook=null;
        hookScript=null;
        hookHasBeenRetracted=true;
        rb=GetComponent<Rigidbody2D>();
    }

    void Update(){
        //杂项
        var dt=Time.deltaTime;
        Vector2 screenCenterPos=new Vector2(Screen.width,Screen.height)/2.0f;
        //更新
        remainingShootingCooldown=math.max(0,remainingShootingCooldown-dt);
        remainingRetractingCooldown = math.max(0, remainingRetractingCooldown - dt);
        playerStateRetractingHook_RemainingHookLivingTime =math.max(0,playerStateRetractingHook_RemainingHookLivingTime-dt);
        playerStateShootingHook_RemainingHookLivingTime=math.max(0,playerStateShootingHook_RemainingHookLivingTime-dt);
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
                if(Input.GetMouseButtonDown(0) && math.abs(remainingShootingCooldown)<eps&&math.abs(remainingRetractingCooldown)<eps){//左键+已冷却
                    //尝试发射钩子，如果归一化出现异常则不发射
                    var handPosMarkerScreenPos=mainCamera.WorldToScreenPoint(handPosMarker.transform.position);
                    var dir=math.normalizesafe(new float2(Input.mousePosition.x-handPosMarkerScreenPos.x,Input.mousePosition.y-handPosMarkerScreenPos.y));
                    if(math.length(dir)>eps){//否则视作异常
                        var deg=-math.atan2(dir.y,dir.x)*(180.0f/math.PI);//Deg!
                        hook=Instantiate(
                            hookPrefab,
                            new Vector3(handPosMarker.transform.position.x,handPosMarker.transform.position.y,-0.2f),
                            Quaternion.Euler(0,0,deg)
                        );
                        hookScript=hook.GetComponent<Hook>();
                        hookScript.player=gameObject;
                        hookScript.playerScript=this;
                        hookScript.handPosMarker=handPosMarker;
                        hookScript.retractingSpeed=hookRetractingSpeed;
                        hookScript.shootingVelocity=dir*hookSpeed;
                        hookScript.colider=hook.GetComponent<CircleCollider2D>();
                        hookScript.soundManager=soundManager;
                        chain=Instantiate(chainPrefab,new Vector3(0,-114514,0),Quaternion.Euler(0,0,0));//别管具体填了什么坐标和角度，不重要
                        hookHasBeenRetracted=false;
                        soundManager.PlaySFXReplace("AnchorShoot");
                        //更新冷却
                        remainingShootingCooldown=shootingCooldown;
                        playerStateShootingHook_RemainingHookLivingTime=playerStateShootingHook_HookMaxLivingTime;
                        //状态转移
                        state=PlayerState.ShootingHook;
                    }
                }
                break;
            case PlayerState.ShootingHook:
                if(hookScript.obstacleCollisionType==1){//拉
                    //处理径向速度
                    r=new float2(
                        hook.transform.position.x-transform.position.x,
                        hook.transform.position.y-transform.position.y
                    );//人->钩子
                    v=new float2(rb.velocity.x,rb.velocity.y);//原来的V_人
                    vr=math.projectsafe(v,r);//v在r的径向上的分量
                    vn=v-vr;//v在r的法向上的分量
                    er=math.normalizesafe(r);//r方向上的单位向量，与r同方向
                    signedAbsVr=math.dot(vr,er);//vr的有方向的模长，正负根据r的方向决定
                    float2 newV=er*math.max(signedAbsVr,beingPulledToHook_Vr0)+vn;//好了喵
                    rb.velocity=newV;
                    //状态转移
                    rb.gravityScale=0;
                    state=PlayerState.BeingPulledToHook;
                }
                else if(
                    Input.GetMouseButtonDown(0) ||
                    math.distance(hook.transform.position,transform.position)>=hookLength ||
                    hookScript.obstacleCollisionType==2
                ){//撤回钩子
                    hookScript.isBeingRetracted=true;
                    hookScript.colider.enabled=false;
                    playerStateRetractingHook_RemainingHookLivingTime=playerStateRetractingHook_HookMaxLivingTime;
                    //状态转移
                    state=PlayerState.RetractingHook;
                }
                else if(hookScript.obstacleCollisionType==0 && math.abs(playerStateShootingHook_RemainingHookLivingTime)<eps){
                    DestroyHookAndReset();
                    //状态转移
                    state=PlayerState.Idle;
                }
                break;
            case PlayerState.RetractingHook:
                if(hookHasBeenRetracted){
                    DestroyHookAndReset();
                    //状态转移
                    state=PlayerState.Idle;
                }
                else if(math.abs(playerStateRetractingHook_RemainingHookLivingTime)<eps){
                    DestroyHookAndReset();
                    //状态转移
                    state=PlayerState.Idle;
                }
                break;
            case PlayerState.BeingPulledToHook:
                if(Input.GetMouseButtonDown(0)){//撤回钩子
                    hookScript.isBeingRetracted=true;
                    hookScript.colider.enabled=false;
                    playerStateRetractingHook_RemainingHookLivingTime=playerStateRetractingHook_HookMaxLivingTime;
                    //状态转移
                    rb.gravityScale=1;
                    remainingRetractingCooldown = retractingCooldown;
                    state =PlayerState.RetractingHook;
                    break;
                }
                //处理径向速度
                r=new float2(
                    hook.transform.position.x-transform.position.x,
                    hook.transform.position.y-transform.position.y
                );//人->钩子
                v=new float2(rb.velocity.x,rb.velocity.y);//原来的V_人
                vr=math.projectsafe(v,r);//v在r的径向上的分量
                vn=v-vr;//v在r的法向上的分量
                er=math.normalizesafe(r);//r方向上的单位向量，与r同方向
                signedAbsVr=math.dot(vr,er);//vr的有方向的模长，正负根据r的方向决定
                if(signedAbsVr>=beingPulledToHook_VrLimit){
                    break;
                }
                float mass=rb.mass;
                rb.AddForce(
                    mass*beingPulledToHook_A*
                    math.normalizesafe(new float2(hook.transform.position.x-transform.position.x,hook.transform.position.y-transform.position.y))
                );
                break;
            default:
                Debug.LogError("?????????");
                break;
        }
        //
        if(chain!=null){
            float2 tmp=new (
                handPosMarker.transform.position.x-hook.transform.position.x,
                handPosMarker.transform.position.y-hook.transform.position.y
            );
            float2 dir=math.normalizesafe(tmp);
            if(math.length(dir)>1e-3f){
                float chainLength=math.length(tmp);
                chain.transform.localScale=new Vector3(1f,chainLength,1f);
                var deg=math.atan2(dir.y,dir.x)*(180.0f/math.PI)-90;
                chain.transform.rotation=Quaternion.Euler(0,0,deg);
                var tmpPos=(hook.transform.position+handPosMarker.transform.position)/2;
                tmpPos.z=-0.1f;
                chain.transform.position=tmpPos;
            }
        }
    }
}
