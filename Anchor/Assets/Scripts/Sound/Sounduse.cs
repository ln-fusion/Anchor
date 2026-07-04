using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounduse : MonoBehaviour
{
    [System.Serializable]
    public class CollisionRule
    {
        public string sfxName; // 要播放的音效名
        public string targetTag; // 可选：匹配 Tag
        public GameObject specificTarget; // 可选：匹配具体对象
    }

    [Header("Collision SFX Rules")]
    // 支持多个规则：一个物体可以配置多个触发对象/类型，每个规则可绑定不同的音效
    public List<CollisionRule> rules = new List<CollisionRule>();

    // 备用默认音效（当没有规则匹配时使用，留空则不播放）
    public string defaultSfxName = "";

    // 在 Enter 时触发
    public bool onlyOnEnter = true;

    [Header("Fire Settings")]
    public bool enableFireSound = false;
    public KeyCode fireKey = KeyCode.None; // 按键触发发射音效
    public string fireSfxName = ""; // 发射时播放的音效名
    public bool replaceOnFire = true; // 发射音效是否覆盖当前正在播放的 SFX（true 使用 PlaySFXReplace）

    // 公共方法：判断与 other 是否匹配任一规则，若匹配则切换对应音效（覆盖播放）
    public void CheckAndPlayCollision(GameObject other)
    {
        Debug.Log("碰撞触发了！对象: " + other.name);

        if (other == null)
            return;

        var mgr = SoundManager.SoundManager.Instance;
        if (mgr == null)
            return;

        // 遍历规则，优先按 targetTag 匹配
        foreach (var rule in rules)
        {
            if (rule == null)
                continue;

            if (!string.IsNullOrEmpty(rule.targetTag))
            {
                if (other.CompareTag(rule.targetTag))
                {
                    mgr.PlaySFXReplace(rule.sfxName);
                    Debug.Log($"[Sounduse] 规则匹配 Tag '{rule.targetTag}'，播放音效: {rule.sfxName} (对象: {other.name})");
                    return;
                }
            }
            else if (rule.specificTarget != null)
            {
                if (other == rule.specificTarget)
                {
                    mgr.PlaySFXReplace(rule.sfxName);
                    Debug.Log($"[Sounduse] 规则匹配 特定对象 '{rule.specificTarget.name}'，播放音效: {rule.sfxName} (对象: {other.name})");
                    return;
                }
            }
        }

        // 没有规则匹配时，使用默认音效（若有）
        if (!string.IsNullOrEmpty(defaultSfxName))
        {
            mgr.PlaySFXReplace(defaultSfxName);
            Debug.Log($"[Sounduse] 无规则匹配，播放默认音效: {defaultSfxName} (对象: {other.name})");
        }
    }

    // 仅保留 2D 物理回调（本项目使用 2D 物理）

    // 2D 物理回调：如果场景使用的是 2D 物理（Rigidbody2D / Collider2D），需要这些方法
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckAndPlayCollision(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!onlyOnEnter)
            CheckAndPlayCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Sounduse] OnTriggerEnter2D: {gameObject.name} 与 {other.gameObject.name} 触发 (tag={other.gameObject.tag})");
        CheckAndPlayCollision(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!onlyOnEnter)
            Debug.Log($"[Sounduse] OnTriggerStay2D: {gameObject.name} 与 {other.gameObject.name} 持续触发 (tag={other.gameObject.tag})");
            CheckAndPlayCollision(other.gameObject);
    }

    private void Update()
    {
        if (!enableFireSound)
            return;

        if (fireKey != KeyCode.None && Input.GetKeyDown(fireKey))
        {
            var mgr = SoundManager.SoundManager.Instance;
            if (mgr == null)
                return;

            if (string.IsNullOrEmpty(fireSfxName))
                return;

            if (replaceOnFire)
                mgr.PlaySFXReplace(fireSfxName);
            else
                mgr.Play(fireSfxName); // 使用原有 Play（PlayOneShot）以允许重叠

            Debug.Log($"[Sounduse] 发射音效触发: {fireSfxName}");
        }
    }
    void Start()
    {
        Debug.Log("Sounduse 脚本已启动，挂载在: " + gameObject.name);
    }
}
