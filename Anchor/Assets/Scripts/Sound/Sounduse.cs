using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Sound Use")]
public class Sounduse : MonoBehaviour
{
        private SoundManager.ISoundService soundService;

    [System.Serializable]
    public class CollisionRule
    {
        public string sfxName;
        public string targetTag; // 匹配 Tag
        public GameObject specificTarget; // 匹配具体对象
    }

    [Header("Collision SFX Rules")]
    public List<CollisionRule> rules = new List<CollisionRule>();

    // 备用默认音效
    public string defaultSfxName = "";

    // 在 Enter 时触发
    public bool onlyOnEnter = true;

    [Header("Fire Settings")]
    public bool enableFireSound = false;
    public KeyCode fireKey = KeyCode.None; 
    public string fireSfxName = ""; 
    // 发射音效始终覆盖当前正在播放的 SFX

    // 判断是否匹配规则，若匹配则切换对应音效
    public void CheckAndPlayCollision(GameObject other)
    {
        if (other == null)
            return;

        if (soundService == null)
        {
            soundService = SoundManager.SoundManager.Instance as SoundManager.ISoundService;
            if (soundService == null)
                return;
        }

        // 优先按 specificTarget匹配，其次按 targetTag
        foreach (var rule in rules)
        {
            if (rule == null)
                continue;
            if (string.IsNullOrEmpty(rule.sfxName))
                continue;

            if (rule.specificTarget != null)
            {
                if (other == rule.specificTarget)
                {
                    soundService.Play(rule.sfxName);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(rule.targetTag))
            {
                if (other.CompareTag(rule.targetTag))
                {
                    soundService.Play(rule.sfxName);
                    return;
                }
            }
        }

        // 没有规则匹配时，使用默认音效
            if (!string.IsNullOrEmpty(defaultSfxName))
            {
                soundService.Play(defaultSfxName);
            }
    }

    //2D
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
        CheckAndPlayCollision(other.gameObject);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!onlyOnEnter)
        {
            CheckAndPlayCollision(other.gameObject);
        }
    }
    
    private void Update()
    {
        if (!enableFireSound)
            return;

        if (fireKey != KeyCode.None && Input.GetKeyDown(fireKey))
        {
            if (soundService == null)
                soundService = SoundManager.SoundManager.Instance as SoundManager.ISoundService;

            if (soundService == null)
                return;

            if (string.IsNullOrEmpty(fireSfxName))
                return;

            // 发射音效统一采用覆盖播放
            soundService.Play(fireSfxName);
        }
    }
    void Start()
    {
        // 启动时尝试注入默认的 SoundManager 实现
        soundService = SoundManager.SoundManager.Instance as SoundManager.ISoundService;
    }
}
