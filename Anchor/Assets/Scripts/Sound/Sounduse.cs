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

    private void Start()
    {
        soundService = SoundManager.SoundManager.Instance as SoundManager.ISoundService;
        if (soundService == null)
            Debug.LogWarning("Sounduse: ISoundService 未找到，音效将无法播放");
    }

    // 判断是否匹配规则，若匹配则切换对应音效
    public void CheckAndPlayCollision(GameObject other)
    {
        if (other == null || soundService == null) return;

        // 优先按 specificTarget匹配，其次按 targetTag
        foreach (var rule in rules)
        {
            if (rule == null || string.IsNullOrEmpty(rule.sfxName)) continue;

            if (rule.specificTarget != null && other == rule.specificTarget)
            {
                soundService.Play(rule.sfxName);
                return;
            }

            if (!string.IsNullOrEmpty(rule.targetTag) && other.CompareTag(rule.targetTag))
            {
                soundService.Play(rule.sfxName);
                return;
            }
        }

        // 没有规则匹配时，使用默认音效
            if (!string.IsNullOrEmpty(defaultSfxName)) soundService.Play(defaultSfxName);
    }

    //2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckAndPlayCollision(collision.gameObject);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!onlyOnEnter) CheckAndPlayCollision(collision.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckAndPlayCollision(other.gameObject);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!onlyOnEnter) CheckAndPlayCollision(other.gameObject);
    }

    private void Update()
    {
        if (!enableFireSound || fireKey == KeyCode.None || string.IsNullOrEmpty(fireSfxName)) return;

        if (Input.GetKeyDown(fireKey)) soundService?.Play(fireSfxName);
    }
}
